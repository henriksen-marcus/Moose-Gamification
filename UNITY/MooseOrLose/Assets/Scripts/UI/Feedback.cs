using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    public RectTransform MooseBackground;
    public RectTransform HuntersBackground;
    public RectTransform ForestBackground;

    public Gradient gradient;

    public int MooseScore;
    public int HunterScore;
    public int ForestScore;

    //Moose
    private int populationPoints;
    private int populationDifferencePoints;

    //Hunter
    public int moosePopulationLowerLimit = 125;
    public int moosePopulationHigherLimit = 180;

    private int populationLimitScore;
    private int shotLastMonthScore;
    private int leftOverMooseScore;


    // Start is called before the first frame update
    void Start()
    {
        TimeManager.instance.OnNewDay += NewDay;
        TimeManager.instance.OnNewYear += NewYear;
    }

    void NewDay()
    {
        MooseCalc();
        HunterCalc();
        ForestCalc();
    }

    void MooseCalc()
    {
        populationPoints = ElgManager.instance.elg_population;

        int size = ElgManager.instance.elg_population_graph.Count;
        if (size > 2)
        {
            populationDifferencePoints = ElgManager.instance.elg_population_graph[size - 1] - ElgManager.instance.elg_population_graph[size - 2];
            populationDifferencePoints *= 10;
        }
        else
        {
            populationDifferencePoints = 0; 
        }

        MooseScore = populationPoints + populationDifferencePoints;
        MooseBackground.GetComponent<Image>().color = gradient.Evaluate((float)MooseScore / 250f);
    }
    void HunterCalc()
    {
        int moosePop = ElgManager.instance.elg_population;
        if (moosePop < moosePopulationLowerLimit)
        {
            populationLimitScore = (moosePop - moosePopulationHigherLimit) * -10;
        }
        else if (moosePop > moosePopulationHigherLimit)
        {
            populationLimitScore = (moosePopulationLowerLimit - moosePop) * -10;
        }
        else
        {
            populationLimitScore = 0;
        }

        shotLastMonthScore = JegerManager.instance.shotLastMonth * 10;

        HunterScore = 150 + populationLimitScore + shotLastMonthScore + leftOverMooseScore;
        HuntersBackground.GetComponent<Image>().color = gradient.Evaluate((float)HunterScore / 300f);
    }
    void ForestCalc()
    {

    }


    void NewYear()
    {
        leftOverMooseScore = (JegerManager.instance.currentFemales + JegerManager.instance.currentMales) * -10;
    }
}
