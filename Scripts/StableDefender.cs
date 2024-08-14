using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StableDefender : MonoBehaviour
{

   
    [SerializeField] GameObject projectile;
    [SerializeField] float creaturingProjectileTime = 1f;
    [SerializeField] float endProjectileTime = 0.5f;
    [SerializeField] float electricDamage ;
    public ParticleSystem electricEffect;
    Transform[] transformPaths;
    bool canSelectNewEnemy = true;
    bool canFireDeceleratorProjectile = false;
    Enemy detectedEnemy;
    GameObject projectiles;
    GameObject projectileHolder;
    private Animator animator;
    
    const string PROJECTILE_HOLDER_NAME = "ToxicProjectileHolder";
    public bool canCreateNewProjectile { get; set; }
 
    // Start is called before the first frame update
    void Start()
    {
        transformPaths = GameObject.FindGameObjectWithTag("Paths").GetComponentsInChildren<Transform>();
        animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
     
        if(projectiles == null)
        {
            canCreateNewProjectile = true;
        }
        
    }
    public void UpdateTarget(Enemy[] enemies)
    {
        if (enemies != null)
        {
            Enemy[] enemyInBounds = new Enemy[enemies.Length];
            Enemy selectedEnemy = null;
            if (gameObject.GetComponent<CircleCollider2D>() != null)
            {
                float maxDistance = gameObject.GetComponent<CircleCollider2D>().radius;

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
            }else
            {
                float maxDistance = gameObject.GetComponent<BoxCollider2D>().size.y/2f;
               
                for (int i = 0; i < enemies.Length; i++)
                {
                    // Debug.Log("Elde edilen enemyilerin sirasi = " + enemies[i].GetInstanceID() + "index numarasi  = " + i);
                    // Debug.Log("Elde edilen enemyinin konumu= " + enemies[i].transform.position);

                   // float distanceToEnemy = Vector2.Distance(transform.position, enemies[i].transform.position);

                    // Debug.Log("tespit edilen dusmanla arasindaki uzaklik = " + distanceToEnemy);
                    /*if (distanceToEnemy < maxDistance)
                    {

                        enemyInBounds[i] = enemies[i];
                    }*/
                    if(gameObject.GetComponent<BoxCollider2D>().OverlapPoint(enemies[i].transform.position))
                    {
                       
                        enemyInBounds[i] = enemies[i];
                    }
                }
                }
            //Yeni eklendi sorun cıkarsa buna bir bak
            canSelectNewEnemy = true;

            for (int k = enemyInBounds.Length - 1; k > -1; k--)
            {
                if (canSelectNewEnemy)
                {
                   /* if (enemyInBounds[k] != null)
                    {
                         //Debug.Log("enemyInBound bilgi = " + enemyInBounds[k].GetInstanceID() + "index numarasi  = " + k);
                         //Debug.Log("enemyInbound pozisyonu= " + enemyInBounds[k].transform.position);
                    }*/

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
                CountingFire();

            }
            else
            {

                detectedEnemy = null;
                canSelectNewEnemy = true;
            }


        }

    }
    public void OutOfBoundEnemy()
    {
    /*    if(detectedEnemy != null)
        {
            float maxDistance = gameObject.GetComponent<CircleCollider2D>().radius;
            float distanceToEnemy = Vector2.Distance(transform.position, detectedEnemy.transform.position);

            // Debug.Log("tespit edilen dusmanla arasindaki uzaklik = " + distanceToEnemy);
            if (distanceToEnemy > maxDistance)
            { }
        }*/
        if (name.Contains("Electric"))
        { 
          
            if (gameObject.GetComponent<LineRenderer>().enabled || FindObjectsOfType<Enemy>().Length == 0)
                    {
                        gameObject.GetComponent<LineRenderer>().enabled = false;
                        electricEffect.Stop();
                       gameObject.GetComponent<AudioSource>().enabled = false;
            }

                    
        }
        else if(name.Contains("Toxic"))
        {
            if (projectiles != null)
            {
                projectiles.GetComponent<Projectile>().setCanDestroyObject(true);
            }
                
        }
        else
        {
            
            animator.SetBool("isAttack", false);
            canFireDeceleratorProjectile = false;
        }
    }
                
    private void Fire(Enemy target)
    {
        
        if (target != null)
        {
            if(name.Contains("Electric"))
            {
               
                gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.GetChild(1).position);
                gameObject.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
                if(!gameObject.GetComponent<LineRenderer>().enabled)
                {
                    gameObject.GetComponent<LineRenderer>().enabled = true;
                    electricEffect.Play();
                    gameObject.GetComponent<AudioSource>().enabled = true;
                }
                //Electric defenderin vurdugu hasar.
                target.TakeDamege(electricDamage * Time.deltaTime);
                Vector3 dir = transform.GetChild(1).position - target.transform.position;

                electricEffect.transform.position = target.transform.position;
               

            }
            else if(name.Contains("Dece"))
            {
               // Debug.Log(GetInstanceID() + "canFireDeceleratorProjectile" + canFireDeceleratorProjectile);
                animator.SetBool("isAttack", true);
                if(canFireDeceleratorProjectile)
                {
                    projectiles = Instantiate(projectile, new Vector3(transform.GetChild(1).position.x,
                        transform.GetChild(1).position.y, transform.GetChild(1).position.z), Quaternion.identity) as GameObject;
                    //Projectile deceleratorProjectile = projectiles.GetComponent<Projectile>();
                    //deceleratorProjectile.setNeededInfoForToxicProjectile(transform.GetChild(1), target);
                    
                    projectiles.GetComponent<Projectile>().InformEnemyPositionForDeceProjectile(target.transform.position);
                   // Debug.Log(GetInstanceID() + "dusmanin poz = " + target.transform.position);
                    projectiles.transform.parent = projectileHolder.transform;
                    canFireDeceleratorProjectile = false;
                    animator.SetBool("isAttack", false);
                }
            }
            else
            {
                if (canCreateNewProjectile)
                {
                    if (transform.position.y - target.transform.position.y < -0.5f)
                    {
                        // Debug.Log("merminin arkadan cıkması gerekiyor.");
                        projectiles = Instantiate(projectile, new Vector3(transform.GetChild(1).position.x, transform.GetChild(1).position.y + 2.1f
                                  , transform.GetChild(1).position.z), Quaternion.Euler(0f, 0f, 180f)) as GameObject;
                        //Projectile fireProjectile = projectiles.GetComponent<Projectile>();
                        projectiles.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
                        projectiles.transform.parent = projectileHolder.transform;

                    }
                    else if (transform.position.y - target.transform.position.y > 0.5f)
                    {
                        //Debug.Log("merminin önden cıkması gerekiyor.");
                        projectiles = Instantiate(projectile, new Vector3(transform.GetChild(1).position.x, transform.GetChild(1).position.y
                                   , transform.GetChild(1).position.z), Quaternion.identity) as GameObject;
                        //Projectile fireProjectile = projectiles.GetComponent<Projectile>();
                        projectiles.GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
                        projectiles.transform.parent = projectileHolder.transform;


                    }
                }
                canCreateNewProjectile = false;
               
 
                if (projectiles != null)
                {

                        projectiles.GetComponent<Projectile>().setNeededInfoForToxicProjectile(transform, target);
                 
                }

                //Debug.Log("sinir icindeki dusman pozisyonu = " + target.transform.position);
            }
        }
            
        
    }
    public void AllowDeceleratorProjectile()
    {
        canFireDeceleratorProjectile = true;
    }
    private void CountingFire()
    {

        if (!projectileHolder)
        {
            CreateProjectileHolder();
        }
        Fire(detectedEnemy);
        
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
