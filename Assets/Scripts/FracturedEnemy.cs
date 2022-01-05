using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FracturedEnemy : MonoBehaviour
{
    [SerializeField] float timeBeforeBeingDestroyed;
    [SerializeField] float breakForce;
    [SerializeField] int damagesOnCollision = 10;
    bool debrisMakeDamages;
    bool canScaleDown;
    void Start()
    {
        Destroy(gameObject, timeBeforeBeingDestroyed);
        //StartCoroutine(WaitBeforeScaleDown());
        foreach (Transform transform in transform)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }
    }

    IEnumerator WaitBeforeScaleDown()
    {
        yield return new WaitForSeconds(timeBeforeBeingDestroyed / 2);
        canScaleDown = true;
    }


    void Update()
    {
        if (canScaleDown)
            transform.DOScale(0, timeBeforeBeingDestroyed / 2);
            //foreach (Transform transform in transform)
            //{
            //    transform.DOScale(0, timeBeforeBeingDestroyed / 2);
            //}
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
}
