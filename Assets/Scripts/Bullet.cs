using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    int damages;
    [SerializeField] float speed = 100;
    [SerializeField] float timeBeforeDestroy = 3f;
    [SerializeField] AnimationCurve curve;
    int impactBeforeDie;
    float time;
    [SerializeField] bool destroyLine;
    bool debrisWillMakeDamages;
    public bool bulletEnnemy = false;

    void Start()
    {
        Destroy(gameObject, timeBeforeDestroy);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (!bulletEnnemy)
        {
            rb.velocity = new Vector3(0, 0, speed * time / 100);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, -(speed * time / 100));
        }
        
        if (!bulletEnnemy)
        {
            Vector3 rot = rb.rotation.eulerAngles;
            rb.rotation = Quaternion.Euler(rot.x + 50, rot.y + 50, rot.z);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bulletEnnemy && other.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            enemy.Damage(damages, destroyLine);
            enemy.SetDebrisMakeDamages(debrisWillMakeDamages);
            CinemachineShake.Instance.ShakeCamera(5f, 1f);
            impactBeforeDie--;

            if (impactBeforeDie == 0)
                Destroy(gameObject);
        }
        else if(bulletEnnemy && other.tag == "Player")
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            player.TakeDommage(1);
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

    public void SetCanDestroyLine(bool thisDestroyLine)
    {
        destroyLine = thisDestroyLine;
    }

    public void SetDebrisMakeDamages(bool isTrue)
    {
        debrisWillMakeDamages = isTrue;
    }
}
