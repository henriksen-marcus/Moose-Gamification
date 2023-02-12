using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [Header("Birch")]
    public Color birchDensity_1;
    public Color birchDensity_2;
    public Color birchDensity_3;
    public Color birchDensity_4;
    public Color birchDensity_5;

    [Header("Pine")]
    public Color pineDensity_1;
    public Color pineDensity_2;
    public Color pineDensity_3;
    public Color pineDensity_4;
    public Color pineDensity_5;

    [Header("Spruce")]
    public Color spruceDensity_1;
    public Color spruceDensity_2;
    public Color spruceDensity_3;
    public Color spruceDensity_4;
    public Color spruceDensity_5;


    //--------------------

    public Color GetColor(ForestType type, ForestDensity density)
    {
        return type switch
        {
            ForestType.Birch => density switch
            {
                ForestDensity.Density1 => birchDensity_1,
                ForestDensity.Density2 => birchDensity_2,
                ForestDensity.Density3 => birchDensity_3,
                ForestDensity.Density4 => birchDensity_4,
                ForestDensity.Density5 => birchDensity_5,
                _ => birchDensity_1
            },
            ForestType.Pine => density switch
            {
                ForestDensity.Density1 => pineDensity_1,
                ForestDensity.Density2 => pineDensity_2,
                ForestDensity.Density3 => pineDensity_3,
                ForestDensity.Density4 => pineDensity_4,
                ForestDensity.Density5 => pineDensity_5,
                _ => pineDensity_1
            },
            ForestType.Spruce => density switch
            {
                ForestDensity.Density1 => spruceDensity_1,
                ForestDensity.Density2 => spruceDensity_2,
                ForestDensity.Density3 => spruceDensity_3,
                ForestDensity.Density4 => spruceDensity_4,
                ForestDensity.Density5 => spruceDensity_5,
                _ => spruceDensity_1
            },
            _ => Color.black
        };
    }


    private void Awake()
    {
        //birch_Healthy = new Color(1f, 0f, 0f, 1f);
        //birch_Damaged = new Color(1f, 0f, 0f, 1f);
        //birch_Dead = new Color(1f, 0f, 0f, 1f);
        //birch_Healthy = new Color(1f, 0f, 0f, 1f);
        //birch_Chopped = new Color(1f, 0f, 0f, 1f);

        //pine_Healthy = new Color(0f, 1f, 0f, 1f);
        //pine_Damaged = new Color(0f, 1f, 0f, 1f);
        //pine_Broken = new Color(0f, 1f, 0f, 1f);
        //pine_Dead = new Color(0f, 1f, 0f, 1f);
        //pine_Chopped = new Color(0f, 1f, 0f, 1f);

        //spruce_Healthy = new Color(0f, 0f, 1f, 1f);
        //spruce_Damaged = new Color(0f, 0f, 1f, 1f);
        //spruce_Broken = new Color(0f, 0f, 1f, 1f);
        //spruce_Dead = new Color(0f, 0f, 1f, 1f);
        //spruce_Chopped = new Color(0f, 0f, 1f, 1f);
    }
}
