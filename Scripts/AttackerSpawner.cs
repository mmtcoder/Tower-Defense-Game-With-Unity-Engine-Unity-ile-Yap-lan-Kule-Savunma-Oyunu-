using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    [Header("Global Values")]
    [SerializeField] bool canSpawnable = true;
    [SerializeField] int addHealthEnemyAsPercent;
    [Header("First Enemy Attributes")]
    [SerializeField] Enemy[] enemiesPerWave;
    [SerializeField] int[] countOfEnemyPerWave;
    [SerializeField] float[] spawnTimes;
    [SerializeField] int jumpStartPathIndex = -2;
    [SerializeField] int jumpEndPathIndex = -2;
    [Header("Allow paths spawnable")]
    public bool [] isCanSpawnFirstPath;
    public bool [] isCanSpawnSecondPath;
    [Header("Second Enemy Attributes")]
    [SerializeField] Enemy[] enemiesSecondPerWave;
    [SerializeField] int[] countOfEnemySecondPerWave;
    [SerializeField] float[] spawnTimesSecond;
    [SerializeField] int spawnPlaceIndex = -1;

    GameObject uiEnvironment;
    GameObject enemyHolder;
    Enemy selectedEnemy;
    Enemy selectedSecondEnemy;
    const string ENEMY_HOLDER_NAME = "EnemyHolder";
    private int currentWave = 0;
    private int maxWave;
    private int numberOfEnemyForNowWave = 0;
    private int numberOfSecondEnemyForNowWave = 0;
    private float spawnTimeForNowWave;
    private float spawnSecondTimeForNowWave;
    private float winOrLoseScreenTimer = 0;
    bool canCreateNextWave = true;
    bool canCheckEnemyState = false;
    

    // Start is called before the first frame update
    void Start()
    {
        maxWave = enemiesPerWave.Length;
        uiEnvironment = FindObjectOfType<UIEnvironment>().gameObject;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawnable)
        {
            if (enemiesPerWave != null)
            {
                CreateEnemyHolder();
            }
            if(canCreateNextWave)
        {
                StartCoroutine(InstantiateEnemy());
                /*
                Enemy enemyInstant = Instantiate(enemy, transform.GetChild(0).position, transform.GetChild(0).rotation)
                as Enemy;
                enemyInstant.transform.parent = enemyHolder.transform;*/
                canCreateNextWave = false;
            }
        else
            {
                if(FindObjectsOfType<Enemy>().Length == 0)
                {

                    canCreateNextWave = true;
                }
            }
        }
      
    }
    IEnumerator InstantiateEnemy()
    {
        /*
         * ikinci dalgayı wavetexti duzenledikten sonra ki kod kısmina iki array boolean ifade eklenecek ve 
         * ikinci yoldan gelen dusman icin cıkan dusman sayisi, dusmanlar ve olusturulma zamani arrayleri eklenecek
         * Enemy sinifinda pathnodeIndex  i ikinci dusman icin indexe gore baslangıc noktasi for dongusunde atanacak.
         */
        if(currentWave != maxWave)
        {
            //Hatayi gidermek icin deger atadik
             selectedEnemy = enemiesPerWave[0];
           
            currentWave++;
            uiEnvironment.GetComponent<UIEnvironment>().SetWaveText(maxWave,currentWave);

            InstantiateEnemiesAndAttributes(currentWave);

            
                if (isCanSpawnFirstPath[currentWave -1])
                {
                    for (int i = 0; i < numberOfEnemyForNowWave; i++)
                    {
                        Enemy enemyInstant = Instantiate(selectedEnemy, transform.GetChild(0).position, transform.GetChild(0).rotation)
                            as Enemy;
                        if (currentWave != 0)
                        {
                            enemyInstant.SetJumpPathsBetweenIndexes(jumpStartPathIndex,jumpEndPathIndex);
                            enemyInstant.AddHealthAsPercent(addHealthEnemyAsPercent * currentWave);
                            if(GetComponent<MakeCrossRoads>() != null)
                        {
                            GetComponent<MakeCrossRoads>().CrossRoadsAlgorithm(enemyInstant);
                        }
                        }
                        //SecondPathSpawn();
                        enemyInstant.transform.parent = enemyHolder.transform;
                        yield return new WaitForSeconds(spawnTimeForNowWave);
                    }
                }
                if (isCanSpawnSecondPath[currentWave -1])
                {
               
                    //We assigned this index to fix the error.
                    selectedSecondEnemy = enemiesSecondPerWave[0];
                    InstantiateSecondEnemiesAndAttributes(currentWave);
                if (selectedSecondEnemy != null)
                {
                    for (int k = 0; k < numberOfSecondEnemyForNowWave; k++)
                    {
                        Enemy enemyInstant = Instantiate(selectedSecondEnemy, transform.GetChild(spawnPlaceIndex).position, transform.GetChild(0).rotation)
                            as Enemy;
                        if (currentWave != 0)
                        {
                            enemyInstant.SetSpawnPoint(spawnPlaceIndex);
                            enemyInstant.AddHealthAsPercent(addHealthEnemyAsPercent * currentWave);
                            if (GetComponent<MakeCrossRoads>() != null)
                            {
                                GetComponent<MakeCrossRoads>().CrossRoadsAlgorithm(enemyInstant);
                            }
                        }

                        enemyInstant.transform.parent = enemyHolder.transform;
                        yield return new WaitForSeconds(spawnSecondTimeForNowWave);
                    }
                }
                    
                }
            
         
           

        }
        else
        {
            if(winOrLoseScreenTimer >= 1.5f)
            {
                winOrLoseScreenTimer = 0;
                canCreateNextWave = false;
                uiEnvironment.GetComponent<UIEnvironment>().CallWinScreen();
                uiEnvironment.GetComponent<UIEnvironment>().SetAudioMute(true);
                Time.timeScale = 0f;
            }
            else
            {
                winOrLoseScreenTimer += Time.deltaTime;
            }
         
        }

       
    }
    private void CreateEnemyHolder()
    {
        enemyHolder = GameObject.Find(ENEMY_HOLDER_NAME);
        if (!enemyHolder)
        {
            enemyHolder = new GameObject(ENEMY_HOLDER_NAME);
        }
    }
   
    public void SetCanSpawnable(bool state)
    {
        canSpawnable = state;
    }
    private void InstantiateEnemiesAndAttributes(int wave)
    {
        spawnTimeForNowWave = spawnTimes[wave - 1];
        numberOfEnemyForNowWave = countOfEnemyPerWave[wave - 1];
        selectedEnemy = enemiesPerWave[wave - 1];
    }
    private void InstantiateSecondEnemiesAndAttributes(int wave)
    {
        spawnSecondTimeForNowWave = spawnTimesSecond[wave - 1];
        numberOfSecondEnemyForNowWave = countOfEnemySecondPerWave[wave - 1];
        selectedSecondEnemy = enemiesSecondPerWave[wave - 1];
    }

    public int GetSpawnPlaceIndex()
    {
        return spawnPlaceIndex;
    }
   
    private void SecondPathSpawn()
    {
        if (isCanSpawnSecondPath[currentWave - 1])
        {

            //We assigned this index to fix the error.
            selectedSecondEnemy = enemiesSecondPerWave[0];
            InstantiateSecondEnemiesAndAttributes(currentWave);
            if (selectedSecondEnemy != null)
            {
               
                    Enemy enemyInstant = Instantiate(selectedSecondEnemy, transform.GetChild(spawnPlaceIndex).position, transform.GetChild(0).rotation)
                        as Enemy;
                    if (currentWave != 0)
                    {
                        enemyInstant.SetSpawnPoint(spawnPlaceIndex);
                        enemyInstant.AddHealthAsPercent(addHealthEnemyAsPercent * currentWave);
                        if (GetComponent<MakeCrossRoads>() != null)
                        {
                            GetComponent<MakeCrossRoads>().CrossRoadsAlgorithm(enemyInstant);
                        }
                    

                    enemyInstant.transform.parent = enemyHolder.transform;
                   
                }
            }

        }
    }
}
