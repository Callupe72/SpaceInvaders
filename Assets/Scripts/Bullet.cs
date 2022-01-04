using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    int damages;
    [SerializeField] float speed = 100;
    [SerializeField] float timeBeforeDestroy = 3f;
    [SerializeField] GameObject scoreDamages;
    [SerializeField] AnimationCurve curve;
    int impactBeforeDie;
    float time;


void Start()
    {
        Destroy(gameObject, timeBeforeDestroy);
    }

    void Update()
    {
        time += Time.deltaTime;
        rb.velocity = new Vector3(0, 0, speed * time / 100);
        Vector3 rot = rb.rotation.eulerAngles;
        rb.rotation = Quaternion.Euler(rot.x + 50, rot.y + 50, rot.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Damage(damages);
            Vector3 spawnPos = collision.transform.position;
            spawnPos = new Vector3(spawnPos.x, spawnPos.y + 1, spawnPos.z);
            ScoreDamages scoreOverEnemy = Instantiate(scoreDamages, spawnPos, Quaternion.identity).GetComponent<ScoreDamages>();
            scoreOverEnemy.SetText(damages);
            ScoreManager.Instance.AddScore(damages);
            CinemachineShake.Instance.ShakeCamera(0.75f, .1f);
            int rand = Random.Range(100, 150);
            XPManager.Instance.AddXP(rand);
            impactBeforeDie--;

            if (impactBeforeDie == 0)
                Destroy(gameObject);
        }
    }

    public void SetDamages(int newDamages)
    {
        damages = newDamages;
    }
    public void SetImpactBeforeDie(int newImpact)
    {
        impactBeforeDie = newImpact;
    }
}
