using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Gender
{
    Male,
    Female
}

public enum ElgState
{
    Walking,
    Eating,
    Running
}


public class Elg : MonoBehaviour
{

    

    [Header("Statistics")]
    public int age_days;
    public int age_months;
    public int age_years;
    public float weight;
    public int antler_tag_number;

    public float hunger;
    private int daysHungry;
    public ElgState AIstate;




    [Header("Genes")]
    public int natural_size;
    public int natural_antler_size;

    public Gender gender;
    public Transform mother;
    public int number_of_children;
    public bool pregnant;
    public int childrenInBelly;
    private int daysPregnant;
    bool hasGrown;

    [Header("Set In Inspector")]
    public GameObject ElgPrefab;
    public GameObject smallAntler;
    public GameObject bigAntler;


    private GameObject Antlers;
    private bool antlersSpawned = false;
    private bool bigAntlersSpawned = false;
    private bool antlersFelled = false;

    void Awake()
    {
        AIstate = ElgState.Walking;
        hasGrown = true;
        hunger = 100;
        daysHungry = 0;
        Antlers = transform.Find("Antlers").gameObject;
        number_of_children = 0;

        age_years = Random.Range(2, 20);
        age_months = Random.Range(0, 12);
        age_days = Random.Range(0, 30);
        if (Random.Range(0, 2) == 0)
        {
            gender = Gender.Male;
            ElgManager.instance.MaleBorn();
        }
        else
        {
            Antlers.SetActive(false);
            gender = Gender.Female;
            ElgManager.instance.FemaleBorn();
        }

        // Genes
        natural_size = Random.Range(0, 17);
        natural_antler_size = Random.Range(0, 17);
        CalculateNewSize();


    }



    private void Start()
    {
        
        TimeManager.instance.OnNewYear += ShedAntlers; //TEMPORARY
        TimeManager.instance.OnSpringBegin += GrowAntlers;
        GrowAntlers();
        StartCoroutine(NextDay());

        // Pregnancy
        if (gender == Gender.Female)
        {
            if (TimeManager.instance.GetMonth() < 3 || TimeManager.instance.GetMonth() > 8)
            {
                childrenInBelly = GetNumberOfChildren();
                if (childrenInBelly > 0)
                {
                    pregnant = true;
                    int random = Random.Range(-10, 10);
                    daysPregnant = TimeManager.instance.GetMonth() > 8 ? ((TimeManager.instance.GetMonth() - 8) * 30) + random : ((8 - (3 - TimeManager.instance.GetMonth())) * 30) + random;
                }

            }
            else
            {
                pregnant = false;
                childrenInBelly = 0;
                daysPregnant = 0;
            }
        }
       
    }

    public IEnumerator NextDay()
    {
        yield return new WaitForSeconds(TimeManager.instance.playSpeed);
        age_days++;
        if (pregnant)
        {
            daysPregnant++;
            if (daysPregnant > 240)
            {
                daysPregnant = 0;
                BirthChildren();
            }
        }


        if (age_days > 29)
        {
            age_days = 0;
            NextMonth();
        }
        NaturalHungerDrain();
        CalculateNewSize();
        
        StartCoroutine(NextDay());
    }

    public void NextMonth()
    {
        age_months++;
        if (age_months > 12)
        {
            age_months = 0;
            NextYear();
        }

        CalculateNewSize();

        if (gender == Gender.Female)
        {
            if (TimeManager.instance.MatingSeason())
            {
                Pregnate();
            }
        }

        if (age_months > 9 && age_years < 1)
        {
            if (!hasGrown)
            {
                hasGrown = true;
                ElgManager.instance.ChildrenGrowUp();
            }
        }

    }

    public void NextYear()
    {
        age_years++;
        NaturalDeath();
    }

    void NaturalDeath()
    {
        if (age_years > 15)
        {
            int random = Random.Range(age_years, 25);
            if (random == 25)
            {
                Debug.Log("Natural Death");
                Die();
            }
        }
    }

    public void Die()
    {
        if (gender == Gender.Male)
        {
            ElgManager.instance.MaleDie();
        }
        else
        {
            ElgManager.instance.FemaleDie();
        }

        if (age_months < 9 && age_years < 1)
        {
            if (mother != null)
                if (mother.GetComponent<Elg>() != null)
                    mother.GetComponent<Elg>().number_of_children--;


            ElgManager.instance.ChildrenDie();
        }
        ElgManager.instance.RemoveFromList(gameObject);
        Destroy(gameObject);
    }


    void CalculateNewSize()
    {
        float age = (float)age_years + (float)age_months / 12f;


        weight = (0.14f * age * age * age) - (7.16f * age * age) + (109f * age) + 14.18f;
        weight = weight * ((natural_size / 40f) + 0.8f);
        if (gender == Gender.Female)
        {
            weight *= 0.85f;
        }

        transform.localScale = new Vector3(0.3f + (weight / 600f), 0.3f + (weight / 600f), 0.3f + (weight / 600f)) ;
    

        CalculateAntlerTags();
    }

    public void SetMother(Transform _mother)
    {
        mother = _mother;
        mother.GetComponent<Elg>().number_of_children++;
    }

    public void NewBorn()
    {
        age_years = 0;
        age_months = 0;
        age_days = 0;
        hasGrown = false;

        ElgManager.instance.ChildrenBorn();

        CalculateNewSize();
    }

