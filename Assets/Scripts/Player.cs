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
    [SerializeField] float life = 3;
    [SerializeField] Life lifeCanva;
    [SerializeField] float damages = 200;
    [Header("Shoot")]
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] Transform bulletSpawner;
    [SerializeField] GameObject bullet;
    [SerializeField] float timeBeforeSpecialAttack = 0.25f;

    [Header("WeaponMovement")]
    [SerializeField] float durationBetweenChangeMoveWeapon = 1f;
    [SerializeField] float offsetMoveWeapon = 0.5f;
    [SerializeField] float offsetMoveWeaponFire = 1f;
    [SerializeField] Transform weaponToMove;
    float initWeaponPosY;
    float initWeaponPosZ;
    float timerBetweenChangeMoveWeapon;
    bool upWeapon;
    public bool moveWeapon;
    [SerializeField] AnimationCurve moveWeaponCurve;
    float timeMoveWeapon;

    [Header("Rotate")]
    [SerializeField] bool rotateOnMove = true;
    [SerializeField] int maxRotation = 30;
    [Range(0, 2)] [SerializeField] float rotationTime = 0.25f;
    bool rotateWeapon;
    [SerializeField] float cooldownRotate = 0.25f;
    float actualRotateCooldown;
    [SerializeField] AnimationCurve rotateCurveStartRotate;
    float actualRotateStartCurve;
    [SerializeField] AnimationCurve rotateCurveEndRotate;
    Rigidbody rb;
    float waitBeforeShoot;
    bool isOnCooldown;
    float pressingTime;
    bool isLoadingSpecialAttack;
    [SerializeField] Transform weaponToRotate;
    bool isDead = false;

    public Transform particleSpawnTransform;

    [Header("Flip")]
    [SerializeField] Transform mesh = null;
    [SerializeField] bool canFlip = false;
    [SerializeField] float flipShipTimer = 0f;
    [Range(0, 2)] [SerializeField] float FlipTime = 0.5f;

    //AIM
    [Header("Aim")]
    [SerializeField] Transform redPoint;
    [SerializeField] LayerMask layerEnemy;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float aimDistance;

    GameObject particlesLoadWeapon;

    //DASH

    [Header("Dash")]
    [SerializeField] float dashSpeed = 1000;
    [Range(0, 1)] [SerializeField] float dashTime = 0.1f;
    [SerializeField] float cooldownReloadDash = 1;
    float dashTimeCalculator;
    float dashReloadingTimeCalculator;
    bool isDashing;
    bool isReloadingDash;
    bool isPlayingRotateSound;

    [Header("Take Damages")]
    public float invisibilityTime = 1f;
    [HideInInspector] public bool isInvisibility;


    //POWER UPS

    [HideInInspector] public bool playerCanDestroyLine = false;
    [HideInInspector] public bool playerCanAim = false;
    [HideInInspector] public bool playerShield = false;
    [HideInInspector] public bool playerCanUsePowerShot = true;
    [HideInInspector] public bool playerCanDash = true;
    [HideInInspector] public bool playerCanMakeDamagesOnDash = false;
    [HideInInspector] public bool debrisMakesDamages = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingWeaponPhysicPos = weaponPhysic.transform.localPosition;
        timerBetweenChangeMoveWeapon = durationBetweenChangeMoveWeapon;
        initWeaponPosY = weaponToMove.transform.position.y;
        initWeaponPosZ = weaponToMove.transform.position.z;
    }

    void Update()
    {

        if (rotateWeapon)
        {
            actualRotateCooldown += Time.deltaTime;
            if (actualRotateCooldown > cooldownRotate)
            {
                rotateWeapon = false;
            }
            weaponToRotate.Rotate(new Vector3(0, -3 * rotateCurveEndRotate.Evaluate(actualRotateCooldown), 0));
        }

        if (moveWeapon)
        {
            timerBetweenChangeMoveWeapon -= Time.deltaTime;
            if (timerBetweenChangeMoveWeapon <= 0f)
            {
                upWeapon = !upWeapon;
                timerBetweenChangeMoveWeapon = durationBetweenChangeMoveWeapon;
            }

            timeMoveWeapon += Time.deltaTime;
            if (timeMoveWeapon > 1)
                timeMoveWeapon = 0;
            if (upWeapon)
            {
                weaponToMove.transform.DOMoveY(weaponToMove.position.y + offsetMoveWeapon / 2 * moveWeaponCurve.Evaluate(timeMoveWeapon), 0.25f);
            }
            else
            {
                weaponToMove.transform.DOMoveY(weaponToMove.position.y - offsetMoveWeapon / 2 * moveWeaponCurve.Evaluate(timeMoveWeapon), 0.25f);
            }
        }
        else
        {
            weaponToMove.transform.DOMoveY(initWeaponPosY, 1);
            timerBetweenChangeMoveWeapon = durationBetweenChangeMoveWeapon;
            upWeapon = false;
        }

        //SHOOT
        if (!UnlockNewPowerManager.Instance.waitSeeNewPower)
        {
            if (GameManager.Instance.currentGameState == GameManager.GameState.InPause || GameManager.Instance.currentGameState == GameManager.GameState.Defeat)
                return;

            PressFire();
        }


        if (isOnCooldown)
        {
            waitBeforeShoot += Time.deltaTime;
            if (waitBeforeShoot >= shootCooldown)
            {
                weaponToMove.transform.DOMoveZ(initWeaponPosZ, 0.1f);
                isOnCooldown = false;
            }
        }

        weaponPhysic.transform.position = transform.position + startingWeaponPhysicPos;

        //AIM

        if (playerCanAim)
        {
            Aim();
        }

        if (Input.GetKeyDown(KeyCode.M))
            TakeDommage(1);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.InPause || GameManager.Instance.currentGameState == GameManager.GameState.Defeat)
            return;

        if (!UnlockNewPowerManager.Instance.waitSeeNewPower)
            Movement();
    }

    void PressFire()
    {
        if (Input.GetButtonUp("Fire"))
        {
            if (particlesLoadWeapon != null)
            {
                Destroy(particlesLoadWeapon);
            }

            actualRotateStartCurve = 0;
            isPlayingRotateSound = false;
            actualRotateCooldown = 0;
            PostProcessManager.Instance.SetLensDistorsion(true, 0);
            PostProcessManager.Instance.SetChromaticAberration(true, 0);
            if (pressingTime > timeBeforeSpecialAttack)
            {
                //Special atack
                Shoot(false);
                waitBeforeShoot = 0;
                weaponToMove.transform.DOMoveZ(initWeaponPosZ, 0.1f);
            }
            else
            {
                //Normal Attack
                if (!isOnCooldown)
                {
                    Shoot(true);
                    waitBeforeShoot = 0;
                    isOnCooldown = true;
                    weaponToMove.transform.DOMoveZ(weaponToMove.position.z - offsetMoveWeaponFire, 0.1f);
                }
            }
            pressingTime = 0;

        }
        if (Input.GetButton("Fire"))
        {
            actualRotateStartCurve += Time.deltaTime;
            weaponToRotate.Rotate(new Vector3(0, -8 * pressingTime * rotateCurveStartRotate.Evaluate(actualRotateStartCurve), 0));
            if (playerCanUsePowerShot)
            {
                if (weaponToMove.position.z == initWeaponPosZ)
                {
                    weaponToMove.transform.DOMoveZ(weaponToMove.position.z - offsetMoveWeaponFire, 0.1f);
                }
                pressingTime += Time.deltaTime;
                pressingTime = Mathf.Clamp(pressingTime, 0, 0.5f);
                PostProcessManager.Instance.SetLensDistorsion(true, pressingTime);
                if (pressingTime > timeBeforeSpecialAttack)
                {
                    PostProcessManager.Instance.SetChromaticAberration(true, pressingTime * 2);
                    if (!isPlayingRotateSound)
                    {
                        AudioManager.Instance.Play2DSound("RotateWeapon");
                        isPlayingRotateSound = true;
                    }
                }
            }
        }
        if (Input.GetButtonDown("Fire"))
        {
            ParticlesManager.Instance.SpawnParticles("LoadingPlayerWeapon", bulletSpawner.transform, bulletSpawner.rotation.eulerAngles, true);
            particlesLoadWeapon = ParticlesManager.Instance.GetLastParticles();
        }
    }
    void Movement()
    {
        float isDashingSpeed = speed;

        #region Dash
        if (playerCanDash)
        {
            if (Input.GetButton("Dash") && !isDashing && !isReloadingDash)
            {
                isDashingSpeed = dashSpeed;
                isDashing = true;
                dashTimeCalculator = 0;
            }

            if (isDashing)
            {
                dashTimeCalculator += Time.deltaTime;
                if (dashTimeCalculator > dashTime)
                {
                    dashTimeCalculator = 0;
                    isDashing = false;
                    isReloadingDash = true;
                    dashReloadingTimeCalculator = 0;
                }
            }

            if (isReloadingDash)
            {
                dashReloadingTimeCalculator += Time.deltaTime;
                if (dashReloadingTimeCalculator > cooldownReloadDash)
                {
                    dashReloadingTimeCalculator = 0;
                    isReloadingDash = false;
                }
            }
        }

        #endregion


        float horizontal = Input.GetAxisRaw("HorizontalMove");

        if (rotateOnMove)
        {
            transform.DORotate(new Vector3(0, 0, -horizontal * maxRotation), rotationTime);
            //playerMesh.transform.DORotate(new Vector3(horizontal * maxRotation * 3 - 90, 90, -90), rotationTime);

            flipShipTimer += Time.deltaTime;
            if (canFlip)
            {
                mesh.DORotate(new Vector3(0, 0, 360 * Mathf.RoundToInt(-horizontal)), FlipTime, RotateMode.LocalAxisAdd).SetEase(Ease.OutFlash).
                    OnComplete(() => mesh.DORotate(new Vector3(0, 0, 0), 0.2f));
                flipShipTimer = 0f;
                canFlip = false;
            }
        }

        if (Mathf.Abs(horizontal) != 1)
        {
            if (flipShipTimer >= 2f)
                canFlip = true;
            horizontal = 0;
            rotateOnMove = false;
            flipShipTimer = 0f;
        }
        else
            rotateOnMove = true;

        rb.velocity = new Vector3(horizontal * Time.fixedDeltaTime * isDashingSpeed, rb.velocity.y, rb.velocity.z);


        //forceFactor = 1.0f - ((weaponRigidbody.transform.position.y - waterLevel) / floatThreshold);

        //if (forceFactor > 0.0f)
        //{
        //    floatForce = -Physics.gravity * (forceFactor - weaponRigidbody.velocity.y * waterDensity);
        //    floatForce += new Vector3(weaponRigidbody.velocity.x, -downForce, weaponRigidbody.velocity.z);
        //    weaponRigidbody.AddForceAtPosition(floatForce, weaponRigidbody.transform.position);
        //}
    }

    void Aim()
    {
        RaycastHit hit;
        Ray ray = new Ray(bulletSpawner.transform.position, Vector3.forward);

        if (Physics.Raycast(ray, out hit, aimDistance, layerEnemy))
        {
            lineRenderer.SetWidth(0.01f, 0.01f);
            lineRenderer.SetPosition(0, bulletSpawner.transform.position);
            lineRenderer.SetPosition(1, hit.point);
            redPoint.position = hit.point;
        }
        else
        {
            redPoint.position = Vector3.one * 9999;
            lineRenderer.SetWidth(0, 0);
        }
    }


    void Shoot(bool normalShoot)
    {
        Bullet bulletTransform = Instantiate(bullet, bulletSpawner.position, Quaternion.identity).GetComponent<Bullet>();
        bulletTransform.SetCanDestroyLine(playerCanDestroyLine);
        bulletTransform.SetDebrisMakeDamages(debrisMakesDamages);
        rotateWeapon = true;

        if (normalShoot)
        {
            //Normal Attack
            AudioManager.Instance.Play2DSound("NormalShot");
            bulletTransform.SetDamages(Mathf.RoundToInt(damages));
            bulletTransform.SetImpactBeforeDie(1);
            bulletTransform.transform.localScale = Vector3.one * 0.1f;
            TrailRenderer bulletTrail = bulletTransform.GetComponent<TrailRenderer>();

            bulletTrail.startWidth = 0.1f;
            bulletTrail.endWidth = 0.05f;
            bulletTrail.time = 0.1f;
        }
        else
        {
            //Special Attack
            AudioManager.Instance.Play2DSound("PowerShot");
            bulletTransform.SetDamages(Mathf.RoundToInt(damages));
            bulletTransform.SetImpactBeforeDie(Mathf.RoundToInt(AudioReaction.Instance.GetDropValue() * 2));
            bulletTransform.transform.localScale = Vector3.one * 0.3f;
            TrailRenderer bulletTrail = bulletTransform.GetComponent<TrailRenderer>();
            bulletTrail.startWidth = 0.3f;
            bulletTrail.endWidth = 0.1f;
            bulletTrail.time = 0.5f;
        }
    }

    public void SetStatsOnLevelUp(LevelData levelData)
    {
        speed = levelData.speed;
        damages = levelData.dommage;
    }

    public void TakeDommage(int dommage)
    {
        PostProcessManager.Instance.PlayerIsTouch(this);
        CinemachineShake.Instance.ShakeCamera(7, .5f);

        life -= dommage;

        lifeCanva.SetColorLife(Mathf.RoundToInt(life));

        if (isDead)
            return;

        if (life <= 0)
        {
            life = 0;
            isDead = true;
            GameManager.Instance.ChangeGameState(GameManager.GameState.Defeat);
        }
    }
}
