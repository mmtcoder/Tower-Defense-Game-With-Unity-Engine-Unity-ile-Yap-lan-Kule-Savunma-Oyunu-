using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    public Text LanguageText;
    public Text LevelSelectionText;
    public Text DonateMenuText;
    public Text QuestionMenuText;
    public Text YesText;
    public Text NoText;
    public Text CreditsText;
    public Text DonateContext;
    public Text[] helpLevelTexts;
    public Font[] helpLevelChangeFont;
    public TextMeshProUGUI[] settingsMenuTexts;
    public Text[] yesNoBackgroundTexts;
    public Text lastLevelWinText;
    private const string LANGUAGE_PREFS_NAME = "LanguageKey";
    private const string LANGUAGE_TURKISH = "LanguageTurkish";
    private const string LANGUAGE_ENGLISH = "LanguageEnglish";

    // Start is called before the first frame update
    void Start()
    {
        GetLanguage();
       /* Language[] language = FindObjectsOfType<Language>();
        if (language.Length > 1)
        {
            for (int i = 0; i < language.Length; i++)
            {
                if (i == 0)
                {
                    DontDestroyOnLoad(language[i].gameObject);
                }
                else
                {
                    Destroy(language[i].gameObject);
                }
            }
        }
        else if (language.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetLanguage()
    {
        if (LANGUAGE_ENGLISH.Contains(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME, LANGUAGE_ENGLISH)))
        {
            PlayerPrefs.SetString(LANGUAGE_PREFS_NAME, LANGUAGE_TURKISH);
            if (LevelSelectionText != null)
            {
                LevelSelectionText.text = "ASAMA SEÇ";
                DonateMenuText.text = "DESTEKLE :)";
                LanguageText.text = "DİL: TÜRKÇE";
                QuestionMenuText.text = "OYUNDAN ÇIKMAK MI İSTİYORSUN?";
                YesText.text = "EVET";
                NoText.text = "HAYIR";
            }
            if(CreditsText != null)
            {
                CreditsText.text = "Bu oyun MÜMTAZ TAŞDELEN tarafından oluşturulmuştur ve Unity3D oyun motoru ile geliştirilmiştir.Kullanılan Ses Efekt Siteleri: https://www.zapsplat.com/ " +
                    " , https://freesfx.co.uk/ ,https://www.freesoundeffects.com/ https://freesound.org/ , https://soundimage.org/, http://soundbible.com/" +
                    " Bazı Kullanılan Resimlerin Sitesi: https://clipart-library.com/, //pixabay.com/vectors/coins-money-financial-currency-3344603/,  https://www.freepik.com/ " +
                    "Kullanılan Font Sitesi: //www.dafont.com/profile.php?user=764521 ";
            }
           /* if(  helpLevelTexts != null && helpLevelTexts.Length != 0)
            {
                helpLevelTexts[0].text = "Oyuna hoş geldin. Bu yardım levelinde savunucuların türünü, özelliklerini ve tıklanma şeklini öğreneceksin.(oka bas)";
                helpLevelTexts[1].text = "Zehir savunucusu(veya kulesi) sadece aşağı ve yukari yönlere ateş edebilir. Haritaya koyarken dikkatli olmalısın.";
                helpLevelTexts[2].text = "Ateş, electric ve yavaşlatıcı kulelerin dairesel menzilleri vardır.Menzilleri yettiği yere kadar vurabilirler.";
                helpLevelTexts[3].text = "Son olarak teknolojik kulesi var. O sonsuz menzile sahiptir ve kamikaze dron gönderir. Haritanın herhangi bir yerine koyabilirsin. (oka bas)";
                helpLevelTexts[4].text = "Düşmanın üstüne tıklayarak dronun hedefini belirleyebilir(şimdi deneyebilirsin) yada kendisi ufak yapay zeka ile hedefini belirler. ";
                helpLevelTexts[5].text = "Tıkladığın her alan bir kare ile gösterilir. Eğer koyacağın yer 1 kareden az mesafe varsa oraya kule koyamazsın.";
                helpLevelTexts[6].text = "'?' işaretli şey sizlerin belirleyeceği bir savunucu olacak. Eğer Google Play yorumlarında talep olursa bir soraki güncellemede koyacağım.";
                helpLevelTexts[7].text = "Eğer yaratiklar haraket halindeyken kuleleri haritaya koymakta zorlanıyorsan, durdurma butonuna basıpda koyabilirsin.";
                helpLevelTexts[8].text = "Isınman için sana küçük bir oyun hazırladım. Tekrar yardıma ihtiyacın olursa 'Asama Seç' bölümünde bulabilirsin.";
            }
            if(settingsMenuTexts != null && settingsMenuTexts.Length != 0)
            {
                settingsMenuTexts[0].SetText("SES AÇIK");
                settingsMenuTexts[1].SetText("YENİDEN BAŞLAT");
                settingsMenuTexts[2].SetText("ANA MENÜ");
                settingsMenuTexts[3].SetText("DEVAM ET");
                settingsMenuTexts[4].SetText("Oyundan çıkmak istiyor musun?");
                settingsMenuTexts[5].SetText("EVET");
                settingsMenuTexts[6].SetText("HAYIR");
            }
            if(yesNoBackgroundTexts != null && yesNoBackgroundTexts.Length != 0)
            {
                yesNoBackgroundTexts[0].text = "LEVEL TAMAMLANDI";
                yesNoBackgroundTexts[1].text = "TAMAM";
                yesNoBackgroundTexts[2].text = "LEVELİ GEÇEMEDİN";
                yesNoBackgroundTexts[3].text = "TEKRAR DENE";
                yesNoBackgroundTexts[4].text = "ANA MENU";
            }*/

            PlayerPrefs.Save();
        }
        //İngilizce kısmı
        else
        {
            PlayerPrefs.SetString(LANGUAGE_PREFS_NAME, LANGUAGE_ENGLISH);
            if (LevelSelectionText != null)
            {
                LevelSelectionText.text = "LEVEL SELECT";
                DonateMenuText.text = "DONATE";
                LanguageText.text = "LANG: ENGLISH";
                QuestionMenuText.text = "DOU YOU WANT TO EXIT THIS GAME?";
                YesText.text = "YES";
                NoText.text = "NO";
            }
            if (CreditsText != null)
            {
                CreditsText.text = "This game is created by MÜMTAZ TAŞDELEN and made with Unity3D game engine.Used Sound Effects Websites: https://www.zapsplat.com/ " +
                    " , https://freesfx.co.uk/ ,https://www.freesoundeffects.com/ https://freesound.org/ , https://soundimage.org/, http://soundbible.com/" +
                    " Some image sources websites: //clipart-library.com/, //pixabay.com/vectors/coins-money-financial-currency-3344603/,  https://www.freepik.com/ " +
                    "Used Font website: //www.dafont.com/profile.php?user=764521 ";
            }
            /*if (helpLevelTexts.Length != 0 && helpLevelTexts != null)
            {
                helpLevelTexts[0].text = "Welcome to game. Here is a Help level for understanding that what type of the defencer you have and their attributes...(press arrow)";
                helpLevelTexts[1].text = "ToxicDefender fires up and down sides. It doesnt fire right and left side. Please becareful when you put it on the map.";
                helpLevelTexts[2].text = "Fire, electric, and decelerator defenders have circle ranges so they can fire everywhere until enemies out of defenders range.";
                helpLevelTexts[3].text = "Finally you have a technological defender. It has infinity range and its gun is a kamikaze drone... (press arrow)";
                helpLevelTexts[4].text = "You can select you enemy by clicking on it (you can try now) or drone can select a target automatically with AI. ";
                helpLevelTexts[5].text = "Everywhere you can touch is indicated by a square indicator. If you have less space than a square indicator, you cannot put a defencer there.";
                helpLevelTexts[6].text = " ' ? ' this mark will be a defender that players want. But not now, If there are lots of rewiew on Google Play, I will put them in the next update.";
                helpLevelTexts[7].text = "If you have trouble putting defenders on the map while the game is active, you can put defenders on the map while game is in pause mode.";
                helpLevelTexts[8].text = "I prepared a little game for you to warm up. If you need help, you can find the Help level in the level selection menu.";
            }
            if (settingsMenuTexts != null && settingsMenuTexts.Length != 0)
            {
                    settingsMenuTexts[0].SetText("SOUND ON");
                    settingsMenuTexts[1].SetText("RESTART");
                    settingsMenuTexts[2].SetText("MAIN MENU");
                    settingsMenuTexts[3].SetText("RESUME");
                    settingsMenuTexts[4].SetText("Do you want to exit this game ?");
                    settingsMenuTexts[5].SetText("YES");
                    settingsMenuTexts[6].SetText("NO");
                
            }
            if (yesNoBackgroundTexts != null && yesNoBackgroundTexts.Length != 0)
            {
                yesNoBackgroundTexts[0].text = "LEVEL COMPLETED";
                yesNoBackgroundTexts[1].text = "OK";
                yesNoBackgroundTexts[2].text = "LEVEL FAILED";
                yesNoBackgroundTexts[3].text = "RESTART";
                yesNoBackgroundTexts[4].text = "MAIN MENU";
            }*/
            PlayerPrefs.Save();
        }
    }
    public void GetLanguage()
    {
        if (LANGUAGE_ENGLISH.Contains(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME, LANGUAGE_ENGLISH)))
        {
            if (LevelSelectionText != null)
            {
                LevelSelectionText.text = "LEVEL SELECT";
                DonateMenuText.text = "DONATE";
                LanguageText.text = "LANG: ENGLISH";
                QuestionMenuText.text = "DOU YOU WANT TO EXIT THIS GAME?";
                YesText.text = "YES";
                NoText.text = "NO";
            }
            if (CreditsText != null)
            {
                CreditsText.text = "This game is created by MÜMTAZ TAŞDELEN and made with Unity3D game engine." +
                    "\nMail Adress: mumtaztasdelen1996@gmail.com"+
                    "\nUsed Sound Effects Websites: https://www.zapsplat.com/" +
                    " ,https://freesfx.co.uk/ ,https://www.freesoundeffects.com/ https://freesound.org/, https://soundimage.org/, http://soundbible.com/" +
                    "\nSome image sources websites: //clipart-library.com/, //pixabay.com/vectors/coins-money-financial-currency-3344603/,  https://www.freepik.com/ " +
                    "\nUsed Font website: //www.dafont.com/profile.php?user=764521 ";
            }
            if (helpLevelTexts.Length != 0 && helpLevelTexts != null)
            {
                for (int i = 0; i < helpLevelTexts.Length; i++)
                {
                    helpLevelTexts[i].font = helpLevelChangeFont[0];
                    helpLevelTexts[i].fontSize = 15;
                }
                helpLevelTexts[0].text = "Welcome to game. Here is a Help level for understanding that what type of the defencer you have and their attributes...(press arrow)";
                helpLevelTexts[1].text = "ToxicDefender fires up and down sides. It doesnt fire right and left side. Please becareful when you put it on the map.";
                helpLevelTexts[2].text = "Fire, electric, and decelerator defenders have circle ranges so they can fire everywhere until enemies out of defenders range.";
                helpLevelTexts[3].text = "Finally you have a technological defender. It has infinity range and its gun is a kamikaze drone... (press arrow)";
                helpLevelTexts[4].text = "You can select you enemy by clicking on it (you can try now) or drone can select a target automatically with AI. ";
                helpLevelTexts[5].text = "Everywhere you can touch is indicated by a square indicator. If you have less space than a square indicator, you cannot put a defencer there.";
                helpLevelTexts[6].text = " ' ? ' this mark will be a defender that players want. But not now, If there are lots of rewiew on Google Play, I will put them in the next update.";
                helpLevelTexts[7].text = "If you have trouble putting defenders on the map while the game is active, you can put defenders on the map while game is in pause mode.";
                helpLevelTexts[8].text = "I prepared a little game for you to warm up. If you need help, you can find the Help level in the level selection menu.";
            }
            if (settingsMenuTexts != null && settingsMenuTexts.Length != 0)
            {
                settingsMenuTexts[0].SetText("SOUND ON");
                settingsMenuTexts[1].SetText("RESTART");
                settingsMenuTexts[2].SetText("MAIN MENU");
                settingsMenuTexts[3].SetText("RESUME");
                settingsMenuTexts[4].SetText("Do you want to exit this game ?");
                settingsMenuTexts[5].SetText("YES");
                settingsMenuTexts[6].SetText("NO");

            }
            if (yesNoBackgroundTexts != null && yesNoBackgroundTexts.Length != 0)
            {
                yesNoBackgroundTexts[0].text = "LEVEL COMPLETED";
                yesNoBackgroundTexts[1].text = "OK";
                yesNoBackgroundTexts[2].fontSize = 30;
                yesNoBackgroundTexts[2].text = "LEVEL FAILED";
                yesNoBackgroundTexts[3].text = "RESTART";
                yesNoBackgroundTexts[4].text = "MAIN MENU";
            }
            if(lastLevelWinText != null)
            {
                lastLevelWinText.text = "If you like the my game and want me to add another levels," +
                    " you can comment on Google play or write to me by mail(Mail is in the Credits Menu) ." +
                    " You can give me your best support by giving 5 stars.";
            }
            if (DonateContext != null)
            {
                DonateContext.text = "I just posted little advertisement in order to not disturb you more in this game. But "
                    + "if you like my game and want to appreciate me, I made a donate button consisting of just 1 dollar.Taxes are not included in the price." +
                    "(Normally prices is determined as Turkish Lira so this slider just takes different prices in Turkish lira. But If you have USD account you will pay 0,99 cent or" +
                    "if you are using another currency, you will pay approximately 1 dolar or more less."+
                    "These lowest prices determined by Google Play, I am telling you this information that I do not want to be misunderstood as I make a discrimination by you, we are all brothers or sisters :)" +
                    "Thank you very much for downloading my game. Of course, you are not obliged to donate. Good games:)";
            }
        }
        else
        {
            if (LevelSelectionText != null)
            {
                LevelSelectionText.text = "ASAMA SEÇ";
                DonateMenuText.text = "DESTEKLE :)";
                LanguageText.text = "DİL: TÜRKÇE";
                QuestionMenuText.text = "OYUNDAN ÇIKMAK MI İSTİYORSUN?";
                YesText.text = "EVET";
                NoText.text = "HAYIR";
            }
            if (CreditsText != null)
            {
                CreditsText.text = "Bu oyun MÜMTAZ TAŞDELEN tarafından oluşturulmuştur ve Unity3D oyun motoru ile geliştirilmiştir." +
"\nMail adres: mumtaztasdelen1996 @gmail.com"+
"\nKullanılan Ses Efekt Siteleri: https://www.zapsplat.com/, https://freesfx.co.uk/ ,https://www.freesoundeffects.com/ https://freesound.org/ , https://soundimage.org/, http://soundbible.com/ "+
               " \nBazı Kullanılan Resimlerin Sitesi: https://clipart-library.com/, //pixabay.com/vectors/coins-money-financial-currency-3344603/,  https://www.freepik.com/"+
               " \nKullanılan Font Sitesi: //www.dafont.com/profile.php?user=764521 ";
            }
            if (helpLevelTexts != null && helpLevelTexts.Length != 0)
            {
                for (int i = 0; i < helpLevelTexts.Length; i++)
                {
                    helpLevelTexts[i].font = helpLevelChangeFont[1];
                    helpLevelTexts[i].fontSize = 18;
                }
                helpLevelTexts[0].text = "Oyuna hos geldin. Bu yardım levelinde savunucuların türünü, özelliklerini ve tıklanma seklini ögreneceksin.(oka bas)";
                helpLevelTexts[1].text = "Zehir savunucusu(veya kulesi) sadece asagı ve yukari yönlere ates edebilir. Haritaya koyarken dikkatli olmalısın.";
                helpLevelTexts[2].text = "Ates, electrik ve yavaslatıcı kulelerin dairesel menzilleri vardır.Menzilleri yettigi yere kadar vurabilirler.";
                helpLevelTexts[3].text = "Son olarak teknolojik kulesi var. O sonsuz menzile sahiptir ve kamikaze drone gönderir. Haritanın herhangi bir yerine koyabilirsin. (oka bas)";
                helpLevelTexts[4].text = "Düsmanın üstüne tıklayarak dronun hedefini belirleyebilir(simdi deneyebilirsin) yada kendisi ufak yapay zeka ile hedefini belirler. ";
                helpLevelTexts[5].text = "Tıkladıgın her alan bir kare ile gösterilir. Eger koyacagın yer 1 kareden az mesafe varsa oraya kule koyamazsın.";
                helpLevelTexts[6].text = "'?' isareti,(yukarida sol alttaki sari soru isareti) sizlerin belirleyecegi bir savunucu olacak. Eger Google Play yorumlarında talep olursa bir soraki güncellemede koyacagım.";
                helpLevelTexts[7].text = "Eger yaratiklar haraket halindeyken kuleleri haritaya koymakta zorlanıyorsan, durdurma butonuna basıpda koyabilirsin.";
                helpLevelTexts[8].text = "Isınman için sana küçük bir oyun hazırladım. Tekrar yardıma ihtiyacın olursa 'Asama Seç' bölümünde bulabilirsin.";
            }
            if (settingsMenuTexts != null && settingsMenuTexts.Length != 0)
            {
                settingsMenuTexts[0].SetText("SES ACIK");
                settingsMenuTexts[1].SetText("YENIDEN BASLAT");
                settingsMenuTexts[2].SetText("ANA MENU");
                settingsMenuTexts[3].SetText("DEVAM ET");
                settingsMenuTexts[4].SetText("Oyundan çıkmak istiyor musun?");
                settingsMenuTexts[5].SetText("EVET");
                settingsMenuTexts[6].SetText("HAYIR");
            }
            if (yesNoBackgroundTexts != null && yesNoBackgroundTexts.Length != 0)
            {
                yesNoBackgroundTexts[0].text = "SEVIYEYI GECTIN";
                yesNoBackgroundTexts[1].text = "TAMAM";
                yesNoBackgroundTexts[2].fontSize = 28;
                yesNoBackgroundTexts[2].text = "LEVEL BASARISIZ";
                yesNoBackgroundTexts[3].text = "TEKRAR DENE";
                yesNoBackgroundTexts[4].text = "ANA MENU";
            }
            if (lastLevelWinText != null)
            {
                lastLevelWinText.text = "Eger oyunumu begendiysen ve baska bölümlerin eklenmesini istiyorsan," +
                    "Google Playden yorum yapabilir veya bana mail atabilirsin(Mail Credits kısmında)" +
                    " Oyunuma 5 yıldız vererek en iyi destegini verebilirsin :).";
            }
            if(DonateContext != null)
            {
                DonateContext.text = " Oyun için sadece ayar menüsünde 1 ufak reklam ve her kazandığınızda bir videolu reklam çıkıyor. Olabildiğince az reklam koydum." +
                    " Ama oyunumu beğenip takdir etmek isterseniz  1, 3 ve 5 TL olarak bana yardımda bulunabilirsiniz.(Vergiler fiyata dahil değildir.Misal 1 liralık destekde bulunmak isterseniz toplam 1.19 TL vermiş olursunuz)" +
                    " Oyunumu indirmeye layik gördüğünüz içinde çok teşekkür ederim. Elbette destek yapmakta mecbur değilsiniz yanlış anlaşılmasın :) İyi oyunlar dilerim.";
            }
        }
    }
}
