using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

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
    [SerializeField] int forestSpawnCount = 1;
    
    float mapSize_x;
    float mapSize_z;
    Vector3 spawnPosition;


    //--------------------


    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();

        CalculateMapSize();
        SpawnForestsOnMap();

    }


    //--------------------


    void CalculateMapSize()
    {
        mapSize_x = mainManager.maplist[0].GetComponent<MeshCollider>().bounds.size.x;
        mapSize_z = mainManager.maplist[0].GetComponent<MeshCollider>().bounds.size.z;
    }

    void SpawnForestsOnMap()
    {
        for (int i = 0; i < forestSpawnCount; i++)
        {
            //Make a random spawn position for this tree (y value i set way above the mesh by purpose)
            spawnPosition = new Vector3((float)Random.Range(-mapSize_x / 2, mapSize_x / 2), 50, (float)Random.Range(-mapSize_z / 2, mapSize_z / 2));

            //Spawn a tree at the random position and place the GameObject in a parent-folder in the hierarchy
            forestSpawnerList.Add(Instantiate(forestPrefab, spawnPosition, Quaternion.identity) as GameObject);
            forestSpawnerList[i].transform.parent = mainManager.forestParent.transform;
        }
    }
}
