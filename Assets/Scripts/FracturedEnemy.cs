using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedEnemy : MonoBehaviour
{
    [SerializeField] float timeBeforeBeingDestroyed;
    [SerializeField] float breakForce;
    void Start()
    {
        Destroy(gameObject, timeBeforeBeingDestroyed);
        foreach (Transform transform in transform)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }
    }
}
