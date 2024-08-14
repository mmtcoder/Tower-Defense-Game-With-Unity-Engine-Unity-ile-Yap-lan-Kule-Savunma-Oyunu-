using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Mermi yokken hedef seciliyor ise mermi tekrar uretilmiyor.Bu sorun duzeltilecek
 */ 
public class TechnologicDefender : MonoBehaviour
{
    
    [SerializeField] GameObject projectile;
 
    Transform[] transformPaths;
    Enemy detectedEnemy;
    Enemy selectedEnemy;
    GameObject projectiles;
    GameObject projectileHolder;
    Vector2 touchedArea;
    private Animator animator;
    bool canSelectNewEnemy = true;
    bool canCreateProjectile = false;
    bool canActiveManuelDetection = false;
    const string PROJECTILE_HOLDER_NAME = "ToxicProjectileHolder";
    // Start is called before the first frame update
    void Start()
    {
        transformPaths = GameObject.FindGameObjectWithTag("Paths").GetComponentsInChildren<Transform>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        //Burada dusman yokken veya mermi olusturulmamısken mermi otomatik hedef alınmasını 36 satırdaki kosul saglar.
        if(enemies.Length == 0 || projectiles == null)
        {
            
         canActiveManuelDetection = false;
            
          
        }//Burada da eger secilmis hedef drone patlamadan once imha olmus ise ve drone hayatta ise 
        //otomatik hedef alınması saglanır.
        else if( selectedEnemy == null && projectiles != null)
        {
            canActiveManuelDetection = false;
        }
            CreateProjectileHolder();
            ManualTargetDetection(enemies);
            AutomaticTargetDetection(enemies);
        
        
    }

    private void AutomaticTargetDetection(Enemy[] enemies)
    {
        if(!canActiveManuelDetection)
        {
            if (enemies != null || enemies.Length != 0)
            {
                Enemy selectedEnemy = null;
                canSelectNewEnemy = true;

                /* if (enemies[k] != null)
                 {
                      //Debug.Log("enemyInBound bilgi = " + enemyInBounds[k].GetInstanceID() + "index numarasi  = " + k);
                      //Debug.Log("enemyInbound pozisyonu= " + enemyInBounds[k].transform.position);
                 }*/

                for (int j = transformPaths.Length - 1; j > -1; j--)
                {

                    for (int k = enemies.Length - 1; k > -1; k--)
                    {
                        if (canSelectNewEnemy)
                        {
                            if (enemies[k] != null)
                            {
                                if (Mathf.Abs(enemies[k].transform.position.y - transformPaths[j].position.y) <= 0.50f ||
                                    Mathf.Abs(enemies[k].transform.position.x - transformPaths[j].position.x) <= 0.50f)
                                {
                                    // Debug.Log("transformPath pozisyonu = " + transformPaths[j].position);

                                    selectedEnemy = enemies[k];
                                   // Debug.Log("Otomatik hedef tespit edildi");
                                    canSelectNewEnemy = false;

                                }
                            }
                        }
                    }
                }
                if (selectedEnemy != null)
                {

                    detectedEnemy = selectedEnemy;
                    InstantiateKamikazeProjectile(detectedEnemy);

                }
                else
                {

                    detectedEnemy = null;
                    canSelectNewEnemy = true;
                }
            }
        }
        
        
    }

    private void ManualTargetDetection(Enemy[] enemies)
    {
        if(enemies.Length ==0 || enemies != null)
        {
            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);
                touchedArea = Camera.main.ScreenToWorldPoint(touch.position);


                if (touch.phase == TouchPhase.Ended)
                {
                    bool checkThisLoop = true;

                    for (int k = enemies.Length - 1; k > -1; k--)
                    {
                        if (checkThisLoop)
                        {
                            if (Mathf.Abs(enemies[k].transform.position.x - touchedArea.x) <= 1f &&
                           Mathf.Abs(enemies[k].transform.position.y - touchedArea.y) <= 1f)
                            {
                                // Debug.Log("Hedef secildi");
                                canActiveManuelDetection = true;
                                selectedEnemy = enemies[k];
                                if(!FindObjectOfType<UIEnvironment>().GetGameIsStopMode())
                                {
                                    FindObjectOfType<UIManager>().EnableArrowSelection(selectedEnemy);
                                }
                              
                               
                                InstantiateKamikazeProjectile(selectedEnemy);
                                checkThisLoop = false;
                            }
                        }

                    }
                }
                //alttaki if kısmi mermi yokken eger hedef secilmis ise merminin olusturulmasini saglar
                //diger turlu bu fonk tıklandıgı zaman calisiyor ve tiklama bittigi zaman eger merminin
                //cikarilma anina denk gelirse mermi olusturulmaz boylece bu hatayi burada gideriyoruz.
            }if(selectedEnemy != null)
            {
                InstantiateKamikazeProjectile(selectedEnemy);
            }
        }
       
    }
    private void InstantiateKamikazeProjectile(Enemy target)
    {
        if (projectiles == null && target != null)
        {
           
            animator.SetBool("isAttack", true);

            if (canCreateProjectile)
            {
                
                projectiles = Instantiate(projectile, transform.GetChild(1).position, Quaternion.identity) as GameObject;
                projectiles.GetComponent<KamikazeProjectile>().AttachAnEnemy(target);
                canCreateProjectile = false;
                animator.SetBool("isAttack", false);
                projectiles.transform.parent = projectileHolder.transform;
            }
        } if(projectiles != null && target != null)
        {
            projectiles.GetComponent<KamikazeProjectile>().AttachAnEnemy(target);
        }
    }
    public void SetCanCreateProjectile()
    {
        canCreateProjectile = true;
    }
    private void CreateProjectileHolder()
    {
        projectileHolder = GameObject.Find(PROJECTILE_HOLDER_NAME);
        if (!projectileHolder)
        {
            projectileHolder = new GameObject(PROJECTILE_HOLDER_NAME);
        }
    }
    public void DestroyProjectile()
    {
        // Debug.Log("Projectile yok edildi");
        Destroy(projectiles);
    }
}