    void Pregnate()
    {
        if (age_years >= 2)
        {
            if (!pregnant)
            {
                childrenInBelly = GetNumberOfChildren();
                if (childrenInBelly > 0)
                    pregnant = true;         
            }

        }
    }

    void BirthChildren()
    {
        for (int i = 0; i < childrenInBelly; i++)
        {

            GameObject go = Instantiate(ElgPrefab, transform.position, Quaternion.identity, ElgManager.instance.transform);
            Elg script = go.GetComponent<Elg>();
            script.NewBorn();
            script.SetMother(transform);
            ElgManager.instance.AddToList(go);
        }
        daysPregnant = 0;
        pregnant=false;
    }


    public int GetNumberOfChildren()
    {
        int num = Random.Range(0, (int)ElgManager.instance.GetPopulationGrowthRate());
        float male_population_age = ElgManager.instance.GetMalePopulationAge();

        float noChildren = GetChanceOfNoChildren(male_population_age);
        // low rate
        if (age_years < 6)
        {
            if (num < noChildren)
            {
                return 0;
            }
            if (num > 85)
            {
                return 2;
            }

        }
        // high rate
        if (age_years >= 6 && age_years < 16)
        {
            if (num < noChildren - 10)
            {
                return 0;
            }
            if (num > 80)
            {
                return 2;
            }

        }

        // No children
        if (age_years > 15)
        {
            return 0;
        }

        return 1;
    }

    void NaturalHungerDrain()
    {

        if (hunger == 0)
        {
            daysHungry++;
            if (daysHungry < 1)
            {
                ChanceToStarve();
            }
        }
        else
        {
            daysHungry = 0;
        }


        switch (TimeManager.instance.GetMonth())
        {
            case 0:
                hunger -= Random.Range(8, 17);
                break;
            case 1:
                hunger -= Random.Range(8, 17);
                break;
            case 2:
                hunger -= Random.Range(10, 26);
                break;
            case 3:
                hunger -= Random.Range(10, 26);
                break;
            case 4:
                hunger -= Random.Range(25, 51);
                break;
            case 5:
                hunger -= Random.Range(25, 51);
                break;
            case 6:
                hunger -= Random.Range(25, 51);
                break;
            case 7:
                hunger -= Random.Range(25, 51);
                break;
            case 8:
                hunger -= Random.Range(10, 26);
                break;
            case 9:
                hunger -= Random.Range(10, 26);
                break;
            case 10:
                hunger -= Random.Range(8, 17);
                break;
            case 11:
                hunger -= Random.Range(8, 17);
                break;
            default:
                hunger -= Random.Range(8,17);
                break;

        }

        hunger = Mathf.Clamp(hunger, 0, 100);
    }

    public float GetAge()
    {
        float age = age_years;
        age += ((float)age_months / 12f);
        return age;
    }


    float GetChanceOfNoChildren(float male_population_age)
    {
        return 29.69f * Mathf.Exp(-0.007f * male_population_age);
    }

    void CalculateAntlerTags()
    {
        int gene = natural_antler_size / 4;
        if (gender == Gender.Female)
        {
            antler_tag_number = 0;
            return;
        }
        if (GetAge() < 1)
        {
            antler_tag_number = 0;
            return;
        }
        if (GetAge() < 2)
        {
            antler_tag_number = gene;
            return;
        }
        if (GetAge() < 6)
        {
            antler_tag_number = (int)f(GetAge()) + gene;
            return;
        }
        if (GetAge() >= 6)
        {
            antler_tag_number = (int)g(GetAge()) + gene;
            return;
        }

        
    }

    void GrowAntlers()
    {
        if (Antlers != null)
        {
            if (age_years < 4 && age_years > 1 && !antlersSpawned)
            {
                antlersSpawned = true;
                bigAntlersSpawned = false;
                foreach (Transform child in Antlers.transform)
                {
                    Destroy(child.gameObject);
                }
                Instantiate(smallAntler, Antlers.transform.position, transform.rotation, Antlers.transform);
            }
            else if (!bigAntlersSpawned && age_years >= 4)
            {
                bigAntlersSpawned = true;
                antlersSpawned = false;
                foreach (Transform child in Antlers.transform)
                {
                    if (child != null)
                        Destroy(child.gameObject);
                }
                Instantiate(bigAntler, Antlers.transform.position, transform.rotation, Antlers.transform);
            }
        }
    }

    void ShedAntlers()
    {
        if (Antlers != null)
        {
            foreach (Transform child in Antlers.transform)
            {
                Destroy(child.gameObject);
            }
            antlersSpawned = false;
            bigAntlersSpawned = false;
        }

    }
    // used to calculate antler size 2-6 years
    float f(float x)
    {
        return (-1.33f * x * x * x) + (16 * x * x) - (53 * x) + 59;
    }
    // used to calculate antler size 6-25 years
    float g(float x)
    {
        return (-0.006f * x * x * x) + (0.3f * x * x) - (4.9f * x) + 45;
    }

    public int NumberOfAntlerTags()
    {
        return antler_tag_number;
    }

    void ChanceToStarve()
    {
        int random = Random.Range(0, 100);
        int extra = 10 * (daysHungry - 1);
        if (age_years < 1)
        {
            if (random + extra > 95)
            {
                Debug.Log("Starved");
                Die();
            }
        }
        else
        {
            if (random + extra > 98)
            {
                Debug.Log("Starved");
                Die();

            }
        }


    }
}
