using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int life = 100;
    [SerializeField] float speed = 10f;
    [SerializeField] Rigidbody rb;
    [SerializeField] FracturedEnemy fracturedEnemy;
    [SerializeField] GameObject scoreDamages;
    bool debrisWillMakeDamages;

    void Start()
    {
        float scale = Random.Range(0.75f, 1.25f);
        transform.localScale = Vector3.one * scale;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(0, 0, -speed / 100);
    }

    public void Damage(int damages, bool destroyLine)
    {
        life -= damages;
        Vector3 spawnPos = transform.position;
        spawnPos = new Vector3(spawnPos.x, spawnPos.y + 1, spawnPos.z);
        ScoreDamages scoreOverEnemy = Instantiate(scoreDamages, spawnPos, Quaternion.identity).GetComponent<ScoreDamages>();
        //scoreOverEnemy.transform.parent = transform;
        scoreOverEnemy.SetText(damages);
        ScoreManager.Instance.AddScore(damages);
        if (life <= 0)
        {
            GetComponentInChildren<Collider>().enabled = false;

            if (destroyLine)
            {
                DestroyThisLine();
            }
            else
            {
                Die();
            }
        }
    }

    public void Die()
    {
        ComboManager.Instance.AddCombo();
        AudioManager.Instance.Play3DSound("ShipExplosion", transform.position);
        int rand = Random.Range(100, 150);
        XPManager.Instance.AddXP(rand);
        Instantiate(fracturedEnemy, transform.position, Quaternion.identity);
        transform.parent.GetComponentInParent<EnemySpawnerManager>().EnemyIsKilled();
        Destroy(gameObject);
    }

    public void SetDebrisMakeDamages(bool isTrue)
    {
        debrisWillMakeDamages = isTrue;
    }

    void DestroyThisLine()
    {
        transform.parent.transform.parent.GetComponent<EnemySpawnerManager>().DestroyLine(transform.parent);
        Die();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (debrisWillMakeDamages)
        {
            if (collision.gameObject.CompareTag("FracturedDebries"))
            {
                FracturedEnemy parent = collision.gameObject.GetComponentInParent<FracturedEnemy>();

                if (parent.GetDebrisMakeDamages())
                {
                    Damage(parent.GetDamagesOnCollision(), false);
                    Destroy(collision.gameObject);
                }
            }
        }

    }
}
