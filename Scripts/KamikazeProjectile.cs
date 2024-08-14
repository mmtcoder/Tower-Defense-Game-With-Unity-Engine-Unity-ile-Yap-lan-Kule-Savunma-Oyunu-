using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class KamikazeProjectile : MonoBehaviour
{
    Projectile projectile;
    private Enemy selectedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        projectile = gameObject.GetComponent<Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckProjectileReachedToTarget();
        GoToTarget();
    }

    private void GoToTarget()
    {
        if(selectedEnemy != null)
        {
            float distThisFrame = projectile.GetProjectileSpeed() * Time.deltaTime;
            float angleSpeed = 100f;
            Vector2 direction = selectedEnemy.transform.position - transform.localPosition;

            Vector3 myLocation = transform.position;
            Vector3 targetLocation = selectedEnemy.transform.position;
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
            
            //Debug.Log(distThisFrame);
        }
    }

    public void AttachAnEnemy(Enemy enemy)
    {
        selectedEnemy = enemy;
    }

    private void CheckProjectileReachedToTarget()
    {
        if(selectedEnemy != null)
        {
            if (Mathf.Abs(selectedEnemy.transform.position.x - transform.position.x) <= 0.6f &&
            Mathf.Abs(selectedEnemy.transform.position.y - transform.position.y) <= 0.6f)
            {
                gameObject.GetComponent<Animator>().SetBool("makeExplosion", true);
            }
        }
        
    }
    public void EnableColliderAndDestroy()
    {
   
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
 
        Destroy(gameObject, 1.2f);
    }
    public void DisableCollider()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

    }
}
