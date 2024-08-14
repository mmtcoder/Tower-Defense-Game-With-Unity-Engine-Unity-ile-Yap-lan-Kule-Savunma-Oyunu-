using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpLevel : MonoBehaviour
{
    public GameObject textBackground;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject text5;
    public GameObject text6;
    public GameObject text7;
    public GameObject text8;
    public GameObject text9;
    public GameObject helperSelector;
    public GameObject electricDef;
    public GameObject fireDef;
    public GameObject deceleraDef;
    public GameObject technoDef;
    public GameObject toxicDef;

    GameObject attackerSpawner;
    GameObject uiManager;
    int textIndex = 0;
    float timer = 0;
    bool canSpawnEnemy = false;
    bool isNeedToSpawnControl = true;
    // Start is called before the first frame update
    void Start()
    {
        attackerSpawner = FindObjectOfType<HelpLevelAtackerSpawner>().gameObject;
        uiManager = FindObjectOfType<UIManager>().gameObject;
        attackerSpawner.GetComponent<HelpLevelAtackerSpawner>().SetCanSpawnable(false);
        uiManager.GetComponent<UIManager>().SetHelpLevelPanelIsActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        ControlSpawnEnemy();
    }

    public void NextButton()
    {
        textIndex++;
        
        switch (textIndex)
        {
            case 1:
                text1.SetActive(false);
                text2.SetActive(true);
                canSpawnEnemy = true;
                toxicDef.SetActive(true);
                break;
            case 2:
                text2.SetActive(false);
                text3.SetActive(true);
                toxicDef.SetActive(false);
                fireDef.SetActive(true);
                deceleraDef.SetActive(true);
                electricDef.SetActive(true);
                canSpawnEnemy = true;
                break;
            case 3:
                text3.SetActive(false);
                text4.SetActive(true);
                fireDef.SetActive(false);
                technoDef.SetActive(true);
                deceleraDef.SetActive(false);
                electricDef.SetActive(false);
                break;
            case 4:
                text4.SetActive(false);
                text5.SetActive(true);
                canSpawnEnemy = true;
                break;
            case 5:
                text5.SetActive(false);
                text6.SetActive(true);
                technoDef.SetActive(false);
                helperSelector.SetActive(true);
                
                break;
            case 6:
                text6.SetActive(false);
                text7.SetActive(true);
                helperSelector.SetActive(false);

                break;
            case 7:
                text7.SetActive(false);
                text8.SetActive(true);
                break;
            case 8:
                text8.SetActive(false);
                text9.SetActive(true);
                break;
            case 9:
                text9.SetActive(false);
                textBackground.SetActive(false);
                isNeedToSpawnControl = false;
                attackerSpawner.GetComponent<HelpLevelAtackerSpawner>().SetCanSpawnable(true);
                uiManager.GetComponent<UIManager>().SetHelpLevelPanelIsActive(false);
                break;
            default:
                break;
        }
    }

    private void ControlSpawnEnemy()
    {
        if(isNeedToSpawnControl)
        {
            if (canSpawnEnemy)
            {
                if (timer >= 4f)
                {
                    canSpawnEnemy = false;
                    attackerSpawner.GetComponent<HelpLevelAtackerSpawner>().SetCanSpawnable(false);
                    timer = 0f;
                }
                else
                {
                    timer += Time.deltaTime;
                    attackerSpawner.GetComponent<HelpLevelAtackerSpawner>().SetCanSpawnable(true);
                }

            }
            else
            {
                attackerSpawner.GetComponent<HelpLevelAtackerSpawner>().SetCanSpawnable(false);
            }
        }
        

    }
}
