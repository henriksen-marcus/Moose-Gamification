using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] GameObject managerParent;
    [SerializeField] GameObject colorManager;
    [SerializeField] GameObject treeManager;

    [Header("Map")]
    public GameObject map;
    [SerializeField] GameObject mapParent;
    public List<GameObject> maplist = new List<GameObject>();

    [Header("Trees")]
    public GameObject treesParent;


    //--------------------


    private void Awake()
    {
        //Spawn the Map
        for (int i = 0; i < 1; i++)
        {
            maplist.Add(Instantiate(map, new Vector3(0, 0, 0), Quaternion.Euler(-90, 0, 0)) as GameObject);
            maplist[i].transform.parent = mapParent.transform;
        }

        //Spawn all Managers into the "Manager folder"
        (Instantiate(colorManager, Vector3.zero, Quaternion.identity) as GameObject).transform.parent = managerParent.transform;
        (Instantiate(treeManager, Vector3.zero, Quaternion.identity) as GameObject).transform.parent = managerParent.transform;
    }
}
