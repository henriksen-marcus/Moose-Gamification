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
    public Tree(Forest inOwner)
    {
        _ownerForest = inOwner;
    }

    private Forest _ownerForest;

    [Header("Tree Age")]
    public int treeAgeInDaysTotal;
    public int year;
    public int month;
    public int day;

    [Header("Tree States")]
    public TreeAge treeAge;
    public TreeHealth treeHealth;
    public ForestType treeType;
    public bool isDead = false;
    int _succeededSeeds = 0;

    [Header("Properties")]
    public float treeHeight;
    public float treeDiameter;
    public float treeVolume;
    public float stemHeight;
    public float budSize;
    public float shadowArea;
    public float shadowFactor;

    [Header("Damages")]
    public float barkDamage;
    public float budDamage;
    public float branchDamage;

    [Header("Food")]
    public int HP;
    public float foodAttached;

    [Header("Genes")]
    public float growthRateHeight;
    public float growthRateDiameter;
    public float darknessTolerance;
    public float soilWaterDrinkability;
    public float growthRateBud;
    float _stemHeightPortionOfTree;
    private float heightGene;
    private float maxHeight;

    private bool _pauseGrowth = false;
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
        SetStemHeight();
        SetBudSize();
        SetTreeVolume();

        SetTreeHP();
    }
    
    public void UpdateStats()
    {
        UpdateTreeAge();

        if (treeHeight < (maxHeight * heightGene))
        {
            UpdateTreeHeight();
            UpdateTreeDiameter();
            UpdateStemHeight();
            UpdateBudSize();
            UpdateTreeVolume();
        }       
        UpdateShadowArea();
        UpdateTreeDie();

        UpdateTreeHealth();
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
                // year = Random.Range(0, 180);
                int roll11 = RollDice(10, 30);
                int roll12 = RollDice(10, 30);
                year = Mathf.Min(roll11, roll12);
                month = Random.Range(1, 13);
                day = Random.Range(1, 32);
                break;
            case ForestType.Spruce:
                // year = Random.Range(0, 230);
                int roll21 = RollDice(10, 50);
                int roll22 = RollDice(10, 50);
                year = Mathf.Min(roll21, roll22);
                month = Random.Range(1, 13);
                day = Random.Range(1, 32);
                break;
            case ForestType.Pine:
                // year = Random.Range(0, 280);
                int roll31 = RollDice(10, 70);
                int roll32 = RollDice(10, 70);
                year = Mathf.Min(roll31, roll32);
                month = Random.Range(1, 13);
                day = Random.Range(1, 32);
                break;
        }

        treeAgeInDaysTotal = year * 365 + month * 30 + day;
    }

    private int RollDice(int n, int s)
    {
        // Sum of n dice each of which goes from 0 to s
        int value = 0;
        for (int i = 0; i < n; i++)
        {
            value += Random.Range(0, s + 1);
        }
        return value;
    }
    
    void SetTreeHeight()
    {
        treeHeight = StartHeight(treeAgeInDaysTotal);
        switch (treeType)
        {
            case ForestType.Birch:
                maxHeight = 22f;
                break;
            case ForestType.Spruce:
                maxHeight = 60f;
                break;
            case ForestType.Pine:
                maxHeight = 85f;
                break;
        }

        treeHeight = Mathf.Clamp(treeHeight, 0, maxHeight * heightGene);
    }
    
    void SetTreeDiameter()
    {
        treeDiameter = treeAgeInDaysTotal * growthRateDiameter;
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
                _stemHeightPortionOfTree = 3;
                stemHeight = treeHeight / _stemHeightPortionOfTree;
                break;
            case ForestType.Spruce:
                _stemHeightPortionOfTree = 8;
                stemHeight = treeHeight / _stemHeightPortionOfTree;
                break;
            case ForestType.Pine:
                _stemHeightPortionOfTree = 2;
                stemHeight = treeHeight / _stemHeightPortionOfTree;
                break;
        }
    }
    
    void SetBudSize()
    {
        budSize = treeAgeInDaysTotal * growthRateBud; 
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
        treeAgeInDaysTotal++;
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
    private void UpdateShadowArea()
    {
        shadowArea = treeDiameter + shadowFactor * treeAgeInDaysTotal / 3650;
    }
    void UpdateTreeHeight()
    {
        if (!_pauseGrowth)
            treeHeight += growthRateHeight;
    }
    void UpdateTreeDiameter()
    {
        if (!_pauseGrowth)
            treeDiameter += growthRateDiameter;
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
            // Birch max 300
            // Spruce max 500
            // Pine max 700
            case ForestType.Birch:
                if (treeAgeInDaysTotal >= 109500)
                {
                    isDead = true;
                    break;
                }
                else
                {
                    // float chance = treeAgeInDaysTotal / 10950;
                    // int n = Random.Range(1, 81);
                    // if (n < chance)
                    //     isDead = true;
                    break;
                }
            case ForestType.Spruce:
                if (treeAgeInDaysTotal >= 182500)
                {
                    isDead = true;
                    break;
                }
                else
                {
                    // float chance = treeAgeInDaysTotal / 18250;
                    // int n = Random.Range(1, 81);
                    // if (n < chance)
                    //     isDead = true;
                    break;
                }
            case ForestType.Pine:
                if (treeAgeInDaysTotal >= 255500)
                {
                    isDead = true;
                    break;
                }
                else
                {
                    // float chance = treeAgeInDaysTotal / 25550;
                    // int n = Random.Range(1, 81);
                    // if (n < chance)
                    //     isDead = true;
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
                    stemHeight += growthRateHeight / _stemHeightPortionOfTree;
                    break;
            }
        }
    }
    void UpdateBudSize()
    {
        budSize += growthRateBud;
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
            //Debug.Log("Tried eating from inedible tree");
            return false;
        }
    }

    public void SetGenes()
    {
        float num = Random.Range(0.75f, 1.25f); // TODO: Vennligst beskriv hva denne verdien er.
        // ^ Er det ikke bare random variance?

        switch (treeType)
        {
            case ForestType.Spruce:
                //Growth per day
                growthRateHeight = 0.0036529f * num;
                growthRateDiameter = 0.00000062f * num;
                growthRateBud = 0.000036529f * num;
                shadowFactor = 0.75f;
                break;
            case ForestType.Pine:
                //Growth per day
                growthRateHeight = 0.0012329f * num;
                growthRateDiameter = 0.00000061f * num;
                growthRateBud = 0.00001232877f * num;
                shadowFactor = 1f;
                break;
            case ForestType.Birch:
                //Growth per day
                growthRateHeight = 0.0006088f * num; // Jeg er mer interessert i disse tallene :P
                growthRateDiameter = 0.000045f * num;
                growthRateBud = 0.000006088f * num;
                shadowFactor = 1.5f;
                break;
        }
        heightGene = Random.Range(0.5f, 1f);
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
        _succeededSeeds = treeType switch
        {
            ForestType.Birch => GetSeed(),
            ForestType.Pine => GetSeed(),
            ForestType.Spruce => GetSeed(),
            _ => 0
        };

        return _succeededSeeds;
    }


    float StartHeight(int daysAlive)
    {
        if (daysAlive < 365 * 3)
        {
            return (0.00109589f * daysAlive) - 0.033333f;
        }


        return (0.002288310502f * daysAlive) - 0.8666667f;
    }
}
