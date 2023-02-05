using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//----- Forest


public enum ForestState_Type
{
    none,

    forestType_Birch,
    forestType_Pine,
    forestType_Spruce
}
public enum ForestState_Density
{
    none,

    forestVolum_1,
    forestVolum_2,
    forestVolum_3,
    forestVolum_4,
    forestVolum_5
}
public enum ForestState_Season
{
    none,

    forestSeason_Spring,
    forestSeason_Summer,
    forestSeason_Fall,
    forestSeason_Winter
}


//----- Tree


public enum TreeState_Age
{
    none,

    treeAge_Child,
    treeAge_Adult,
    treeAge_Old,
    treeAge_Dead
}
public enum TreeState_Health
{
    none,

    treeHealth_Healthy,
    treeHealth_Damaged,
    treeHealth_Broken,
    treeHealth_Dead,
    treeHealth_Chopped
}