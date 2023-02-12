using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ---------- Enums for forests ---------- //

public enum Season { Spring, Summer, Fall, Winter, None}

public enum ForestType
{
    Birch,
    Pine,
    Spruce,
    None
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


public enum ForestDensity
{
    Density1,
    Density2,
    Density3,
    Density4,
    Density5,
    None
}

// ---------- Enums for trees ---------- //

public enum TreeAge
{
    Child,
    Adult,
    Old,
    Dead,
    None
}

public enum TreeHealth
{
    Healthy,
    Damaged,
    Broken,
    Dead,
    Chopped,
    None
}