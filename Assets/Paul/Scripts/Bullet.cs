using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    int damages;
    [SerializeField] float speed = 100;
    [SerializeField] float timeBeforeDestroy = 3f;

    void Start()
    {
        Destroy(gameObject, timeBeforeDestroy);
    }

    void Update()
    {
        rb.velocity = new Vector3(0,0, speed / 100);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Damage(damages);
            Destroy(gameObject);
        }
    }

    public void SetDamages(int newDamages)
    {
        damages = newDamages;
    }
}
