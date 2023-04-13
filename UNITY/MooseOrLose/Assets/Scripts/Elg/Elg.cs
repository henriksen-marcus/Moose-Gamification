using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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


public class Elg : ClickableObject
{
    [Header("Statistics")]
    public int age_days;
    public int age_months;
    public int age_years;
    public float weight;
    public int antler_tag_number;

    public float hunger;
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

    [SerializeField] List<GameObject> antlers = new List<GameObject>();

    [Header("Set In Inspector")]
    public GameObject ElgPrefab;

    public GameObject AdultMale;
    public GameObject AdultFemale;
    public GameObject Child;


    private GameObject Antlers;
#pragma warning disable 0414
    private bool antlersSpawned = false;
#pragma warning restore 0414

    private bool dead = false;
    [HideInInspector]
    public bool hasPartner;
    public bool hasMated;

    private Vector3 WinterLocation;
    private Vector3 SummerLocation;



    void Awake()
    {
        // set default values
        AIstate = ElgState.Walking;
        hasGrown = true;
        hunger = 100;
        Antlers = transform.Find("Antlers").gameObject;
        number_of_children = 0;

        age_years = Random.Range(2, 20);
        age_months = Random.Range(0, 12);
        age_days = Random.Range(0, 30);
        if (Random.Range(0, 2) == 0)
        {
            gender = Gender.Male;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name != "Antlers")
                {
                    Destroy(child.gameObject);
                }
            }
            Instantiate(AdultMale, transform);
            foreach (Transform child in transform)
            {
                child.transform.localPosition = new Vector3(0, 1, 0);
            }
            ElgManager.instance.MaleBorn();
        }
        else
        {
            Antlers.SetActive(false);
            gender = Gender.Female;

            foreach(Transform child in transform)
            {
                if (child.gameObject.name != "Antlers")
                {
                    Destroy(child.gameObject);
                }
            }
            Instantiate(AdultFemale, transform);
            foreach (Transform child in transform)
            {
                child.transform.localPosition = new Vector3(0, 1, 0);
            }
            ElgManager.instance.FemaleBorn();
        }

        // Genes
        natural_size = Random.Range(0, 17);
        natural_antler_size = Random.Range(0, 17);
        CalculateNewSize();
        pregnant = false;
        childrenInBelly = 0;
        daysPregnant = 0;
        hasPartner = false;
        hasMated = false;
        
        if (gameObject.GetComponent<Outline>() == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 7f;
            outline.enabled = false;
        }

        GetComponent<NavMeshAgent>().speed = 5;
    }
    
    private void Start()
    {    
        TimeManager.instance.OnNewYear += ShedAntlers; //TEMPORARY
        TimeManager.instance.OnSpringBegin += GrowAntlers;
        GrowAntlers();
        TimeManager.instance.OnNewDay += NextDay;


        WinterLocation = transform.position;
        SummerLocation = transform.position;
        while(WinterLocation.y >= -1f)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(Random.Range(-200, 200), 10, Random.Range(-200, 200)), out hit, 200, 1);
            if (hit.hit)
                WinterLocation = hit.position;
        }

        if (gameObject.GetComponent<Outline>() == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 7f;
            outline.enabled = false;
            outline.OutlineMode = Outline.Mode.OutlineHidden;
        }
        
        
    }

    private void Update()
    {
        if (!IsSelected) ToggleOutline(false);
    }

    public override ClickableObjectInfo GetClickInfo()
    {
        ClickableObjectInfo returnObject = new ClickableObjectInfo();
        returnObject.type = ClickableObjectInfo.ObjectType.Moose;
        returnObject.age_years = age_years;
        returnObject.age_months = age_months;
        returnObject.age_years = age_years;
        returnObject.weight = (int)weight;

        returnObject.gender = gender;
        switch(gender)
        {
            case Gender.Male:
                returnObject.antler_tags = antler_tag_number;
                break;
            case Gender.Female:
                returnObject.pregnant = pregnant;
                returnObject.days_pregnant = daysPregnant;
                returnObject.children_in_belly = childrenInBelly;
                break;
            default:
                break;
        }

        return returnObject;
    }
    public void SpawnPregnant()
    {
        // Pregnancy
        if (gender == Gender.Female && age_years > 1)
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

    public Vector3 GetWinterLocation() { return WinterLocation; }
    public Vector3 GetSummerLocation() { return SummerLocation; }

    public void NextDay()
    {
        if (!dead)
        {
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
        }

        
    }

    public void NextMonth()
    {
        age_months++;
        if (age_months > 12)
        {
            age_months = 0;
            NextYear();
        }

        if (age_years > 0)
        {
            if (!hasGrown)
            {
                hasGrown = true;
                foreach (Transform child in transform)
                {
                    if (child.gameObject.name != "Antlers")
                    {
                        Destroy(child.gameObject);
                    }
                }
                if(gender == Gender.Male)
                {
                    Instantiate(AdultMale, transform);
                    foreach (Transform child in transform)
                    {
                        child.transform.localPosition = new Vector3(0, 1, 0);
                    }
                }
                    
                else
                {
                    Instantiate(AdultFemale, transform);
                    foreach (Transform child in transform)
                    {
                        child.transform.localPosition = new Vector3(0, 1, 0);
                    }
                }
                    
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
            int random = Random.Range(age_years, 26);
            if (random == 25)
            {

                Die();
            }
        }
    }

    public void SetSummerDestination(Vector3 dest)
    {
        SummerLocation = dest;
    }

    public void Die()
    {
        if (!dead)
        {
            dead = true;
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
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Antlers")
            {
                Destroy(child.gameObject);
            }
        }
        Instantiate(Child, transform);
        foreach (Transform child in transform)
        {
            child.transform.localPosition = Vector3.zero;
        }
        ElgManager.instance.ChildrenBorn();

        CalculateNewSize();
    }

    public void pregnate(Elg father)
    {

        if (!pregnant)
        {
            childrenInBelly = GetNumberOfChildren(father.GetAge());
            if (childrenInBelly > 0)
            {
                pregnant = true;         
            }
            hasMated = true;
        }

        
    }

    void BirthChildren()
    {
        for (int i = 0; i < childrenInBelly; i++)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position, out hit, 200, 1);
            if (!hit.hit) return;
            GameObject go = Instantiate(ElgPrefab, hit.position, Quaternion.identity, ElgManager.instance.transform);
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


    public int GetNumberOfChildren(float fatherAge)
    {
        int num = Random.Range(0, (int)ElgManager.instance.GetPopulationGrowthRate());

        float noChildren = GetChanceOfNoChildren(fatherAge);
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
        
        if (gender == Gender.Female)
        {
            antler_tag_number = 0;
            return;
        }
        antler_tag_number = (int)g(GetAge());



    }

    void GrowAntlers()
    {
        if (antler_tag_number == 0)
        {
            return;
        }
        if (Antlers != null)
        {            
            antlersSpawned = true;
            foreach (Transform child in Antlers.transform)
            {
                Destroy(child.gameObject);
            }
            if (antlers.Count > antler_tag_number - 3)
            {
                if (hasGrown)
                {
                    int num = antler_tag_number;
                    if (num < 12)
                    {
                        num = 12;
                    }
                    Instantiate(antlers[num - 3], Antlers.transform.position, transform.rotation, Antlers.transform);
                }
                else
                {
                    int num = antler_tag_number;
                    if (num > 11)
                    {
                        num = 11;
                    }
                    Instantiate(antlers[num - 3], Antlers.transform.position, transform.rotation, Antlers.transform);
                }
                   
            }                
            else
            {
                Debug.Log("FailedToSpawnAntlers");
                Debug.Log(antler_tag_number);
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
        return hasGrown ? Mathf.Clamp((7.2f + (2 * (natural_antler_size / 16))) * (1.85f * Mathf.Pow(0.7f,x)) * Mathf.Pow(x, 1.44f),12,30) : Mathf.Clamp((7.2f + (2 * (natural_antler_size / 16))) * (1.85f * Mathf.Pow(0.7f, x)) * Mathf.Pow(x, 1.44f), 3, 11);
    }

    public int NumberOfAntlerTags()
    {
        return antler_tag_number;
    }

    private void OnDestroy()
    {
        TimeManager.instance.OnNewYear -= ShedAntlers; //TEMPORARY
        TimeManager.instance.OnSpringBegin -= GrowAntlers;
        TimeManager.instance.OnNewDay -= NextDay;
    }
}
