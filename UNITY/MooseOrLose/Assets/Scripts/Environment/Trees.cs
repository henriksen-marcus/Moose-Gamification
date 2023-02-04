using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trees
{
    ForestManager forestManager;

    [Header("Tree Age")]
    public float treeAge_InDaysTotal;
    public int year;
    public int month;
    public int day;

    [Header("Tree States")]
    public TreeState_Age treeState_Age;
    public TreeState_Health treeState_Health;
    public ForestState_Type treeState_Type;
    public bool isDead = false;

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
    public int HP;
    public float foodAttached;

    [Header("Genes")]
    public float growthRate_Height;
    public float growthRate_Diameter;
    public float darknessTolerance;
    public float soilWaterDrinkability;


    //--------------------


    public void SetStats()
    {
        SetGenes();

        SetTreeAge();

        SetTreeHeight();
        SetTreeDiameter();

        SetTreeHP();
    }
    public void UpdateStats()
    {
        UpdateTreeAge();

        UpdateTreeHeight();
        UpdateTreeDiameter();

        UpdateTreeHealth();

        UpdateTreeDie();
    }


    //--------------------


    void SetTreeAge()
    {
        if (treeState_Type == ForestState_Type.treeState_Birch)
        {
            year = Random.Range(0, 90);
            month = Random.Range(1, 13);
            day = Random.Range(1, 30);

        }
        else if (treeState_Type == ForestState_Type.treeState_Spruce)
        {

        }
        else if (treeState_Type == ForestState_Type.treeState_Pine)
        {

        }

        treeAge_InDaysTotal = (year * 365) + (month * 30) + (day);
    }
    void SetTreeHeight()
    {
        if (treeState_Type == ForestState_Type.treeState_Birch)
        {
            treeHeight = year * growthRate_Height * 365;
            treeHeight += month * growthRate_Height * 30;
            treeHeight += day * growthRate_Height;
        }
        else if (treeState_Type == ForestState_Type.treeState_Spruce)
        {

        }
        else if (treeState_Type == ForestState_Type.treeState_Pine)
        {

        }
    }
    void SetTreeDiameter()
    {
        if (treeState_Type == ForestState_Type.treeState_Birch)
        {
            treeDiameter = year * growthRate_Diameter * 365;
            treeDiameter += month * growthRate_Diameter * 30;
            treeDiameter += day * growthRate_Diameter;
        }
        else if (treeState_Type == ForestState_Type.treeState_Spruce)
        {

        }
        else if (treeState_Type == ForestState_Type.treeState_Pine)
        {

        }
    }
    void SetTreeHP()
    {
        HP = Random.Range(10, 100);
    }


    //--------------------


    void UpdateTreeAge()
    {
        treeAge_InDaysTotal += 1;
        day += 1;

        if (day >= 31)
            month++;

        if (month >= 13)
            year++;
    }
    void UpdateTreeHeight()
    {
        treeHeight += growthRate_Height;
    }
    void UpdateTreeDiameter()
    {
        treeDiameter += growthRate_Diameter;
    }
    void UpdateTreeHealth()
    {
        if (HP >= 0)
        {
            treeState_Health = TreeState_Health.treeState_Chopped;
        }
        else if (HP >= 10)
        {
            treeState_Health = TreeState_Health.treeState_Broken;
        }
        else if (HP >= 40)
        {
            treeState_Health = TreeState_Health.treeState_Damaged;
        }
        else if (HP >= 70)
        {
            treeState_Health = TreeState_Health.treeState_Healthy;
        }
        else
        {
            treeState_Health = TreeState_Health.treeState_Healthy;
        }
    }
    void UpdateTreeDie()
    {
        if (treeAge_InDaysTotal >= 15900)
        {
            int var = Random.Range(0, 7300);

            if (var <= 1)
            {
                isDead = true;
            }
        }
        else if (treeAge_InDaysTotal >= 23900)
        {
            int var = Random.Range(0, 5300);

            if (var <= 2)
            {
                isDead = true;
            }
        }
        else if (treeAge_InDaysTotal >= 25900)
        {
            int var = Random.Range(0, 3300);

            if (var <= 3)
            {
                isDead = true;
            }
        }
        else if (treeAge_InDaysTotal >= 27900)
        {
            int var = Random.Range(0, 1300);

            if (var <= 4)
            {
                isDead = true;
            }
        }
        else if (treeAge_InDaysTotal >= 28900)
        {
            int var = Random.Range(0, 300);

            if (var <= 5)
            {
                isDead = true;
            }
        }
        else if (treeAge_InDaysTotal >= 30000)
        {
            int var = Random.Range(0, 50);

            if (var <= 6)
            {
                isDead = true;
            }
        }
        else
        {
            isDead = false;
        }

        if (isDead)
        {
            treeAge_InDaysTotal = 0;
            year = 0;
            month = 0;
            day = 0;
        }
    }


    //--------------------


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

    public void SetGenes()
    {
        float var = (float)Random.Range(0.75f, 1.25f);

        if (treeState_Type == ForestState_Type.treeState_Birch)
        {
            //Growth per day
            growthRate_Height = 0.0006088f * var;
            growthRate_Diameter = 0.0000122f * var;
        }
        else if (treeState_Type == ForestState_Type.treeState_Spruce)
        {
            //Growth per day
            growthRate_Height = 0;
            growthRate_Diameter = 0;
        }
        else if (treeState_Type == ForestState_Type.treeState_Pine)
        {
            //Growth per day
            growthRate_Height = 0;
            growthRate_Diameter = 0;
        }
    }
}
