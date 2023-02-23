using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JegerManager : MonoBehaviour
{

    public static JegerManager instance;

    [Header("Jeger GameObject")]
    public GameObject jeger;

    [Header("Spawning")]
    public int jeger_startpopulation = 5;
    public Vector3 spawn_location;

    [Header("Stats")]
    public int jeger_population;

    [Header("Array")]
    [SerializeField] List<GameObject> jeger_list;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < jeger_startpopulation; i++)
        {
            GameObject go = Instantiate(jeger, spawn_location + new Vector3(Random.Range(-10,10), 0 ,Random.Range(-10,10)), Quaternion.identity, transform);
            jeger_list.Add(go);
        }
        jeger_population = jeger_startpopulation;
    }


    public void AddToList(GameObject go)
    {
        jeger_population++;
        jeger_list.Add(go);
    }

    public void RemoveFromList(GameObject go)
    {
        jeger_population--;
        jeger_list.Remove(go);
    }


    public void PauseAgents(bool input)
    {
        if (input)
        {
            foreach (GameObject go in jeger_list)
            {
                go.GetComponent<NavMeshAgent>().isStopped = true;
            }
        }
        else
        {
            foreach (GameObject go in jeger_list)
            {
                go.GetComponent<NavMeshAgent>().isStopped = false;
            }
        }
    }

}
