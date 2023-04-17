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
    public static ForestManager instance;

    [Header("GameObjects")]
    public GameObject forestPrefab;
    public List<GameObject> forestSpawnerList;

    [Header("Tree type Spawning - Height requirements")]
    public Vector2 BirchForestSpawn = new Vector2(int.MinValue, -0.1f);
    public Vector2 PineForestSpawn = new Vector2(-0.1f, 0.1f);
    public Vector2 SpruceForestSpawn = new Vector2(0.1f, int.MaxValue);

    [Header("Amount of Trees to Spawn")]
    [SerializeField] int forestSpawnCount = 500;

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
    // Holds references to the forest component of each GameObject in forestSpawnerList.
    private List<Forest> forestList;
    
    /* How long it takes to update all forests once an update is initiated.
     * Lower value = more processor power required in that time period. */
    private float _timeToUpdateForests = 5f;

    /// <summary>
    /// How many forests we will update the next tick.
    /// </summary>
    private int bufferSize;

    /// <summary>
    /// The index we are at in the forestList when updating all trees.
    /// </summary>
    private int currentIndex;
    
    private float deltaTime;

    public List<int> forestDensityAverage;
    public List<int> forestQuantityAverage;
    public List<int> forestTreeAgeAverage;

    [Header("Timers")]
    private Timer timer = new();
    private Timer updateTimer = new();

    //--------------------


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        mainManager = FindObjectOfType<MainManager>();
        forestSpawnerList = new List<GameObject>(forestSpawnCount);

        forestList = new List<Forest>(forestSpawnCount);

        CalculateMapSize();
        SpawnForestsOnMap();

    }
    private void Start()
    {
        SubscribeToEvents();
        // UpdateForests();
        AddCamera();
        Statistics();
    }


    //--------------------


    void CalculateMapSize()
    {
        mapSize_x = mainManager.maplist[0].transform.GetChild(0).GetComponent<MeshCollider>().bounds.size.x;
        mapSize_z = mainManager.maplist[0].transform.GetChild(0).GetComponent<MeshCollider>().bounds.size.z;
    }

    void SpawnForestsOnMap()
    {
        //timer.Start("Spawn Forests");

        //Spawn Forest
        for (int i = 0; i < forestSpawnCount;)
        {
            bool reset = false;

            //Make a random spawn position for this tree (y value i set way above the mesh by purpose)
            spawnPosition = new Vector3(Random.Range(-mapSize_x / 2f, mapSize_x / 2f), 50f, Random.Range(-mapSize_z / 2f, mapSize_z / 2f));

            //Spawn a tree at the random position and place the GameObject in a parent-folder in the hierarchy
            forestSpawnerList.Add(Instantiate(forestPrefab, spawnPosition, Quaternion.identity) as GameObject);
            
            forestSpawnerList[i].transform.parent = mainManager.forestParent.transform;
            forestSpawnerList[i].transform.localScale = new Vector3(1, 1, 1);
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
                            if (positionRetryCounter % respawnAmount != 0) continue;
                            Destroy(forestSpawnerList[i]);
                            forestSpawnerList.RemoveAt(i);
                            reset = true;
                            break;
                        }
                    }
                    break;
                }
            }

            if (!reset) i++;
            else forestSpawnCount--;
        }

        //print(timer);

        // Store a ref to each forest component
        forestSpawnerList.ForEach(obj => forestList.Add(obj.GetComponent<Forest>()));
    }

    /** Runs every time the month changes. Updates every forest over time. */
    void UpdateForests()
    {
        // Update any remaining trees
        if (currentIndex < forestList.Count - 1 && TimeManager.instance.GetDay() > 0)
        {
            for (var i = currentIndex; i < forestList.Count; i++)
            {
                forestList[i].UpdateTreeStats();
            }
        }
        //forestList.ForEach(f => f.SetColor(Color.red));
        currentIndex = bufferSize = 0;
        updateTimer.Start();
    }

    /** Calculate the amount of forests to be updated next tick. */
    private int GetBufferSize()
    {
        // Directly minus the index (not index+1) because we have not yet processed the current index.
        int remainingForests = forestList.Count - currentIndex;
        float remainingSeconds = Math.Clamp(_timeToUpdateForests - updateTimer.GetTime(), 0f, float.MaxValue);
        float currentFPS = 1.0f / Time.unscaledDeltaTime;
        float remainingFrames = currentFPS * remainingSeconds;

        //print("Updated: " + (forestList.Count - remainingTrees) + " Remaining trees: " + remainingTrees +" Current FPS: " + currentFPS + "\nRemaining Seconds: " + remainingSeconds + " Trees per frame: " + ((int)Math.Ceiling(remainingTrees / remainingFrames)) + " Remaining frames: " + remainingFrames);
        
        return (int)Math.Ceiling(remainingForests / remainingFrames); // Forests per frame
    }
    
    /** Get the next set of forests to update. */
    List<Forest> GetNextBuffer()
    {
        // Nothing more to update
        if (currentIndex + bufferSize >= forestList.Count) return null;
        
        // Jump to the next index for the next buffer
        currentIndex += bufferSize;
        
        // Prevent going out of list scope
        bufferSize = Math.Clamp(GetBufferSize(), 0, forestList.Count - currentIndex);

        if (bufferSize == 0) return null;
        
        //print("curIndex: " + currentIndex + " bufSize: " + bufferSize);
        return forestList.GetRange(currentIndex, bufferSize);
    }

     void Update()
     {
         GetNextBuffer()?.ForEach(forest =>
         {
             // forest.UpdateTreeStats();
             forest.UpdateSpawnedTrees();
             //forest.DidUpdate();
         });
     }

     //--------------------

    void SubscribeToEvents()
    {
        // TimeManager.instance.OnNewDay += TreeCount;
        TimeManager.instance.OnNewDay += Statistics;
        TimeManager.instance.OnNewDay += UpdateForests;
    }
    
    // void TreeCount()
    // {
    //     totalTreesAmount = 0;
    //     forestList.ForEach(forest => totalTreesAmount += forest.treeList.Count);
    //
    //     #region Get Density
    //     //List<float> tempList_Birch = new List<float>();
    //     //List<float> tempList_Spruce = new List<float>();
    //     //List<float> tempList_Pine = new List<float>();
    //
    //     //for (int i = 0; i < forestSpawnerList.Count; i++)
    //     //{
    //     //    if (forestSpawnerList[i].GetComponent<Forest>().forestState_Type == ForestState_Type.forestType_Birch)
    //     //    {
    //     //        tempList_Birch.Add(forestSpawnerList[i].GetComponent<Forest>().forest_Density);
    //     //    }
    //     //    else if(forestSpawnerList[i].GetComponent<Forest>().forestState_Type == ForestState_Type.forestType_Spruce)
    //     //    {
    //     //        tempList_Spruce.Add(forestSpawnerList[i].GetComponent<Forest>().forest_Density);
    //     //    }
    //     //    else if (forestSpawnerList[i].GetComponent<Forest>().forestState_Type == ForestState_Type.forestType_Pine)
    //     //    {
    //     //        tempList_Pine.Add(forestSpawnerList[i].GetComponent<Forest>().forest_Density);
    //     //    }
    //     //}
    //
    //     //densityBirch.x = tempList_Birch[0];
    //     //densityBirch.y = tempList_Birch[0];
    //
    //     //for (int i = 0; i < tempList_Birch.Count; i++)
    //     //{
    //     //    if (densityBirch.x > tempList_Birch[i])
    //     //    {
    //     //        densityBirch.x = tempList_Birch[i];
    //     //    }
    //
    //     //    if (densityBirch.y < tempList_Birch[i])
    //     //    {
    //     //        densityBirch.y = tempList_Birch[i];
    //     //    }
    //     //}
    //
    //     //densitySpruce.x = tempList_Spruce[0];
    //     //densitySpruce.y = tempList_Spruce[0];
    //
    //     //for (int i = 0; i < tempList_Spruce.Count; i++)
    //     //{
    //     //    if (densitySpruce.x > tempList_Spruce[i])
    //     //    {
    //     //        densitySpruce.x = tempList_Spruce[i];
    //     //    }
    //
    //     //    if (densitySpruce.y < tempList_Spruce[i])
    //     //    {
    //     //        densitySpruce.y = tempList_Spruce[i];
    //     //    }
    //     //}
    //
    //     //densityPine.x = tempList_Pine[0];
    //     //densityPine.y = tempList_Pine[0];
    //
    //     //for (int i = 0; i < tempList_Pine.Count; i++)
    //     //{
    //     //    if (densityPine.x > tempList_Pine[i])
    //     //    {
    //     //        densityPine.x = tempList_Pine[i];
    //     //    }
    //
    //     //    if (densityPine.y < tempList_Pine[i])
    //     //    {
    //     //        densityPine.y = tempList_Pine[i];
    //     //    }
    //     //}
    //     #endregion
    // }

    void Statistics()
    {
        float density = 0;
        float quantity = 0;
        int age = 0;
        forestList.ForEach(forest =>
        {
            density += forest.forestDensity;
            quantity += forest.GetForestTreeAmount();   
            age += forest.GetAverageAge();
        });
        forestDensityAverage.Add((int)(density / forestList.Count));
        forestQuantityAverage.Add((int)(quantity / forestList.Count));
        forestTreeAgeAverage.Add((age / forestList.Count));
    }


    void AddCamera()
    {
        
    }
    
    public void GetNumTrees(ref int birch, ref int spruce, ref int pine)
    {
        birch = spruce = pine = 0;

        foreach (var forest in forestList)
        {
            foreach (var tree in forest.treeList)
            {
                switch (tree.treeType)
                {
                    case ForestType.Birch:
                        birch++;
                        break;
                    case ForestType.Spruce:
                        spruce++;
                        break;
                    case ForestType.Pine:
                        pine++;
                        break;
                }
            }
        }
    }
}

// Update trees before:     0,2350874s
// Update trees after:      0,1629381
