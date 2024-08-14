
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public const string MAIN_MENU_NAME = "MainMenu";
    public const string LEVEL_SELECTION_NAME = "LevelSelection";
    public const string HELP_LEVEL_NAME = "HelpLevel";
    public const string CREDITS_MENU_NAME = "CreditsMenu";
    public const string DONATE_MENU = "DonateMenu";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenuScreen()
    {
        SceneManager.LoadScene(MAIN_MENU_NAME);
    }
    public void LevelSelectionScreen()
    {
        SceneManager.LoadScene(LEVEL_SELECTION_NAME);
    }
    public void LevelsScreen(int getLevelIndex)
    {
        SceneManager.LoadScene(4 + getLevelIndex);
    }
    public void HelpLevelScreen()
    {
        SceneManager.LoadScene(HELP_LEVEL_NAME);
        Time.timeScale = 1;
    }
    public void CreditsMenu()
    {
        SceneManager.LoadScene(CREDITS_MENU_NAME);

    }
    public void DonateMenu()
    {
        SceneManager.LoadScene(DONATE_MENU);
    }
}
