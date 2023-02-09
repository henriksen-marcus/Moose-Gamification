using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Random = UnityEngine.Random;

public class ForestManager : MonoBehaviour
{
    MainManager mainManager;

    [Header("GameObjects")]
    public GameObject forestPrefab;
    public List<GameObject> forestSpawnerList;

    [Header("Tree type Spawning - Height requirements")]
    public Vector2 BirchForestSpawn = new Vector2(int.MinValue, -0.1f);
    public Vector2 PineForestSpawn = new Vector2(-0.1f, 0.1f);
    public Vector2 SpruceForestSpawn = new Vector2(0.1f, int.MaxValue);

    [Header("Amount of Trees to Spawn")]
    [SerializeField] int forestSpawnCount = 700;

    [Header("Spawning")]
    [SerializeField] int respawnAmount = 10;
    [SerializeField] float collisionRangeStart = 8;
    float collisionRange;
    float mapSize_x;
    float mapSize_z;
    Vector3 spawnPosition;

    [Header("Info")]
    public int totalTreesAmount;
    public int treesDiedOfAge;
    public int treesBirth;

    public Vector2 densityBirch;
    public Vector2 densitySpruce;
    public Vector2 densityPine;

    [Header("Optimization")]
    /// <summary>
    /// Holds references to the forest component of each GameObject in forestSpawnerList.
    /// </summary>
    public List<Forest> forestList;

    /// <summary>
    /// Holds all the trees that will be updated in the next tick.
    /// </summary>
    private List<Forest> updateTickList;

    /// <summary>
    /// How many forests we will load into the updateTickList each tick.
    /// </summary>
    private int bufferSize;

    /// <summary>
    /// The index we are at in the forestList when updating all trees.
    /// </summary>
    private int currentIndex;
    
    private float deltaTime;
    
    [Header("Timers")]
    private Timer timer = new();
    private Timer updateTimer = new();

    private static bool runOnce;


    //--------------------


    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
        forestSpawnerList = new List<GameObject>(forestSpawnCount);

        forestList = new List<Forest>(forestSpawnCount);

