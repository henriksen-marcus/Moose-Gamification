using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [Header("Birch")]
    public Color birch_Healthy;
    public Color birch_Damaged;
    public Color birch_Broken;
    public Color birch_Dead;
    public Color birch_Chopped;

    [Header("Pine")]
    public Color pine_Healthy;
    public Color pine_Damaged;
    public Color pine_Broken;
    public Color pine_Dead;
    public Color pine_Chopped;

    [Header("Spruce")]
    public Color spruce_Healthy;
    public Color spruce_Damaged;
    public Color spruce_Broken;
    public Color spruce_Dead;
    public Color spruce_Chopped;


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
