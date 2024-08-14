
using UnityEngine;

public class Defender : MonoBehaviour
{

    // [SerializeField] Projectile projectile2;
    [SerializeField] int defenderCost = 100;
    
    
    
    const string ENEMY_HOLDER_NAME = "EnemyHolder";
    

    private void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        Enemy[] detectedEnemys = FindObjectsOfType<Enemy>();
      
        if(name.Contains("Fire"))
        {
            gameObject.GetComponent<RotatebleDefender>().UpdateTarget(detectedEnemys);
        }else if(gameObject.GetComponent<StableDefender>())
        {
            gameObject.GetComponent<StableDefender>().UpdateTarget(detectedEnemys);
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!name.Contains("Fire"))
        {
            if(gameObject.GetComponent<StableDefender>())
            {
                gameObject.GetComponent<StableDefender>().OutOfBoundEnemy();
            }
            
        }
        
    }

    public int GetDefenderCost()
    {
        return defenderCost;
    }
}
