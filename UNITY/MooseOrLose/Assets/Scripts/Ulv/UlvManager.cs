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
    public int ulv_packs;
    public int ulv_males;
    public int ulv_females;
    public int ulv_children;

    [Header("List of Spawned GameObjects")]
    [SerializeField] public List<List<GameObject>> ulv_list = new List<List<GameObject>>();

    public GameObject Ulv;
    public GameObject UlvWarning;
    // Start is called before the first frame update


    // Update is called once per frame
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        TimeManager.instance.OnNewMonth += NewMonth;
        UlvWarning = GameObject.Find("Screen Canvas").transform.Find("WolfWarning").gameObject;
    }

    void NewMonth()
    {
        if (TimeManager.instance.GetMonth() == 6 || TimeManager.instance.GetMonth() == 2)
        {
            SpawnPack(UnityEngine.Random.Range(2, 4 + 1), new Vector3(UnityEngine.Random.Range(-200, 200), 10, 180));
        }
        else
        {
            for (int i = 0; i < ulv_list.Count; i++)
            {
                for (int j = 0; j < ulv_list[i].Count; j++)
                {
                    Destroy(ulv_list[i][j]);
                }
            }
            
            ulv_list.Clear();
            ulv_population = 0;
            ulv_packs = 0;
            ulv_males = 0;
            ulv_females = 0;
            ulv_children = 0;
        }
        
    }

    public void SpawnPack(int size, Vector3 position)
    {
            List<GameObject> pack = new List<GameObject>();
            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, 200, 1);
            for (int j = 0; j < size; j++)
            {
                GameObject go = Instantiate(Ulv, hit.position, Quaternion.identity, transform);
                pack.Add(go);
                ulv_population++;
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
            ulv_list.Add(pack);
            ulv_packs++;
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

    public void PauseAgents(bool input)
    {
        if (input)
        {
            foreach (List<GameObject> go in ulv_list)
            {
                foreach (GameObject go2 in go)
                {
                    go2.GetComponent<NavMeshAgent>().isStopped = true;
                }
            }
        }
        else
        {
            foreach (List<GameObject> go in ulv_list)
            {
                foreach(GameObject go2 in go)
                {
                    go2.GetComponent<NavMeshAgent>().isStopped = false;
                }
            }
        }
    }

    public void IncreaseUlvPacks()
    {
        ulv_packs++;
    }

    public void DecreaseUlvPacks()
    {
        ulv_packs--;
    }

    private void Update()
    {
        if (ulv_population > 0)
        {
            UlvWarning.SetActive(true);
        }
        else
        {
            UlvWarning.SetActive(false);
        }
    }
}
