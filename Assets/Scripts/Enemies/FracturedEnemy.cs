using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class FracturedEnemy : MonoBehaviour
{
    [SerializeField] float breakForce;
    [SerializeField] int damagesOnCollision = 10;
    bool debrisMakeDamages;

    void Start()
    {
        Destroy(gameObject, 5);
        foreach (Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce / 100000;
            rb.AddForce(force);

            DissolveEffect dissolve = rb.GetComponent<DissolveEffect>();
            dissolve.activeEffectAfterTime = 1f;
            dissolve.ChangeMat(false);
        }
    }

    public int GetDamagesOnCollision()
    {
        return damagesOnCollision;
    }
    public void SetDebrisMakeDamages(bool isTrue)
    {
        debrisMakeDamages = isTrue;
    }
    public bool GetDebrisMakeDamages()
    {
        return debrisMakeDamages;
    }

    public void SetBreakForce(float force)
    {
        breakForce = force;
    }
}
