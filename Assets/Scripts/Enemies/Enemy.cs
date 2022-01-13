using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

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

    protected float timeBeforeShow;
    void Start()
    {
        float scale = Random.Range(1.5f, 2f);
        transform.localScale = Vector3.one * scale;
        maxLife = life;
        ChangeFactor();
        if (shipType != EnemySpawnerManager.ShipType.Pinata && shipType != EnemySpawnerManager.ShipType.Shooter)
        {
            foreach (Transform item in transform)
            {
                if (item.GetComponent<DissolveEffect>())
                {
                    item.GetComponent<DissolveEffect>().activeEffectAfterTime = timeBeforeShow;
                    item.GetComponent<DissolveEffect>().ChangeMat(true);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.InPause || GameManager.Instance.currentGameState == GameManager.GameState.Defeat)
            return;
    }

    public void Damage(int damages, bool destroyLine)
    {
        if (CVM != null && CVM.gameObject.activeInHierarchy)
        {
            return;
        }
        life -= damages * Mathf.RoundToInt(AudioReaction.Instance.GetDropValue());
        if (ParticlesManager.Instance.GetCanParticles())
            ParticlesManager.Instance.SpawnParticles("EnemyTakesDamages", transform, transform.rotation.eulerAngles, false);
        ChangeFactor();
        int rand = RandomXpGiven();
        XPManager.Instance.AddXP(rand);
        Vector3 spawnPos = transform.position;
        spawnPos = new Vector3(spawnPos.x, spawnPos.y + 10, spawnPos.z - 30);
        ScoreDamages scoreOverEnemy = Instantiate(scoreDamages, spawnPos, Quaternion.Euler(Vector3.zero)).GetComponent<ScoreDamages>();
        scoreOverEnemy.transform.localScale = Vector3.one * scoreOverEnemy.transform.localScale.x * Mathf.Clamp(AudioReaction.Instance.GetDropValue(), 1, 100);
        Color oldColor = scoreOverEnemy.GetText().fontMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
        var factor = AudioReaction.Instance.GetDropValue() * 1;
        Color newColor = new Color(oldColor.r * factor, oldColor.g * factor, oldColor.b * factor, oldColor.a);
        scoreOverEnemy.GetText().fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, newColor);
        scoreOverEnemy.GetText().fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, .3f);
        scoreOverEnemy.transform.parent = EnemySpawnerManager.Instance.scoreParent;
        scoreOverEnemy.SetText(damages * Mathf.RoundToInt(AudioReaction.Instance.GetDropValue()));
        ScoreManager.Instance.AddScore(damages * Mathf.RoundToInt(AudioReaction.Instance.GetDropValue()));
        AudioManager.Instance.Play2DSound(soundToPlayOnDamages);
        CinemachineShake.Instance.ShakeCamera(AudioReaction.Instance.GetDropValue(), .5f);
        PerfectManager.Instance.SpawnText();
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
            AudioManager.Instance.Play2DSound(soundToPlayOnDie);
            if(ParticlesManager.Instance.GetCanParticles())
                ParticlesManager.Instance.SpawnParticles("PinataDeath", transform, Vector3.zero, false);
            SlowMotionManager.Instance.SlowMotion(2);
            GetComponent<Collider>().enabled = false;
        }
        if (EnemySpawnerManager.Instance.GetEnemyStillAlive() == 1)
        {
            AudioManager.Instance.Play2DSound("ZoomLastEnnemy");

            if (ActiveJuiceManager.Instance.ZoomIsOn)
                CVM.gameObject.SetActive(true);
            SlowMotionManager.Instance.SlowMotion(3f);
            StartCoroutine(WaitBeforeDestroy());
        }
        else
        {
            Die(false);
        }
    }

    void Die(bool isLast)
    {
        ComboManager.Instance.AddCombo();
        AudioManager.Instance.Play2DSound(soundToPlayOnDie);
        if (ActiveJuiceManager.Instance.ExplosionIsOn)
        {
            if (!isLast)
                Instantiate(fracturedEnemy, transform.position, Quaternion.identity);
            else
            {
                FracturedEnemy frac = Instantiate(fracturedEnemy, transform.position, Quaternion.identity).GetComponent<FracturedEnemy>();
                frac.SetBreakForce(1);
            }
        }
        CVM.transform.parent = transform.parent;
        Destroy(CVM, 2f);
        transform.parent.GetComponentInParent<EnemySpawnerManager>().EnemyIsKilled();
        Destroy(gameObject);
    }

    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(1f);
            AudioManager.Instance.Play2DSound(soundToPlayOnDie);
        yield return new WaitForSeconds(.8f);
        Die(true);
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

    public void SetTimeBeforeShow(float time)
    {
        timeBeforeShow = time;
    }
}
