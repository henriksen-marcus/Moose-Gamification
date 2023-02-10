using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;


public class ElgManager : MonoBehaviour
{
    static public ElgManager instance;




    [Header("Stats")]
    public int elg_population;
    public int elg_males;
    public int elg_females;
    public int elg_children;
    public int carrying_capacity = 500;

    public List<int> elg_population_graph;

    public float male_population_age;

    [Header("Spawning Preferences")]
    [SerializeField] int start_population;


    public GameObject ElgPrefab;
    [SerializeField] public TextMeshProUGUI populationUI;
    [SerializeField] public TextMeshProUGUI malesUI;
    [SerializeField] public TextMeshProUGUI femalesUI;
    [SerializeField] public TextMeshProUGUI childrenUI;


    [Header("List of Spawned GameObjects")]
    [SerializeField] List<GameObject> elg_list = new List<GameObject>(); 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        elg_population = start_population;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject Canvas = GameObject.Find("UI_Canvas");
        populationUI = Canvas.transform.Find("ElgPopulation").transform.Find("Background").transform.Find("Population").GetComponent<TextMeshProUGUI>();
        malesUI = Canvas.transform.Find("ElgPopulation").transform.Find("Background").transform.Find("Male").GetComponent<TextMeshProUGUI>();
        femalesUI = Canvas.transform.Find("ElgPopulation").transform.Find("Background").transform.Find("Female").GetComponent<TextMeshProUGUI>();
        childrenUI = Canvas.transform.Find("ElgPopulation").transform.Find("Background").transform.Find("Children").GetComponent<TextMeshProUGUI>();

        elg_children = 0;

        float loop = start_population;


        // Spawn without children
        if (TimeManager.instance.GetMonth() < 3)
        {
            for (int i = 0; i < loop; i++)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(new Vector3(UnityEngine.Random.Range(-200, 200), 10, UnityEngine.Random.Range(-200, 200)), out hit, 200, 1);

                GameObject go = Instantiate(ElgPrefab, hit.position, Quaternion.identity, transform);
                elg_list.Add(go);
            }
        }
        // Spawn with children
        else
        {
            for (int i = 0; i < loop; i++)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(new Vector3(UnityEngine.Random.Range(-200, 200), 10, UnityEngine.Random.Range(-200, 200)), out hit, 200, 1);

                GameObject go = Instantiate(ElgPrefab, hit.position, Quaternion.identity, transform);
                elg_list.Add(go);

                if (go.GetComponent<Elg>().gender == Gender.Female)
                {
                    Elg script = go.GetComponent<Elg>();
                    int children = script.GetNumberOfChildren();
                    loop -= children;
                    for (int j = 0; j < children; j++)
                    {
                        GameObject go1 = Instantiate(ElgPrefab, go.transform.position, Quaternion.identity, ElgManager.instance.transform);
                        Elg script1 = go1.GetComponent<Elg>();
                        script1.NewBorn();
                        script1.age_months = TimeManager.instance.GetMonth() - 3;
                        script1.SetMother(go.transform);
                        elg_list.Add(go1);
                        
                    }

                }
            }

        }

        PopulationChanged();
        male_population_age = MalePopulationAge();
        elg_population_graph.Add(elg_population);
        TimeManager.instance.OnNewMonth += NewMonth;
    }

    private void Update()
    {
        if (populationUI != null)
            populationUI.SetText(elg_population.ToString());

        if (malesUI != null)
            malesUI.SetText(elg_males.ToString());

        if (femalesUI != null)
            femalesUI.SetText(elg_females.ToString());

        if (childrenUI != null)
            childrenUI.SetText(elg_children.ToString());
    }

    public void MaleBorn()
    {
        elg_males++;
    }

    public void MaleDie()
    {
        elg_males--;
    }

    public void FemaleBorn()
    {
        elg_females++;
    }

    public void FemaleDie()
    {
        elg_females--;
    }

    public void ChildrenGrowUp()
    {
        elg_children--;
    }

    public void ChildrenDie()
    {
        elg_children--;
    }

    public void ChildrenBorn()
    {
        elg_children++;
    }


    public void AddToList(GameObject go)
    {
        PopulationChanged();
        elg_population++;
        elg_list.Add(go);
    }

    public void RemoveFromList(GameObject go)
    {
        PopulationChanged();
        elg_population--;
        elg_list.Remove(go);
    }


    public event Action OnPopulationChanged;
    public void PopulationChanged()
    {
        if (OnPopulationChanged != null)
        {
            OnPopulationChanged();
        }
    }


    float MalePopulationAge()
    {
        float age = 0;
        foreach(GameObject go in elg_list)
        {
            if (go.GetComponent<Elg>().gender == Gender.Male)
            {
                age += go.GetComponent<Elg>().GetAge();
            }
        }
        age /= (float)elg_males;
        return age;

    }


    public float GetMalePopulationAge()
    {
        return male_population_age;
    }

    public void SetMalePopulationAge()
    {
        male_population_age = MalePopulationAge();
    }

    public float GetPopulationGrowthRate()
    {
        return (1 - (((float)elg_population / (float)carrying_capacity) * ((float)elg_population / (float)carrying_capacity))) * 100;
    }


    void NewMonth()
    {
        elg_population_graph.Add(elg_population);
    }
}
