using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Forest : MonoBehaviour
{
    MainManager mainManager;
    ForestManager forestManager;
    ColorManager colorManager;
    TimeManager timeManager;

    [SerializeField] int treesAmountInForest;
    [SerializeField] int minTreesInForest = 1750;
    [SerializeField] int maxTreesInForest = 3500;

    [Header("Tree States")]
    public ForestType forestType;
    public ForestDensity forestDensity;
    public TreeHealth forestHealth;
    public Season currentSeason;

    public float forest_Height;
    public float forest_Density;

    Trees[] treeArray;
    public List<Trees> treeList;

    private MeshRenderer meshRenderer;

    // Debugging
    private bool didUpdate;
    private int updateCount;
    public bool hasBeenCalled;


    //--------------------

    /** Debugging */
    public void DidUpdate()
    {
        hasBeenCalled = true;
        didUpdate = true;
        updateCount++;
        SetColor(Color.green);
    }

    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
        forestManager = FindObjectOfType<ForestManager>();
        colorManager = FindObjectOfType<ColorManager>();

        meshRenderer = gameObject.GetComponent<MeshRenderer>();

        treesAmountInForest = UnityEngine.Random.Range(minTreesInForest, maxTreesInForest);

        RaycastPosition();
    }
    
    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();

        SubscribeToEvents();

        SetForestType();
        MakeTreesInForestArray();

        //---

        MoveFromArrayToList();

        //---

        SetForestSeason();
        SetForestHealth();
        SetForestColor();

        UpdateTreeStats();
    }


    //--------------------


    void MakeTreesInForestArray()
    {
        treeArray = new Trees[treesAmountInForest];

        for (int i = 0; i < treesAmountInForest; i++)
        {
            treeArray[i] = new Trees
            {
                //Tree age and height
                treeType = forestType
            };
            treeArray[i].SetStats();
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
        treeList = new List<Trees>(treeArray);

        treeArray = null;
    }


    //--------------------

    public void SetColor(Color col)
    {
        meshRenderer.materials[0].color = col;
    }

    void SetForestType()
    {
        if (gameObject.transform.position.y >= forestManager.BirchForestSpawn.x && gameObject.transform.position.y <= forestManager.BirchForestSpawn.y)
        {
            forestType = ForestType.Birch;
        }
        else if (gameObject.transform.position.y >= forestManager.PineForestSpawn.x && gameObject.transform.position.y <= forestManager.PineForestSpawn.y)
        {
            forestType = ForestType.Pine;
        }
        else if (gameObject.transform.position.y >= forestManager.SpruceForestSpawn.x && gameObject.transform.position.y <= forestManager.SpruceForestSpawn.y)
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
        forestDensity = forestType switch
        {
            ForestType.Birch => forest_Density switch
            {
                < 30 => ForestDensity.Density1,
                < 40 => ForestDensity.Density2,
                < 50 => ForestDensity.Density3,
                < 60 => ForestDensity.Density4,
                _ => ForestDensity.Density5
            },
            ForestType.Spruce => forest_Density switch
            {
                < 400 => ForestDensity.Density1,
                < 600 => ForestDensity.Density2,
                < 800 => ForestDensity.Density3,
                < 1000 => ForestDensity.Density4,
                _ => ForestDensity.Density5
            },
            ForestType.Pine => forest_Density switch
            {
                < 140 => ForestDensity.Density1,
                < 200 => ForestDensity.Density2,
                < 250 => ForestDensity.Density3,
                < 300 => ForestDensity.Density4,
                _ => ForestDensity.Density5
            },
            _ => ForestDensity.None
        };
        
        SetColor(colorManager.GetColor(forestType, forestDensity));
    }


    //--------------------


    #region Functions for a Moose to call
    public int GetForestTreeAmount() { return treeList.Count; }

    public Trees GetATree(int a) { return treeList[a]; }

    public Trees GetOptimalTreeToEat()
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
        TimeManager.instance.OnNewYear += UpdateBirth;
    }
    
    void UpdateForestStats()
    {
        SetForestSeason();

        SetForestHealth();

        SetForestColor();
    }

    public void UpdateTreeStats()
    {
        UpdateForestStats();

        forest_Density = 0;
        forest_Height = 0;
        
        for (int i = 0; i < treeList.Count;)
        {
            treeList[i].UpdateStats();
            
            forest_Density += treeList[i].treeVolume;
            forest_Height += treeList[i].treeHeight;

            // Check for dead trees
            if (treeList[i].isDead)
            {
                forestManager.treesDiedOfAge++;
                treeList.RemoveAt(i);
            }
            else i++;
        }
        
        int treeCount = treeList.Count;
        forest_Height = treeCount == 0 ? 0 : forest_Height / treeCount;
        forest_Density *= 0.001f; // Divide by 1000
    }

    void UpdateBirth()
    {
        for (int i = 0; i < treeList.Count; i++)
        {
            for (int j = 0; j < treeList[i].CheckIfGettingBirth(); j++)
            {
                treeList.Add(new Trees());
                treeList[treeList.Count - 1].treeType = forestType;
                treeList[treeList.Count - 1].SetBirth();
            }

            forestManager.treesBirth += treeList[i].CheckIfGettingBirth();
        }
    }
}