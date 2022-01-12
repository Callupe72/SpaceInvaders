//using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Vague_Data dataWave;
    [SerializeField] TMP_Text ennemyRestantText;
    static System.Random rnd = new System.Random();

    [SerializeField] GameObject[] enemiesToSpawn;

    public Transform scoreParent;

    [Header("Speed")]
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] float horizontalTime = 1f;
    [SerializeField] float frontSpeed = 1f;
    [SerializeField] float frontTime = 1f;
    [SerializeField] bool wasRight;

    float speedCurrentTime;

    Vector3 startingPos;


    Direction currentDirection;
    public enum Direction
    {
        Front,
        Left,
        Right,
    }
    public enum ShipType
    {
        Normal,
        Shooter,
        Tank,
        Pinata,
    }

    [System.Serializable]
    public class ShipStats
    {
        public Vector3 shipPositions;
        public Transform lineParent;
        public ShipType shipType;
        public int line;
        public int enemyNum;
    }
    List<int> randomizeList = new List<int>();
    List<ShipStats> shipsStats = new List<ShipStats>();
    int currentLineNumbers;
    int currentEnemiesPerLine;
    int currentTankEnemies;
    int currentShooterEnemies;
    int currentNormalEnemies;
    int currentPinata;

    [Header("Space")]
    [SerializeField] int spaceBetweenLines = 2;
    [SerializeField] int spaceBetweenEnemies = 2;
    [SerializeField] int currentWave = -1;

    int enemyStillAlive;
    bool isPinataWave;


    [Header("Wave")]
    [SerializeField] GameObject waveParent;
    [SerializeField] float waveAnimationTime;
    float waveCurrentTime;
    [SerializeField] AnimationCurve waveAnimCurve;


    public static EnemySpawnerManager Instance;
    private bool canMoveWaveTxt;

    float xPosition = 10;

    Player player;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        startingPos = transform.position;
        //startTime = 1;
        //start = 1400;
        //end = -1400;
        CreateNewWave();
        player = FindObjectOfType<Player>();
    }

    void OnDrawGizmos()
    {
        Color col = Color.red;
        col = new Vector4(1, 0, 0, 0.5f);
        Gizmos.color = col;

        Gizmos.DrawCube(transform.position, new Vector3(xPosition, 30, 20));
    }

    void Update()
    {
        //if (canMoveWaveTxt)
        //{
        //    waveCurrentTime += Time.deltaTime;
        //    float xValue = Curve(DeltaTime());
        //    waveText.GetComponent<RectTransform>().anchoredPosition = new Vector2(xValue, waveText.GetComponent<RectTransform>().anchoredPosition.y);
        //    if (waveCurrentTime > waveAnimationTime)
        //    {
        //        canMoveWaveTxt = false;
        //        waveText.gameObject.SetActive(false);
        //    }
        //}

        if (GameManager.Instance.currentGameState == GameManager.GameState.InPause || GameManager.Instance.currentGameState == GameManager.GameState.Defeat)
            return;


        if (!isPinataWave && enemyStillAlive > 0)
        {
            switch (currentDirection)
            {
                case Direction.Front:
                    Movement(new Vector3(0, 0, -1));
                    break;
                case Direction.Left:
                    Movement(new Vector3(-1, 0, 0));
                    break;
                case Direction.Right:
                    Movement(new Vector3(1, 0, 0));
                    break;
                default:
                    break;
            }
        }

    }

    void Movement(Vector3 dir)
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.InPause || GameManager.Instance.currentGameState == GameManager.GameState.Defeat)
            return;

        speedCurrentTime += Time.deltaTime;
        Vector3 newPos = Vector3.zero;
        float time = 0;
        if (time == 0)
        {
            if (dir.y != 0)
            {
                //GO Left / right
                newPos = dir * horizontalSpeed + transform.position;
                time = horizontalTime;
            }
            else
            {
                newPos = dir * frontSpeed + transform.position;
                time = frontTime;
            }
        }

        transform.DOMove(newPos, time);
        if (speedCurrentTime > time)
        {
            speedCurrentTime = 0;
            currentDirection = NewDir();
        }
    }

    Direction NewDir()
    {
        if (currentDirection == Direction.Front)
        {
            if (wasRight)
            {
                wasRight = false;
                return Direction.Right;
            }
            else
            {
                wasRight = true;
                return Direction.Left;
            }
        }
        else
        {
            return Direction.Front;
        }
    }

    //float start;
    //float end = -1400;
    //float startTime;
    //float Curve(float delta)
    //{
    //    return (end - start) * waveAnimCurve.Evaluate(delta * waveAnimationTime) + start;
    //}
    //float DeltaTime()
    //{
    //    float timeDelta = Time.time - startTime;

    //    if (timeDelta < waveAnimationTime)
    //    {
    //        return timeDelta / waveAnimationTime;
    //    }
    //    else
    //    {
    //        return 1;
    //    }
    //}

    public void CreateNewWave()
    {
        StartCoroutine(WaitBeforeCreateNewWave());
    }


    IEnumerator WaitBeforeCreateNewWave()
    {

        currentWave++;
        yield return new WaitForSeconds(1);
        transform.position = startingPos;
        waveParent.gameObject.SetActive(true);
        waveParent.GetComponentInChildren<TextMeshProUGUI>().text = "Wave " + (currentWave + 1).ToString();
        //waveText.GetComponent<RectTransform>().anchoredPosition = new Vector2(1400, waveText.GetComponent<RectTransform>().anchoredPosition.y);
        waveCurrentTime = 0;
        canMoveWaveTxt = true;
        //startTime = Time.time;
        yield return new WaitForSeconds(waveAnimationTime);
        waveParent.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);


        //GET DATA
        currentEnemiesPerLine = dataWave.Data[currentWave].ennemyByLine;
        currentLineNumbers = dataWave.Data[currentWave].NbOfLine;
        currentTankEnemies = dataWave.Data[currentWave].Tank;
        currentShooterEnemies = dataWave.Data[currentWave].Fire;
        currentPinata = dataWave.Data[currentWave].Pinata;
        currentNormalEnemies = (currentEnemiesPerLine * currentLineNumbers) - (currentTankEnemies + currentShooterEnemies + currentPinata);
        enemyStillAlive = (currentEnemiesPerLine * currentLineNumbers);

        isPinataWave = currentPinata > 0;

        //enemyStillAlive = waves[currentWave].GetLinesNumbers() * waves[currentWave].GetEnemyPerLine();
        //for (int i = 0; i < currentLineNumbers; i++)
        //{
        //    SpawnLine(i);
        //}

        shipsStats.Clear();
        for (int i = 0; i < enemyStillAlive; i++)
        {
            ShipStats ship = new ShipStats();
            shipsStats.Add(ship);
        }
        for (int i = 0; i < currentTankEnemies; i++)
        {
            shipsStats[i].shipType = ShipType.Tank;
        }
        for (int i = currentTankEnemies; i < currentShooterEnemies + currentTankEnemies; i++)
        {
            shipsStats[i].shipType = ShipType.Shooter;
        }
        for (int i = 0; i < currentPinata; i++)
        {
            shipsStats[i].shipType = ShipType.Pinata;
        }
        randomizeList.Clear();

        var numbers = new List<int>(Enumerable.Range(0, shipsStats.Count));
        randomizeList = numbers.OrderBy(a => rnd.Next()).ToList();


        for (int i = 0; i < currentLineNumbers; i++)
        {
            SpawnLine(i);
        }

        ennemyRestantText.text = enemyStillAlive + "/" + (currentEnemiesPerLine * currentLineNumbers);

        xPosition = currentEnemiesPerLine * spaceBetweenEnemies + currentEnemiesPerLine % 2 + (horizontalSpeed * 2);

        if (isPinataWave)
            xPosition += 20 * 2;

        player.SetMixMaxPos(-xPosition, xPosition);
    }

    void SpawnLine(int lineIndex)
    {
        GameObject line = new GameObject();
        line.name = "EnemyLine" + lineIndex.ToString();
        line.transform.parent = transform;
        for (int i = 0; i < currentEnemiesPerLine; i++)
        {
            float posX = (i + 1) / 2;
            if (i % 2 == 0)
            {
                posX *= -1;
            }
            Vector3 pos = new Vector3(posX * spaceBetweenEnemies, 2, lineIndex * spaceBetweenLines);

            int index = lineIndex * currentEnemiesPerLine + i;

            SpawnEnemy(lineIndex, i, pos, line.transform, shipsStats[randomizeList[index]].shipType);
        }
    }
    void SpawnEnemy(int lineNum, int enemyIndex, Vector3 pos, Transform parent, ShipType shipType)
    {
        GameObject enemyToSpawn = enemiesToSpawn[0];
        Vector3 rot = new Vector3(-90, 180, 0);

        switch (shipType)
        {
            case ShipType.Normal:
                break;
            case ShipType.Shooter:
                rot = new Vector3(-180, 0, 270);
                enemyToSpawn = enemiesToSpawn[1];
                break;
            case ShipType.Tank:
                enemyToSpawn = enemiesToSpawn[2];
                break;
            case ShipType.Pinata:
                enemyToSpawn = enemiesToSpawn[3];
                break;
            default:
                break;
        }
        GameObject enemy = Instantiate(enemyToSpawn, pos, Quaternion.Euler(rot));
        enemy.GetComponent<Enemy>().shipType = shipType;
        enemy.GetComponent<Enemy>().SetTimeBeforeShow(((float)currentLineNumbers - (float)lineNum +1 ) / 5);
        enemy.name = "EnemyL" + lineNum + "N" + enemyIndex;
        enemy.transform.parent = parent;
    }

    public void DestroyLine(Transform lineToDestroy)
    {
        StartCoroutine(WaitToDestroyLine(lineToDestroy));
    }

    public void EnemyIsKilled()
    {
        enemyStillAlive--;
        enemyStillAlive = Mathf.Clamp(enemyStillAlive, 0, 999999);
        ennemyRestantText.text = enemyStillAlive + "/" + (currentEnemiesPerLine * currentLineNumbers);
        if (enemyStillAlive == 0)
        {
            StartCoroutine(WaitBeforeStartWave());
        }
    }

    public int GetEnemyStillAlive()
    {
        return enemyStillAlive;
    }

    IEnumerator WaitBeforeStartWave()
    {
        yield return new WaitForSeconds(1f);
        if (currentWave < dataWave.Data.Count)
        {
            if (isPinataWave)
            {
                UnlockNewPowerManager.Instance.AddNewPower();
            }
            else
            {
                CreateNewWave();
            }
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.Victory);
        }
    }

    IEnumerator WaitToDestroyLine(Transform lineToDestroy)
    {

        SlowMotionManager.Instance.SlowMotion();
        yield return new WaitForSeconds(1f);
        ParticlesManager.Instance.SpawnParticles("DestroyLine", lineToDestroy, Vector3.zero, false);
        foreach (Transform enemy in lineToDestroy)
        {
            enemy.GetComponent<Enemy>().PrepareToDie();
        }
    }
}