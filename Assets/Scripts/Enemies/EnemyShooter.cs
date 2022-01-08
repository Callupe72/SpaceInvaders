using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    [Header("Shooter")]
    [SerializeField] int damages;
    [SerializeField] GameObject prefabBullet = null;
    [SerializeField] Transform posBullet = null;
    [SerializeField] float durationBetweenShootMin = 2f;
    [SerializeField] float durationBetweenShootMax = 5f;

    float timerShoot;

    private void Start()
    {
        timerShoot = Random.Range(durationBetweenShootMin, durationBetweenShootMax);
    }

    private void Update()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.InPause || GameManager.Instance.currentGameState == GameManager.GameState.Defeat)
            return;
        if (timerShoot > 0)
        {
            timerShoot -= Time.deltaTime;
            if (timerShoot <= 0)
            {
                Shoot();
                timerShoot = Random.Range(durationBetweenShootMin, durationBetweenShootMax);
            }
        }
    }

    public void Shoot()
    {
        GameObject go = Instantiate(prefabBullet, posBullet.position, Quaternion.Euler(transform.forward), transform );
        go.GetComponent<Bullet>().bulletEnnemy = true;
    }
}
