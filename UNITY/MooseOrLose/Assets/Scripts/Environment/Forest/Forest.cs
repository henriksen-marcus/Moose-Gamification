using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public ForestState_Type forestState_Type;
    public TreeState_Health forestState_Health;
    public ForestState_Season forestState_Season;

    public float forest_Height;
    public float forest_Density;

    public Trees[] treeList;


    //--------------------


    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
        forestManager = FindObjectOfType<ForestManager>();
        colorManager = FindObjectOfType<ColorManager>();

        treesAmountInForest = Random.Range(minTreesInForest, maxTreesInForest);

        RaycastPosition();
    }
    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();

        SubscribeToEvents();

        SetForestType();
        MakeTreesInForestList();

        SetForestSeason();
        SetForestHealth();
        SetForestColor();
    }


    //--------------------


    void MakeTreesInForestList()
    {
        treeList = new Trees[treesAmountInForest];

        for (int i = 0; i < treesAmountInForest; i++)
        {
            treeList[i] = new Trees();

            //Tree age and height
            treeList[i].treeState_Type = forestState_Type;

            treeList[i].SetStats();
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


    //--------------------


    void SetForestType()
    {
        if (gameObject.transform.position.y >= forestManager.BirchForestSpawn.x && gameObject.transform.position.y <= forestManager.BirchForestSpawn.y)
        {
            forestState_Type = ForestState_Type.treeState_Birch;
        }
        else if (gameObject.transform.position.y >= forestManager.PineForestSpawn.x && gameObject.transform.position.y <= forestManager.PineForestSpawn.y)
        {
            forestState_Type = ForestState_Type.treeState_Pine;
        }
        else if (gameObject.transform.position.y >= forestManager.SpruceForestSpawn.x && gameObject.transform.position.y <= forestManager.SpruceForestSpawn.y)
        {
            forestState_Type = ForestState_Type.treeState_Spruce;
        }
        else
        {
            forestState_Type = ForestState_Type.none;
        }
    }
    void SetForestHealth()
    {
        int forestHealth_Counter = 0;

        //Get the Health State of a Tree
        for (int i = 0; i < treeList.Length; i++)
        {
            forestHealth_Counter += (int)treeList[i].treeState_Health;
        }

        forestHealth_Counter /= treeList.Length;

        //Calculate Forest Health State
        #region
        if (forestHealth_Counter >= 2)
        {
            forestState_Health = TreeState_Health.treeState_Healthy;
        }
        else if (forestHealth_Counter >= 1)
        {
            forestState_Health = TreeState_Health.treeState_Damaged;
        }
        else if (forestHealth_Counter >= 0)
        {
            forestState_Health = TreeState_Health.treeState_Broken;
        }
        else
        {
            forestState_Health = TreeState_Health.treeState_Dead;
        }
        #endregion
    }
    void SetForestSeason()
    {
        if (timeManager.IsSpring())
        {
            forestState_Season = ForestState_Season.treeState_Spring;
        }
        else if (timeManager.IsSummer())
        {
            forestState_Season = ForestState_Season.treeState_Summer;
        }
        else if (timeManager.IsAutumn())
        {
            forestState_Season = ForestState_Season.treeState_Fall;
        }
        else if (timeManager.IsWinter())
        {
            forestState_Season = ForestState_Season.treeState_Winter;
        }
    }
    void SetForestColor()
    {
        if (forestState_Type == ForestState_Type.treeState_Birch)
        {
            if (forestState_Health == TreeState_Health.treeState_Healthy)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Healthy;
            }
            else if (forestState_Health == TreeState_Health.treeState_Damaged)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Damaged;
            }
            else if (forestState_Health == TreeState_Health.treeState_Broken)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Broken;
            }
            else if (forestState_Health == TreeState_Health.treeState_Dead)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Dead;
            }
            else if (forestState_Health == TreeState_Health.treeState_Chopped)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Chopped;
            }
        }
        else if (forestState_Type == ForestState_Type.treeState_Pine)
        {
            if (forestState_Health == TreeState_Health.treeState_Healthy)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Healthy;
            }
            else if (forestState_Health == TreeState_Health.treeState_Damaged)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Damaged;
            }
            else if (forestState_Health == TreeState_Health.treeState_Broken)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Broken;
            }
            else if (forestState_Health == TreeState_Health.treeState_Dead)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Dead;
            }
            else if (forestState_Health == TreeState_Health.treeState_Chopped)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Chopped;
            }
        }
        else if (forestState_Type == ForestState_Type.treeState_Spruce)
        {
            if (forestState_Health == TreeState_Health.treeState_Healthy)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Healthy;
            }
            else if (forestState_Health == TreeState_Health.treeState_Damaged)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Damaged;
            }
            else if (forestState_Health == TreeState_Health.treeState_Broken)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Broken;
            }
            else if (forestState_Health == TreeState_Health.treeState_Dead)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Dead;
            }
            else if (forestState_Health == TreeState_Health.treeState_Chopped)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Chopped;
            }
        }
    }


    //--------------------


    #region Functions for a Moose to call
    public int GetForestTreeAmount()
    {
        return treeList.Length;
    }

    public Trees GetATree(int a)
    {
        return treeList[a];
    }

    public Trees GetOptimalTreeToEat()
    {
        //Search for Healthy trees under 3 meter
        for (int i = 0; i < treeList.Length; i++)
        {
            if (treeList[i].treeState_Health == TreeState_Health.treeState_Healthy && treeList[i].stemHeight < 3)
            {
                return treeList[i];
            }
        }

        //Search for Damaged trees under 3 meter
        for (int i = 0; i < treeList.Length; i++)
        {
            if (treeList[i].treeState_Health == TreeState_Health.treeState_Damaged && treeList[i].stemHeight < 3)
            {
                return treeList[i];
            }
        }

        //Search for Broken trees under 3 meter
        for (int i = 0; i < treeList.Length; i++)
        {
            if (treeList[i].treeState_Health == TreeState_Health.treeState_Broken && treeList[i].stemHeight < 3)
            {
                return treeList[i];
            }
        }

        print("Found no edible trees in this Forest");

        return null;
    }
    #endregion


    //--------------------


    void SubscribeToEvents()
    {
        TimeManager.instance.OnNewDay += UpdateTreeStats;
        TimeManager.instance.OnNewDay += UpdateForestStats;
    }
    void UpdateForestStats()
    {
        SetForestSeason();

        SetForestHealth();

        SetForestColor();
    }
    void UpdateTreeStats()
    {
        for (int i = 0; i < treeList.Length; i++)
        {
            treeList[i].UpdateStats();

            forestManager.totalTreesAmount += treeList.Length;

            if (treeList[i].isDead)
            {
                treeList[i].isDead = false;
                forestManager.treesDiedOfAge++;
            }
        }
    }
}

