using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public class Tree
{
    ForestManager forestManager;

    [Header("Tree Age")]
    public float treeAge_InDaysTotal;
    public int year;
    public int month;
    public int day;

    [Header("Tree States")]
    public TreeAge treeAge;
    public TreeHealth treeHealth;
    public ForestType treeType;
    public bool isDead = false;
    int succeededSeeds = 0;

    [Header("Properties")]
    public float treeHeight;
    public float treeDiameter;
    public float treeVolume;
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

    private bool _pauseGrowth = false;
    //--------------------

    private void Start()
    {
        TimeManager.instance.OnSpringBegin += ResumeGrowth;
    }

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
        SetStemHeight();
        SetBudSize();
        SetTreeVolume();

        SetTreeHP();
    }
    
    public void UpdateStats()
    {
        UpdateTreeAge();

        UpdateTreeHeight();
        UpdateTreeDiameter();
        UpdateStemHeight();
        UpdateBudSize();
        UpdateTreeVolume();

        UpdateTreeHealth();

        UpdateTreeDie();
    }


    //--------------------

    private void ResumeGrowth()
    {
        _pauseGrowth = false;
    }
    
    void SetTreeAge()
    {
        switch (treeType)
        {
            case ForestType.Birch:
                year = Random.Range(0, 90);
                month = Random.Range(1, 13);
                day = Random.Range(1, 30);
                break;
            case ForestType.Spruce:
                year = Random.Range(0, 350);
                month = Random.Range(1, 13);
                day = Random.Range(1, 30);
                break;
            case ForestType.Pine:
                year = Random.Range(0, 450);
                month = Random.Range(1, 13);
                day = Random.Range(1, 30);
                break;
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
        HP = treeType switch
        {
            ForestType.Birch => Random.Range(50, 100),
            ForestType.Spruce => Random.Range(70, 100),
            ForestType.Pine => Random.Range(20, 100),
            _ => HP
        };
    }
    
    void SetStemHeight()
    {
        switch (treeType)
        {
            case ForestType.Birch:
                stemHeightPortionOfTree = 3;
                stemHeight = treeHeight / stemHeightPortionOfTree;
                break;
            case ForestType.Spruce:
                stemHeightPortionOfTree = 8;
                stemHeight = treeHeight / stemHeightPortionOfTree;
                break;
            case ForestType.Pine:
                stemHeightPortionOfTree = 2;
                stemHeight = treeHeight / stemHeightPortionOfTree;
                break;
        }
    }
    
    void SetBudSize()
    {
        budSize = treeAge_InDaysTotal * growthRate_Bud; 
    }
    
    void SetTreeVolume()
    {
        treeVolume = 0.5f * Mathf.Pow((Mathf.PI * treeHeight * (0.5f * treeDiameter)), 2f);
    }


    //--------------------
    
    /* TODO: It's better if we update each tree's date through forestmanager.
     That way we ensure that it's correct and don't rely on each Tree to keep the date. */
    void UpdateTreeAge()
    {
        treeAge_InDaysTotal++;
        day++;

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
        if (!_pauseGrowth)
            treeHeight += growthRate_Height;
    }
    
    void UpdateTreeDiameter()
    {
        if (!_pauseGrowth)
            treeDiameter += growthRate_Diameter;
    }
    
    void UpdateTreeHealth()
    {
        treeHealth = HP switch
        {
            0 => TreeHealth.Chopped,
            <= 10 => TreeHealth.Broken,
            <= 40 => TreeHealth.Damaged,
            <= 70 => TreeHealth.Healthy,
            _ => TreeHealth.Healthy
        };
    }
    
    void UpdateTreeDie()
    {
        switch (treeType)
        {
            //60 to 90 years
            // ->50 years
            case ForestType.Birch when treeAge_InDaysTotal <= 18250:
            {
                if (Random.Range(1, 10000) == 1) isDead = true;
                break;
            }
            // ->60 years
            case ForestType.Birch when treeAge_InDaysTotal <= 21900:
            {
                if (Random.Range(1, 8000) <= 2) isDead = true; 
                break;
            }
            // ->70 years
            case ForestType.Birch when treeAge_InDaysTotal <= 25550:
            {
                if (Random.Range(1, 6000) <= 3) isDead = true;
                break;
            }
            // ->80 years
            case ForestType.Birch when treeAge_InDaysTotal <= 29200:
            {
                if (Random.Range(1, 3000) <= 4) isDead = true;
                break;
            }
            // ->85 years
            case ForestType.Birch when treeAge_InDaysTotal <= 31025:
            {
                if (Random.Range(1, 1500) <= 5) isDead = true;
                break;
            }
            // ->90 years
            case ForestType.Birch when treeAge_InDaysTotal <= 32850:
            {
                if (Random.Range(1, 750) <= 6) isDead = true;
                break;
            }
            case ForestType.Birch:
                isDead = false;
                break;
            //250 to 350 years
            // ->250 years
            case ForestType.Spruce when treeAge_InDaysTotal <= 91250:
            {
                if (Random.Range(1, 10000) <= 1) isDead = true;
                break;
            }
            // ->270 years
            case ForestType.Spruce when treeAge_InDaysTotal <= 98550:
            {
                if (Random.Range(1, 8000) <= 2) isDead = true;
                break;
            }
            // ->290 years
            case ForestType.Spruce when treeAge_InDaysTotal <= 105850:
            {
                if (Random.Range(1, 6000) <= 3) isDead = true;
                break;
            }
            // ->310 years
            case ForestType.Spruce when treeAge_InDaysTotal <= 113150:
            {
                if (Random.Range(1, 3000) <= 4) isDead = true;
                break;
            }
            // ->330 years
            case ForestType.Spruce when treeAge_InDaysTotal <= 120450:
            {
                if (Random.Range(1, 1500) <= 5) isDead = true;
                break;
            }
            // ->350 years
            case ForestType.Spruce when treeAge_InDaysTotal <= 127750:
            {
                if (Random.Range(1, 750) <= 6) isDead = true;
                break;
            }
            //50 to 450 years
            // ->50 years
            case ForestType.Pine when treeAge_InDaysTotal <= 18250:
            {
                if (Random.Range(1, 100000) <= 1) isDead = true;
                break;
            }
            // ->100 years
            case ForestType.Pine when treeAge_InDaysTotal <= 36500:
            {
                if (Random.Range(1, 80000) <= 2) isDead = true;
                break;
            }
            // ->150 years
            case ForestType.Pine when treeAge_InDaysTotal <= 54750:
            {
                if (Random.Range(1, 60000) <= 3) isDead = true;
                break;
            }
            // ->200 years
            case ForestType.Pine when treeAge_InDaysTotal <= 73000:
            {
                if (Random.Range(1, 30000) <= 4) isDead = true;
                break;
            }
            // ->250 years
            case ForestType.Pine when treeAge_InDaysTotal <= 91250:
            {
                if (Random.Range(1, 15000) <= 5) isDead = true;
                break;
            }
            // ->300 years
            case ForestType.Pine when treeAge_InDaysTotal <= 109500:
            {
                if (Random.Range(1, 7500) <= 6) isDead = true;
                break;
            }
            // ->350 years
            case ForestType.Pine when treeAge_InDaysTotal <= 127750:
            {
                if (Random.Range(1, 3500) <= 7) isDead = true;
                break;
            }
            // ->400 years
            case ForestType.Pine when treeAge_InDaysTotal <= 146000:
            {
                if (Random.Range(1, 1750) <= 8) isDead = true;
                break;
            }
            // ->450 years
            case ForestType.Pine when treeAge_InDaysTotal <= 164250:
            {
                if (Random.Range(1, 1000) <= 9) isDead = true;
                break;
            }
            default:
                isDead = false;
                break;
        }
    }
    
    void UpdateStemHeight()
    {
        // TODO: All paths returned the same value. Update when proper data is acquired.
        if (!_pauseGrowth)
        {
            switch (treeType)
            {
                case ForestType.Birch:
                case ForestType.Spruce:
                case ForestType.Pine:
                    stemHeight += growthRate_Height / stemHeightPortionOfTree;
                    break;
            }
        }
    }

    void UpdateBudSize()
    {
        budSize += growthRate_Bud;
    }
    
    void UpdateTreeVolume()
    {
        var a = Mathf.PI * treeHeight * (0.5f * treeDiameter);
        treeVolume = a * a * 0.5f;
    }


    //--------------------

    public bool Edible()
    {
        if (treeHeight < 3 /* budSize > some value maybe */)
            return true;
        
        return false;
    }

    public bool EatFromTree()
    {
        // switch (treeHealth)
        // {
        //     case TreeHealth.Healthy:
        //         treeHealth = TreeHealth.Damaged;
        //         break;
        //     case TreeHealth.Damaged:
        //         treeHealth = TreeHealth.Broken;
        //         break;
        //     case TreeHealth.Broken:
        //         treeHealth = TreeHealth.Chopped;
        //         break;
        //     default:
        //         //Debug.Log("There is no more food to get from this tree");
        //         break;
        // }
        if (Edible())
        {
            _pauseGrowth = true;
            budSize = 0;
            return true;
        }
        else
        {
            Debug.Log("Tried eating from inedible tree");
            return false;
        }
    }

    public void SetGenes()
    {
        float num = Random.Range(0.75f, 1.25f); // TODO: Vennligst beskriv hva denne verdien er.
        // ^ Er det ikke bare random variance?

        switch (treeType)
        {
            case ForestType.Birch:
                //Growth per day
                growthRate_Height = 0.0006088f * num; // Jeg er mer interessert i disse tallene :P
                growthRate_Diameter = 0.0000122f * num;
                growthRate_Bud = 0.000006088f * num;
                break;
            case ForestType.Spruce:
                //Growth per day
                growthRate_Height = 0.0036529f * num;
                growthRate_Diameter = 0.00000062f * num;
                growthRate_Bud = 0.000036529f * num;
                break;
            case ForestType.Pine:
                //Growth per day
                growthRate_Height = 0.0012329f * num;
                growthRate_Diameter = 0.00000061f * num;
                growthRate_Bud = 0.00001232877f * num;
                break;
        }
    }

    public int CheckIfGettingBirth()
    {
        // Local function for more concise code
        int GetSeed()
        {
            switch (year)
            {
                case >= 10:
                    // Roll dice for "giving birth"
                    if (Random.Range(0, 2) == 0)
                    {
                        return treeHeight switch
                        {
                            >= 15 => Random.Range(0, 4),
                            >= 10 => Random.Range(0, 3),
                            >= 5 => Random.Range(0, 2),
                            _ => 0
                        };
                    }
                    break;
                case >= 5:
                    if (Random.Range(0, 4) == 0)
                    {
                        return treeHeight switch
                        {
                            >= 15 => Random.Range(0, 4),
                            >= 10 => Random.Range(0, 3),
                            >= 5 => Random.Range(0, 2),
                            _ => 0
                        };
                    }
                    break;
            }
            return 0;
        }

        /* Previous code had the same outcome for every tree type, putting this
         here for when proper data for each tree type is acquired. Just add
         another argument to the local func and encapsulate it with another switch. */
        succeededSeeds = treeType switch
        {
            ForestType.Birch => GetSeed(),
            ForestType.Pine => GetSeed(),
            ForestType.Spruce => GetSeed(),
            _ => 0
        };

        return succeededSeeds;
    }
}
