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
            enemy.Damage(damages, destroyLine);
            enemy.SetDebrisMakeDamages(debrisWillMakeDamages);
            CinemachineShake.Instance.ShakeCamera(0.75f, .1f);
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

    public void SetCanDestroyLine(bool thisDestroyLine)
    {
        destroyLine = thisDestroyLine;
    }

    public void SetDebrisMakeDamages(bool isTrue)
    {
        debrisWillMakeDamages = isTrue;
    }
}
