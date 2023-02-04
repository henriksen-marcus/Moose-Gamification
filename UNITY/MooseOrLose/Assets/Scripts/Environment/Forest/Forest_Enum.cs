using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForestState_Type
{
    none,

    treeState_Birch,
    treeState_Pine,
    treeState_Spruce
}

public enum TreeState_Age
{
    none,

    treeState_Child,
    treeState_Adult,
    treeState_Old,
    treeState_Dead
}

public enum TreeState_Health
{
    none,

    treeState_Healthy,
    treeState_Damaged,
    treeState_Broken,
    treeState_Dead,
    treeState_Chopped
}

public enum ForestState_Season
{
    none,

    treeState_Spring,
    treeState_Summer,
    treeState_Fall,
    treeState_Winter
}