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


    //Forest
    [SerializeField] private int densityScore;
    [SerializeField] private int averageTreeScore;
    [SerializeField] private int averageAgeScore;

    private int densityScorePrev;
    private int averageTreeScorePrev;
    private int averageAgeScorePrev;

    private bool hasSpawnedPopulationPopUp1;
    private bool hasSpawnedPopulationPopUp2;

    private bool hasSpawnedPopulationLimit;
    private bool hasSpawnedShotLastMonth;
#pragma warning disable 0414
    private bool hasSpawnedLeftOverMoose;
#pragma warning restore 0414

    private bool hasSpawnedForest1;
    private bool hasSpawnedForest2;
    private bool hasSpawnedForest3;
    private bool hasSpawnedForest4;
    private bool hasSpawnedForest5;
    private bool hasSpawnedForest6;

    private List<int> populationCounts;
    private int weekDay;

    private HoverableUIElement MooseToolTip;
    private HoverableUIElement HunterToolTip;
    private HoverableUIElement ForestToolTip;

    private void Awake()
    {
        MooseToolTip = transform.Find("Vertical").Find("MooseBackground").GetComponent<HoverableUIElement>();
        HunterToolTip = transform.Find("Vertical").Find("HuntersBackground").GetComponent<HoverableUIElement>();
        ForestToolTip = transform.Find("Vertical").Find("ForestBackground").GetComponent<HoverableUIElement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        populationCounts = new List<int>();

        TimeManager.Instance.OnNewDay += NewDay;
        TimeManager.Instance.OnNewYear += NewYear;
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
        hasSpawnedForest1 = false;
        hasSpawnedForest2 = false;
        hasSpawnedForest3 = false;
        hasSpawnedForest4 = false;
        hasSpawnedForest5 = false;
        hasSpawnedForest6 = false;
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

        ToolTips();
        UpdatePrevious();
    }

    void MooseCalc()
    {
        populationPoints = ElgManager.instance.elg_population;

        int size = populationCounts.Count;
        if (size > 2)
        {
            populationDifferencePoints = populationCounts[size - 1] - populationCounts[size - 2];
            populationDifferencePoints *= 3;
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
            populationLimitScore = (moosePopulationLowerLimit - moosePop) * -3;
        }
        else if (moosePop > moosePopulationHigherLimit)
        {
            populationLimitScore = (moosePop - moosePopulationHigherLimit) * -3;
        }
        else
        {
            populationLimitScore = 0;
        }

        shotLastMonthScore = JegerManager.instance.shotLastMonth * 3;
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
        int averageDensity = ForestManager.instance.forestDensityAverage[ForestManager.instance.forestDensityAverage.Count - 1];
        densityScore = Mathf.FloorToInt((1 - ((float)averageDensity / 500f)) * 100);

        averageTreeScore = Mathf.FloorToInt((float)ForestManager.instance.forestQuantityAverage[ForestManager.instance.forestQuantityAverage.Count - 1] * 0.025f);

        float averageAge = ForestManager.instance.forestTreeAgeAverage[ForestManager.instance.forestTreeAgeAverage.Count - 1];
        if (averageAge == 0)
        {
            averageAge = 250;
        }
        float score =  map(averageAge,0,500,100,10);
        averageAgeScore = Mathf.FloorToInt(Mathf.Clamp(score, 0f, 100f));

        ForestScore = densityScore + averageTreeScore + averageAgeScore;
        ForestBackground.GetComponent<Image>().color = gradient.Evaluate((float)ForestScore / 300f);
        ForestPopUp();
    }
    void ForestPopUp()
    {
        if (densityScore != densityScorePrev)
        {           
            if (densityScore < 60)
            {
                if (!hasSpawnedForest1)
                {
                    hasSpawnedForest1 = true;
                    SpawnPopUp("Forest", "The forest density is getting too low");
                }

            }
            else if (densityScore > 60)
            {
                hasSpawnedForest1 = false;
            }


            if (densityScore > 150)
            {
                if (!hasSpawnedForest4)
                {
                    hasSpawnedForest4 = true;
                    SpawnPopUp("Forest", "The forest density is growing, the forest is happy");
                }

            }
            else if (densityScore < 150)
            {
                hasSpawnedForest4 = false;
            }

        }
        densityScorePrev = densityScore;

        if(averageTreeScore != averageTreeScorePrev)
        {
            if (averageTreeScore < 20)
            {
                if (!hasSpawnedForest2)
                {
                    hasSpawnedForest2 = true;
                    SpawnPopUp("Forest", "The forests has too few trees");
                }

            }
            else if (averageTreeScore > 20)
            {
                hasSpawnedForest2 = false;
            }
            if (averageTreeScore > 80)
            {
                if (!hasSpawnedForest5)
                {
                    hasSpawnedForest5 = true;
                    SpawnPopUp("Forest", "The forests is growing, and it is happy");
                }

            }
            else if (averageTreeScore < 80)
            {
                hasSpawnedForest5 = false;
            }

        }
        averageTreeScorePrev = averageTreeScore;

        if (averageAgeScore != averageAgeScorePrev)
        {
            if (averageAgeScore < 50)
            {
                if (!hasSpawnedForest3)
                {
                    hasSpawnedForest3 = true;
                    SpawnPopUp("Forest", "The forests are old, so growth has been slowed down");
                }

            }
            else if (averageAgeScore > 50)
            {
                hasSpawnedForest3 = false;
            }
            if (averageAgeScore > 100)
            {
                if (!hasSpawnedForest6)
                {
                    hasSpawnedForest6 = true;
                    SpawnPopUp("Forest", "The forests are getting younger, so growth is speeding up!");
                }

            }
            else if (averageAgeScore < 100)
            {
                hasSpawnedForest6 = false;
            }
        }
    }
    public static float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
    void NewYear()
    {
        leftOverMooseScore = (JegerManager.instance.currentFemales + JegerManager.instance.currentMales) * -5;
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

    void ToolTips()
    {
        Color popColor = gradient.Evaluate((float)populationPoints / 200f);

        int popDiff = Mathf.Clamp(populationDifferencePoints, -100,100);
        popDiff += 100;
        Color popDiffColor = gradient.Evaluate((float)popDiff / 200f);


        MooseToolTip.SetText(ColorString("Population: " + populationPoints.ToString(), popColor) +
                             ColorString("Growth: " + populationDifferencePoints.ToString(), popDiffColor));



        int popLimit = Mathf.Clamp(populationLimitScore, -100, 0);
        popLimit += 100;
        Color popLimitColor = gradient.Evaluate((float)popLimit/100f);

        int shot = Mathf.Clamp(shotLastMonthScore, 0, 100);
        Color shotLastMonthColor = gradient.Evaluate((float)shot / 100f);

        int leftOverMoose = Mathf.Clamp(leftOverMooseScore, -100, 0);
        leftOverMoose += 100;
        Color leftOverMooseColor = gradient.Evaluate((float)leftOverMoose/100f);

        HunterToolTip.SetText(ColorString("Base: 150", Color.yellow) +
                              ColorString("Population Control: " + populationLimitScore.ToString(), popLimitColor) +
                              ColorString("Shooting: " + shotLastMonthScore.ToString(), shotLastMonthColor) +
                              ColorString("Meeting targets: " + leftOverMooseScore.ToString(), leftOverMooseColor));

        Color DensityColor = gradient.Evaluate((float)densityScore / 150f);
        Color AgeColor = gradient.Evaluate((float)averageAgeScore / 100f);
        Color TreeAmountColor = gradient.Evaluate((float)averageTreeScore / 80f);

        ForestToolTip.SetText(ColorString("Density Score: " + densityScore.ToString(), DensityColor) + 
                              ColorString("Average Tree Amount: " + averageTreeScore.ToString(), TreeAmountColor) + 
                              ColorString("Tree Age Score: " + averageAgeScore.ToString(), AgeColor));
    }
    string ColorString(string text, Color color)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "\n" + "</color>";
    }
}
