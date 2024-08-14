
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIEnvironment : MonoBehaviour
{
    public Text waveText;
    public Text healthText;
    public Text coinText;
    public GameObject stopButton;
    public GameObject x1Button;
    public GameObject x2Button;
    public GameObject settingsBackGround;
    public GameObject soundButtonText;
    public GameObject yesNoBackGround;
    public GameObject questionText;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject darknessEffect;

    private Vector2 touchedArea;
    private bool isGameStopMode = false;
    private bool isX1Mode = true;
    private bool muted;
    private bool isSoundOff = false;
    private bool checkClickOutOfSettingsBG = false;
    private bool isActiveRestartMenu = false;
    private bool couldDownIsActive = false;
    private bool isActiveMainMenu = false;
    private float loseScreenTimer = 0;
    private const string LEVEL_STORY_NAME = "LevelReached";
    private const string LANGUAGE_PREFS_NAME = "LanguageKey";
    private const string LANGUAGE_TURKISH = "LanguageTurkish";
    private const string LANGUAGE_ENGLISH = "LanguageEnglish";

    // string gameId = "3725693";
    //public string placementId = "SettingMenu";
    //public string videPlacementId = "LevelComplete";
    //bool testMode = false;
    bool isTimes;
    // Start is called before the first frame update
    void Start()
    {
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;


        isTimes = true;
        Yodo1U3dAds.SetLogEnable(true);

        Yodo1U3dAds.SetUserConsent(true);
        Yodo1U3dAds.SetTagForUnderAgeOfConsent(false);
        Yodo1U3dAds.SetDoNotSell(false);
        //Yodo Initialize SDK
        Yodo1U3dAds.InitializeSdk();

        Yodo1U3dSDK.setBannerdDelegate((Yodo1U3dConstants.AdEvent adEvent, string error) =>
        {
            Debug.Log("[Yodo1 Ads] BannerdDelegate:" + adEvent + "\n" + error);
            switch (adEvent)
            {
                case Yodo1U3dConstants.AdEvent.AdEventClick:
                    Debug.Log("[Yodo1 Ads] Banner advertising has been clicked.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventClose:
                    Debug.Log("[Yodo1 Ads] Banner advertising has been closed.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowSuccess:
                    Debug.Log("[Yodo1 Ads] Banner advertising has been shown.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowFail:
                    Debug.Log("[Yodo1 Ads] Banner advertising show failed, the error message:" + error);
                    break;
            }
        });

        Yodo1U3dSDK.setInterstitialAdDelegate((Yodo1U3dConstants.AdEvent adEvent, string error) =>
        {
            Debug.Log("[Yodo1 Ads] InterstitialAdDelegate:" + adEvent + "\n" + error);
            switch (adEvent)
            {
                case Yodo1U3dConstants.AdEvent.AdEventClick:
                    Debug.Log("[Yodo1 Ads] Interstital advertising has been clicked.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventClose:
                    Debug.Log("[Yodo1 Ads] Interstital advertising has been closed.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowSuccess:
                    Debug.Log("[Yodo1 Ads] Interstital advertising has been shown.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowFail:
                    Debug.Log("[Yodo1 Ads] Interstital advertising show failed, the error message:" + error);
                    break;
            }

        });

        //These codes belengs to Unity Adverb
        //Advertisement.Initialize(gameId, testMode);
        //Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_RIGHT);

    }

    // Update is called once per frame
    void Update()
    {
        GestureDetector();
        ControllofAllAudios(isSoundOff);
        StartLoseScreenCountDown();
        DisableBannerAdv(settingsBackGround);
    }
    public void SetWaveText(int maxWave,int wave)
    {
        string s = string.Format("Wave {0}/{1}", maxWave,wave);
        waveText.text = s;
    }
    private void StartLoseScreenCountDown()
    {
        if(couldDownIsActive)
        {
            if (loseScreenTimer >= 1.5f)
            {
                loseScreenTimer = 0;
                loseScreen.SetActive(true);
                Time.timeScale = 0;
                SetAudioMute(true);
            }
            else
            {
                loseScreenTimer += Time.deltaTime;
            }
        }
    }
    public void SetHealthText(int health)
    {
        int currentHealth = GetHealthText();
        currentHealth -= health;
        if(currentHealth <= 0)
        {
            // Debug.Log("lose screen will come here");
            couldDownIsActive = true;
        }
        else
        {
            healthText.text = currentHealth.ToString();
        }
        
    }
    public int GetHealthText()
    {
        return int.Parse(healthText.text);
    }
    public void SetCoinText(int coin)
    {
       // Debug.Log("harcana yada eklenen para = " + coin);
        int getBalance = GetCoinText();
        getBalance += coin;
       // Debug.Log("getBalance durumu = " + getBalance);
        coinText.text = getBalance.ToString();
        
    }
    public int GetCoinText()
    {
        return int.Parse(coinText.text);
    }
    public void StopGame()
    {
        if(!isGameStopMode)
        {
            FindObjectOfType<UIManager>().DisableAllowShowCanvas();
            isGameStopMode = true;
            
            SetAudioMute(true);
            stopButton.GetComponent<Image>().color = new Color32(255, 29, 0, 255);
            Time.timeScale = 0;
            FindObjectOfType<UIManager>().SetArrowSelection(false);
        }
        else
        {
            
            FindObjectOfType<UIManager>().DisableAllowShowCanvas();
            isGameStopMode = false;
            if(!isSoundOff)
            {
                SetAudioMute(false);
            }
            stopButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            if(isX1Mode)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 2;
            }
            FindObjectOfType<UIManager>().SetArrowSelection(true);
        }
    }
    /*
     * Bu fonksiyon sadece bir anlık sesin susturulmasını yani kullanici ya durdur butonuna bastigi 
     * yada setting butonuna bastigi zaman calisir.Cunku burada hicbir obje hareket etmiyor.
     */
    public void SetAudioMute(bool mute)
    {
      
            AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            for (int index = 0; index < sources.Length; ++index)
            {
                sources[index].mute = mute;
            }
            muted = mute;
        
       
    }

    public void SelectX1andX2Mode()
    {
        if(!isGameStopMode)
        {
            FindObjectOfType<UIManager>().DisableAllowShowCanvas();
            if (isX1Mode)
            {
                isX1Mode = false;
                x1Button.SetActive(false);
                x2Button.SetActive(true);
                Time.timeScale = 2;
            }
            else
            {
                isX1Mode = true;
                x1Button.SetActive(true);
                x2Button.SetActive(false);
                Time.timeScale = 1;
            }
        }
  
    }
    private void GestureDetector()
    {
        if (Input.touchCount > 0)
        {
            //bu canvas kamera disinda ayri oldugu icin tiklanilan bolgeyi dunya pozisyonuna cevirmeye gerek yok;
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                //HideIfClickedOutside ilk once yazilmasi butona tiklandigi zaman setting menunun disina 
                //tiklanmayi sayacak ve menu gorunecek gorundukten sonrada tiklanmanin control etme
                //durumu(checkClickOutOfSettingsBG) true yapilir, setting menu acildiktan sonraki tiklamalar kontrol edilir.
                if(settingsBackGround.activeInHierarchy)
                {
                    HideIfClickedOutside(settingsBackGround, checkClickOutOfSettingsBG, touch.position);
                }
                else if(yesNoBackGround.activeInHierarchy)
                {
                    /*Kullanıcı burada yesNoBackground tikladigi zaman disariye tiklanmada menunun kapanmamasi icin
                     * bu fonksiyonu bos birakiyorum.
                     */
                
                }

                if (settingsBackGround.activeInHierarchy || yesNoBackGround.activeInHierarchy)
                {
                    checkClickOutOfSettingsBG = true;
                }else
                {
                    checkClickOutOfSettingsBG = false;
                }
                
            }
        }
    }
    private void HideIfClickedOutside(GameObject panel, bool isCanCheckClick, Vector2 touchedPoint)
    {
        if(isCanCheckClick)
        {
           
            
            if (panel.activeInHierarchy &&
           !RectTransformUtility.RectangleContainsScreenPoint(
               panel.GetComponent<RectTransform>(),
               touchedPoint, Camera.main))
            {
                if (isX1Mode)
                {
                   
                    Time.timeScale = 1f;
                    if(!isSoundOff)
                    {

                        SetAudioMute(false);
                    }
                   
                }
                else
                {
                    Time.timeScale = 2f;
                    if(!isSoundOff)
                    {
                        
                        SetAudioMute(false);
                    }
                   
                }
                if(isGameStopMode)
                {
                    Time.timeScale = 0f;
                    SetAudioMute(true);
                }
               
                panel.SetActive(false);
                darknessEffect.SetActive(false);
                //Advertisement.Banner.Hide();
                Yodo1U3dAds.HideBanner();
            }
        }
       
    }
    public void ShowSettingsMenu()
    {
        if(!settingsBackGround.activeInHierarchy)
        {
            settingsBackGround.SetActive(true);
            darknessEffect.SetActive(true);
            Time.timeScale = 0f;
            
            if(!isSoundOff)
            {
                if (!muted)
                {
                    SetAudioMute(true);
                }
            }
            if (!Yodo1U3dAds.BannerIsReady())
            {
                Debug.Log("[Yodo1 Ads] Banner ad has not been cached.");
                return;
            }
            if (isTimes)
            {
                isTimes = false;
                //Set the Yodo1 banner position to botton right
                Yodo1U3dAds.SetBannerAlign(Yodo1U3dConstants.BannerAdAlign.BannerAdAlignRight | Yodo1U3dConstants.BannerAdAlign.BannerAdAlignBotton);
            }
            //Show banner ad
            Yodo1U3dAds.ShowBanner();
          //  StartCoroutine(ShowBannerWhenInitialized());

        }
    }
    public bool getIsShowSettingMenu()
    {
        return settingsBackGround.activeInHierarchy;
    }
    public bool getIsShowingYNMenu()
    {
        return yesNoBackGround.activeInHierarchy;
    }

    public void SetSoundState()
    {
        if(!isSoundOff)
        {
            isSoundOff = true;
         
            if(LANGUAGE_ENGLISH.Contains(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
            {
                soundButtonText.GetComponent<TextMeshProUGUI>().SetText("SOUND OFF");
            }
            else
            {
                soundButtonText.GetComponent<TextMeshProUGUI>().SetText("SES KAPALI");
            }
            
            
        }
        else
        {
            isSoundOff = false;

            if (LANGUAGE_ENGLISH.Contains(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
            {
                soundButtonText.GetComponent<TextMeshProUGUI>().SetText("SOUND ON");
            }
            else
            {
                soundButtonText.GetComponent<TextMeshProUGUI>().SetText("SES ACIK");
            }

        }
    }
    /*
     * Bu fonksiyon sesin devamlı olarak susturur yani kullanici sesi kapatma butonuna bastigi zaman calisir.
     * @param state sesin durumu
     */
    private void ControllofAllAudios(bool state)
    {
        if(isSoundOff)
        {
            AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            for (int index = 0; index < sources.Length; ++index)
            {
                sources[index].mute = state;
            }
        }
    }
    public void ResumeButton()
    {
        if (isX1Mode)
        {

            Time.timeScale = 1f;
            if (!isSoundOff)
            {

                SetAudioMute(false);
            }

        }
        else
        {
            Time.timeScale = 2f;
            if (!isSoundOff)
            {

                SetAudioMute(false);
            }

        }
        if (isGameStopMode)
        {
            Time.timeScale = 0f;
            SetAudioMute(true);
        }
        settingsBackGround.SetActive(false);
        darknessEffect.SetActive(false);
        //Advertisement.Banner.Hide();
        Yodo1U3dAds.HideBanner();
    }
    public void RestratButton()
    {
        if(!yesNoBackGround.activeInHierarchy)
        {
            isActiveRestartMenu = true;
            settingsBackGround.SetActive(false);
            yesNoBackGround.SetActive(true);
            if(LANGUAGE_ENGLISH.Contains(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
            {
                questionText.GetComponent<TextMeshProUGUI>().text = "Do you want to restart this game?";
            }else
            {
                questionText.GetComponent<TextMeshProUGUI>().text = "Bu leveli yeniden oynamak ister misin?";
            }
           
        }
    }
    public void MainMenuButton()
    {
        if(!yesNoBackGround.activeInHierarchy)
        {
            isActiveMainMenu = true;
            settingsBackGround.SetActive(false);
            yesNoBackGround.SetActive(true);
            if (LANGUAGE_ENGLISH.Contains(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
            {
                questionText.GetComponent<TextMeshProUGUI>().text = "Do you want to exit this game?";
            }
            else
            {
                questionText.GetComponent<TextMeshProUGUI>().text = "Ana menuye gitmek istiyor musun?";
            }
        
        }
    }
    public void YesButton()
    {
        if(isActiveRestartMenu)
        {
            Time.timeScale = 1;
            SetAudioMute(false);
            FindObjectOfType<SceneLoadManager>().RestartScene();
            //Advertisement.Banner.Hide();
            Yodo1U3dAds.HideBanner();

        }
        else if(isActiveMainMenu)
        {
            //Ana menu sahnesi eklendiginde bu fonksiyon kullanilacak
            Time.timeScale = 1;
            FindObjectOfType<SceneLoadManager>().MainMenuScreen();
            // Advertisement.Banner.Hide();
            Yodo1U3dAds.HideBanner();

        }
    }
    public void NoButton()
    {
        if(isActiveRestartMenu)
        {
            yesNoBackGround.SetActive(false);
            settingsBackGround.SetActive(true);
        }
        else if(isActiveMainMenu)
        {
            yesNoBackGround.SetActive(false);
            settingsBackGround.SetActive(true);
        }
    }

    public void LScreenRestartButton()
    {
        FindObjectOfType<SceneLoadManager>().RestartScene();
    }
    public void CallWinScreen()
    {
        if(!winScreen.activeInHierarchy)
        {
            winScreen.SetActive(true);
        }
    }
    public void WinOkButton()
    {
        if(winScreen.activeInHierarchy)
        {
            /*Eger kullanici diger levelleri acmis iken dusuk leveden oynamak isterse diger yuksek levellerinde
             * sifirlanmamasi(yani ileriki acik olan level butonlarin kapanmamasi) icin asagidaki if kontrolu
             * gereklidir.Donate menusu ve baska birsey eklenirse asagidaki sayilar ona gore guncellenecek!!.
             */
            if(PlayerPrefs.GetInt(LEVEL_STORY_NAME) <= SceneManager.GetActiveScene().buildIndex -4 && SceneManager.GetActiveScene().buildIndex -4 < 15)
            {
                PlayerPrefs.SetInt(LEVEL_STORY_NAME, SceneManager.GetActiveScene().buildIndex - 3);
            }
            FindObjectOfType<UIManager>().DisableAllowShowCanvas();

            //Show Interstital with Yodo
            if (Yodo1U3dAds.InterstitialIsReady())
            {
                Yodo1U3dAds.ShowInterstitial();
            }
            else
            {
                Debug.Log("[Yodo1 Ads] Interstitial ad has not been cached.");
            }

            //ShowInterstitialAd();
            FindObjectOfType<SceneLoadManager>().LevelSelectionScreen();
        }
    }
   /* public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(videPlacementId))
        {
            Advertisement.Show(videPlacementId);
           
        }
        else
        {
            //Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }*/
    /*IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(placementId);
        
    }*/
    public bool GetGameIsStopMode()
    {
        return isGameStopMode;
    }
    private void DisableBannerAdv(GameObject panel)
    {
        if(!panel.activeInHierarchy)
        {
            //Advertisement.Banner.Hide();

            //Yodo1 Hide Banner
            Yodo1U3dAds.HideBanner();
        }
    }
}
