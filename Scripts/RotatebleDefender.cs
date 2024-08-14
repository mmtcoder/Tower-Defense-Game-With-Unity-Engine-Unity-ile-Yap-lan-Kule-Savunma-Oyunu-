using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatebleDefender : MonoBehaviour
{
    [SerializeField] Transform gunPlace;
    [Header("Turret Attributes")]
    [SerializeField] GameObject projectile;
    
    [SerializeField] float angularSpeed = 170f;
   
    const string PROJECTILE_HOLDER_NAME = "ToxicProjectileHolder";
    float oldAngle = 0f;
    float recordVelo = 0f;
    bool eksiYukseksendenArtiYukseksene = false;
    bool artiYukseksendenEksiYukseksene = false;
    bool canSelectNewEnemy = true;
    public bool canCreateNewProjectile { get; set; }

    Transform[] transformPaths;
    Enemy detectedEnemy;
    GameObject projectiles;

    GameObject projectileHolder;
    // Start is called before the first frame update
    void Start()
    {
        transformPaths = GameObject.FindGameObjectWithTag("Paths").GetComponentsInChildren<Transform>();
        canCreateNewProjectile = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (projectiles != null)
        {
            canCreateNewProjectile = projectiles.GetComponent<Projectile>().getCanDestroyObject();
            
        }
        else
        {
            canCreateNewProjectile = true;
            
        }
    }
    public void UpdateTarget(Enemy[] enemies)
    {
        if (enemies != null)
        {

            float maxDistance = gameObject.GetComponent<CircleCollider2D>().radius;
            Enemy[] enemyInBounds = new Enemy[enemies.Length];
            Enemy selectedEnemy = null;
            //Debug.Log("Array uzunlugu = " + enemies.Length);
            for (int i = 0; i < enemies.Length; i++)
            {
                // Debug.Log("Elde edilen enemyilerin sirasi = " + enemies[i].GetInstanceID() + "index numarasi  = " + i);
                // Debug.Log("Elde edilen enemyinin konumu= " + enemies[i].transform.position);

                float distanceToEnemy = Vector2.Distance(transform.position, enemies[i].transform.position);

                // Debug.Log("tespit edilen dusmanla arasindaki uzaklik = " + distanceToEnemy);
                if (distanceToEnemy < maxDistance)
                {

                    enemyInBounds[i] = enemies[i];
                }
            }
            canSelectNewEnemy = true;
            for (int k = enemyInBounds.Length - 1; k > -1; k--)
            {
                if (canSelectNewEnemy)
                {
                    if (enemyInBounds[k] != null)
                    {
                        // Debug.Log("enemyInBound bilgi = " + enemyInBounds[k].GetInstanceID() + "index numarasi  = " + k);
                        // Debug.Log("enemyInbound pozisyonu= " + enemyInBounds[k].transform.position);
                    }

                    for (int j = transformPaths.Length - 1; j > -1; j--)
                    {
                        if (enemyInBounds[k] != null)
                        {
                            if (Mathf.Abs(enemyInBounds[k].transform.position.y - transformPaths[j].position.y) <= 0.50f ||
                                Mathf.Abs(enemyInBounds[k].transform.position.x - transformPaths[j].position.x) <= 0.50f)
                            {
                                // Debug.Log("transformPath pozisyonu = " + transformPaths[j].position);
                                selectedEnemy = enemyInBounds[k];
                                canSelectNewEnemy = false;

                            }
                        }

                    }

                }
            }

            if (selectedEnemy != null)
            {
                
                detectedEnemy = selectedEnemy;
                RotationalMove(detectedEnemy);

            }
            else
            {
       
                detectedEnemy = null;
                canSelectNewEnemy = true;
            }


        }

    }
    /*
     * RotationalMove fonksiyonunda defenderin donus haraketi yapilmasi saglanmistir.Defenderin
     * ani donus yapmamasi icin oldAngle instance field yani en yukarida tanimlanmistir.
     * Arctan aldigi degerler -90 ile +90 arasıdır ve Defenderin donus acisinin her iki yana donmesini
     * saglamak icin dondurmeyi -180 ile +180 arasi yapildi.
     * @param detectedEnemy, tespit edilen dusmani yerini tespit etmek ve ona dogru defendirin dondurulmesini
     * saglar.
     */
    public void RotationalMove(Enemy detectedEnemy)
    {

        if (detectedEnemy != null)
        {
            float numerator = detectedEnemy.transform.position.x - transform.position.x;
            float denominator = detectedEnemy.transform.position.y - transform.position.y;
            float result = numerator / denominator;
            float deg = (Mathf.Atan(result) * Mathf.Rad2Deg);


           //  Debug.Log(gameObject.GetHashCode().ToString() + "bulunan ham degree = " + deg);


            if (denominator > Mathf.Epsilon)
            {

                if (numerator >= 0)
                {

                     //Debug.Log("1. bolge");
                     //Debug.Log("Payda sıfırdan buyuk, pay sıfırdan buyuk");
    

                    if (eksiYukseksendenArtiYukseksene)
                    {
                        //Debug.Log("eksiYukseksendenArtiYukseksene durumu etkin");
                        if (oldAngle > -180)
                        {
                            
                            oldAngle -= 3f;
                            transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, oldAngle);
                            //Debug.Log("Old Angle -180 e azaltiliyor" + oldAngle);
                        }
                     
                        else if (oldAngle == 0f)
                        {
                            oldAngle += Mathf.Epsilon;
                        }
                        if(oldAngle < -180)
                        {
                            oldAngle = oldAngle + 360f;
                            eksiYukseksendenArtiYukseksene = false;
                            artiYukseksendenEksiYukseksene = true;
                           // Debug.Log("Old Angle +360 derece + kendisi eklendi =" + oldAngle);
                        }
                       //  Debug.Log("eksiYukseksenden ... metoddaki OldAngle = " + oldAngle);
                    }
                    else
                    {

                        float newDeg = 180 - deg;
                        if (oldAngle > newDeg)
                        {
                            oldAngle -= angularSpeed * Time.deltaTime;

                        }
                        else
                        {
                            oldAngle += angularSpeed * Time.deltaTime;
                        }

                        if (Mathf.Abs(newDeg - oldAngle) >= (angularSpeed + 3) * Time.deltaTime)
                        {
                            if(Mathf.Abs(oldAngle - 180) < 1)
                            {
                                oldAngle += 1f;
                            }
                            transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, oldAngle);
                            artiYukseksendenEksiYukseksene = true;
                           // Debug.Log("1. bolgedeki new degree = " + newDeg + "ve oldAngle = " + oldAngle);
                        }
                        else
                        {
                            // Debug.Log("1. bolge Hedef takipte");
                            transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, newDeg);
                            CountingFire();
                            oldAngle = newDeg;
                            artiYukseksendenEksiYukseksene = true;

                        }
                    }

                }
                else
                {

                    // Debug.Log("2. bolge");
                    // Debug.Log("Payda sıfırdan buyuk, pay sıfırdan kucuk");
                   //  Debug.Log("OldAngle = " + oldAngle);
                    if (artiYukseksendenEksiYukseksene)
                    {
                       // Debug.Log("artiYukseksendenEksiYukseksene durumu etkin");
                        if (oldAngle < 180)
                        {
                            oldAngle += 3f;
                            transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, oldAngle);
                        }
                        else if (oldAngle == 0)
                        {
                            oldAngle += Mathf.Epsilon;
                        }
                        if(oldAngle > 180)
                        {
                            //burada bazen dusman takib hizina ulasilmadan dondurulmesi yarida kaliyor yani
                            //dusman menzilden cikiyor bu yuzden dusman eger 1. bolgeden ilk geciyorsa mutlaka
                            //"eksiYukseksendenArtiYukseksene" = true olmalı ve oldAngle -360 dereceden cikarilmali
                            oldAngle = oldAngle - 360f;
                            artiYukseksendenEksiYukseksene = false;
                            eksiYukseksendenArtiYukseksene = true;
                        }
                         // Debug.Log("artiYuksenden.. metoddaki oldAngle = " + oldAngle);
                    }
                    else
                    {
                        float newDeg = -deg - 180;
                        if (oldAngle > newDeg)
                        {
                         
                            oldAngle -= angularSpeed * Time.deltaTime;
                        }
                        else
                        {
                          
                            oldAngle += angularSpeed * Time.deltaTime;
                        }
                        if (Mathf.Abs(newDeg - oldAngle) >= (angularSpeed + 3) * Time.deltaTime)
                        {
                            if (Mathf.Abs(oldAngle + 180) < 1)
                            {
                                oldAngle += -1f;
                            }
                            transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, oldAngle);
                            eksiYukseksendenArtiYukseksene = true;
                           // Debug.Log("2. bolgedeki new degree = " + newDeg + "ve oldAngle = " + oldAngle);
                        }
                        else
                        {
                           // Debug.Log(" 2. bolge Hedef takipte");
                            transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, newDeg);
                            CountingFire();
                            oldAngle = newDeg;
                            eksiYukseksendenArtiYukseksene = true;
                        }
                    }

                }
            }
            //Arctan 0 da sonsuzdur o yuzden 0 olmamasını saglamak gerekir.
            else if (denominator >= -Mathf.Epsilon && denominator <= Mathf.Epsilon)
            {
                denominator += 0.2f;

            }
            else
            {
                if (numerator >= 0)
                {
                     // Debug.Log("4. bolge");
                     // Debug.Log("Payda sıfırdan kucuk, pay sıfırdan buyuk");
                    float newDeg = -deg;
                    if (oldAngle > newDeg)
                    {
                        oldAngle -= angularSpeed * Time.deltaTime;
                    }
                    else
                    {
                        oldAngle += angularSpeed * Time.deltaTime;
                    }
                    if (Mathf.Abs(newDeg - oldAngle) >= (angularSpeed + 3) * Time.deltaTime)
                    {
                        transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, oldAngle);
                        artiYukseksendenEksiYukseksene = false;
                        eksiYukseksendenArtiYukseksene = false;
                    }
                    else
                    {
                        // Debug.Log("Hedef takipte");
                        transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, newDeg);
                        CountingFire();
                        oldAngle = newDeg;
                        artiYukseksendenEksiYukseksene = false;
                        eksiYukseksendenArtiYukseksene = false;
                        float cıkart = newDeg - recordVelo;
                        //  Debug.Log("Takip etme hızı = " + cıkart);
                        recordVelo = newDeg;

                    }
                }
                else
                {
                    // Debug.Log("3. bolge");
                     //Debug.Log("Payda sıfırdan kucuk, pay sıfırdan kucuk");
                    float newDeg = -deg;
                    if (oldAngle > newDeg)
                    {
                        oldAngle -= angularSpeed * Time.deltaTime;
                    }
                    else
                    {
                        oldAngle += angularSpeed * Time.deltaTime;
                    }

                    if (Mathf.Abs(newDeg - oldAngle) >= (angularSpeed + 3) * Time.deltaTime)
                    {
                        transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, oldAngle);
                        artiYukseksendenEksiYukseksene = false;
                        eksiYukseksendenArtiYukseksene = false;
                    }
                    else
                    {
                        //  Debug.Log("Hedef takipte");
                        transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, newDeg);
                        CountingFire();
                        oldAngle = newDeg;
                        artiYukseksendenEksiYukseksene = false;
                        eksiYukseksendenArtiYukseksene = false;
                        float cıkart = newDeg - recordVelo;

                        //Debug.Log("Takip etme hızı = " + cıkart);
                        recordVelo = newDeg;

                    }


                }

            }


        }
        else
        {
            detectedEnemy = null;
        }
    }
    
    //Gerekesiz Fonksiyon
    private void CountingFire()
    {

       if (!projectileHolder)
        {
            CreateProjectileHolder();
        }
     
        InstantiateStableProjectile(gunPlace);
        /*if (creaturingProjectileTime == 1)
        {
            
        }
        
        creaturingProjectileTime -= Time.deltaTime;
        if (creaturingProjectileTime <= endProjectileTime)
        {
            creaturingProjectileTime = 1f;
        }*/

    }
    private void InstantiateStableProjectile(Transform defenderHeader)
    {
       
        
    
            if (defenderHeader != null)
            {

            if(canCreateNewProjectile)
            {
                projectiles = Instantiate(projectile, new Vector3(defenderHeader.transform.GetChild(0).position.x, defenderHeader.transform.GetChild(0).position.y
                       , defenderHeader.transform.GetChild(0).position.z), defenderHeader.rotation) as GameObject;
                Projectile fireProjectile = projectiles.GetComponent<Projectile>();
                if (projectiles != null)
                {

                    projectiles.GetComponent<AudioSource>().enabled = true;
                        fireProjectile.attachEnemyToFollow(defenderHeader);
             
                }
                projectiles.transform.parent = projectileHolder.transform;

            }
            canCreateNewProjectile = false;


        }
       

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

