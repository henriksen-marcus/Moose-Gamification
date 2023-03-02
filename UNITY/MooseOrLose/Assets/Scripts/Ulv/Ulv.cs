using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulv : MonoBehaviour
{


    public GameObject ulvPrefab;

    [Header("Statistics")]
    [SerializeField] public float hunger;
    public int age_days;
    public int age_months;
    public int age_years;
    public float weight;

    public Gender gender;
    public Transform leader;
    public List<GameObject> pack;

    [Header("Genes")]
    public int natural_size;
    public int natural_mature_age;

    bool hasGrown;

    public bool isLeader;
    [HideInInspector]
    public bool hasTarget;

    void Awake()
    {
        isLeader = false;
        hasGrown = true;
        hasTarget = false;

        age_years = Random.Range(2, 21);
        age_months = Random.Range(0, 12);
        age_days = Random.Range(0, 30);
        if (Random.Range(0, 2) == 0)
        {
            gender = Gender.Male;
            UlvManager.instance.MaleBorn();
        }
        else
        {
            gender = Gender.Female;
            UlvManager.instance.FemaleBorn();
        }

        // Genes
        natural_size = Random.Range(0, 16);
        natural_mature_age = Random.Range(5, 9);
        CalculateNewSize();


    }

    // Start is called before the first frame update
    void Start()
    {
        hunger = 100;
        TimeManager.instance.OnNewDay += NextDay;
        TimeManager.instance.OnNewDay += NaturalHungerDrain;
    }

    // Update is called once per frame
    void NaturalHungerDrain()
    {

        hunger -= Random.Range(8,15);
        hunger = Mathf.Clamp(hunger, 0, 100);

    }

    public void NextDay()
    { 

        age_days++;
        if (age_days > 29)
        {
            age_days = 0;
            NextMonth();
        }
        CalculateNewSize();
        
    }
    public void NextMonth()
    {
        age_months++;
        if (age_months > 12)
        {
            age_months = 0;
            NextYear();
            
        }
        
    }

    public void NextYear()
    {
        age_years++;
        if (age_years > 1 && !hasGrown)
        {
            hasGrown = true;
            UlvManager.instance.ChildrenGrowUp();
        }
        NaturalDeath();
    }

    void CalculateNewSize()
    {
        float age = (float)age_years + (float)age_months / 12f;


        weight = (0.01f * age * age * age) - (0.53f * age * age) + (7.69f * age) + 1.29f;
        weight = weight * ((natural_size / 40f) + 0.8f);
        transform.localScale = new Vector3(0.3f + (weight / 600f), 0.3f + (weight / 600f), 0.3f + (weight / 600f));

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

    public void Die()
    {
        if (isLeader)
        {
            if (pack.Count > 1)
            {
                int newLeader = 0;
                for (int i = 0; i < pack.Count; i++)
                {
                    if (!pack[i].GetComponent<Ulv>().isLeader)
                    {
                        newLeader = i;
                    }
                }
                pack[newLeader].GetComponent<Ulv>().isLeader = true;
            }
            else
            {
                UlvManager.instance.DecreaseUlvPacks();
            }
        }


        if (gender == Gender.Male)
        {
            UlvManager.instance.MaleDie();
        }
        else
        {
            UlvManager.instance.FemaleDie();
        }

        if (age_years < 2)
        {
            UlvManager.instance.ChildrenDie();
        }
        UlvManager.instance.RemoveFromList(gameObject);
        Destroy(gameObject);
    }

    public void loneWolf()
    {
        isLeader = true;
        UlvManager.instance.IncreaseUlvPacks();
    }



}
