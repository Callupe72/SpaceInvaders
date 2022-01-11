using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using TMPro;

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
        if (CVM != null  && CVM.gameObject.activeInHierarchy)
        {
            return;
        }
        life -= damages * Mathf.RoundToInt(AudioReaction.Instance.GetDropValue());
        ParticlesManager.Instance.SpawnParticles("EnemyTakesDamages", transform, transform.rotation.eulerAngles, false);
        ChangeFactor();
        int rand = RandomXpGiven();
        XPManager.Instance.AddXP(rand);
        Vector3 spawnPos = transform.position;
        spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - 30);
        ScoreDamages scoreOverEnemy = Instantiate(scoreDamages, spawnPos, Quaternion.Euler(Vector3.zero)).GetComponent<ScoreDamages>();
        scoreOverEnemy.transform.localScale = Vector3.one * scoreOverEnemy.transform.localScale.x * Mathf.Clamp(AudioReaction.Instance.GetDropValue(), 1, 100);
        Color oldColor = scoreOverEnemy.GetText().fontMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
        var intensity = (oldColor.r + oldColor.g + oldColor.b);
        var factor =  AudioReaction.Instance.GetDropValue() * 3;
        Color newColor = new Color(oldColor.r * factor, oldColor.g * factor, oldColor.b * factor, oldColor.a);
        scoreOverEnemy.GetText().fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, newColor);
        scoreOverEnemy.GetText().fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, .3f);
        scoreOverEnemy.transform.parent = EnemySpawnerManager.Instance.scoreParent;
        scoreOverEnemy.SetText(damages * Mathf.RoundToInt(AudioReaction.Instance.GetDropValue()));
        ScoreManager.Instance.AddScore(damages * Mathf.RoundToInt(AudioReaction.Instance.GetDropValue()));
        AudioManager.Instance.Play2DSound(soundToPlayOnDamages);
        CinemachineShake.Instance.ShakeCamera(AudioReaction.Instance.GetDropValue(), .5f);
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
        }
        if (EnemySpawnerManager.Instance.GetEnemyStillAlive() == 1)
        {
            CVM.gameObject.SetActive(true);
            SlowMotionManager.Instance.SlowMotion(3f);
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
        AudioManager.Instance.Play2DSound(soundToPlayOnDie);
        Instantiate(fracturedEnemy, transform.position, Quaternion.identity);
        CVM.transform.parent = transform.parent;
        Destroy(CVM, 2f);
        transform.parent.GetComponentInParent<EnemySpawnerManager>().EnemyIsKilled();
        Destroy(gameObject);
    }

    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(1.8f);
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
