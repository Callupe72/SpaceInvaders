using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int life = 100;
    [SerializeField] float speed = 10f;
    [SerializeField] Rigidbody rb;
    [SerializeField] FracturedEnemy fracturedEnemy;

    void Start()
    {
        float scale = Random.Range(0.75f, 1.25f);
        transform.localScale = Vector3.one * scale;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(0, 0, -speed / 100);
    }

    public void Damage(int damages)
    {
        life -= damages;
        if (life <= 0)
        {
            DestroyThisLine();
        }
    }

    public void Die()
    {
        Instantiate(fracturedEnemy, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void DestroyThisLine()
    {
        transform.parent.transform.parent.GetComponent<EnemySpawnerManager>().DestroyLine(transform.parent);
        Die();
    }

    void Update()
    {
        
    }
}
