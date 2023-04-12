using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.AI;

public class Forest : MonoBehaviour
{
    private ForestManager _forestManager;
    private ColorManager _colorManager;

    [SerializeField] int treesAmountInForest;
    [SerializeField] public float averageAge;
    private const int MinTreesInForest = 1;
    private const int MaxTreesInForest = 1800;

    [Header("Tree States")]
    public ForestType forestType;
    public ForestDensity forestDensityLevel;
    private ForestDensity lastDensity;
    public TreeHealth forestHealth;
    public Season currentSeason;

    [Header("Tree Spawning")]
    [SerializeField] GameObject Tree;
#pragma warning disable 0414
    [SerializeField] float spawningRadius;
#pragma warning restore 0414
    public float forestHeight = 0;
    public float forestDensity = 0;

    private Tree[] _treeArray;
    public List<Tree> treeList;
    private List<Tree> trackableTrees;
    private List<GameObject> spawnedTrees;

    int borderRadius = 8;
    // float maxDistanceVariation = 1f;
    float lastDistance = 2f;

    // Debugging
    /*private bool didUpdate;
    private int updateCount;
    public bool hasBeenCalled;*/


    //--------------------

    /** Debugging */
    /*public void DidUpdate()
    {
        hasBeenCalled = true;
        _didUpdate = true;
        _updateCount++;
        SetColor(Color.green);
    }*/

    private void Awake()
    {
        _forestManager = FindObjectOfType<ForestManager>();
        _colorManager = FindObjectOfType<ColorManager>();

        trackableTrees = new List<Tree>();
        spawnedTrees = new List<GameObject>();  
        treesAmountInForest = UnityEngine.Random.Range(MinTreesInForest, MaxTreesInForest);

        RaycastPosition();
    }
    
    private void Start()
    {
        spawningRadius = 10f;
        SubscribeToEvents();

        SetForestType();
        MakeTreesInForestArray();

        //---

        MoveFromArrayToList();

        //---

        SetForestSeason();
        SetForestHealth();
        UpdateForestDensity();
        lastDensity = forestDensityLevel;
        UpdateTreeStats();

        switch (forestType)
        {
            case ForestType.Spruce:
                var loadedObject1 = Resources.Load("Trees/Spruce/Spruce1");
                Tree = (GameObject)loadedObject1;
                break;
            case ForestType.Pine:
                var loadedObject2 = Resources.Load("Trees/Pine/Pine1");
                Tree = (GameObject)loadedObject2;
                break;
            case ForestType.Birch:
                var loadedObject3 = Resources.Load("Trees/Birch/Birch1");
                Tree = (GameObject)loadedObject3;
                break;
            default:
                break;
        }
        SpawnTrees();
    }


    //--------------------

    public int GetAverageAge() { return float.IsNaN(averageAge) ? 0 : Mathf.FloorToInt(averageAge); }
    void MakeTreesInForestArray()
    {
        _treeArray = new Tree[treesAmountInForest];

        for (int i = 0; i < treesAmountInForest; i++)
        {
            _treeArray[i] = new Tree(this)
            {
                //Tree age and height
                treeType = forestType
            };
            _treeArray[i].SetStats();
        }
    }

