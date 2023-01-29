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

        InvokeRepeating("NaturalHungerDrain", 0, (float)TimeManager.instance.playSpeed / 10f);
    }

    // Update is called once per frame
    void NaturalHungerDrain()
    {
        hunger -= 3;
        hunger = Mathf.Clamp(hunger, 0, 100);
    }
}
