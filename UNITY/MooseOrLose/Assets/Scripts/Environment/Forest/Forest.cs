using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Forest : MonoBehaviour
{
    private ForestManager _forestManager;
    private ColorManager _colorManager;

    [SerializeField] int treesAmountInForest;
    [SerializeField] float averageAge;
    private int minTreesInForest = 1500;
    private int maxTreesInForest = 2500;

    [Header("Tree States")]
    public ForestType forestType;
    public ForestDensity forestDensityLevel;
    public TreeHealth forestHealth;
    public Season currentSeason;

    public float forestHeight;
    public float forestDensity;

    private Tree[] _treeArray;
    public List<Tree> treeList;

    private MeshRenderer _meshRenderer;

    // Debugging
    private bool _didUpdate;
    private int _updateCount;
    public bool hasBeenCalled;


    //--------------------

    /** Debugging */
    public void DidUpdate()
    {
        hasBeenCalled = true;
        _didUpdate = true;
        _updateCount++;
        SetColor(Color.green);
    }

    private void Awake()
    {
        _forestManager = FindObjectOfType<ForestManager>();
        _colorManager = FindObjectOfType<ColorManager>();

        _meshRenderer = gameObject.GetComponent<MeshRenderer>();

        treesAmountInForest = UnityEngine.Random.Range(minTreesInForest, maxTreesInForest);

        RaycastPosition();
    }
    
    private void Start()
    {
        SubscribeToEvents();

        SetForestType();
        MakeTreesInForestArray();

        //---

        MoveFromArrayToList();

        //---

        SetForestSeason();
        SetForestHealth();
        UpdateForestDensity();
        SetForestColor();

        UpdateTreeStats();
    }


    //--------------------


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

    public void SetColor(Color col)
    {
        _meshRenderer.materials[0].color = col;
    }

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

    void SetForestColor()
    {
        SetColor(_colorManager.GetColor(forestType, forestDensityLevel));
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
    }
    
    void UpdateForestStats()
    {
        SetForestSeason();

        SetForestHealth();

        UpdateForestDensity();
        SetForestColor();

        treesAmountInForest = treeList.Count;
        
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

        forestDensity = 0;
        forestHeight = 0;
        
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
            else i++;
        }
        
        int treeCount = treeList.Count;
        forestHeight = treeCount == 0 ? 0 : forestHeight / treeCount;
        forestDensity *= 0.0025f; // Divide by 400
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
    }
}