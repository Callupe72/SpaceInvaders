using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] Camera cam;
    [Header("Shoot")]
    [SerializeField] int damages = 200;
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] Transform bulletSpawner;
    [SerializeField] GameObject bullet;
    [SerializeField] float timeBeforeSpecialAttack = 0.25f;
    [Header("Movement")]
    [SerializeField] int speed;
    [SerializeField] bool smoothMovement = false;
    [Header("Rotate")]
    [SerializeField] bool rotateOnMove = true;
    [SerializeField] int maxRotation = 30;
    [Range(0,2)] [SerializeField] float rotationTime = 0.25f;
    Rigidbody rb;
    float waitBeforeShoot;
    bool isOnCooldown;
    float pressingTime;
    bool isLoadingSpecialAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire"))
        {
            PostProcessManager.Instance.SetLensDistorsion(true, 0);
            PostProcessManager.Instance.SetChromaticAberration(true, 0);
            if (pressingTime > timeBeforeSpecialAttack)
            {
                //Special atack
                Shoot(false);
                waitBeforeShoot = 0;
            }
            else
            {
                //Normal Attack
                if (!isOnCooldown)
                {
                    Shoot(true);
                    waitBeforeShoot = 0;
                    isOnCooldown = true;
                }
            }
            pressingTime = 0;

        }
        if (Input.GetButton("Fire"))
        {
            pressingTime += Time.deltaTime;
            pressingTime = Mathf.Clamp(pressingTime, 0, 0.5f);
            PostProcessManager.Instance.SetLensDistorsion(true, pressingTime);
            if (pressingTime > timeBeforeSpecialAttack)
            {
                PostProcessManager.Instance.SetChromaticAberration(true, pressingTime * 2);
            }
        }

        if (isOnCooldown)
        {
            waitBeforeShoot += Time.deltaTime;
            if (waitBeforeShoot >= shootCooldown)
            {
                isOnCooldown = false;
            }
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (rotateOnMove)
        {
            transform.DORotate(new Vector3(0, 0, -horizontal * maxRotation),rotationTime);
        }
        if (!smoothMovement)
        {
            if (Mathf.Abs(horizontal) != 1)
            {
                horizontal = 0;
            }
        }
        rb.velocity = new Vector3(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);
    }

    void Shoot(bool normalShoot)
    {
        Bullet bulletTransform = Instantiate(bullet, bulletSpawner.position, Quaternion.identity).GetComponent<Bullet>();
        if (normalShoot)
        {
            //Normal Attack
            bulletTransform.SetDamages(damages);
            bulletTransform.SetImpactBeforeDie(1);
        }
        else
        {
            //Special Attack
            bulletTransform.SetDamages(damages);
            bulletTransform.SetImpactBeforeDie(3);
        }
    }
}
