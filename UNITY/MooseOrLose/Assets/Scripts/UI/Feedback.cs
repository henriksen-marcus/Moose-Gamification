using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Feedback : MonoBehaviour
{
    public RectTransform MooseBackground;
    public RectTransform HuntersBackground;
    public RectTransform ForestBackground;
    public GameObject PopUp;

    RectTransform FeedbackPopUpContainer;

    public Gradient gradient;

    public int MooseScore;
    public int HunterScore;
    public int ForestScore;

    //Moose
    [SerializeField] private int populationPoints;
    [SerializeField] private int populationDifferencePoints;
    private int populationPointsPrev;
    private int populationDifferencePointsPrev;

    //Hunter
    public int moosePopulationLowerLimit = 125;
    public int moosePopulationHigherLimit = 180;

    
    [SerializeField] private int populationLimitScore;
    [SerializeField] private int shotLastMonthScore;
    [SerializeField] private int leftOverMooseScore;
    private int populationLimitScorePrev;
    private int shotLastMonthScorePrev;
    private int leftOverMooseScorePrev;

    private bool hasSpawnedPopulationPopUp1;
    private bool hasSpawnedPopulationPopUp2;
    private bool hasSpawnedPopulationLimit;
    private bool hasSpawnedShotLastMonth;
    private bool hasSpawnedLeftOverMoose;

    private List<int> populationCounts;
    private int weekDay;


    // Start is called before the first frame update
    void Start()
    {
        populationCounts = new List<int>();
        TimeManager.instance.OnNewDay += NewDay;
        TimeManager.instance.OnNewYear += NewYear;
        FeedbackPopUpContainer = (RectTransform)transform.parent.Find("FeedbackPopUpContainer");
        if (FeedbackPopUpContainer == null)
        {
            Debug.Log("Failed To Get Pop Up Container");
        }
        populationPoints = 150;
        populationPointsPrev = 150;
        hasSpawnedPopulationPopUp1 = false;
        hasSpawnedPopulationPopUp2 = false;
        hasSpawnedPopulationLimit = false;
        hasSpawnedShotLastMonth = false;
        hasSpawnedLeftOverMoose = false;
        weekDay = 0;
    }

    void NewDay()
    {
        weekDay++;       
        if (weekDay > 6)
        {
            weekDay = 0;
            populationCounts.Add(ElgManager.instance.elg_population);
        }
        if (populationCounts.Count > 3)
        {
            populationCounts.RemoveAt(0);
        }
        MooseCalc();
        HunterCalc();
        ForestCalc();

        UpdatePrevious();
    }

    void MooseCalc()
    {
        populationPoints = ElgManager.instance.elg_population;

        int size = populationCounts.Count;
        if (size > 2)
        {
            populationDifferencePoints = populationCounts[size - 1] - populationCounts[size - 2];
            populationDifferencePoints *= 5;
        }
        else
        {
            populationDifferencePoints = ElgManager.instance.elg_population - 150; 
        }

        MooseScore = populationPoints + populationDifferencePoints;
        MooseBackground.GetComponent<Image>().color = gradient.Evaluate((float)MooseScore / 300f);
        MoosePopUp();
    }

    void MoosePopUp()
    {
        if (populationDifferencePoints != populationDifferencePointsPrev)
        {           
            if (populationDifferencePoints > 30)
            {
                SpawnPopUp("Moose", "Moose population is growing and they are happy");
            }
            if (populationDifferencePoints < -20 && populationDifferencePoints >  -70)
            {               
                SpawnPopUp("Moose", "Moose population is dropping and they are becoming unhappy");
            }
            if (populationDifferencePoints < -70)
            {
                SpawnPopUp("Moose", "Moose population is dropping quickly and they are becoming very unhappy");
            }
        }
        if (populationPoints != populationPointsPrev)
        {
            if (populationPoints > 100)
            {
                hasSpawnedPopulationPopUp2 = false;
                hasSpawnedPopulationPopUp1 = false;
            }
            else if (populationPoints < 100 && populationPoints > 80 && !hasSpawnedPopulationPopUp1)
            {
                hasSpawnedPopulationPopUp1 = true;
                hasSpawnedPopulationPopUp2 = false;
                SpawnPopUp("Moose", "Moose population is getting low and its affecting the moose's happiness");
            }
            else if (populationPoints < 80 && !hasSpawnedPopulationPopUp2)
            {
                hasSpawnedPopulationPopUp1 = false;
                hasSpawnedPopulationPopUp2 = true;
                SpawnPopUp("Moose", "Moose population is getting very low! They are now getting very unhappy");
            }

            
        }
    }
    void HunterCalc()
    {
        int moosePop = ElgManager.instance.elg_population;
        if (moosePop < moosePopulationLowerLimit)
        {           
            populationLimitScore = (moosePopulationLowerLimit - moosePop) * -10;
        }
        else if (moosePop > moosePopulationHigherLimit)
        {
            
            populationLimitScore = (moosePop - moosePopulationHigherLimit) * -10;
        }
        else
        {
            populationLimitScore = 0;
        }

        shotLastMonthScore = JegerManager.instance.shotLastMonth * 10;
        HunterScore = 150 + populationLimitScore + shotLastMonthScore + leftOverMooseScore;
        HuntersBackground.GetComponent<Image>().color = gradient.Evaluate((float)HunterScore / 300f);
        HunterPopUp();
    }

    void HunterPopUp()
    {
        if (populationLimitScore != populationLimitScorePrev)
        {

            int pop = ElgManager.instance.elg_population;
            if (pop > moosePopulationHigherLimit)
            {
                if (!hasSpawnedPopulationLimit)
                {
                    hasSpawnedPopulationLimit = true;
                    SpawnPopUp("Hunter", "The moose population is getting too high, the hunters are getting unhappy");
                }

            }
            else if (pop < moosePopulationLowerLimit)
            {
                if (!hasSpawnedPopulationLimit)
                {
                    hasSpawnedPopulationLimit = true;
                    SpawnPopUp("Hunter", "The moose population is getting too low, the hunters are getting unhappy");
                }
                    
            }
            else
            {
                hasSpawnedPopulationLimit = false;
            }
            
        }
        if (shotLastMonthScore != shotLastMonthScorePrev)
        {
            

            if (shotLastMonthScore < -20 )
            {
                if (!hasSpawnedShotLastMonth)
                {
                    hasSpawnedShotLastMonth = true;
                    SpawnPopUp("Hunter", "The hunters are shooting less moose and are losing happiness");
                }

             
            }
            else if (shotLastMonthScore > 20)
            {
                if (!hasSpawnedShotLastMonth)
                {
                    hasSpawnedShotLastMonth = true;
                    SpawnPopUp("Hunter", "The hunters are shooting enough and are getting happier");
                }
            }
            else
            {
                hasSpawnedShotLastMonth = false;
            }
        }
        if (leftOverMooseScore != leftOverMooseScorePrev)
        {
            if (leftOverMooseScore < -20)
            {
                if (!hasSpawnedShotLastMonth)
                {
                    hasSpawnedLeftOverMoose = true;
                    SpawnPopUp("Hunter", "The hunters met their target and are getting happier because of it");
                }
            }
            else if (leftOverMooseScore > 20)
            {
                if (!hasSpawnedShotLastMonth)
                {
                    hasSpawnedLeftOverMoose = true;
                    SpawnPopUp("Hunter", "The hunters didn't meet their hunting targets and they are getting unhappy");
                }

            }
            else
            {
                hasSpawnedLeftOverMoose = false;
            }
        }
    }
    void ForestCalc()
    {

    }


    void NewYear()
    {
        leftOverMooseScore = (JegerManager.instance.currentFemales + JegerManager.instance.currentMales) * -10;
    }

    void UpdatePrevious()
    {
        populationPointsPrev = populationPoints;
        populationDifferencePointsPrev = populationDifferencePoints;
        populationLimitScorePrev = populationLimitScore;
        shotLastMonthScorePrev = shotLastMonthScore;
        leftOverMooseScorePrev = leftOverMooseScore;
    }

    void SpawnPopUp(string title, string text)
    {
        GameObject popUp = Instantiate(PopUp, FeedbackPopUpContainer);
        TextMeshProUGUI Title = popUp.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        Title.text = title;
        TextMeshProUGUI Text = popUp.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        Text.text = text;

    }
}
