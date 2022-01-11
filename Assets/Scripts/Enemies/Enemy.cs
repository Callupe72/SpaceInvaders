using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float life = 100;
    protected float maxLife;
    protected float factor;
    [SerializeField] string soundToPlayOnDamages;
    [SerializeField] string soundToPlayOnDie;
    [Header("Die")]
    [SerializeField] FracturedEnemy fracturedEnemy;
    [SerializeField] GameObject scoreDamages;
    [Header("Random")]
    [SerializeField] int randomXpGivenMin = 100;
    [SerializeField] int randomXpGivenMax = 150;
    bool debrisWillMakeDamages;
    [Header("Cam")]
    public CinemachineVirtualCamera CVM = null;

    [HideInInspector] public EnemySpawnerManager.ShipType shipType;
    void Start()
    {
        float scale = Random.Range(0.75f, 1.25f);
        transform.localScale = Vector3.one * scale;
        maxLife = life;
        ChangeFactor();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.InPause || GameManager.Instance.currentGameState == GameManager.GameState.Defeat)
            return;
    }

    public void Damage(int damages, bool destroyLine)
    {
        if (CVM != null  && CVM.enabled)
        {
            return;
        }
        life -= damages;
        ParticlesManager.Instance.SpawnParticles("EnemyTakesDamages", transform, transform.rotation.eulerAngles, false);
        ChangeFactor();
        int rand = RandomXpGiven();
        XPManager.Instance.AddXP(rand);
        Vector3 spawnPos = transform.position;
        spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - 30);
        ScoreDamages scoreOverEnemy = Instantiate(scoreDamages, spawnPos, Quaternion.Euler(Vector3.zero)).GetComponent<ScoreDamages>();
        scoreOverEnemy.transform.parent = EnemySpawnerManager.Instance.scoreParent;
        scoreOverEnemy.SetText(damages);
        ScoreManager.Instance.AddScore(damages);
        AudioManager.Instance.Play3DSound(soundToPlayOnDamages, transform.position);
        if (life <= 0)
        {
            GetComponentInChildren<Collider>().enabled = false;

            if (destroyLine)
            {
                DestroyThisLine();
            }
            else
            {
                PrepareToDie();
            }
        }
    }

    void ChangeFactor()
    {
        factor = 1 - (life / maxLife);
        if (GetComponent<EnemyPinata>())
            GetComponent<EnemyPinata>().ChangeValue();
    }

    public void PrepareToDie()
    {
        if (shipType == EnemySpawnerManager.ShipType.Pinata)
        {
            ParticlesManager.Instance.SpawnParticles("PinataDeath", transform, Vector3.zero, false);
            SlowMotionManager.Instance.SlowMotion(2);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(WaitBeforeDestroy());
        }
        else if (EnemySpawnerManager.Instance.GetEnemyStillAlive() == 1)
        {
            CVM.enabled = true;
            SlowMotionManager.Instance.SlowMotion(2f);
            StartCoroutine(WaitBeforeDestroy());
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        ComboManager.Instance.AddCombo();
        AudioManager.Instance.Play3DSound(soundToPlayOnDie, transform.position);
        Instantiate(fracturedEnemy, transform.position, Quaternion.identity);
        CVM.transform.parent = transform.parent;
        Destroy(CVM, 2f);
        transform.parent.GetComponentInParent<EnemySpawnerManager>().EnemyIsKilled();
        Destroy(gameObject);
    }

    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(1f);
        Die();
    }

    public void SetDebrisMakeDamages(bool isTrue)
    {
        debrisWillMakeDamages = isTrue;
    }

    void DestroyThisLine()
    {
        transform.parent.transform.parent.GetComponent<EnemySpawnerManager>().DestroyLine(transform.parent);
        PrepareToDie();
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

    public int RandomXpGiven()
    {
        return Random.Range(randomXpGivenMin, randomXpGivenMax);
    }
}
