using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Forest : MonoBehaviour
{
    [System.Serializable]
    public class Trees
    {
        [Header("Basic Info")]
        [SerializeField] float treeAge;
        [SerializeField] float treeHeight;

        [Header("Tree States")]
        [SerializeField] string treeState_Age;
        [SerializeField] string treeState_Health;


        //-----


        #region Get/Set
        public void SetTreeAge(float a)
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

        public void SetTreeState_Age(string a)
        {
            treeState_Age = a;
        }
        public string GetTreeState_Age()
        {
            return treeState_Age;
        }

        public void SetTreeState_Health(string a)
        {
            treeState_Health = a;
        }
        public string GetTreeState_Health()
        {
            return treeState_Health;
        }
        #endregion
    };

    Trees trees;

    ForestManager treeManager;
    ColorManager colorManager;

    [SerializeField] int treesAmountInForest;

    public Trees[] treeList;

    [Header("Tree States")]
    public string forestState_Type;
    public string forestState_Health;
    public string forestState_Season;

    [Header("Health of the Forest")]
    public float forestHealth;
    float forestHealth_Counter;


    //--------------------


    private void Awake()
    {
        treeManager = FindObjectOfType<ForestManager>();
        colorManager = FindObjectOfType<ColorManager>();

        treesAmountInForest = Random.Range(300, 500);

        RaycastPosition();
        MakeTreesInForestList();

        SetForestHealth();

        SetForestType();
        SetForestColor();

        SetTreeHealth();
    }
    private void Update()
    {
        //UpdateTreeAging();
        //CheckTreeState_Age();
    }


    //--------------------


    void MakeTreesInForestList()
    {
        treeList = new Trees[treesAmountInForest];

        for (int i = 0; i < treesAmountInForest; i++)
        {
            treeList[i] = new Trees();

            //Tree age and height
            treeList[i].SetTreeAge((float)Random.Range(0f, 120f));
            treeList[i].SetTreeHeight((float)Random.Range(0f, 80f));

            //Tree Health
            int a = Random.Range(0, 3);
            if (a == 0)
            {
                treeList[i].SetTreeState_Health(TreeState_Health.treeState_Healthy.ToString());
            }
            else if (a == 1)
            {
                treeList[i].SetTreeState_Health(TreeState_Health.treeState_Damaged.ToString());
            }
            else if (a == 2)
            {
                treeList[i].SetTreeState_Health(TreeState_Health.treeState_Broken.ToString());
            }
        }
    }

    void RaycastPosition()
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
    }

    void SetTreeHealth()
    {
        for (int i = 0; i < treeList.Length; i++)
        {
            int a = Random.Range(0, 4);

            if (a == 0)
            {
                treeList[i].SetTreeState_Health(TreeState_Health.treeState_Healthy.ToString());
            }
            else if (a == 1)
            {
                treeList[i].SetTreeState_Health(TreeState_Health.treeState_Damaged.ToString());
            }
            else if (a == 2)
            {
                treeList[i].SetTreeState_Health(TreeState_Health.treeState_Broken.ToString());
            }
            else if (a == 3)
            {
                treeList[i].SetTreeState_Health(TreeState_Health.treeState_Chopped.ToString());
            }
            else
            {
                treeList[i].SetTreeState_Health(TreeState_Health.treeState_Healthy.ToString());
            }
        }
    }
    void SetTreeSeason()
    {
        forestState_Season = ForestState_Season.treeState_Spring.ToString();
    }

    void UpdateTreeAging()
    {
        for (int i = 0; i < treeList.Length; i++)
        {
            treeList[i].AddTreeAge(Time.deltaTime / 1); //Insert parameter for TIME here
        }
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

    void SetForestHealth()
    {
        forestHealth_Counter = 0;

        //Get Forest health-parameter
        #region 
        for (int i = 0; i < treeList.Length; i++)
        {
            if (treeList[i].GetTreeState_Health() == TreeState_Health.treeState_Healthy.ToString())
            {
                forestHealth_Counter += 3;
            }
            else if (treeList[i].GetTreeState_Health() == TreeState_Health.treeState_Damaged.ToString())
            {
                forestHealth_Counter += 2;
            }
            else if (treeList[i].GetTreeState_Health() == TreeState_Health.treeState_Broken.ToString())
            {
                forestHealth_Counter += 1;
            }
            else
            {
                forestHealth_Counter += 0;
            }
        }

        forestHealth = forestHealth_Counter / treeList.Length;
        #endregion

        //Calculate Forest Health State
        #region
        for (int i = 0; i < treeList.Length; i++)
        {
            if (forestHealth >= 2)
            {
                forestState_Health = TreeState_Health.treeState_Healthy.ToString();
            }
            else if (forestHealth >= 1)
            {
                forestState_Health = TreeState_Health.treeState_Damaged.ToString();
            }
            else if (forestHealth >= 0)
            {
                forestState_Health = TreeState_Health.treeState_Broken.ToString();
            }
            else
            {
                forestState_Health = TreeState_Health.treeState_Dead.ToString();
            }
        }
        #endregion
    }
    void SetForestType()
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
    void SetForestColor()
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
}

