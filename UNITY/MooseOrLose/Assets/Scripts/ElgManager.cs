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
    }

    // Start is called before the first frame update
    void Start()
    {
        elg_children = 0;
        elg_population = start_population;
        float loop = start_population;
        for (int i = 0; i < loop; i++)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(UnityEngine.Random.Range(-200,200), 10, UnityEngine.Random.Range(-200,200)), out hit, 200, 1);

            GameObject go = Instantiate(ElgPrefab, hit.position, Quaternion.identity, transform);
            elg_list.Add(go);
        }
        PopulationChanged();
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
}
