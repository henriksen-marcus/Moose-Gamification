using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    TreeManager treeManager;
    ColorManager colorManager;

    [Header("Tree Age")]
    [SerializeField] float treeAge;

    [Header("Tree States")]
    [SerializeField] string treeState_Type;
    [SerializeField] string treeState_Age;
    [SerializeField] string treeState_Health;
    [SerializeField] string treeState_Season;

    float healthCounter;


    //--------------------


    private void Awake()
    {
        treeManager = FindObjectOfType<TreeManager>();
        colorManager = FindObjectOfType<ColorManager>();

        treeAge = (float)Random.Range(0f, 120f);

        RaycastPosition();

        UpdateTreeAging();
        SetTreeHealth();
        SetTreeSeason();

        SetTreeType();
        SetTreeColor();
    }

    private void Update()
    {
        //Update growth 

        UpdateTreeAging();
        UpdateTreehealth();
        SetTreeColor();
        //SetTreeColor();
    }


    //--------------------


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

    void UpdateTreeAging()
    {
        treeAge += Time.deltaTime / 1; //Insert parameter for TIME here

        if (treeAge <= 20)
        {
            gameObject.transform.localScale = new Vector3(0.075f, 0.08f, 0.075f);

            treeState_Age = TreeState_Age.treeState_Child.ToString();
        }
        else if (treeAge <= 80)
        {
            gameObject.transform.localScale = new Vector3(0.15f, 0.08f, 0.15f);

            treeState_Age = TreeState_Age.treeState_Adult.ToString();
        }
        else if (treeAge <= 120)
        {
            gameObject.transform.localScale = new Vector3(0.2f, 0.08f, 0.2f);

            treeState_Age = TreeState_Age.treeState_Old.ToString();
        }
        else if (treeAge <= 150)
        {
            gameObject.transform.localScale = new Vector3(0.2f, 0.08f, 0.2f);
            //gameObject.transform.localRotation = Quaternion.Euler(-90, 0, 0);

            treeState_Age = TreeState_Health.treeState_Dead.ToString();
        }
        else
        {
            //Remove tree from map
            for (int i = 0; i < treeManager.treeSpawnerList.Count; i++)
            {
                if (treeManager.treeSpawnerList[i].GetComponent<Tree>().gameObject == gameObject)
                {
                    treeManager.treeSpawnerList.RemoveAt(i);
                    Destroy(gameObject);
                }
            }
        }
    }
    void SetTreeHealth()
    {
        int a = Random.Range(0, 4);
        if (a == 0)
        {
            treeState_Health = TreeState_Health.treeState_Healthy.ToString();
        }
        else if (a == 1)
        {
            treeState_Health = TreeState_Health.treeState_Damaged.ToString();
        }
        else if (a == 2)
        {
            treeState_Health = TreeState_Health.treeState_Broken.ToString();
        }
        else if (a == 3)
        {
            treeState_Health = TreeState_Health.treeState_Chopped.ToString();
        }
        else
        {
            treeState_Health = TreeState_Health.treeState_Healthy.ToString();
        }
    }
    void UpdateTreehealth()
    {
        healthCounter += Time.deltaTime; 

        if(healthCounter >= Random.Range(0, 500))
        {
            int a = Random.Range(0, 4);
            if (a == 0)
            {
                treeState_Health = TreeState_Health.treeState_Healthy.ToString();
            }
            else if (a == 1)
            {
                treeState_Health = TreeState_Health.treeState_Damaged.ToString();
            }
            else if (a == 2)
            {
                treeState_Health = TreeState_Health.treeState_Broken.ToString();
            }
            else if (a == 3)
            {
                treeState_Health = TreeState_Health.treeState_Chopped.ToString();
            }
            else
            {
                treeState_Health = TreeState_Health.treeState_Healthy.ToString();
            }

            healthCounter = 0;
        }
    }
    void SetTreeSeason()
    {
        treeState_Season = TreeState_Season.treeState_Spring.ToString();
    }

    void SetTreeType()
    {
        if (gameObject.transform.position.y >= treeManager.BirchSpawn.x && gameObject.transform.position.y <= treeManager.BirchSpawn.y)
        {
            treeState_Type = TreeState_Type.treeState_Birch.ToString();
        }
        else if (gameObject.transform.position.y >= treeManager.PineSpawn.x && gameObject.transform.position.y <= treeManager.PineSpawn.y)
        {
            treeState_Type = TreeState_Type.treeState_Pine.ToString();
        }
        else if (gameObject.transform.position.y >= treeManager.SpruceSpawn.x && gameObject.transform.position.y <= treeManager.SpruceSpawn.y)
        {
            treeState_Type = TreeState_Type.treeState_Spruce.ToString();
        }
        else
        {
            treeState_Type = TreeState_Type.none.ToString();
        }
    }
    void SetTreeColor()
    {
        if (treeState_Type == TreeState_Type.treeState_Birch.ToString())
        {
            if (treeState_Health == TreeState_Health.treeState_Healthy.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.birch_Healthy;
            }
            else if (treeState_Health == TreeState_Health.treeState_Damaged.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.birch_Damaged;
            }
            else if (treeState_Health == TreeState_Health.treeState_Broken.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.birch_Broken;
            }
            else if (treeState_Health == TreeState_Health.treeState_Dead.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.birch_Dead;
            }
            else if (treeState_Health == TreeState_Health.treeState_Chopped.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.birch_Chopped;
            }
        }
        else if (treeState_Type == TreeState_Type.treeState_Pine.ToString())
        {
            if (treeState_Health == TreeState_Health.treeState_Healthy.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.pine_Healthy;
            }
            else if (treeState_Health == TreeState_Health.treeState_Damaged.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.pine_Damaged;
            }
            else if (treeState_Health == TreeState_Health.treeState_Broken.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.pine_Broken;
            }
            else if (treeState_Health == TreeState_Health.treeState_Dead.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.pine_Dead;
            }
            else if (treeState_Health == TreeState_Health.treeState_Chopped.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.pine_Chopped;
            }
        }
        else if (treeState_Type == TreeState_Type.treeState_Spruce.ToString())
        {
            if (treeState_Health == TreeState_Health.treeState_Healthy.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.spruce_Healthy;
            }
            else if (treeState_Health == TreeState_Health.treeState_Damaged.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.spruce_Damaged;
            }
            else if (treeState_Health == TreeState_Health.treeState_Broken.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.spruce_Broken;
            }
            else if (treeState_Health == TreeState_Health.treeState_Dead.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.spruce_Dead;
            }
            else if (treeState_Health == TreeState_Health.treeState_Chopped.ToString())
            {
                gameObject.GetComponent<MeshRenderer>().material.color = colorManager.spruce_Chopped;
            }
        }
    }
}