    public void RaycastPosition()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, int.MaxValue))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);

            if (hit.collider.tag == "Map")
            {
                gameObject.transform.position = hit.point;

                //Move the tree a bit up from the ground
                gameObject.transform.position += new Vector3(0, 0.007f, 0);
            }
        }

        if (gameObject.transform.position.y >= 20f)
        {
            gameObject.SetActive(false);
        }
    }

    void MoveFromArrayToList()
    {
        treeList = new List<Tree>(_treeArray);

        _treeArray = null;
    }


    //--------------------


    void SetForestType()
    {
        if (gameObject.transform.position.y >= _forestManager.BirchForestSpawn.x && gameObject.transform.position.y <= _forestManager.BirchForestSpawn.y)
        {
            forestType = ForestType.Birch;
        }
        else if (gameObject.transform.position.y >= _forestManager.PineForestSpawn.x && gameObject.transform.position.y <= _forestManager.PineForestSpawn.y)
        {
            forestType = ForestType.Pine;
        }
        else if (gameObject.transform.position.y >= _forestManager.SpruceForestSpawn.x && gameObject.transform.position.y <= _forestManager.SpruceForestSpawn.y)
        {
            forestType = ForestType.Spruce;
        }
        else
        {
            forestType = ForestType.None;
        }
    }

    // TODO: Dette burde ikke oppdateres i tick! Heller pr. mnd via forest manager.
    void SetForestSeason() { currentSeason = TimeManager.instance.currentSeason; }

    void SetForestHealth()
    {
        // Sum the health of the trees
        var averageHealth = treeList.Aggregate(0, (sum, nextTree) => sum + (int)nextTree.treeHealth);
        
        // Get the average
        averageHealth = treeList.Count == 0 ? 0 : averageHealth / treeList.Count;

        //Calculate Forest Health State
        forestHealth = averageHealth switch
        {
            >= 2 => TreeHealth.Healthy,
            >= 1 => TreeHealth.Damaged,
            >= 0 => TreeHealth.Broken,
            _ => TreeHealth.Dead
        };
    }


    void UpdateForestDensity()
    {
        // Since shadow area is tree type dependent now, forest density is already scaled,
        // so level should be based off the same number (e.g. a pine forest will always take slower to reach max density than a birch tree,
        // and thus end up with more trees)
        
        // forestDensityLevel = forestType switch
        // {
        //     ForestType.Birch => forestDensity switch
        //     {
        //         < 30 => ForestDensity.Density1,
        //         < 40 => ForestDensity.Density2,
        //         < 50 => ForestDensity.Density3,
        //         < 60 => ForestDensity.Density4,
        //         _ => ForestDensity.Density5
        //     },
        //     ForestType.Spruce => forestDensity switch
        //     {
        //         < 400 => ForestDensity.Density1,
        //         < 600 => ForestDensity.Density2,
        //         < 800 => ForestDensity.Density3,
        //         < 1000 => ForestDensity.Density4,
        //         _ => ForestDensity.Density5
        //     },
        //     ForestType.Pine => forestDensity switch
        //     {
        //         < 140 => ForestDensity.Density1,
        //         < 200 => ForestDensity.Density2,
        //         < 250 => ForestDensity.Density3,
        //         < 300 => ForestDensity.Density4,
        //         _ => ForestDensity.Density5
        //     },
        //     _ => ForestDensity.None
        // };
        forestDensityLevel = forestDensity switch
        {
            < 10 => ForestDensity.Density1,
            < 40 => ForestDensity.Density2,
            < 70 => ForestDensity.Density3,
            < 100 => ForestDensity.Density4,
            _ => ForestDensity.Density5
        };

        
    }

    //--------------------


    #region Functions for a Moose to call
    public int GetForestTreeAmount() { return treeList.Count; }

    public Tree GetATree(int a) { return treeList[a]; }

    public Tree GetOptimalTreeToEat()
    {
        var bestTree = treeList.Find(t => t.treeHealth == TreeHealth.Healthy && t.stemHeight < 3);
        if (bestTree != null) return bestTree;

        bestTree = treeList.Find(t => t.treeHealth == TreeHealth.Damaged && t.stemHeight < 3);
        
        return bestTree ?? treeList.Find(t => t.treeHealth == TreeHealth.Broken && t.stemHeight < 3);
    }
    #endregion


    //--------------------


    void SubscribeToEvents()
    {
        TimeManager.instance.OnSpringBegin += UpdateBirth;

        TimeManager.instance.OnNewMonth += UpdateSpawnedTrees;
        TimeManager.instance.OnNewMonth += UpdateAverageAge;
    }
    
    void UpdateForestStats()
    {
        SetForestSeason();

        SetForestHealth();

        UpdateForestDensity();
    }

    private void UpdateAverageAge()
    {
        long lTemp = 0;
        foreach (var tree in treeList)
        {
            lTemp += tree.treeAgeInDaysTotal;
        }
        float fTemp = (float)lTemp / treeList.Count;
        averageAge = fTemp / 365;
    }
    
    public void UpdateTreeStats()
    {
        UpdateForestStats();

        for (int i = 0; i < treeList.Count;)
        {
            treeList[i].UpdateStats();
            
            forestDensity += treeList[i].shadowArea;
            forestHeight += treeList[i].treeHeight;

            // Check for dead trees
            if (treeList[i].isDead)
            {
                _forestManager.treesDiedOfAge++;
                treeList.RemoveAt(i);
            }
            i++;
        }
        treesAmountInForest = treeList.Count;
        
        forestHeight = treesAmountInForest == 0 ? 0 : forestHeight / treesAmountInForest;
        forestDensity *= 0.005f; // Divide by 200
    }

    void UpdateBirth()
    {
        if (forestDensityLevel != ForestDensity.Density5)
        {
            for (int i = 0; i < treeList.Count; i++)
            {
                for (int j = 0; j < treeList[i].CheckIfGettingBirth(); j++)
                {
                    treeList.Add(new Tree(this));
                    treeList[^1].treeType = forestType;
                    treeList[^1].SetBirth();
                }

                _forestManager.treesBirth += treeList[i].CheckIfGettingBirth();
            }
        }
        treesAmountInForest = treeList.Count;
    }

    void SpawnTrees()
    {
        UpdateTreeStats();
        int numberOfTrees = 0;
        switch (forestDensityLevel)
        {
            case ForestDensity.Density1:
                numberOfTrees = 3;
                break;
            case ForestDensity.Density2:
                numberOfTrees = 5;
                break;
            case ForestDensity.Density3:
                numberOfTrees = 7;
                break;
            case ForestDensity.Density4:
                numberOfTrees = 11;
                break;
            case ForestDensity.Density5:
                numberOfTrees = 13;
                break;
            default:
                break;
        }
        if (forestType == ForestType.Birch && forestDensityLevel > ForestDensity.Density1)
        {
            numberOfTrees--;
        }

        
        for (int i = 0;i < numberOfTrees;i++)
        {
            var distanceFromMiddle = UnityEngine.Random.Range(0, borderRadius);
            // distanceFromMiddle = Mathf.Clamp(
            //     distanceFromMiddle,
            //     lastDistance - maxDistanceVariation,
            //     lastDistance + maxDistanceVariation);
            lastDistance = distanceFromMiddle;

            var angle = i * 360f / numberOfTrees;
            var angleInRadians = angle * Mathf.Deg2Rad;
            var x = Mathf.Cos(angleInRadians);
            var z = Mathf.Sin(angleInRadians);
            var offset = new Vector3(x, 0f, z).normalized;
            offset *= distanceFromMiddle;
            Vector3 position = transform.position + offset + new Vector3(0,5,0);
            
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Map");
            Ray ray = new Ray(position, new Vector3(0, -1, 0));
            if (Physics.Raycast(position, new Vector3(0, -1, 0), out hit,200f, mask))
            {
                GameObject obj = Instantiate(Tree, hit.point, Quaternion.identity, gameObject.transform);
                int rand = UnityEngine.Random.Range(0, treeList.Count - 1);
                trackableTrees.Add(treeList[rand]);
                float treeHeight = (float)trackableTrees[trackableTrees.Count - 1].treeHeight;
                
                float scale = map(treeHeight, 0f, 250f, 0.6f, 1f);
                obj.transform.localScale = new Vector3(scale, scale, scale);
                spawnedTrees.Add(obj);
            }
        }

        
    }

    public static float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }

    public void UpdateSpawnedTrees()
    {
        if (lastDensity != forestDensityLevel)
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
                spawnedTrees.Clear();
                trackableTrees.Clear();
            }
            SpawnTrees();
        }
        else
        {
            for (int i = 0; i < spawnedTrees.Count; i++)
            {
                float treeHeight = (float)trackableTrees[i].treeHeight;
                float scale = map(treeHeight, 0f, 250f, 0.6f, 1f);
                spawnedTrees[i].transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        lastDensity = forestDensityLevel;
    }
}