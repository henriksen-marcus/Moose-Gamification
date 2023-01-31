using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class TreeManager : MonoBehaviour
{
    MainManager mainManager;

    [Header("GameObjects")]
    public GameObject treePrefab;
    public List<GameObject> treeSpawnerList;

    [Header("Tree type Spawning - Height requirements")]
    public Vector2 BirchSpawn = new Vector2(int.MinValue, -0.1f);
    public Vector2 PineSpawn = new Vector2(-0.1f, 0.1f);
    public Vector2 SpruceSpawn = new Vector2(0.1f, int.MaxValue);

    [Header("Amount of Trees to Spawn")]
    [SerializeField] int treeSpawnCount = 20000;

    [Header("Trees")]
    public GameObject treesFolderParent;
    public GameObject treesParent;

    float mapSize_x;
    float mapSize_z;
    Vector3 spawnPosition;


    //--------------------


    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();

        //Spawn "Map Folder" to "MainManager Folder" in Hirearchy
        Instantiate(treesFolderParent, Vector3.zero, Quaternion.identity);


        //Spawn "Trees Folder" to "MainManager Folder"->"Map Folder" in Hirearchy
        Instantiate(treesParent, Vector3.zero, Quaternion.identity).transform.parent = treesFolderParent.transform; ;

        CalculateMapSize();
        SpawnTreesOnMap();
    }
    private void Update()
    {
        //Enables that the tree types requirements can be regulated from the Inspector during runtime
        //RuntimeTreeTypeRegulation();
    }


    //--------------------


    void CalculateMapSize()
    {
        mapSize_x = mainManager.maplist[0].GetComponent<MeshCollider>().bounds.size.x;
        mapSize_z = mainManager.maplist[0].GetComponent<MeshCollider>().bounds.size.z;
    }

    void SpawnTreesOnMap()
    {
        for (int i = 0; i < treeSpawnCount; i++)
        {
            //Make a random spawn position for this tree (y value i set way above the mesh by purpose)
            spawnPosition = new Vector3((float)Random.Range(-mapSize_x / 2, mapSize_x / 2), 50, (float)Random.Range(-mapSize_z / 2, mapSize_z / 2));

            //Spawn a tree at the random position and place the GameObject in a parent-folder in the hierarchy
            treeSpawnerList.Add(Instantiate(treePrefab, spawnPosition, Quaternion.identity) as GameObject);
            treeSpawnerList[i].transform.parent = treesParent.transform;
        }
    }

    void RuntimeTreeTypeRegulation()
    {
        for (int i = 0; i < treeSpawnerList.Count; i++)
        {
            if (treeSpawnerList[i].transform.position.y >= BirchSpawn.x && treeSpawnerList[i].transform.position.y <= BirchSpawn.y)
            {
                treeSpawnerList[i].GetComponent<Tree>().gameObject.SetActive(true);
            }
            else if (treeSpawnerList[i].transform.position.y >= PineSpawn.x && treeSpawnerList[i].transform.position.y <= PineSpawn.y)
            {
                treeSpawnerList[i].GetComponent<Tree>().gameObject.SetActive(true);
            }
            else if (treeSpawnerList[i].transform.position.y >= SpruceSpawn.x && treeSpawnerList[i].transform.position.y <= SpruceSpawn.y)
            {
                treeSpawnerList[i].GetComponent<Tree>().gameObject.SetActive(true);
            }
            else
            {
                treeSpawnerList[i].GetComponent<Tree>().gameObject.SetActive(false);
            }
        }
    }
}
