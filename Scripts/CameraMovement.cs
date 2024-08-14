using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    
    public Vector3 startPos;
    public Vector3 direction;
    public GameObject forbiddenAreaSign;

    public float yPosOfCamera = 14.49f;
    
    public float minYPosOfCamera = 9.49f;
    private float moveSpeed = 4f;
    GameObject uiManager;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    startPos = Camera.main.ScreenToWorldPoint(touch.position);
                    
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    uiManager.SetActive(false);
                    forbiddenAreaSign.SetActive(false);
                    // Determine direction by comparing the current touch position with the initial one
                    direction = Camera.main.ScreenToWorldPoint(touch.position) - startPos;
                    //Debug.Log("surukleme yonu = " + direction);
                   /* if(direction.x < 0 && transform.position.x < maxXPosOfCamera)
                    {
                       // Debug.Log("kameranın x pozisyonu = " + transform.position.x);
                        transform.position = new Vector3(transform.position.x - direction.x * Time.deltaTime * moveSpeed,
                          transform.position.y, -10);
                    }else if(direction.x > 0 && transform.position.x > xPosOfCamera)
                    {
                       // Debug.Log("kameranın x pozisyonu = " + transform.position.x);
                        transform.position = new Vector3(transform.position.x - direction.x * Time.deltaTime * moveSpeed,
                         transform.position.y, -10);
                    }*/
                    //Buraya kesin belirlenen koordinatlarda durmasi icin ek bir algorima eklenmeli
                    if((direction.y < 0 && transform.position.y < yPosOfCamera))
                        {
                        float cameraSpeed = direction.y * Time.deltaTime * moveSpeed;
                        if(transform.position.y - cameraSpeed < yPosOfCamera)
                        {
                            transform.position = new Vector3(transform.position.x,
                        transform.position.y - cameraSpeed, -10);
                        }else if(transform.position.y - cameraSpeed >= yPosOfCamera)
                        {
                            transform.position = new Vector3(transform.position.x,
                          yPosOfCamera, -10);
                        }
                        
                    }
                    else if (direction.y > 0 && transform.position.y > minYPosOfCamera)
                    {
                        float cameraSpeed2 = direction.y * Time.deltaTime * moveSpeed;
                        //Debug.Log("kameranın x pozisyonu = " + transform.position.x);
                        if(transform.position.y - cameraSpeed2 > minYPosOfCamera)
                        {
                            transform.position = new Vector3(transform.position.x,
                         transform.position.y - cameraSpeed2, -10);
                        }
                        else if(transform.position.y - cameraSpeed2 <= minYPosOfCamera)
                        {
                            transform.position = new Vector3(transform.position.x,
                         minYPosOfCamera, -10);
                        }
                        
                    }
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    uiManager.SetActive(true);
                    uiManager.GetComponent<UIManager>().SetAllowForbiddenAS(false);
                    break;
            }
        }
    }
}
