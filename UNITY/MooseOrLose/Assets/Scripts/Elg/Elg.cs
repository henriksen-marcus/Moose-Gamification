using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Elg : MonoBehaviour
{
    public enum Gender
    {
        Male,
        Female
    }

    public GameObject ElgPrefab;

    [Header("Statistics")]
    public int age_days;
    public int age_months;
    public int age_years;
    public int weight;

    public Gender gender;
    public Transform mother;

    [Header("Genes")]
    public int natural_size;
    public int natural_mature_age;

    bool hasBirthed;
    bool hasGrown;


    void Awake()
    {

        hasBirthed = false;
        hasGrown = true;

        age_years = Random.Range(2, 12) + Random.Range(0, 12);
        age_months = Random.Range(0, 12);
        age_days = Random.Range(0, 30);
        if (Random.Range(0, 2) == 0)
        {
            gender = Gender.Male;
            ElgManager.instance.MaleBorn();
        }
        else
        {
            gender = Gender.Female;
            ElgManager.instance.FemaleBorn();
        }

        // Genes
        natural_size = Random.Range(0, 16);
        natural_mature_age = Random.Range(5,9);
        CalculateNewSize();

       


    }



    private void Start()
    {
        GetComponent<NavMeshAgent>().speed = 4 * (1/TimeManager.instance.playSpeed);
        StartCoroutine(NextDay());
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

        if (gender == Gender.Female)
        {
            if (TimeManager.instance.MatingSeason())
            {
                BirthChild();
            }
        }
    }

    public void NextYear()
    {
        hasBirthed = false;
        age_years++;
        if (age_years > 1 && !hasGrown)
        {
            hasGrown = true;
            ElgManager.instance.ChildrenGrowUp();
        }
        CalculateNewSize();
        NaturalDeath();
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
        ElgManager.instance.DecreasePopulation();
        if (gender == Gender.Male)
        {
            ElgManager.instance.MaleDie();
        }
        else
        {
            ElgManager.instance.FemaleDie();
        }

        if (age_years < 2)
        {
            ElgManager.instance.ChildrenDie();
        }
        Destroy(gameObject);
    }


    void CalculateNewSize()
    {
        // Dont grow if you reached mature age
        if (age_years > natural_mature_age)
        {
            weight = (10 + (natural_size / 2)) * 10 + (10 + (natural_size / 2)) * (natural_mature_age / 2) * 3;
        }
        // First Year grow faster
        else if (age_years < 1)
        {
            weight = (10 + (natural_size / 2)) + (10 + (natural_size / 2)) * (age_years + (age_months / 12)) * 10;
        }
        // Slower growth after first year
        else if (age_years > 0)
        {
            weight = (10 + (natural_size / 2)) * 10 + (10 + (natural_size / 2)) * (age_years + ((age_months / 12)) / 2) * 3;
        }

        transform.localScale = new Vector3(0.3f + (weight / 600f), 0.3f + (weight / 600f), 0.3f + (weight / 600f)) ;
        
    }

    public void SetMother(Transform _mother)
    {
        mother = _mother;
    }

    public void NewBorn()
    {
        age_years = 0;
        age_months = 0;
        age_days = 0;
        hasGrown = false;
        if (Random.Range(0, 2) == 0)
        {
            gender = Gender.Male;
        }
        else
        {
            gender = Gender.Female;
        }
        ElgManager.instance.ChildrenBorn();

        CalculateNewSize();
    }

    void BirthChild()
    {
        if (age_years >= 2)
        {
            if (!hasBirthed)
            {
                GameObject go = Instantiate(ElgPrefab, transform.position, Quaternion.identity);
                ElgManager.instance.IncreasePopulation();
                go.GetComponent<Elg>().NewBorn();
                go.GetComponent<Elg>().SetMother(transform);

                bool twins = Random.Range(0, 6) == 5;

                if (twins)
                {
                    GameObject go2 = Instantiate(ElgPrefab, transform.position, Quaternion.identity);
                    ElgManager.instance.IncreasePopulation();
                    go2.GetComponent<Elg>().NewBorn();
                    go2.GetComponent<Elg>().SetMother(transform);

                }

                hasBirthed = true;
            }

        }
    }

    
}
