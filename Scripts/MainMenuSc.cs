using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSc : MonoBehaviour
{
    public GameObject languageButton;
    public GameObject yesNoBackGround;
    
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitButton()
    {
        languageButton.SetActive(false);
        MakeDarknessEffect(true,yesNoBackGround);
        yesNoBackGround.SetActive(true);
    }

    private void MakeDarknessEffect(bool state,GameObject panel)
    {
        if(state)
        {
            Image[] images = FindObjectsOfType<Image>();
            SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
            for (int i = 0; i < images.Length; i++)
            {
                if(panel.GetComponent<Image>() != null)
                {
                    if (images[i] != panel.GetComponent<Image>())
                    {
                        images[i].color = new Color32(170, 170, 170, 255);
                    }
                }else
                {
                    images[i].color = new Color32(170, 170, 170, 255);
                }
               
            }
            for (int k = 0; k < spriteRenderers.Length; k++)
            {
                if(panel.GetComponent<SpriteRenderer>() != null)
                {
                    if (images[k] != panel.GetComponent<SpriteRenderer>())
                    {
                        spriteRenderers[k].color = new Color32(170, 170, 170, 255);
                    }
                }else
                {
                    spriteRenderers[k].color = new Color32(170, 170, 170, 255);
                }
                
                
            }
        }
        else
        {
            Image[] images = FindObjectsOfType<Image>();
            SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != yesNoBackGround.GetComponent<Image>())
                {
                    images[i].color = new Color32(255, 255, 255, 255);
                }
            }
            for (int k = 0; k < spriteRenderers.Length; k++)
            {
                spriteRenderers[k].color = new Color32(255, 255, 255, 255);
            }
        }
    }
       public void YesButton()
    {
        Application.Quit();
    }

    public void NoButton()
    {
        languageButton.SetActive(true);
        MakeDarknessEffect(false,null);
        yesNoBackGround.SetActive(false);
    }
    public void CreditsButton()
    {
        FindObjectOfType<SceneLoadManager>().CreditsMenu();
    }
    public void LanguageButton()
    {
        FindObjectOfType<Language>().SetLanguage();
    }
}
