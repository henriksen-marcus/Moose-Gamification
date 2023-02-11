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
    int successedSeeds = 0;

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
    public float growthRate_Bud;

    float stemHeightPortionOfTree;


    //--------------------


    public void SetBirth()
    {
        SetGenes();

        SetTreeHP();
    }
    public void SetStats()
    {
        SetGenes();

        SetTreeAge();

        SetTreeHeight();
        SetTreeDiameter();
        SetStemHeihgt();
        SetBudSize();
        SetTreeVolum();

        SetTreeHP();
    }
    public void UpdateStats()
    {
        UpdateTreeAge();

        UpdateTreeHeight();
        UpdateTreeDiameter();
        UpdateStemHeight();
        UpdateBudSize();
        UpdateTreeVolum();

        UpdateTreeHealth();

        UpdateTreeDie();
    }


    //--------------------


    void SetTreeAge()
    {
        if (treeState_Type == ForestState_Type.forestType_Birch)
        {
            year = Random.Range(0, 90);
            month = Random.Range(1, 13);
            day = Random.Range(1, 30);

        }
        else if (treeState_Type == ForestState_Type.forestType_Spruce)
        {
            year = Random.Range(0, 350);
            month = Random.Range(1, 13);
            day = Random.Range(1, 30);
        }
        else if (treeState_Type == ForestState_Type.forestType_Pine)
        {
            year = Random.Range(0, 450);
            month = Random.Range(1, 13);
            day = Random.Range(1, 30);
        }

        treeAge_InDaysTotal = (year * 365) + (month * 30) + (day);
    }
    void SetTreeHeight()
    {
        treeHeight = treeAge_InDaysTotal * growthRate_Height;
    }
    void SetTreeDiameter()
    {
        treeDiameter = treeAge_InDaysTotal * growthRate_Diameter;
    }
    void SetTreeHP()
    {
        if (treeState_Type == ForestState_Type.forestType_Birch)
        {
            HP = Random.Range(50, 100);
        }
        else if (treeState_Type == ForestState_Type.forestType_Spruce)
        {
            HP = Random.Range(70, 100);
        }
        else if (treeState_Type == ForestState_Type.forestType_Pine)
        {
            HP = Random.Range(20, 100);
        }
    }
    void SetStemHeihgt()
    {
        if (treeState_Type == ForestState_Type.forestType_Birch)
        {
            stemHeightPortionOfTree = 3;
            stemHeight = treeHeight / stemHeightPortionOfTree;
        }
        else if (treeState_Type == ForestState_Type.forestType_Spruce)
        {
            stemHeightPortionOfTree = 8;
            stemHeight = treeHeight / stemHeightPortionOfTree;
        }
        else if (treeState_Type == ForestState_Type.forestType_Pine)
        {
            stemHeightPortionOfTree = 2;
            stemHeight = treeHeight / stemHeightPortionOfTree;
        }
    }
    void SetBudSize()
    {
        budSize = treeAge_InDaysTotal * growthRate_Bud; 
    }
    void SetTreeVolum()
    {
        treeVolum = 0.5f * Mathf.Pow((Mathf.PI * treeHeight * (0.5f * treeDiameter)), 2f);
    }


    //--------------------


    void UpdateTreeAge()
    {
        treeAge_InDaysTotal += 1;
        day += 1;

        if (day >= 31)
        {
            month++;
            day = 0;
        }

        if (month >= 13)
        {
            year++;
            month = 0;
        }
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
        if (HP == 0)
        {
            treeState_Health = TreeState_Health.treeHealth_Chopped;
        }
        else if (HP <= 10)
        {
            treeState_Health = TreeState_Health.treeHealth_Broken;
        }
        else if (HP <= 40)
        {
            treeState_Health = TreeState_Health.treeHealth_Damaged;
        }
        else if (HP <= 70)
        {
            treeState_Health = TreeState_Health.treeHealth_Healthy;
        }
        else
        {
            treeState_Health = TreeState_Health.treeHealth_Healthy;
        }
    }
    void UpdateTreeDie()
    {
        if (treeState_Type == ForestState_Type.forestType_Birch)
        {
            //60 to 90 years
            if (treeAge_InDaysTotal <= 18250) // ->50 years
            {
                int var = Random.Range(1, 10000);

                if (var <= 1)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 21900) // ->60 years
            {
                int var = Random.Range(1, 8000);

                if (var <= 2)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 25550) // ->70 years
            {
                int var = Random.Range(1, 6000);

                if (var <= 3)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 29200) // ->80 years
            {
                int var = Random.Range(1, 3000);

                if (var <= 4)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 31025) // ->85 years
            {
                int var = Random.Range(1, 1500);

                if (var <= 5)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 32850) // ->90 years
            {
                int var = Random.Range(1, 750);

                if (var <= 6)
                {
                    isDead = true;
                }
            }
            else
            {
                isDead = false;
            }
        }
        else if (treeState_Type == ForestState_Type.forestType_Spruce)
        {
            //250 to 350 years
            if (treeAge_InDaysTotal <= 91250) // ->250 years
            {
                int var = Random.Range(1, 10000);

                if (var <= 1)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 98550) // ->270 years
            {
                int var = Random.Range(1, 8000);

                if (var <= 2)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 105850) // ->290 years
            {
                int var = Random.Range(1, 6000);

                if (var <= 3)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 113150) // ->310 years
            {
                int var = Random.Range(1, 3000);

                if (var <= 4)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 120450) // ->330 years
            {
                int var = Random.Range(1, 1500);

                if (var <= 5)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 127750) // ->350 years
            {
                int var = Random.Range(1, 750);

                if (var <= 6)
                {
                    isDead = true;
                }
            }
            else
            {
                isDead = false;
            }
        }
        else if (treeState_Type == ForestState_Type.forestType_Pine)
        {
            //50 to 450 years
            if (treeAge_InDaysTotal <= 18250) // ->50 years
            {
                int var = Random.Range(1, 100000);

                if (var <= 1)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 36500) // ->100 years
            {
                int var = Random.Range(1, 80000);

                if (var <= 2)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 54750) // ->150 years
            {
                int var = Random.Range(1, 60000);

                if (var <= 3)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 73000) // ->200 years
            {
                int var = Random.Range(1, 30000);

                if (var <= 4)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 91250) // ->250 years
            {
                int var = Random.Range(1, 15000);

                if (var <= 5)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 109500) // ->300 years
            {
                int var = Random.Range(1, 7500);

                if (var <= 6)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 127750) // ->350 years
            {
                int var = Random.Range(1, 3500);

                if (var <= 7)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 146000) // ->400 years
            {
                int var = Random.Range(1, 1750);

                if (var <= 8)
                {
                    isDead = true;
                }
            }
            else if (treeAge_InDaysTotal <= 164250) // ->450 years
            {
                int var = Random.Range(1, 1000);

                if (var <= 9)
                {
                    isDead = true;
                }
            }
            else
            {
                isDead = false;
            }
        }
    }
    void UpdateStemHeight()
    {
        if (treeState_Type == ForestState_Type.forestType_Birch)
        {
            stemHeight += growthRate_Height / stemHeightPortionOfTree;
        }
        else if (treeState_Type == ForestState_Type.forestType_Spruce)
        {
            stemHeight += growthRate_Height / stemHeightPortionOfTree;
        }
        else if (treeState_Type == ForestState_Type.forestType_Pine)
        {
            stemHeight += growthRate_Height / stemHeightPortionOfTree;
        }
    }
    void UpdateBudSize()
    {
        budSize += growthRate_Bud;
    }
    void UpdateTreeVolum()
    {
        treeVolum = 0.5f * Mathf.Pow((Mathf.PI * treeHeight * (0.5f * treeDiameter)), 2f);
    }


    //--------------------


    public void EatFromTree()
    {
        if (treeState_Health == TreeState_Health.treeHealth_Healthy)
        {
            treeState_Health = TreeState_Health.treeHealth_Damaged;
        }
        else if (treeState_Health == TreeState_Health.treeHealth_Damaged)
        {
            treeState_Health = TreeState_Health.treeHealth_Broken;
        }
        else if (treeState_Health == TreeState_Health.treeHealth_Broken)
        {
            treeState_Health = TreeState_Health.treeHealth_Chopped;
        }
        else
        {
            //Debug.Log("There are no more food to get from this tree");
        }
    }

    public void SetGenes()
    {
        float var = (float)Random.Range(0.75f, 1.25f);

        if (treeState_Type == ForestState_Type.forestType_Birch)
        {
            //Growth per day
            growthRate_Height = 0.0006088f * var;
            growthRate_Diameter = 0.0000122f * var;
            growthRate_Bud = 0.000006088f * var;
        }
        else if (treeState_Type == ForestState_Type.forestType_Spruce)
        {
            //Growth per day
            growthRate_Height = 0.0036529f * var;
            growthRate_Diameter = 0.00000062f * var;
            growthRate_Bud = 0.000036529f * var;
        }
        else if (treeState_Type == ForestState_Type.forestType_Pine)
        {
            //Growth per day
            growthRate_Height = 0.0012329f * var;
            growthRate_Diameter = 0.00000061f * var;
            growthRate_Bud = 0.00001232877f * var;
        }
    }

    public int CheckIfGettingBirth()
    {
        int chanceOfBirth = 0;
        successedSeeds = 0;

        //Birch
        if (treeState_Type == ForestState_Type.forestType_Birch)
        {
            if (year >= 10)
            {
                chanceOfBirth = Random.Range(0, 2);

                if (chanceOfBirth == 0)
                {
                    if (treeHeight >= 5)
                        successedSeeds = Random.Range(0, 2);
                    else if (treeHeight >= 10)
                        successedSeeds = Random.Range(0, 3);
                    else if (treeHeight >= 15)
                        successedSeeds = Random.Range(0, 4);
                }
            }
            else if (year >= 5)
            {
                chanceOfBirth = Random.Range(0, 4);

                if (chanceOfBirth == 0)
                {
                    if (treeHeight >= 5)
                        successedSeeds = Random.Range(0, 2);
                    else if (treeHeight >= 10)
                        successedSeeds = Random.Range(0, 3);
                    else if (treeHeight >= 15)
                        successedSeeds = Random.Range(0, 4);
                }
            }
        }
        //Spruce - Not accurate numbers
        else if (treeState_Type == ForestState_Type.forestType_Spruce)
        {
            if (year >= 10)
            {
                chanceOfBirth = Random.Range(0, 2);

                if (chanceOfBirth == 0)
                {
                    if (treeHeight >= 5)
                        successedSeeds = Random.Range(0, 2);
                    else if (treeHeight >= 10)
                        successedSeeds = Random.Range(0, 3);
                    else if (treeHeight >= 15)
                        successedSeeds = Random.Range(0, 4);
                }
            }
            else if (year >= 5)
            {
                chanceOfBirth = Random.Range(0, 4);

                if (chanceOfBirth == 0)
                {
                    if (treeHeight >= 5)
                        successedSeeds = Random.Range(0, 2);
                    else if (treeHeight >= 10)
                        successedSeeds = Random.Range(0, 3);
                    else if (treeHeight >= 15)
                        successedSeeds = Random.Range(0, 4);
                }
            }
        }
        //Pine - Not accurate numbers
        else if (treeState_Type == ForestState_Type.forestType_Pine)
        {
            if (year >= 10)
            {
                chanceOfBirth = Random.Range(0, 2);

                if (chanceOfBirth == 0)
                {
                    if (treeHeight >= 5)
                        successedSeeds = Random.Range(0, 2);
                    else if (treeHeight >= 10)
                        successedSeeds = Random.Range(0, 3);
                    else if (treeHeight >= 15)
                        successedSeeds = Random.Range(0, 4);
                }
            }
            else if (year >= 5)
            {
                chanceOfBirth = Random.Range(0, 4);

                if (chanceOfBirth == 0)
                {
                    if (treeHeight >= 5)
                        successedSeeds = Random.Range(0, 2);
                    else if (treeHeight >= 10)
                        successedSeeds = Random.Range(0, 3);
                    else if (treeHeight >= 15)
                        successedSeeds = Random.Range(0, 4);
                }
            }
        }

        return successedSeeds;
    }
}
