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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButton("Fire"))
        {
            if (!isOnCooldown)
            {
                Shoot();
                waitBeforeShoot = 0;
                isOnCooldown = true;
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
        rb.velocity = new Vector3(horizontal * speed, 0, 0);

        
    }

    void Shoot()
    {
        Bullet bulletTransform = Instantiate(bullet, bulletSpawner.position, Quaternion.identity).GetComponent<Bullet>();
        bulletTransform.SetDamages(damages);
    }
}
