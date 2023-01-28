using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElgManager : MonoBehaviour
{

    public GameObject mElg;

    [Header("Spawning Preferences")]
    [SerializeField] int start_population;



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < start_population; i++)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(Random.Range(-200,200), 10, Random.Range(-200,200)), out hit, 200, 1);

            GameObject go = Instantiate(mElg, hit.position, Quaternion.identity);

            if (go.GetComponent<Elg>().gender == Elg.Gender.Female)
            {
                GameObject go2 = Instantiate(mElg, hit.position, Quaternion.identity);

                go2.GetComponent<Elg>().NewBorn();
                go2.GetComponent<Elg>().SetMother(go.transform);
                start_population--;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
