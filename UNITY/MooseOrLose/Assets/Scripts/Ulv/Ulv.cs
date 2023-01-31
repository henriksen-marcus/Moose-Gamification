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

    bool hasBirthed;
    bool hasGrown;

    public bool isLeader;


    void Awake()
    {
        isLeader = false;
        hasBirthed = false;
        hasGrown = true;

        age_years = Random.Range(2, 12) + Random.Range(0, 12);
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
        StartCoroutine(NaturalHungerDrain());
        StartCoroutine(NextDay());
        TimeManager.instance.OnNewYear += NewYearTM;
    }

    // Update is called once per frame
    IEnumerator NaturalHungerDrain()
    {
        hunger -= 30;
        hunger = Mathf.Clamp(hunger, 0, 100);
        yield return new WaitForSeconds(TimeManager.instance.playSpeed);
        StartCoroutine(NaturalHungerDrain());
    }

    public IEnumerator NextDay()
    {

        age_days++;
        if (age_days > 30)
        {
            age_days = 0;
            NextMonth();
        }
        CalculateNewSize();
        yield return new WaitForSeconds(TimeManager.instance.playSpeed);
        StartCoroutine(NextDay());
    }
    public void NextMonth()
    {
        age_months++;
        if (age_months > 12)
        {
            age_months = 0;
            NextYear();
            return;
        }
        CalculateNewSize();
    }

    public void NextYear()
    {
        age_years++;
        if (age_years > 1 && !hasGrown)
        {
            hasGrown = true;
            UlvManager.instance.ChildrenGrowUp();
        }
        CalculateNewSize();
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
            int random = Random.Range(age_years, 25);
            if (random == 25)
            {
                Die();
            }
        }
    }

    public void Die()
    {
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


    void NewYearTM()
    {
        hasBirthed = false;
    }
}
