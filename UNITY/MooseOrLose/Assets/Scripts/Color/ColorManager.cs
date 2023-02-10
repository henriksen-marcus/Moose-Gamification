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
