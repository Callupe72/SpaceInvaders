using System.Collections;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] EnemyWaves[] waves;
    [Header("Space")]
    [SerializeField] int spaceBetweenLines = 2;
    [SerializeField] int spaceBetweenEnemies = 2;
    int currentWave = -1;

    void Start()
    {
        CreateNewWave();
    }

    void CreateNewWave()
    {
        currentWave++;
        for (int i = 0; i < waves[currentWave].GetLinesNumbers(); i++)
        {
            SpawnLine(i);
        }
    }

    void SpawnLine(int lineIndex)
    {
        GameObject line = new GameObject();
        line.name = "EnemyLine" + lineIndex.ToString();
        line.transform.parent = transform;
        for (int i = 0; i < waves[currentWave].GetEnemyPerLine(); i++)
        {
            SpawnEnemy(lineIndex, i, line.transform);
        }
    }

    void SpawnEnemy(int lineNum, int enemyIndex, Transform parent)
    {
        float posX = (enemyIndex + 1) / 2;
        if (enemyIndex % 2 == 0)
        {
            posX *= -1;
        }
        Vector3 pos = new Vector3(posX * spaceBetweenEnemies, 2, lineNum * spaceBetweenLines);
        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.Euler(-90, 180, 0));
        enemy.name = "EnemyL" + lineNum + "N" + enemyIndex;
        enemy.transform.parent = parent;
    }

    public void DestroyLine(Transform lineToDestroy)
    {
        StartCoroutine(WaitToDestroyLine(lineToDestroy));
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

[System.Serializable]
public class EnemyWaves
{
    [SerializeField] int enemyPerLines = 10;
    [SerializeField] int linesNumbers = 5;

    public int GetEnemyPerLine()
    {
        return enemyPerLines;
    }
    public int GetLinesNumbers()
    {
        return linesNumbers;
    }
}