using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField] int damages = 200;
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] Transform bulletSpawner;
    [SerializeField] GameObject bullet;
    [SerializeField] int speed;
    Rigidbody rb;
    float waitBeforeShoot;
    bool isOnCooldown;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
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
        rb.velocity = new Vector3(horizontal * speed, 0, 0);
    }

    void Shoot()
    {
        Bullet bulletTransform = Instantiate(bullet, bulletSpawner.position, Quaternion.identity).GetComponent<Bullet>();
        bulletTransform.SetDamages(damages);
    }
}
