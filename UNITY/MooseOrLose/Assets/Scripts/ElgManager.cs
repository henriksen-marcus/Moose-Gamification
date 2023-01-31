using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


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
            NavMesh.SamplePosition(new Vector3(Random.Range(-200,200), 10, Random.Range(-200,200)), out hit, 200, 1);

            Instantiate(ElgPrefab, hit.position, Quaternion.identity, transform);
        }
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

    public void IncreasePopulation()
    {
        elg_population++;
    }

    public void IncreasePopulation(int num)
    {
        elg_population += num;
    }

    public void DecreasePopulation()
    {
        elg_population--;
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
}
