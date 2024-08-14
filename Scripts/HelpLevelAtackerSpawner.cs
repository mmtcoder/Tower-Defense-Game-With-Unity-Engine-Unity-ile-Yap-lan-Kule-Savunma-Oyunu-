using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpLevelAtackerSpawner : MonoBehaviour
{
    [SerializeField] Enemy[] enemies;
    [SerializeField] bool canSpawnable = true;
    [SerializeField] float spawnTime;
    [SerializeField] int numberOfEnemyPerWave;
    [SerializeField] int maxWave;
    GameObject uiEnvironment;
    GameObject enemyHolder;
    const string ENEMY_HOLDER_NAME = "EnemyHolder";
     const string HELP_LEVEL_ISCOMPLETED_NAME = "HelpLevelIsComp";
     const string HELP_LEVEL_STATE_NAME = "HelpLevelState";
    private int currentWave = 0;


    bool canCreateNextWave = true;
    bool canCheckEnemyState = false;

    // Start is called before the first frame update
    void Start()
    {
        uiEnvironment = FindObjectOfType<UIEnvironment>().gameObject;
        /* Enemy enemyInstant = Instantiate(enemy, transform.GetChild(0).position, transform.GetChild(0).rotation)
               as Enemy;
         */
    }

    // Update is called once per frame
    void Update()
    {
        if (GetCanSpawnable())
        {
            if (enemies != null)
            {
                CreateEnemyHolder();
            }
            if (canCreateNextWave)
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
                if (FindObjectsOfType<Enemy>().Length == 0)
                {

                    canCreateNextWave = true;
                }
            }
        }

    }
    IEnumerator InstantiateEnemy()
    {

        if (currentWave != maxWave)
        {
            //Hatayi gidermek icin deger atadik
            Enemy selectedEnemy = enemies[1];
            currentWave++;
            uiEnvironment.GetComponent<UIEnvironment>().SetWaveText(maxWave, currentWave);
            /* enemies[0]= birdEnemy
             * enemies[1]= StrangeEnemy
             * enemies[2]= TurtleEnemy
             * enemies[3]= SpiderEnemy
             */

            if (currentWave == 1)
            {
                spawnTime = 1;
                numberOfEnemyPerWave = 1;
                selectedEnemy = enemies[1];
            }
            else if (currentWave == 2)
            {
                spawnTime = 1;
                numberOfEnemyPerWave = 2;
                selectedEnemy = enemies[0];
            }
            else if (currentWave == 3)
            {
                spawnTime = 1;
                numberOfEnemyPerWave = 2;
                selectedEnemy = enemies[1];
            }
            else if (currentWave == 4)
            {
                spawnTime = 1;
                numberOfEnemyPerWave = 3;
                selectedEnemy = enemies[2];
            }
            else if (currentWave == 5)
            {
                spawnTime = 1;
                numberOfEnemyPerWave = 5;
                selectedEnemy = enemies[0];
            }
            else if (currentWave == 6)
            {
                spawnTime = 2;
                numberOfEnemyPerWave = 6;
                selectedEnemy = enemies[1];
            }
            else if (currentWave == 7)
            {
                spawnTime = 1;
                numberOfEnemyPerWave = 7;
                selectedEnemy = enemies[0];
            }
            else if (currentWave == 8)
            {
                spawnTime = 1;
                numberOfEnemyPerWave = 8;
                selectedEnemy = enemies[1];
            }
            else if (currentWave == 9)
            {
                spawnTime = 1;
                numberOfEnemyPerWave = 17;
                selectedEnemy = enemies[1];
            }
            else if (currentWave == 10)
            {
                spawnTime = 2;
                numberOfEnemyPerWave = 20;
                selectedEnemy = enemies[0];
            }
            for (int i = 0; i < numberOfEnemyPerWave; i++)
            {
                Enemy enemyInstant = Instantiate(selectedEnemy, transform.GetChild(0).position, transform.GetChild(0).rotation)
                    as Enemy;
                if (currentWave != 0)
                {
                    enemyInstant.AddHealthAsPercent(10 * currentWave);
                }

                enemyInstant.transform.parent = enemyHolder.transform;
                yield return new WaitForSeconds(spawnTime);
            }

        }
        else
        {

            canCreateNextWave = false;
            uiEnvironment.GetComponent<UIEnvironment>().CallWinScreen();
            uiEnvironment.GetComponent<UIEnvironment>().SetAudioMute(true);
            //Bu kod gerekli degil dokunma sorunu tespiti icin koydum.
            FindObjectOfType<UIManager>().SetHelpLevelPanelIsActive(false);
            PlayerPrefs.SetString(HELP_LEVEL_STATE_NAME, HELP_LEVEL_ISCOMPLETED_NAME);
            PlayerPrefs.Save();
            
            Time.timeScale = 0f;
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
    public bool GetCanSpawnable()
    {
        return canSpawnable;
    }
}
