using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trees
{
    [Header("Basic Info")]
    public float treeAge;

    [Header("Tree Age")]
    public int age;
    public string year;
    public string month;
    public string day;

    [Header("Tree States")]
    public TreeState_Age treeState_Age;
    public TreeState_Health treeState_Health;
    public ForestState_Type treeState_Type;

    [Header("Properties")]
    public float treeHeight;
    public float treeDiameter;
    public float treeVolum;
    public float stemHeight;
    public float budSize;

    [Header("Damages")]
    public float barkDamage;
    public float budDamage;
    public float branchDamage;

    [Header("Food")]
    public float foodAttached;

    [Header("Genes")]
    public float growthRate_Height = 0.1f;
    public float growthRate_Diameter = 0.01f;
    public float darknessTolerance;
    public float soilWaterDrinkability;


    //--------------------


    void Awake()
    {
        SetAge();

        SetBaseStats();


    }


    //--------------------


    void SetAge()
    { 

    }
    void SetBaseStats()
    {
        age = Random.Range(0, 120);
        treeHeight = (float)Random.Range(0f, 60f);
        treeDiameter = (float)Random.Range(0.01f, 1.5f);

        //Tree Health
        int a = Random.Range(0, 3);
        if (a == 0)
        {
            treeState_Health = TreeState_Health.treeState_Healthy;
        }
        else if (a == 1)
        {
            treeState_Health = TreeState_Health.treeState_Damaged;
        }
        else if (a == 2)
        {
            treeState_Health = TreeState_Health.treeState_Broken;
        }
    }


    public void EatFromTree()
    {
        if (treeState_Health == TreeState_Health.treeState_Healthy)
        {
            treeState_Health = TreeState_Health.treeState_Damaged;
        }
        else if (treeState_Health == TreeState_Health.treeState_Damaged)
        {
            treeState_Health = TreeState_Health.treeState_Broken;
        }
        else if (treeState_Health == TreeState_Health.treeState_Broken)
        {
            treeState_Health = TreeState_Health.treeState_Chopped;
        }
        else
        {
            Debug.Log("There are no more food to get from this tree");
        }
    }

    public void Genes()
    {

    }
}
