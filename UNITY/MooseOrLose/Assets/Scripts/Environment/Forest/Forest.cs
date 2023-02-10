using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Forest : MonoBehaviour
{
    MainManager mainManager;
    ForestManager forestManager;
    ColorManager colorManager;
    TimeManager timeManager;

    [SerializeField] int treesAmountInForest;
    [SerializeField] static int minTreesInForest = 1750;
    [SerializeField] static int maxTreesInForest = 3500;

    [Header("Tree States")]
    public ForestState_Type forestState_Type;
    public ForestState_Density forestState_Density;
    public TreeState_Health forestState_Health;
    public ForestState_Season forestState_Season;

    public float forest_Height;
    public float forest_Density;

    Trees[] treeArray;
    public List<Trees> treeList;

    private bool didUpdate = false;
    private int updateCount = 0;
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

    /** Debugging */
    public void SetColor(Color col)
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].color = col;
    }


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
            treeArray[i] = new Trees
            {
                //Tree age and height
                treeState_Type = forestState_Type
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

        forestHealth_Counter = treeList.Count == 0 ? 0 : forestHealth_Counter / treeList.Count;
        //forestHealth_Counter /= treeList.Count; // Division by zero

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
        if (TimeManager.instance.IsSpring())
        {
            forestState_Season = ForestState_Season.forestSeason_Spring;
        }
        else if (TimeManager.instance.IsSummer())
        {
            forestState_Season = ForestState_Season.forestSeason_Summer;
        }
        else if (TimeManager.instance.IsAutumn())
        {
            forestState_Season = ForestState_Season.forestSeason_Fall;
        }
        else if (TimeManager.instance.IsWinter())
        {
            forestState_Season = ForestState_Season.forestSeason_Winter;
        }
        else
        {
            forestState_Season = ForestState_Season.forestSeason_Summer;
        }
    }


    void SetForestColor()
    {
        switch (forestState_Type)
        {
            case ForestState_Type.forestType_Birch:
                switch (forest_Density)
                {
                    case < 30:
                        forestState_Density = ForestState_Density.forestVolum_1;
                        break;
                    case < 40:
                        forestState_Density = ForestState_Density.forestVolum_2;
                        break;
                    case < 50:
                        forestState_Density = ForestState_Density.forestVolum_3;
                        break;
                    case < 60:
                        forestState_Density = ForestState_Density.forestVolum_4;
                        break;
                    default:
                        forestState_Density = ForestState_Density.forestVolum_5;
                        break;
                }
                break;

            case ForestState_Type.forestType_Spruce:
                switch (forest_Density)
                {
                    case < 400:
                        forestState_Density = ForestState_Density.forestVolum_1;
                        break;
                    case < 600:
                        forestState_Density = ForestState_Density.forestVolum_2;
                        break;
                    case < 800:
                        forestState_Density = ForestState_Density.forestVolum_3;
                        break;
                    case < 1000:
                        forestState_Density = ForestState_Density.forestVolum_4;
                        break;
                    default:
                        forestState_Density = ForestState_Density.forestVolum_5;
                        break;
                }
                break;

            case ForestState_Type.forestType_Pine:
                switch (forest_Density)
                {
                    case < 140:
                        forestState_Density = ForestState_Density.forestVolum_1;
                        break;
                    case < 200:
                        forestState_Density = ForestState_Density.forestVolum_2;
                        break;
                    case < 250:
                        forestState_Density = ForestState_Density.forestVolum_3;
                        break;
                    case < 300:
                        forestState_Density = ForestState_Density.forestVolum_4;
                        break;
                    default:
                        forestState_Density = ForestState_Density.forestVolum_5;
                        break;
                }
                break;
        }

        switch (forestState_Type)
        {
            case ForestState_Type.forestType_Birch:
                switch (forestState_Density)
                {
                    case ForestState_Density.forestVolum_1:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_1;
                        break;
                    case ForestState_Density.forestVolum_2:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_2;
                        break;
                    case ForestState_Density.forestVolum_3:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_3;
                        break;
                    case ForestState_Density.forestVolum_4:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_4;
                        break;
                    case ForestState_Density.forestVolum_5:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birchDensity_5;
                        break;
                }
                break;
            case ForestState_Type.forestType_Pine:
                switch (forestState_Density)
                {
                    case ForestState_Density.forestVolum_1:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_1;
                        break;
                    case ForestState_Density.forestVolum_2:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_2;
                        break;
                    case ForestState_Density.forestVolum_3:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_3;
                        break;
                    case ForestState_Density.forestVolum_4:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_4;
                        break;
                    case ForestState_Density.forestVolum_5:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pineDensity_5;
                        break;
                }
                break;
            case ForestState_Type.forestType_Spruce:
                switch (forestState_Density)
                {
                    case ForestState_Density.forestVolum_1:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_1;
                        break;
                    case ForestState_Density.forestVolum_2:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_2;
                        break;
                    case ForestState_Density.forestVolum_3:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_3;
                        break;
                    case ForestState_Density.forestVolum_4:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_4;
                        break;
                    case ForestState_Density.forestVolum_5:
                        gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruceDensity_5;
                        break;
                }
                break;
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

        //print("Found no edible trees in this Forest");

        return null;
    }
    #endregion


    //--------------------


    void SubscribeToEvents()
    {
        //TimeManager.instance.OnNewDay += UpdateTreeStats;

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

        forest_Height = !treeList.Any() ? 0 : forest_Height / treeList.Count();
        //forest_Height /= treeList.Count; // Possible division by zero
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