using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElgManager : MonoBehaviour
{
    static public ElgManager instance;

    public GameObject ElgPrefab;

    [Header("Stats")]
    public int elg_population;

    [Header("Spawning Preferences")]
    [SerializeField] int start_population;

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
        elg_population = start_population;

        for (int i = 0; i < start_population; i++)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(Random.Range(-200,200), 10, Random.Range(-200,200)), out hit, 200, 1);

            GameObject go = Instantiate(ElgPrefab, hit.position, Quaternion.identity);

            if (go.GetComponent<Elg>().gender == Elg.Gender.Female)
            {
                GameObject go2 = Instantiate(ElgPrefab, hit.position, Quaternion.identity);

                go2.GetComponent<Elg>().NewBorn();
                go2.GetComponent<Elg>().SetMother(go.transform);
                start_population--;
            }
            
        }
    }

    public void IncreasePopulation()
    {
        elg_population++;
    }

    public void DecreasePopulation()
    {
        elg_population--;
    }


}
