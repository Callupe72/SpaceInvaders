using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform playerMesh;


    [Header("FloatingWeapon")]
    [SerializeField] Transform weaponPhysic;
    Vector3 startingWeaponPhysicPos;

    float forceFactor;
    Vector3 floatForce;

    [Header("PlayerStats")]
    [SerializeField] float speed = 10;
    [SerializeField] float life = 100;
    [SerializeField] float damages = 200;
    [Header("Shoot")]
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] Transform bulletSpawner;
    [SerializeField] GameObject bullet;
    [SerializeField] float timeBeforeSpecialAttack = 0.25f;
    [Header("Movement")]
    [SerializeField] bool smoothMovement = false;
    [Header("Rotate")]
    [SerializeField] bool rotateOnMove = true;
    [SerializeField] int maxRotation = 30;
    [Range(0, 2)] [SerializeField] float rotationTime = 0.25f;
    Rigidbody rb;
    float waitBeforeShoot;
    bool isOnCooldown;
    float pressingTime;
    bool isLoadingSpecialAttack;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingWeaponPhysicPos = weaponPhysic.transform.localPosition;
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

        weaponPhysic.transform.position = transform.position + startingWeaponPhysicPos;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (rotateOnMove)
        {
            transform.DORotate(new Vector3(0, 0, -horizontal * maxRotation), rotationTime);
            //playerMesh.transform.DORotate(new Vector3(horizontal * maxRotation * 3 - 90, 90, -90), rotationTime);
        }
        if (!smoothMovement)
        {
            if (Mathf.Abs(horizontal) != 1)
            {
                horizontal = 0;
            }
        }
        rb.velocity = new Vector3(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y, rb.velocity.z);




        //forceFactor = 1.0f - ((weaponRigidbody.transform.position.y - waterLevel) / floatThreshold);

        //if (forceFactor > 0.0f)
        //{
        //    floatForce = -Physics.gravity * (forceFactor - weaponRigidbody.velocity.y * waterDensity);
        //    floatForce += new Vector3(weaponRigidbody.velocity.x, -downForce, weaponRigidbody.velocity.z);
        //    weaponRigidbody.AddForceAtPosition(floatForce, weaponRigidbody.transform.position);
        //}

    }

    void Shoot(bool normalShoot)
    {
        Bullet bulletTransform = Instantiate(bullet, bulletSpawner.position, Quaternion.identity).GetComponent<Bullet>();
        if (normalShoot)
        {
            //Normal Attack
            bulletTransform.SetDamages(Mathf.RoundToInt(damages));
            bulletTransform.SetImpactBeforeDie(1);
        }
        else
        {
            //Special Attack
            bulletTransform.SetDamages(Mathf.RoundToInt(damages));
            bulletTransform.SetImpactBeforeDie(AudioReaction.Instance.GetDropValue() * 2);
        }
    }

    public void SetStatsOnLevelUp(LevelData levelData)
    {
        speed = levelData.speed;
        damages = levelData.dommage;
        life = levelData.defense;
    }
}
