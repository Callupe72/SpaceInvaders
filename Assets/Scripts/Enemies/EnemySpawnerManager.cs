//using System;
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
    public List<ShipStats> shipsStats = new List<ShipStats>();
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

    void Start()
    {
        CreateNewWave();
    }

    void CreateNewWave()
    {
        currentWave++;

        //GET DATA
        currentEnemiesPerLine = dataWave.Data[currentWave].ennemyByLine;
        currentLineNumbers = dataWave.Data[currentWave].NbOfLine;
        currentTankEnemies = dataWave.Data[currentWave].Tank;
        currentShooterEnemies = dataWave.Data[currentWave].Fire;
        currentPinata = dataWave.Data[currentWave].Pinata;
        currentNormalEnemies = (currentEnemiesPerLine * currentLineNumbers) - (currentTankEnemies + currentShooterEnemies + currentPinata);
        enemyStillAlive = (currentEnemiesPerLine * currentLineNumbers);

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
                rot = new Vector3(-180, 0, 90);
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
        ennemyRestantText.text = enemyStillAlive + "/" + (currentEnemiesPerLine * currentLineNumbers);
        if (enemyStillAlive == 0)
        {
            if (currentWave < dataWave.Data.Count)
            {
                CreateNewWave();
            }
            else
            {
                GameManager.Instance.ChangeGameState(GameManager.GameState.Victory);
            }
        }
    }

    IEnumerator WaitToDestroyLine(Transform lineToDestroy)
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
        foreach (Transform enemy in lineToDestroy)
        {
            enemy.GetComponent<Enemy>().Die();
        }
    }

}