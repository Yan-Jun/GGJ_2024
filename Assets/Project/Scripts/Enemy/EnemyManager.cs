using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject FlyEnemy;

    [SerializeField] Andras andras;
    [SerializeField] Paimon paimon;

    //wave
    public float waveCurrentInterval;
    public float waveSpawnInterval;
    public float waveNextMinusSpawnInterval;
     
    public float waveCurrentDelay;
    public float waveSpawnDelay;
    public float waveNextMinusSpawnDelay;
     
    public int waveCurrentAmount;
    public int waveSpawnAmount;
    public int waveNextAddAmount;
     
    public int waveCount;

    [Space(10f)]

    //continus
    public float continusCurrentInterval;
    public float continusSpawnInterval;
    public float continusNextMinusSpawnInterval;

    public float continusCurrentDelay;
    public float continusSpawnDelay;
    public float continusNextMinusSpawnDelay;

    public int continusCurrentAmount;
    public int continusSpawnAmount;
    public int continusNextAddAmount;

    public int continusCount;

    private void Update()
    {
        waveCurrentInterval -= Time.deltaTime;
        waveCurrentDelay -= Time.deltaTime;

        continusCurrentInterval -= Time.deltaTime;
        continusCurrentDelay -= Time.deltaTime;


        if (waveCurrentDelay <= 0 && waveCurrentAmount <= 0)
        {
            waveCurrentDelay = waveSpawnDelay;
            waveSpawnDelay = Mathf.Clamp(waveSpawnDelay - waveNextMinusSpawnDelay, .01f, 99999);


            waveSpawnInterval = Mathf.Clamp(waveSpawnInterval - waveNextMinusSpawnInterval, .01f, 99999);

            waveCurrentAmount = waveSpawnAmount;
            waveSpawnAmount += waveNextAddAmount;

            waveCount++;
        }

        if (continusCurrentDelay <= 0 && continusCurrentAmount <= 0)
        {
            continusCurrentDelay = continusSpawnDelay;
            continusSpawnDelay = Mathf.Clamp(continusSpawnDelay - continusNextMinusSpawnDelay, .01f, 99999);

            
            continusSpawnInterval = Mathf.Clamp(continusSpawnInterval - continusNextMinusSpawnInterval, .01f, 99999);

            continusCurrentAmount = continusSpawnAmount;
            continusSpawnAmount += continusNextAddAmount;

            continusCount++;
        }


        if (CheckCanSpawn(waveCurrentAmount, waveCurrentInterval))
        {
            waveCurrentInterval = waveSpawnInterval;
            SpawnEnemy(waveCount);
            Debug.Log("wave");
            waveCurrentAmount--;
        }


        if (CheckCanSpawn(continusCurrentAmount, continusCurrentInterval))
        {
            continusCurrentInterval = continusSpawnInterval;
            SpawnEnemy(continusCount);
            Debug.Log("continus");
            continusCurrentAmount--;
        }
            
    }

    private bool CheckCanSpawn(int Amount, float Interval)
    {
        if (Amount > 0 && Interval <= 0)
        {
            return true;
        }
        return false;
    }

    private void SpawnEnemy(int count)
    {
        if (Random.Range(0, 3) == 0)
        {
            //spawn walk enemy
            FlyEnemy flyEnemy = Instantiate(FlyEnemy, GetSpawnPoint(), Quaternion.identity).GetComponent<FlyEnemy>();
            flyEnemy.Health += count*5;
            flyEnemy.MaxHealth += count*5;
            flyEnemy.ExplodeRadius += count;
            flyEnemy.Speed += count*3;
            flyEnemy.Andras = andras;
            flyEnemy.Paimon = paimon;
        }
        else
        {
            FlyEnemy flyEnemy = Instantiate(FlyEnemy, GetSpawnPoint(), Quaternion.identity).GetComponent<FlyEnemy>();
            flyEnemy.Health += count * 5;
            flyEnemy.MaxHealth += count * 5;
            flyEnemy.ExplodeRadius += count;
            flyEnemy.Speed += count * 3;
            flyEnemy.Andras = andras;
            flyEnemy.Paimon = paimon;
        }
    }

    private Vector2 GetSpawnPoint()
    {
        Vector2 rngPoint = new Vector2(Random.Range(-195f, 195f), Random.Range(-60f, 120f));
        if (rngPoint.y >= 60f)
        {
            //top
            rngPoint.y = 120f;
        }
        else if (rngPoint.x > 0)
        {
            //right
            rngPoint.x = 195f;
        }
        else
        {
            //left
            rngPoint.x = -195f;
        }
        return rngPoint;
    }
}
