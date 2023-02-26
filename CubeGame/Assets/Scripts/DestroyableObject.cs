using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public float forceRequired = 5.0f;
    public GameObject burstPrefab;


    private void OnCollisionEnter(Collision col)
    {
        if (col.impulse.magnitude > forceRequired)
        {
            Instantiate(burstPrefab, col.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);   
        }

    }
}
