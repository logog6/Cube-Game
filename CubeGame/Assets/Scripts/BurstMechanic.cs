using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstMechanic : MonoBehaviour
{
    public float duration = 1.25f;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - startTime > duration)
        {
            Destroy(this.gameObject);
        }
    }
}
