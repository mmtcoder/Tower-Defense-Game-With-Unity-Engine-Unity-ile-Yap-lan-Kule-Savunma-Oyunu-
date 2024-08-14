using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    public const string LEVEL_STORY_NAME = "LevelReached";
    public const string HELP_LEVEL_NOTCOMPLETED_NAME = "HelpLevelIsNotComp";
    public const string HELP_LEVEL_ISCOMPLETED_NAME = "HelpLevelIsComp";
    public const string HELP_LEVEL_STATE_NAME = "HelpLevelState";
    
    // Start is called before the first frame update
    void Start()
    {
        
        int levelReached = PlayerPrefs.GetInt(LEVEL_STORY_NAME, 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if( i + 1> levelReached)
            {
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().color = new Color32(150, 150, 150, 255);
            }
            else
            {
                levelButtons[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel(int levelNumber)
    {
       
       
        if (levelNumber == 1 && HELP_LEVEL_NOTCOMPLETED_NAME.Equals(PlayerPrefs.GetString(HELP_LEVEL_STATE_NAME,HELP_LEVEL_NOTCOMPLETED_NAME)))
        {
            FindObjectOfType<SceneLoadManager>().HelpLevelScreen();
        }else
        {
            FindObjectOfType<SceneLoadManager>().LevelsScreen(levelNumber);
        }
     
        Destroy(FindObjectOfType<StartMusic>().gameObject);
        Time.timeScale = 1f;
    }
    public void HelpLevel()
    {
        FindObjectOfType<SceneLoadManager>().HelpLevelScreen();
        Destroy(FindObjectOfType<StartMusic>().gameObject);
    }
}
