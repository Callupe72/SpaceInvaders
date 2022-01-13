using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum PlayerDirection
    {
        Right,
        Normal,
        Left,
    }

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
    [SerializeField] Animator meshAnimator = null;
    float flipShipTimer = 0f;
    float oldFlipShipTimer = 0f;
    float normalModeTimer = 0f;
    PlayerDirection oldDirection = PlayerDirection.Normal;
    PlayerDirection newDirection = PlayerDirection.Normal;

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
    [SerializeField] GameObject shield;
    [SerializeField] GameObject fracturedPlayer;

    //POWER UPS

    [HideInInspector] public bool playerCanDestroyLine = false;
    [HideInInspector] public bool playerCanAim = false;
    [HideInInspector] public bool playerCanShield = false;
    [HideInInspector] public bool playerCanUsePowerShot = true;
    [HideInInspector] public bool playerCanDash = true;
    [HideInInspector] public bool playerCanMakeDamagesOnDash = false;
    [HideInInspector] public bool debrisMakesDamages = false;
    [SerializeField] float timeBeforeLooping;

    [SerializeField] Vector2 minMaxPos;

    public GameObject[] border;

    bool activeTrails = true;
    [SerializeField] GameObject[] trails;

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
        //for (int i = 0; i < border.Length; i++)

        //{
        //    border[i].transform.localScale = new Vector3(Mathf.Clamp(AudioReaction.Instance.GetDropValue(),2,100),9000, 2);
        //}

        if (activeTrails != ActiveJuiceManager.Instance.TrailsIsOn && trails.Length > 0)
        {
            activeTrails = ActiveJuiceManager.Instance.TrailsIsOn;
            foreach (GameObject item in trails)
            {
                item.SetActive(activeTrails);
            }
        }

        float currentXPos = Mathf.Clamp(transform.position.x, minMaxPos.x, minMaxPos.y);
        transform.position = new Vector3(currentXPos, transform.position.y, transform.position.z);

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

            if (!shield.activeInHierarchy)
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

    void LateUpdate()
    {
        meshAnimator.SetBool("FlipRight", false);
        meshAnimator.SetBool("FlipLeft", false);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.InPause || GameManager.Instance.currentGameState == GameManager.GameState.Defeat)
            return;

        if (!UnlockNewPowerManager.Instance.waitSeeNewPower)
            Movement();

        if (playerCanShield && !UnlockNewPowerManager.Instance.waitSeeNewPower)
        {
            
            shield.gameObject.SetActive(Input.GetButton("Shield"));
            if (Input.GetButtonDown("Shield"))
            {
                if (ParticlesManager.Instance.GetLastParticles() && ParticlesManager.Instance.GetLastParticles().name == "LoadingPlayerWeapon")
                {
                    Destroy(ParticlesManager.Instance.GetLastParticles());
                }
            }
        }
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
                PostProcessManager.Instance.DashChangeAberation();
                CinemachineShake.Instance.ShakeCamera(5, 1f);
                AudioManager.Instance.Play2DSound("Dash");
                ParticlesManager.Instance.SpawnParticles("DashEffect", transform, Vector3.zero, true);
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
        ChangeState(horizontal);
        if (rotateOnMove)
            transform.DORotate(new Vector3(0, 0, -horizontal * maxRotation), rotationTime);
        if (oldFlipShipTimer >= timeBeforeLooping)
            ChangeDirection();

        if (Mathf.Approximately(horizontal, 0))
        {
            horizontal = 0;
            rotateOnMove = false;
        }
        else
        {
            rotateOnMove = true;
        }

        float hasShield = 1;

        if (shield.activeInHierarchy)
        {
            hasShield = 0.25f;
        }


        if (Input.GetAxisRaw("HorizontalMove") != 0)
            rb.velocity = new Vector3(horizontal * Time.fixedDeltaTime * isDashingSpeed * 2 * hasShield, rb.velocity.y, rb.velocity.z);
        else
            rb.velocity = Vector3.zero;
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
            bulletTransform.SetDamages(Mathf.RoundToInt(damages) * 2);
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
        if (shield.activeInHierarchy)
            return;

        AudioManager.Instance.Play2DSound("PlayerLooseLife");
        PostProcessManager.Instance.PlayerIsTouch(this);
        CinemachineShake.Instance.ShakeCamera(10, .7f);

        life -= dommage;

        lifeCanva.SetColorLife(Mathf.RoundToInt(life));

        if (isDead)
            return;

        if (life <= 0)
        {
            life = 0;
            isDead = true;
            GameManager.Instance.ChangeGameState(GameManager.GameState.Defeat);
            Die();
        }
    }

    private void ChangeState(float direction)
    {
        oldDirection = newDirection;

        oldFlipShipTimer = flipShipTimer;
        flipShipTimer += Time.fixedDeltaTime;
        normalModeTimer += Time.fixedDeltaTime;

        if (Mathf.Approximately(direction, 0))
        {
            if (normalModeTimer >= 0.2f)
                newDirection = PlayerDirection.Normal;
        }
        else if (direction > 0f)
        {
            newDirection = PlayerDirection.Right;
            normalModeTimer = 0f;
        }
        else
        {
            newDirection = PlayerDirection.Left;
            normalModeTimer = 0f;
        }

        if (oldDirection != newDirection)
            flipShipTimer = 0f;
    }

    private void ChangeDirection()
    {
        //Debug.Log($"Dans la fonction, old : {oldDirection} new : {newDirection}");
        if (!ActiveJuiceManager.Instance.BarrelRollIsOn)
        {
            return;
        }
        if (oldDirection == PlayerDirection.Left && newDirection == PlayerDirection.Right)
            meshAnimator.SetBool("FlipRight", true);
        else if (oldDirection == PlayerDirection.Right && newDirection == PlayerDirection.Left)
            meshAnimator.SetBool("FlipLeft", true);
    }

    public void SetMixMaxPos(float min, float max)
    {
        minMaxPos = new Vector2(min, max);
        border[0].transform.DOMoveX(min - 7, 1);
        border[1].transform.DOMoveX(max + 7, 1);
    }

    private void Die()
    {
        ScoreManager.Instance.SeeScore();
        Instantiate(fracturedPlayer, meshAnimator.transform.GetChild(0).transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator DefeatPanel()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.Defeat();
    }

    public void AddLife(int lifeToAdd)
    {
        life += lifeToAdd;
        life = Mathf.Clamp(life, 1, 3);
        lifeCanva.SetColorLife(Mathf.RoundToInt(life));
    }
}