        CalculateMapSize();
        SpawnForestsOnMap();

    }
    private void Start()
    {
        SubscribeToEvents();
        // Init list with the estimate of amount of trees
        updateTickList = new List<Forest>(forestSpawnCount / (int)TimeManager.instance.defaultPlaySpeed);
        UpdateForests();
        LoadForestBuffer();
    }


    //--------------------


    void CalculateMapSize()
    {
        mapSize_x = mainManager.maplist[0].transform.GetChild(0).GetComponent<MeshCollider>().bounds.size.x;
        mapSize_z = mainManager.maplist[0].transform.GetChild(0).GetComponent<MeshCollider>().bounds.size.z;
    }

    void SpawnForestsOnMap()
    {
        timer.Start("Spawn Forests");

        //Spawn Forest
        for (int i = 0; i < forestSpawnCount;)
        {
            bool reset = false;

            //Make a random spawn position for this tree (y value i set way above the mesh by purpose)
            spawnPosition = new Vector3(Random.Range(-mapSize_x / 2f, mapSize_x / 2f), 50f, Random.Range(-mapSize_z / 2f, mapSize_z / 2f));

            //Spawn a tree at the random position and place the GameObject in a parent-folder in the hierarchy
            forestSpawnerList.Add(Instantiate(forestPrefab, spawnPosition, Quaternion.identity) as GameObject);
            
            forestSpawnerList[i].transform.parent = mainManager.forestParent.transform;
            forestSpawnerList[i].name = "Forest (" + i + ")";

            //Check if another Forest is located on this position
            for (int j = 1; j < forestSpawnerList.Count; j++)
            {
                int positionRetryCounter = 0;
                collisionRange = collisionRangeStart;

                if ((forestSpawnerList[i].transform.position.x <= (forestSpawnerList[j - 1].transform.position.x + collisionRange) &&
                    forestSpawnerList[i].transform.position.x >= (forestSpawnerList[j - 1].transform.position.x - collisionRange))
                    &&
                    (forestSpawnerList[i].transform.position.z <= (forestSpawnerList[j - 1].transform.position.z + collisionRange) &&
                    forestSpawnerList[i].transform.position.z >= (forestSpawnerList[j - 1].transform.position.z - collisionRange)))
                {
                    //If there is another Forest in this position, move this forest to a new location
                    //print(forestSpawnerList[i].name + " (" + forestSpawnerList[i].transform.position + ") overlaps with " + forestSpawnerList[j - 1].name + " (" + forestSpawnerList[j - 1].transform.position + ") ");

                    bool isAlone = false;

                    while (!isAlone)
                    {
                        bool isOccupied = false;

                        forestSpawnerList[i].transform.position = new Vector3((float)Random.Range(-mapSize_x / 2, mapSize_x / 2), 50, (float)Random.Range(-mapSize_z / 2, mapSize_z / 2));

                        //print(forestSpawnerList[i].name + " changed Position to: " + forestSpawnerList[i].transform.position);

                        for (int k = 1; k < forestSpawnerList.Count; k++)
                        {
                            //If moved position is occupied, move again
                            if ((forestSpawnerList[i].transform.position.x <= (forestSpawnerList[k - 1].transform.position.x + collisionRange) &&
                            forestSpawnerList[i].transform.position.x >= (forestSpawnerList[k - 1].transform.position.x - collisionRange))
                            &&
                            (forestSpawnerList[i].transform.position.z <= (forestSpawnerList[k - 1].transform.position.z + collisionRange) &&
                            forestSpawnerList[i].transform.position.z >= (forestSpawnerList[k - 1].transform.position.z - collisionRange)))
                            {
                                //print(forestSpawnerList[i].name + " retries position transforming");

                                positionRetryCounter++;

                                isOccupied = true;
                                break;
                            }
                        }

                        if (!isOccupied)
                        {
                            //Reset stats from Forest's Awake
                            forestSpawnerList[i].GetComponent<Forest>().RaycastPosition();
                            //forestSpawnerList[i].GetComponent<Forest>().SetForestHealth();
                            //forestSpawnerList[i].GetComponent<Forest>().SetForestType();
                            //forestSpawnerList[i].GetComponent<Forest>().SetForestColor();

                            isAlone = true;
                        }
                        else
                        {
                            //If Forest have tried to move X times, destroy the object
                            if (positionRetryCounter % respawnAmount == 0)
                            {
                                Destroy(forestSpawnerList[i]);
                                forestSpawnerList.RemoveAt(i);

                                reset = true;
                                break;
                            }
                        }
                    }
                    break;
                }
            }

            if (!reset)
            {
                i++;
            }
            else
            {
                forestSpawnCount--;
            }
        }

        print(timer);

        // Store a ref to each forest component
        forestSpawnerList.ForEach(obj => forestList.Add(obj.GetComponent<Forest>()));
        currentIndex = forestList.Count;
    }

    private int updtCnt = 0;
    
    /// <summary>
    /// Runs every time the day changes. Updates every forest over time.
    /// </summary>
    void UpdateForests()
    {
        if (!runOnce)
        {
            runOnce = true;
        }
        else
        {
            var cnt2 = forestList.Count(obj => !obj.didUpdate);

            if (cnt2 > 0)
            {
                print("Not everything updated!!!!!!! NUM: " + cnt2);
            }
        }
        forestList.ForEach(obj => obj.didUpdate = false);
        //forestList.ForEach(obj => print("UpdateCount for Forest: " + obj.updateCount + " . Number of updates: " + updtCnt));
        
        print("CurIndex: " + currentIndex); 
        /* If we somehow didn't finish updating all the trees in our time window,
         update the rest now. */
        if (currentIndex != forestList.Count && currentIndex != 0)
        {
            bufferSize = Math.Clamp(forestList.Count - (currentIndex + 1), 0, int.MaxValue);
            
            if (bufferSize > 0)
            {
                print("WARNING: Didn't update all trees in time! Updating " + (forestList.Count - (currentIndex + 1)) + " now... index:" + currentIndex + " size: " + forestList.Count);
                updateTickList.Clear();
                updateTickList.AddRange(forestList.GetRange(currentIndex, bufferSize));
                currentIndex += bufferSize;
                updateTickList.ForEach(obj =>
                {
                    obj.UpdateTreeStats();
                    obj.didUpdate = true;
                    obj.updateCount++;
                });
            
                print("Updated " + cnt + " trees on the last tick. Total is " + (cnt + bufferSize) + " with fix.");
            }
        }
        else
        {
            print("Updated " + cnt + " trees on the last tick. CurrentIndex: " + currentIndex);
        }

        var myCount = forestList.Count(obj => !obj.gotAdded);
        if (updtCnt > 0 && myCount > 0) print("Objects didn't get added. Count: " + myCount);
        
        cnt = 0;
        // Start measuring how far into the current day we are
        updateTimer.Start();
        currentIndex = 0;

        //forestList.ForEach(obj => obj.UpdateTreeStats());
        updtCnt++;
    }
    
    private int GetBufferSize()
    {
        int remainingTrees = /*forestList.Count(obj => !obj.didUpdate);*/ forestList.Count - (currentIndex + 1);
        float remainingSeconds = TimeManager.instance.playSpeed - updateTimer.GetTime();
        float currentFPS = 1.0f / Time.unscaledDeltaTime;
        float remainingFrames = currentFPS * remainingSeconds;

        //print("Updated: " + (forestList.Count - remainingTrees) + " Remaining trees: " + remainingTrees +" Current FPS: " + currentFPS + "\nRemaining Seconds: " + remainingSeconds + " Trees per frame: " + ((int)Math.Ceiling(remainingTrees / remainingFrames)) + " Remaining frames: " + remainingFrames);
        
        return (int)Math.Ceiling(remainingTrees / remainingFrames); // Trees per frame
    }

    private int cnt;
    void LoadForestBuffer()
    {
        // Update buffer size each tick to compensate for variable FPS
        bufferSize = GetBufferSize();
        switch (bufferSize)
        {
            case 0: return;
            default:
                // Ensure we don't go out of scope
                if ((currentIndex + bufferSize) > (forestList.Count - 1))
                {
                    bufferSize = forestList.Count - currentIndex; // Not +1 here because math
                    print("Buffersize went out of scope. Amount: " + ((currentIndex + bufferSize) - (forestList.Count - 1)));
                }

                updateTickList.Clear();

                if (bufferSize <= 0) return;

                //updateTickList.AddRange(forestList.GetRange(currentIndex, bufferSize));
                for (int i = currentIndex; i < currentIndex + bufferSize; i++)
                {
                    updateTickList.Add(forestList[i]);
                    forestList[i].gotAdded = true;
                }
                
                //print("Adding to updateTickList, currentIndex: " + currentIndex + " bufferSize: " + bufferSize + " ending index: " + (currentIndex + bufferSize) + " First ObjID: " + forestList[currentIndex].GetInstanceID() + " Last ObjID: " + forestList[currentIndex+bufferSize].GetInstanceID());
                
                currentIndex += bufferSize; // We land on the next element, avoiding duplicates
                break;
        }
    }

    void UpdateForests2()
    {
        currentIndex = 0;
        bufferSize = 0;
        updateTimer.Start();
    }

    // Get the next set of forests to update.
    List<Forest> GetNextBuffer()
    {
        bufferSize = GetBufferSize();

        // Jump to the next index for the next buffer
        currentIndex += bufferSize;
        
        return forestList.GetRange(currentIndex, bufferSize);
    }

    void Update()
    {
        //LoadForestBuffer();
        
        // Update the current buffer
        /*updateTickList.ForEach(obj =>
        {
            obj.UpdateTreeStats();
            obj.didUpdate = true;
            obj.updateCount++;
            ++cnt;
        });*/
        
        GetNextBuffer().ForEach(forest =>
        {
            forest.UpdateTreeStats();
            forest.updateCount++;
        });
        
    }
    
    /*private struct MyParallelJob : IJobParallelFor
    {
        //public NativeArray<float> result;
        private List<Forest> l;

        public MyParallelJob(List<Forest> a)
        {
            l = new List<Forest>(a);
        }

        public void Execute(int i)
        {
            l[i].UpdateTreeStats();
        }
    }*/

    //--------------------

    void SubscribeToEvents()
    {
        TimeManager.instance.OnNewDay += TreeCount;
        TimeManager.instance.OnNewDay += UpdateForests;
    }
    void TreeCount()
    {
        totalTreesAmount = 0;

        for (int i = 0; i < forestSpawnerList.Count; i++)
        {
            totalTreesAmount += forestSpawnerList[i].GetComponent<Forest>().treeList.Count;
        }

        #region Get Density
        //List<float> tempList_Birch = new List<float>();
        //List<float> tempList_Spruce = new List<float>();
        //List<float> tempList_Pine = new List<float>();

        //for (int i = 0; i < forestSpawnerList.Count; i++)
        //{
        //    if (forestSpawnerList[i].GetComponent<Forest>().forestState_Type == ForestState_Type.forestType_Birch)
        //    {
        //        tempList_Birch.Add(forestSpawnerList[i].GetComponent<Forest>().forest_Density);
        //    }
        //    else if(forestSpawnerList[i].GetComponent<Forest>().forestState_Type == ForestState_Type.forestType_Spruce)
        //    {
        //        tempList_Spruce.Add(forestSpawnerList[i].GetComponent<Forest>().forest_Density);
        //    }
        //    else if (forestSpawnerList[i].GetComponent<Forest>().forestState_Type == ForestState_Type.forestType_Pine)
        //    {
        //        tempList_Pine.Add(forestSpawnerList[i].GetComponent<Forest>().forest_Density);
        //    }
        //}

        //densityBirch.x = tempList_Birch[0];
        //densityBirch.y = tempList_Birch[0];

        //for (int i = 0; i < tempList_Birch.Count; i++)
        //{
        //    if (densityBirch.x > tempList_Birch[i])
        //    {
        //        densityBirch.x = tempList_Birch[i];
        //    }

        //    if (densityBirch.y < tempList_Birch[i])
        //    {
        //        densityBirch.y = tempList_Birch[i];
        //    }
        //}

        //densitySpruce.x = tempList_Spruce[0];
        //densitySpruce.y = tempList_Spruce[0];

        //for (int i = 0; i < tempList_Spruce.Count; i++)
        //{
        //    if (densitySpruce.x > tempList_Spruce[i])
        //    {
        //        densitySpruce.x = tempList_Spruce[i];
        //    }

        //    if (densitySpruce.y < tempList_Spruce[i])
        //    {
        //        densitySpruce.y = tempList_Spruce[i];
        //    }
        //}

        //densityPine.x = tempList_Pine[0];
        //densityPine.y = tempList_Pine[0];

        //for (int i = 0; i < tempList_Pine.Count; i++)
        //{
        //    if (densityPine.x > tempList_Pine[i])
        //    {
        //        densityPine.x = tempList_Pine[i];
        //    }

        //    if (densityPine.y < tempList_Pine[i])
        //    {
        //        densityPine.y = tempList_Pine[i];
        //    }
        //}

        #endregion
    }
}
