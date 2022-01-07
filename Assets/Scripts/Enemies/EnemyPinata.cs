using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPinata : Enemy
{
    [Header("Pinata")]
    [SerializeField] Transform principalMesh;
    float audioReaction;
    [SerializeField] AnimationCurve destroyCurve;
    [SerializeField] float breakForce = 100;

    void Start()
    {
        
    }


    void Update()
    {
        transform.Rotate(0.1f, 0.15f, 0);
        audioReaction = 1 + factor;
        
    }

    public void ChangeValue()
    {
        principalMesh.localScale *= 1.03f;
        for (int i = 0; i < principalMesh.childCount; i++)
        {
            float rand = Random.Range(0, 0.07f);
            principalMesh.GetChild(i).localScale = principalMesh.GetChild(i).localScale * (1-rand);
        }
        //foreach (Rigidbody rb in principalMesh.transform.GetComponentsInChildren<Rigidbody>())
        //{
        //    Vector3 force = (rb.transform.position - principalMesh.transform.position).normalized * breakForce;
        //    force = new Vector3(100,100,100);
        //    Debug.Log(force);
        //    rb.AddForce(force);
        //}
    }
}
