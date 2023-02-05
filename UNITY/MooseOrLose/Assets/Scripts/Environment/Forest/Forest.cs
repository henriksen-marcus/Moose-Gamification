using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;

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
    public ForestState_Density forestState_Density;
    public TreeState_Health forestState_Health;
    public ForestState_Season forestState_Season;

    public float forest_Height;
    public float forest_Density;

    Trees[] treeArray;
    public List<Trees> treeList;


    //--------------------


    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
        forestManager = FindObjectOfType<ForestManager>();
        colorManager = FindObjectOfType<ColorManager>();

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
            treeArray[i] = new Trees();

            //Tree age and height
            treeArray[i].treeState_Type = forestState_Type;

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


    void SetForestType()
    {
        if (gameObject.transform.position.y >= forestManager.BirchForestSpawn.x && gameObject.transform.position.y <= forestManager.BirchForestSpawn.y)
        {
            forestState_Type = ForestState_Type.forestType_Birch;
        }
        else if (gameObject.transform.position.y >= forestManager.PineForestSpawn.x && gameObject.transform.position.y <= forestManager.PineForestSpawn.y)
        {
            forestState_Type = ForestState_Type.forestType_Pine;
        }
        else if (gameObject.transform.position.y >= forestManager.SpruceForestSpawn.x && gameObject.transform.position.y <= forestManager.SpruceForestSpawn.y)
        {
            forestState_Type = ForestState_Type.forestType_Spruce;
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
        for (int i = 0; i < treeList.Count; i++)
        {
            forestHealth_Counter += (int)treeList[i].treeState_Health;
        }

        forestHealth_Counter /= treeList.Count;

        //Calculate Forest Health State
        #region
        if (forestHealth_Counter >= 2)
        {
            forestState_Health = TreeState_Health.treeHealth_Healthy;
        }
        else if (forestHealth_Counter >= 1)
        {
            forestState_Health = TreeState_Health.treeHealth_Damaged;
        }
        else if (forestHealth_Counter >= 0)
        {
            forestState_Health = TreeState_Health.treeHealth_Broken;
        }
        else
        {
            forestState_Health = TreeState_Health.treeHealth_Dead;
        }
        #endregion
    }
    void SetForestSeason()
    {
        if (timeManager.IsSpring())
        {
            forestState_Season = ForestState_Season.forestSeason_Spring;
        }
        else if (timeManager.IsSummer())
        {
            forestState_Season = ForestState_Season.forestSeason_Summer;
        }
        else if (timeManager.IsAutumn())
        {
            forestState_Season = ForestState_Season.forestSeason_Fall;
        }
        else if (timeManager.IsWinter())
        {
            forestState_Season = ForestState_Season.forestSeason_Winter;
        }
    }
    void SetForestColor()
    {
        if (forestState_Type == ForestState_Type.forestType_Birch)
        {
            if (forest_Density < 30)
            {
                forestState_Density = ForestState_Density.forestVolum_1;
            }
            else if (forest_Density < 40)
            {
                forestState_Density = ForestState_Density.forestVolum_2;
            }
            else if (forest_Density < 50)
            {
                forestState_Density = ForestState_Density.forestVolum_3;
            }
            else if (forest_Density < 60)
            {
                forestState_Density = ForestState_Density.forestVolum_4;
            }
            else
            {
                forestState_Density = ForestState_Density.forestVolum_5;
            }
        }
        else if (forestState_Type == ForestState_Type.forestType_Spruce)
        {
            if (forest_Density < 400)
            {
                forestState_Density = ForestState_Density.forestVolum_1;
            }
            else if (forest_Density < 600)
            {
                forestState_Density = ForestState_Density.forestVolum_2;
            }
            else if (forest_Density < 800)
            {
                forestState_Density = ForestState_Density.forestVolum_3;
            }
            else if (forest_Density < 1000)
            {
                forestState_Density = ForestState_Density.forestVolum_4;
            }
            else
            {
                forestState_Density = ForestState_Density.forestVolum_5;
            }
        }
        else if (forestState_Type == ForestState_Type.forestType_Pine)
        {
            if (forest_Density < 140)
            {
                forestState_Density = ForestState_Density.forestVolum_1;
            }
            else if (forest_Density < 200)
            {
                forestState_Density = ForestState_Density.forestVolum_2;
            }
            else if (forest_Density < 250)
            {
                forestState_Density = ForestState_Density.forestVolum_3;
            }
            else if (forest_Density < 300)
            {
                forestState_Density = ForestState_Density.forestVolum_4;
            }
            else
            {
                forestState_Density = ForestState_Density.forestVolum_5;
            }
        }

        if (forestState_Type == ForestState_Type.forestType_Birch)
        {
            if (forestState_Density == ForestState_Density.forestVolum_1)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_1;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_2)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_2;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_3)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_3;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_4)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_4;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_5)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_5;
            }
        }
        else if (forestState_Type == ForestState_Type.forestType_Pine)
        {
            if (forestState_Density == ForestState_Density.forestVolum_1)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_1;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_2)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_2;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_3)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_3;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_4)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_4;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_5)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_5;
            }
        }
        else if (forestState_Type == ForestState_Type.forestType_Spruce)
        {
            if (forestState_Density == ForestState_Density.forestVolum_1)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_1;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_2)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_2;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_3)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_3;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_4)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_4;
            }
            else if (forestState_Density == ForestState_Density.forestVolum_5)
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_5;
            }
        }
    }


    //--------------------


    #region Functions for a Moose to call
    public int GetForestTreeAmount()
    {
        return treeList.Count;
    }

    public Trees GetATree(int a)
    {
        return treeList[a];
    }

    public Trees GetOptimalTreeToEat()
    {
        //Search for Healthy trees under 3 meter
        for (int i = 0; i < treeList.Count; i++)
        {
            if (treeList[i].treeState_Health == TreeState_Health.treeHealth_Healthy && treeList[i].stemHeight < 3)
            {
                return treeList[i];
            }
        }

        //Search for Damaged trees under 3 meter
        for (int i = 0; i < treeList.Count; i++)
        {
            if (treeList[i].treeState_Health == TreeState_Health.treeHealth_Damaged && treeList[i].stemHeight < 3)
            {
                return treeList[i];
            }
        }

        //Search for Broken trees under 3 meter
        for (int i = 0; i < treeList.Count; i++)
        {
            if (treeList[i].treeState_Health == TreeState_Health.treeHealth_Broken && treeList[i].stemHeight < 3)
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

        TimeManager.instance.OnNewYear += UpdateBirth;
    }
    void UpdateForestStats()
    {
        SetForestSeason();

        SetForestHealth();

        SetForestColor();
    }
    void UpdateTreeStats()
    {
        forest_Density = 0;
        forest_Height = 0;

        for (int i = 0; i < treeList.Count;)
        {
            //Update Tree Stats
            treeList[i].UpdateStats();

            //Update Forest Density
            forest_Density += treeList[i].treeVolum;

            //Update Forest Height
            forest_Height += treeList[i].treeHeight;

            //Check for dead trees
            if (treeList[i].isDead)
            {
                forestManager.treesDiedOfAge++;

                treeList.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        forest_Height /= treeList.Count;
        forest_Density /= 1000;
    }

    void UpdateBirth()
    {
        for (int i = 0; i < treeList.Count; i++)
        {
            for (int j = 0; j < treeList[i].CheckIfGettingBirth(); j++)
            {
                treeList.Add(new Trees());
                treeList[treeList.Count - 1].treeState_Type = forestState_Type;
                treeList[treeList.Count - 1].SetBirth();
            }

            forestManager.treesBirth += treeList[i].CheckIfGettingBirth();
        }
    }
}