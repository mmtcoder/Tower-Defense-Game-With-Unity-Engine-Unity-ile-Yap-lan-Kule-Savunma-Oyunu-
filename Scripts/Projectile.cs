

using UnityEngine;

public class Projectile : MonoBehaviour
{


    [SerializeField] float fireDamage ;
    [SerializeField] float toxicSmokeDamage ;
    [SerializeField] float deceleratorDamage ;
    [SerializeField] float kamikazeDamage ;
    [SerializeField] float toxicEffectDestroyTime;
    [SerializeField] float enemySlowDownTime;
    [SerializeField] float enemySlowDownRate;
    [SerializeField] float speed = 8.0f;
    Transform transformHeaderPlace;
    Vector2 enemyPlaceForDeceProjectile;
    
    Enemy detectedEnemy;
    bool canDestroyObject = false;
    bool canDestroyToxicFire = false;
    bool canStopDeceProjectileMotion = true;
    Collider2D colliderOfProjectile;
    private Animator deceProjectileAnimator;

    // Start is called before the first frame update
    void Start()
    {
        deceProjectileAnimator = gameObject.GetComponent<Animator>();
        
    }

   
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(name.Contains("Fire"))
        {
            collision.GetComponent<Enemy>().TakeDamege(fireDamage * Time.deltaTime);
        }
        else if(name.Contains("Toxic"))
        {
            collision.GetComponent<Enemy>().TakeDamege(toxicSmokeDamage *Time.deltaTime);
        }
        setCanDestroyObject(false);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if(name.Contains("Fire"))
        {
            
        }*/
       // Debug.Log("dusman gorulmuyor");
        setCanDestroyObject(true);

    }
    public void attachEnemyToFollow(Transform header)
    {
        transformHeaderPlace = header;
    }
    public bool getCanDestroyObject()
    { return canDestroyObject; }

    // Update is called once per frame
    void Update()
    {
        
        if (canDestroyObject)
        {
            if(name.Contains("Toxic"))
            {
              
                Destroy(gameObject, 0.5f);
            }
            //Eklenecek defendirlarin mermileri bu sinifi kapsiyor ise yok etme kismina buraya mermilerin isimlerini ekle
            else if(name.Contains("Fire"))
            {
                
                Destroy(gameObject);
            }
           
        }
        //Bütün mermi türleri icin gecerli. haritada dusman yoksa mermilerin hepsini temizle
        else if (FindObjectsOfType<Enemy>().Length == 0)
        {
           
            Destroy(gameObject);
        }
        else
        {
            MoveUnstableProjectile();
            MoveStableProjectile();
        }

        /*if (canDestroyToxicFire)
        {
            Debug.Log("toxic fire yoket");
            Destroy(gameObject, 0.52f);
        }*/
    }
    public void setNeededInfoForToxicProjectile(Transform gunPlace, Enemy target)
    {
        transformHeaderPlace = gunPlace;
        detectedEnemy = target;
    }
    public void InformEnemyPositionForDeceProjectile(Vector2 enemy)
    {
        enemyPlaceForDeceProjectile = enemy;
    }
    public void setCanDestroyObject(bool state)
    {
        canDestroyObject = state;
    }
    private void MoveUnstableProjectile()
    {
        float distance;
        if (gameObject != null && name.Contains("Fire"))
        {
            transform.position = transformHeaderPlace.GetChild(0).position;
            transform.rotation = transformHeaderPlace.rotation;
        }else if(gameObject != null  && name.Contains("Dece"))
        {
           // Debug.Log(GetInstanceID() + " burada dusman algilamasi var");                                                                   
          
            if(enemyPlaceForDeceProjectile != null)
            {
                if (canStopDeceProjectileMotion)
                {

                  //  Debug.Log("dusman pozisyonu " + enemyPlaceForDeceProjectile);
                    //Debug.Log(GetInstanceID() + " haraket ettirilmeden önceki pozisyonu = " + transform.position);
                    transform.position = Vector2.MoveTowards(transform.position, enemyPlaceForDeceProjectile, speed * Time.deltaTime);
                    // Debug.Log(GetInstanceID() + " haraket ettirdikten sonraki pozisyonu = " + transform.position);
                    distance = Vector2.Distance(transform.position, enemyPlaceForDeceProjectile);
                    if (distance <= speed * Time.deltaTime)
                    {
                        deceProjectileAnimator.SetBool("makeExplosion", true);
                        gameObject.GetComponent<CircleCollider2D>().enabled = true;
                        canStopDeceProjectileMotion = false;
                    }
                }
            }
 
                }

    }
    private void MoveStableProjectile()
    {
        if(name.Contains("Toxic") && gameObject != null)
        {
            if(detectedEnemy != null && transformHeaderPlace != null)
            {
              
                if (transformHeaderPlace.position.y - detectedEnemy.transform.position.y < -0.5f)
                {
                    gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 4;
                    transform.position = new Vector3(transformHeaderPlace.GetChild(1).position.x, transformHeaderPlace.GetChild(1).position.y +2.28f
                               , transformHeaderPlace.GetChild(1).position.z);
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                   
                }
                else if (transformHeaderPlace.position.y - detectedEnemy.transform.position.y > 0.5f)
                {
                   // Debug.Log("merminin önden cıkması gerekiyor.");
                    gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 6;
                    transform.position = new Vector3(transformHeaderPlace.GetChild(1).position.x, transformHeaderPlace.GetChild(1).position.y
                               , transformHeaderPlace.GetChild(1).position.z);
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                   
                }
            }
        }
        
    }
    public float getToxicSmokeDamage()
    {
        return toxicSmokeDamage;
    }
  public float getDeceProjectileDamage()
    {
        return deceleratorDamage;
    }
    public float GetKamikazeProjectileDamage()
    { return kamikazeDamage; }
    public float getToxicEffectDestroyTime()
    {
        return toxicEffectDestroyTime;
    }
    public float getSlowDownTime()
    {
        return enemySlowDownTime;
    }
    public float getSlowDownRate()
    {
        return enemySlowDownRate;
    }
    public void DestroyDeceProjectile()
    {
        
        deceProjectileAnimator.SetBool("makeExplosion", false);
        Destroy(gameObject);
    }
    public float GetProjectileSpeed()
    {
        return speed;
    }
    public void SetProjectileSpeed(float speed)
    {
        this.speed = speed;
    }
   
    //Merminin Hedefi takip etmesi icin bu fonksiyon kullanilmalidir.
    //transform.position = Vector2.MoveTowards(transform.position, detectedEnemy.transform.position, step);

}
