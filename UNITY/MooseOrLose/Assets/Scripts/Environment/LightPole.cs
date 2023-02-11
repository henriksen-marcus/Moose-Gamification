using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightPole : MonoBehaviour
{

    [SerializeField] Light lightComp;

    // Start is called before the first frame update
    void Start()
    {
        lightComp = GetComponent<Light>();
    }

    public bool IsOn()
    {
        return lightComp.enabled;
    }

    public void Switch(bool state)
    {
        lightComp.enabled = state;
    }
}
