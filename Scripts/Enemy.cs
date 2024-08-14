
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    [Header("Unity Stuff")]
    [SerializeField] Image healthBackGround;
    [SerializeField] Image healthBar;
    [SerializeField] ParticleSystem toxicEffect;
 
    [Header("Enemy Attributes")]
    [SerializeField] float speed = 4f;
    [SerializeField] float angleSpeed = 200f;
    public float startHealth ;
    [SerializeField] int coinReward = 5;
    [Header("Gormek icin degere dokunma!")]
    public float currentHealth;
    GameObject paths;
    Transform enemyPathTransform = null;
    UIEnvironment uiEnvironment;
    private int pathNodeIndex = 0;
    private int jumpStartPathIndex = -3;
    private int jumpEndPathIndex = -3;
    private int crossRoadStartIndex = -4;
    private int crossRoadSelectedWayIndex = -4;
    private int crossRoadFirstWayEndIdex = -5;
    private int crossRoadFirstConnectMainWayIndex = -5;

    private float currentSpeed;

    float currentSlowDownTime = 0f;
    float slowDownRate = 0f;
    float slowDownEffectDestroyTime;
    float currentToxicTimer = 0f;
    float toxicEffectDamage = 0f;
    float toxicEffectDestroyTime;
    

    bool canStopToxicEffect = true;
    bool effectOfToxic = false;
    bool effectOfSlowDown = false;
    private bool activateSecondPath = false;
    // Start is called before the first frame update
    void Start()
    {
        paths = GameObject.FindGameObjectWithTag("Paths");
        currentHealth = startHealth;
        currentSpeed = speed;
        uiEnvironment = FindObjectOfType<UIEnvironment>();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyEnemy();
        MoveEnemysTowardPaths();
        if(canStopToxicEffect)
        {
           // Debug.Log("toxicEffectin durması gerek");
            toxicEffect.Stop();
        }else
        {
          if(effectOfToxic)
            {
               // Debug.Log("toxic effect destroy time = " + toxicEffectDestroyTime);
                if(currentToxicTimer >= toxicEffectDestroyTime)
                {
                    //Eger enemy toxic mermisine degmiyorsa ve etki suresi bitmis ise
                    //effect ve hasar alinmasi durdurulur.
                    canStopToxicEffect = true;
                    effectOfToxic = false;
                    currentToxicTimer = 0f;
                    //Debug.Log("effectin durmasi gerek");

                }
                else
                {
                    float tocixEffectDamageWithTime = toxicEffectDamage * Time.deltaTime;
                    TakeDamege(tocixEffectDamageWithTime / 2f); ;
                    currentToxicTimer += Time.deltaTime;
                   // Debug.Log("effectin calisip enemynin hasar alması gerek");
                }
            }
        }
        if (effectOfSlowDown)
        {
            if (currentSlowDownTime >= slowDownEffectDestroyTime)
            {

                currentSlowDownTime = 0f;
                currentSpeed = getSpeed();
                effectOfSlowDown = false;
            }
            else
            {

                currentSpeed = getSpeed() - (getSpeed() * (slowDownRate / 100));
               
                currentSlowDownTime += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Toxic mermisi hedefe degdigi zaman toxic effecti başlatilir ve canStopToxicEffect ifadesi
        //false verilerek effectin durdurulmasi onlenilir.
        if(collision.name.Contains("ToxicPro"))
        {
            toxicEffectDestroyTime = collision.GetComponent<Projectile>().getToxicEffectDestroyTime();
            canStopToxicEffect = false;
            toxicEffect.Play();
        }  
        if(collision.name.Contains("DeceleratorPro"))
        {
            if(collision != null)
            {
                effectOfSlowDown = true;
                currentSlowDownTime = 0f;
                TakeDamege(collision.GetComponent<Projectile>().getDeceProjectileDamage());
                slowDownEffectDestroyTime = collision.GetComponent<Projectile>().getSlowDownTime();
                slowDownRate = collision.GetComponent<Projectile>().getSlowDownRate();
            }
            
        }
        if(collision.name.Contains("Kami"))
        {
            TakeDamege(collision.GetComponent<Projectile>().GetKamikazeProjectileDamage());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Eğer başka bir toxicDefenderın mermisine deydiyse effectin zamanlayıcısını sıfırlar.
        if(collision.name.Contains("ToxicPro"))
        {
            currentToxicTimer = 0f;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
    if(collision != null && collision.name.Contains("ToxicPro"))
        {
            //Toxicprojectile in verdiği hasar okunur ve hedef menzilden çiktiktan sonra zehirden hasar alinmasi başlatilir.
            toxicEffectDamage = collision.GetComponent<Projectile>().getToxicSmokeDamage();
           // Debug.Log("enemy toxic hasarı = " + toxicEffectDamage);
            effectOfToxic = true;
            //Debug.Log("enemynin hasar alinmasi baslatılması gerek");
        }
       
        
    }
    private void GetPathNextNode()
    {
      
        if (pathNodeIndex < paths.transform.childCount )
        {
            if (pathNodeIndex  == jumpStartPathIndex + 1)
            {
                pathNodeIndex = jumpEndPathIndex;
            }
            if(pathNodeIndex == crossRoadStartIndex +1)
            {
                pathNodeIndex = crossRoadSelectedWayIndex;
            }
            else if(pathNodeIndex == crossRoadFirstWayEndIdex +1)
            {
                pathNodeIndex = crossRoadFirstConnectMainWayIndex;
            }
         

            enemyPathTransform = paths.transform.GetChild(pathNodeIndex);
            pathNodeIndex++;
            
        }
       

        
    }
    private void MoveEnemysTowardPaths()
    {
        if (enemyPathTransform == null)
        {
            GetPathNextNode();
            if (enemyPathTransform == null)
            {
                GetEndedNodePath();
            }
        }
        if (enemyPathTransform != null)
        {
            Vector2 direction = enemyPathTransform.position - transform.localPosition;
            /* if (gameObject.transform.position != enemyPathTransform.position )
             {
                 transform.Translate(new Vector2( speed * Time.deltaTime, 0));
             }*/
            float distThisFrame = currentSpeed * Time.deltaTime;
            

            if (direction.magnitude <= distThisFrame)
            {
                enemyPathTransform = null;

            }
            else
            {
                Vector3 myLocation = transform.position;
                Vector3 targetLocation = enemyPathTransform.position;
                targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position

                // vector from this object towards the target location
                Vector3 vectorToTarget = targetLocation - myLocation;
                // rotate that vector by 90 degrees around the Z axis
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;

                // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
                // (resulting in the X axis facing the target)
                Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

                // changed this from a lerp to a RotateTowards because you were supplying a "speed" not an interpolation value
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angleSpeed * Time.deltaTime);

                transform.position = new Vector2(transform.position.x + direction.normalized.x * distThisFrame,
                   transform.position.y + direction.normalized.y * distThisFrame);
             
              
            }
        }
    }
    private void GetEndedNodePath()
    {
        FindObjectOfType<UIEnvironment>().SetHealthText(1);
        Destroy(gameObject);
    }
   public void TakeDamege(float damage)
    {
        if(currentHealth > 0 )
        {
            currentHealth -= damage;
            healthBackGround.gameObject.SetActive(true);
            healthBar.fillAmount = currentHealth / startHealth;
        }
    }
    private void DestroyEnemy()
    {
        if(currentHealth <= 0)
        {
            if(uiEnvironment != null)
            {
                uiEnvironment.GetComponent<UIEnvironment>().SetCoinText(GetCoinReward());
            }
           

            Destroy(gameObject);
        }
    }
    private int GetCoinReward()
    {
        return coinReward;
    }
    public float getHealth()
    {
        //Debug.Log("startHealth getter fonk = " + startHealth);
        return startHealth;
    }
    public float getSpeed()
    { return speed; }
    public void AddHealthAsPercent(float rate)
    {
        startHealth =getHealth() + (getHealth() * (rate / 100f));
        currentHealth = getHealth();
       //string s = string.Format("startHealth{0} ve currentHealth{1}", startHealth, currentHealth);
        //Debug.Log(s);
    }
    public void SetSpawnPoint(int pathIndex)
    {
        pathNodeIndex = pathIndex;
    }
    public void ActivateSecondPath(bool state)
    {
        activateSecondPath = state;
    }
    public void SetJumpPathsBetweenIndexes(int indexStart,int indexEnd)
    {
        jumpStartPathIndex = indexStart;
        jumpEndPathIndex = indexEnd;
    }
    public void MakeCrossRoad(int indexStart,int indexSelectedWay,int indexFirstWayEnd, int indexFirstWayToMainWay)
    {
        crossRoadStartIndex = indexStart;
        crossRoadSelectedWayIndex = indexSelectedWay;
        crossRoadFirstWayEndIdex = indexFirstWayEnd;
        crossRoadFirstConnectMainWayIndex = indexFirstWayToMainWay;
    }
}
