using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

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

    private float timer = 0f;



    //--------------------


    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
        forestSpawnerList = new List<GameObject>(forestSpawnCount);

        CalculateMapSize();
        SpawnForestsOnMap();

    }
    private void Start()
    {
        SubscribeToEvents();
    }


    //--------------------


    void CalculateMapSize()
    {
        mapSize_x = mainManager.maplist[0].transform.GetChild(0).GetComponent<MeshCollider>().bounds.size.x;
        mapSize_z = mainManager.maplist[0].transform.GetChild(0).GetComponent<MeshCollider>().bounds.size.z;
    }

    void SpawnForestsOnMap()
    {
        timer = Time.realtimeSinceStartup;


        //Spawn Forest
        for (int i = 0; i < forestSpawnCount;)
        {
            bool reset = false;

            //Make a random spawn position for this tree (y value i set way above the mesh by purpose)
            spawnPosition = new Vector3((float)Random.Range(-mapSize_x / 2, mapSize_x / 2), 50, (float)Random.Range(-mapSize_z / 2, mapSize_z / 2));

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

        timer = Time.realtimeSinceStartup - timer;
        print("Elapsed: " + timer);
    }


    //--------------------


    void SubscribeToEvents()
    {
        TimeManager.instance.OnNewDay += TreeCount;
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
