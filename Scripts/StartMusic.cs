using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        StartMusic[] musics = FindObjectsOfType<StartMusic>();
        if (musics.Length > 1)
        {
            for (int i = 0; i < musics.Length; i++)
            {
                if (i == 0)
                {
                    DontDestroyOnLoad(musics[i].gameObject);
                }
                else
                {
                    Destroy(musics[i].gameObject);
                }
            }
        }
        else if (musics.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
