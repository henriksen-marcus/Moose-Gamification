using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Gender
{
    Male,
    Female
}


public class Elg : MonoBehaviour
{

    public GameObject ElgPrefab;

    [Header("Statistics")]
    public int age_days;
    public int age_months;
    public int age_years;
    public float weight;

    public float hunger;

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
        hunger = 100;

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
        StartCoroutine(NextDay());
        TimeManager.instance.OnNewYear += NewYearTM;
    }

    public IEnumerator NextDay()
    {
        yield return new WaitForSeconds(TimeManager.instance.playSpeed);
        age_days++;
        if (age_days > 30)
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
        age_years++;
        if (!hasGrown)
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

        ElgManager.instance.ChildrenBorn();

        CalculateNewSize();
    }

    void BirthChild()
    {
        if (age_years >= 2)
        {
            if (!hasBirthed)
            {
                int numberOfChildren = GetNumberOfChildren();


                for (int i = 0; i < numberOfChildren; i++)
                {

                    GameObject go = Instantiate(ElgPrefab, transform.position, Quaternion.identity, ElgManager.instance.transform);

                    go.GetComponent<Elg>().NewBorn();
                    go.GetComponent<Elg>().SetMother(transform);
                    ElgManager.instance.AddToList(go);
                }

                hasBirthed = true;
            }

        }
    }

    void NewYearTM()
    {
        hasBirthed = false;
    }


    int GetNumberOfChildren()
    {
        int num = Random.Range(0, 11);
        if (age_years < 6)
        {
            if (num < 3)
            {
                return 0;
            }
            if (num > 8)
            {
                return 2;
            }

        }
        if (age_years > 6 && age_years < 16)
        {
            if (num < 1)
            {
                return 0;
            }
            if (num > 8)
            {
                return 2;
            }

        }
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


}
