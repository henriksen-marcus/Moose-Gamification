using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulv : MonoBehaviour
{
    public float hunger;

    // Start is called before the first frame update
    void Start()
    {
        hunger = 100;
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 5 * (1 / TimeManager.instance.playSpeed);
        StartCoroutine(NaturalHungerDrain());
    }

    // Update is called once per frame
    IEnumerator NaturalHungerDrain()
    {
        hunger -= 2;
        hunger = Mathf.Clamp(hunger, 0, 100);
        yield return new WaitForSeconds((float)TimeManager.instance.playSpeed / 10f);
        StartCoroutine(NaturalHungerDrain());
    }
}
