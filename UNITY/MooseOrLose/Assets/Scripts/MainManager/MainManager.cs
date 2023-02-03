using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] GameObject managerParent;
    [SerializeField] GameObject colorManager;
    [SerializeField] GameObject forestManager;

    [SerializeField] GameObject timeManager;
    [SerializeField] GameObject mooseManager;
    [SerializeField] GameObject wolfManager;
    [SerializeField] GameObject hunterManager;

    [Header("Map")]
    public GameObject map;
    public GameObject mapParent;
    public List<GameObject> maplist = new List<GameObject>();

    [Header("Forests")]
    public GameObject forestParent;


    //--------------------


    private void Awake()
    {
        //Spawn "Map Folder" to Hirearchy
        Instantiate(mapParent, Vector3.zero, Quaternion.identity);

        //Spawn the "Map" into "Map Folder"
        for (int i = 0; i < 1; i++)
        {
            maplist.Add(Instantiate(map, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject);
            maplist[i].transform.parent = mapParent.transform;
        }

        //Spawn all Managers into the "Manager folder"
        (Instantiate(colorManager, Vector3.zero, Quaternion.identity) as GameObject).transform.parent = managerParent.transform;
        (Instantiate(forestManager, Vector3.zero, Quaternion.identity) as GameObject).transform.parent = managerParent.transform;

        (Instantiate(timeManager, Vector3.zero, Quaternion.identity) as GameObject).transform.parent = managerParent.transform;
        (Instantiate(mooseManager, Vector3.zero, Quaternion.identity) as GameObject).transform.parent = managerParent.transform;
        (Instantiate(wolfManager, Vector3.zero, Quaternion.identity) as GameObject).transform.parent = managerParent.transform;
        (Instantiate(hunterManager, Vector3.zero, Quaternion.identity) as GameObject).transform.parent = managerParent.transform;
    }
}
