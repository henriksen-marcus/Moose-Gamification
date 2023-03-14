using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackPopUp : MonoBehaviour
{
    private float timer;
    public float lifetime;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            Destroy(transform.gameObject);
        }
    }
}
