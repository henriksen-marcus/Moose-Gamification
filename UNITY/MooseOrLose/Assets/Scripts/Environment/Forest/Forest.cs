using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Forest : MonoBehaviour
{
    [System.Serializable]
    public class Trees
    {
        [Header("Basic Info")]
        [SerializeField] float treeAge;

        [Header("Tree States")]
        [SerializeField] string treeState_Age;
        [SerializeField] TreeState_Health treeState_Health;

        [Header("Properties")]
        [SerializeField] float treeHeight;
        [SerializeField] float treeDiameter;
        [SerializeField] float treeVolum;
        [SerializeField] float stemHeight;
        [SerializeField] float budSize;

        [Header("Damages")]
        [SerializeField] float barkDamage;
        [SerializeField] float budDamage;
        [SerializeField] float branchDamage;

        [Header("Food")]
        [SerializeField] float foodAttached;

        [Header("Genes")]
        public float growthRate_Height = 0.1f;
        public float growthRate_Diameter = 0.01f;
        [SerializeField] float darknessTolerance;
        [SerializeField] float soilWaterDrinkability;


        //-----


        #region Get/Set
        public void SetTreeAge(int a)
        {
            treeAge = a;
        }
        public float GetTreeAge()
        {
            return treeAge;
        }
        public void AddTreeAge(float a)
        {
            treeAge += a;
        }

        public void SetTreeHeight(float a)
        {
            treeHeight = a;
        }
        public float GetTreeHeight()
        {
            return treeHeight;
        }
        public void AddTreeHeight(float a)
        {
            treeHeight += a;
        }

        public void SetTreeDiameter(float a)
        {
            treeDiameter = a;
        }
        public float GetTreeDiameter()
        {
            return treeDiameter;
        }
        public void AddTreeDiameter(float a)
        {
            treeDiameter += a;
        }

        public void SetTreeState_Age(string a)
        {
            treeState_Age = a;
        }
        public string GetTreeState_Age()
        {
            return treeState_Age;
        }

        public void SetTreeState_Health(int a)
        {
            treeState_Health = (TreeState_Health)a;
        }
        public int GetTreeState_Health()
        {
            return (int)treeState_Health;
        }
        #endregion

        public void EatFromTree()
        {
            if (GetTreeState_Health() == (int)TreeState_Health.treeState_Healthy)
            {
                SetTreeState_Health((int)TreeState_Health.treeState_Damaged);
            }
            else if (GetTreeState_Health() == (int)TreeState_Health.treeState_Damaged)
            {
                SetTreeState_Health((int)TreeState_Health.treeState_Broken);
            }
            else if (GetTreeState_Health() == (int)TreeState_Health.treeState_Broken)
            {
                SetTreeState_Health((int)TreeState_Health.treeState_Chopped);
            }
            else
            {
                print("There are no more food to get from this tree");
            }
        }
    };

    Trees trees;

    ForestManager treeManager;
    ColorManager colorManager;

    [SerializeField] int treesAmountInForest;
    [SerializeField] int minTreesInForest = 100;
    [SerializeField] int maxTreesInForest = 1000;

    public Trees[] treeList;

    [Header("Tree States")]
    public string forestState_Type;
    public string forestState_Health;
    public string forestState_Season;
    public float forest_Height;
    public float forest_Density;


    //--------------------


    private void Awake()
    {
        treeManager = FindObjectOfType<ForestManager>();
        colorManager = FindObjectOfType<ColorManager>();

        treesAmountInForest = Random.Range(minTreesInForest, maxTreesInForest);

        RaycastPosition();
        MakeTreesInForestList();

        SetTreeHealth();
        SetForestHealth();

        SetForestType();
        SetForestColor();

    }
    private void Start()
    {
        SubscribeToEvents();
    }


    //--------------------


    void MakeTreesInForestList()
    {
        treeList = new Trees[treesAmountInForest];

        for (int i = 0; i < treesAmountInForest; i++)
        {
            treeList[i] = new Trees();

            //Tree age and height
            treeList[i].SetTreeAge(Random.Range(0, 120));
            treeList[i].SetTreeHeight((float)Random.Range(0f, 60f));
            treeList[i].SetTreeDiameter((float)Random.Range(0.01f, 1.5f));

            //Tree Health
            int a = Random.Range(0, 3);
            if (a == 0)
            {
                treeList[i].SetTreeState_Health((int)TreeState_Health.treeState_Healthy);
            }
            else if (a == 1)
            {
                treeList[i].SetTreeState_Health((int)TreeState_Health.treeState_Damaged);
            }
            else if (a == 2)
            {
                treeList[i].SetTreeState_Health((int)TreeState_Health.treeState_Broken);
            }
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

    void SetTreeHealth()
    {
        for (int i = 0; i < treeList.Length; i++)
        {
            int a = Random.Range(0, 80);

            if (a >= 30)
            {
                treeList[i].SetTreeState_Health((int)TreeState_Health.treeState_Healthy);
            }
            else if (a >= 50)
            {
                treeList[i].SetTreeState_Health((int)TreeState_Health.treeState_Damaged);
            }
            else if (a >= 65)
            {
                treeList[i].SetTreeState_Health((int)TreeState_Health.treeState_Broken);
            }
            else if (a >= 75)
            {
                treeList[i].SetTreeState_Health((int)TreeState_Health.treeState_Chopped);
            }
            else
            {
                treeList[i].SetTreeState_Health((int)TreeState_Health.treeState_Healthy);
            }
        }
    }
    void SetTreeSeason()
    {
        forestState_Season = ForestState_Season.treeState_Spring.ToString();
    }

    void CheckTreeState_Age()
    {
        for (int i = 0; i < treeList.Length; i++)
        {
            if (treeList[i].GetTreeAge() <= 20)
            {
                treeList[i].SetTreeState_Age(TreeState_Age.treeState_Child.ToString());
            }
            else if(treeList[i].GetTreeAge() <= 80)
            {
                treeList[i].SetTreeState_Age(TreeState_Age.treeState_Adult.ToString());
            }
            else if (treeList[i].GetTreeAge() <= 120)
            {
                treeList[i].SetTreeState_Age(TreeState_Age.treeState_Old.ToString());
            }
            else if (treeList[i].GetTreeAge() <= 150)
            {
                treeList[i].SetTreeState_Age(TreeState_Health.treeState_Dead.ToString());
            }
            else
            {
                treeList[i].SetTreeAge(0);
            }
        }
    }

    public void SetForestHealth()
    {
        int forestHealth_Counter = 0;

        //Get the Health State of a Tree
        for (int i = 0; i < treeList.Length; i++)
        {
            forestHealth_Counter += treeList[i].GetTreeState_Health();
        }

        forestHealth_Counter /= treeList.Length;

        //Calculate Forest Health State
        #region
        if (forestHealth_Counter >= 2)
        {
            forestState_Health = TreeState_Health.treeState_Healthy.ToString();
        }
        else if (forestHealth_Counter >= 1)
        {
            forestState_Health = TreeState_Health.treeState_Damaged.ToString();
        }
        else if (forestHealth_Counter >= 0)
        {
            forestState_Health = TreeState_Health.treeState_Broken.ToString();
        }
        else
        {
            forestState_Health = TreeState_Health.treeState_Dead.ToString();
        }
        #endregion
    }
    public void SetForestType()
    {
        if (gameObject.transform.position.y >= treeManager.BirchForestSpawn.x && gameObject.transform.position.y <= treeManager.BirchForestSpawn.y)
        {
            forestState_Type = ForestState_Type.treeState_Birch.ToString();
        }
        else if (gameObject.transform.position.y >= treeManager.PineForestSpawn.x && gameObject.transform.position.y <= treeManager.PineForestSpawn.y)
        {
            forestState_Type = ForestState_Type.treeState_Pine.ToString();
        }
        else if (gameObject.transform.position.y >= treeManager.SpruceForestSpawn.x && gameObject.transform.position.y <= treeManager.SpruceForestSpawn.y)
        {
            forestState_Type = ForestState_Type.treeState_Spruce.ToString();
        }
        else
        {
            forestState_Type = ForestState_Type.none.ToString();
        }
    }
    public void SetForestColor()
    {
        if (forestState_Type == ForestState_Type.treeState_Birch.ToString())
        {
            if (forestState_Health == TreeState_Health.treeState_Healthy.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Healthy;
            }
            else if (forestState_Health == TreeState_Health.treeState_Damaged.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Damaged;
            }
            else if (forestState_Health == TreeState_Health.treeState_Broken.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Broken;
            }
            else if (forestState_Health == TreeState_Health.treeState_Dead.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Dead;
            }
            else if (forestState_Health == TreeState_Health.treeState_Chopped.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.birch_Chopped;
            }
        }
        else if (forestState_Type == ForestState_Type.treeState_Pine.ToString())
        {
            if (forestState_Health == TreeState_Health.treeState_Healthy.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Healthy;
            }
            else if (forestState_Health == TreeState_Health.treeState_Damaged.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Damaged;
            }
            else if (forestState_Health == TreeState_Health.treeState_Broken.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Broken;
            }
            else if (forestState_Health == TreeState_Health.treeState_Dead.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Dead;
            }
            else if (forestState_Health == TreeState_Health.treeState_Chopped.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.pine_Chopped;
            }
        }
        else if (forestState_Type == ForestState_Type.treeState_Spruce.ToString())
        {
            if (forestState_Health == TreeState_Health.treeState_Healthy.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Healthy;
            }
            else if (forestState_Health == TreeState_Health.treeState_Damaged.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Damaged;
            }
            else if (forestState_Health == TreeState_Health.treeState_Broken.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Broken;
            }
            else if (forestState_Health == TreeState_Health.treeState_Dead.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Dead;
            }
            else if (forestState_Health == TreeState_Health.treeState_Chopped.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().materials[0].color = colorManager.spruce_Chopped;
            }
        }
    }

    void UpdateTreeStats()
    {
        float heightCounter = 0;
        int forestHealth_Counter = 0;

        for (int i = 0; i < treeList.Length; i++)
        {
            treeList[i].AddTreeAge(1);

            heightCounter += treeList[i].GetTreeHeight();

            forestHealth_Counter += treeList[i].GetTreeState_Health();

            treeList[i].AddTreeHeight(treeList[i].growthRate_Height);
            treeList[i].AddTreeDiameter(treeList[i].growthRate_Diameter);
        }

        //Forest Height
        forest_Height = heightCounter / treeList.Length;

        //Forest Density
        forest_Density = (float)treeList.Length / 1000f;

        //Calculate Forest Health State
        #region 
        forestHealth_Counter /= treeList.Length;

        if (forestHealth_Counter <= 1)
        {
            forestState_Health = TreeState_Health.treeState_Healthy.ToString();
        }
        else if (forestHealth_Counter <= 2)
        {
            forestState_Health = TreeState_Health.treeState_Damaged.ToString();
        }
        else if (forestHealth_Counter <= 3)
        {
            forestState_Health = TreeState_Health.treeState_Broken.ToString();
        }
        else if (forestHealth_Counter <= 4)
        {
            forestState_Health = TreeState_Health.treeState_Dead.ToString();
        }
        else
        {
            forestState_Health = TreeState_Health.treeState_Chopped.ToString();
        }
        #endregion
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
        //Search for Healthy trees
        for (int i = 0; i < treeList.Length; i++)
        {
            if (treeList[i].GetTreeState_Health() == (int)TreeState_Health.treeState_Healthy)
            {
                return treeList[i];
            }
        }

        //Search for Damaged trees
        for (int i = 0; i < treeList.Length; i++)
        {
            if (treeList[i].GetTreeState_Health() == (int)TreeState_Health.treeState_Damaged)
            {
                return treeList[i];
            }
        }

        //Search for Broken trees
        for (int i = 0; i < treeList.Length; i++)
        {
            if (treeList[i].GetTreeState_Health() == (int)TreeState_Health.treeState_Broken)
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
        TimeManager.instance.OnNewDay += SetForestColor;
    }
}

