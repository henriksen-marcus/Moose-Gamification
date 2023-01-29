using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class UlvManager : MonoBehaviour
{

    static public UlvManager instance;
    [Header("Stats")]
    public int ulv_population;
    public int ulv_males;
    public int ulv_females;
    public int ulv_children;


    [Header("List of Spawned GameObjects")]
    [SerializeField] List<List<GameObject>> ulv_list = new List<List<GameObject>>();

    public GameObject Ulv;
    // Start is called before the first frame update


    // Update is called once per frame
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        int numberOfPacks = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < numberOfPacks; i++)
        {
            // Spawn
            int wolvedInPack = UnityEngine.Random.Range(2, 4);
            List<GameObject> pack = new List<GameObject>();
            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(UnityEngine.Random.Range(-200, 200), 10, UnityEngine.Random.Range(-200, 200)), out hit, 200, 1);
            for (int j = 0; j < wolvedInPack; j++)
            {
                GameObject go = Instantiate(Ulv, hit.position, Quaternion.identity, transform);
                pack.Add(go);
            }


            // Decide Leader
            int best = 0;
            int bestSize = 0;
            for (int j = 0; j < pack.Count; j++)
            {
                int wolfsize = pack[j].GetComponent<Ulv>().natural_size;
                if (wolfsize > bestSize && pack[j].GetComponent<Ulv>().gender == Gender.Male)
                {
                    best = j;
                    bestSize = wolfsize;
                }
            }
            pack[best].GetComponent<Ulv>().isLeader = true;

            for (int j = 0; j < pack.Count; j++)
            { 
                if (j != best)
                {
                    pack[j].GetComponent<Ulv>().leader = pack[best].transform;
                }
                pack[j].GetComponent<Ulv>().pack = pack;
            }


        }
    }


    public void MaleBorn()
    {
        ulv_males++;
    }

    public void MaleDie()
    {
        ulv_males--;
    }

    public void FemaleBorn()
    {
        ulv_females++;
    }

    public void FemaleDie()
    {
        ulv_females--;
    }

    public void ChildrenGrowUp()
    {
        ulv_children--;
    }

    public void ChildrenDie()
    {
        ulv_children--;
    }

    public void ChildrenBorn()
    {
        ulv_children++;
    }


    public void AddToList(List<GameObject> pack)
    {
        PopulationChanged();
        ulv_population += pack.Count;
        ulv_list.Add(pack);
    }

    public void RemoveFromList(GameObject wolf)
    {
        PopulationChanged();
        ulv_population--; 

        for (int i = 0; i < ulv_list.Count; i++)
        {
            if (ulv_list[i].Contains(wolf))
            {
                ulv_list[i].Remove(wolf);
                if (ulv_list[i].Count == 0)
                {
                    ulv_list.RemoveAt(i);
                }
            }
        }
        
    }

    public event Action OnUlvPopulationChanged;
    public void PopulationChanged()
    {
        if (OnUlvPopulationChanged != null)
        {
            OnUlvPopulationChanged();
        }
    }
}