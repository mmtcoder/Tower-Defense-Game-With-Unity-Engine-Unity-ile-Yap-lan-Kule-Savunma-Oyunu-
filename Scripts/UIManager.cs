
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI staffs")]
    [SerializeField] GameObject mainBackGround;
    [SerializeField] GameObject selectedPlace;
    [SerializeField] GameObject circleRangeOfDefender;
    [SerializeField] GameObject squareRangeOfDefender;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject forbiddenAreaSign;
    [SerializeField] Collider2D collider1;
    [SerializeField] Collider2D collider2;

    [Header("Camera Position")]
    public float cameraWidhtForWorldSpace;
    public float cameraHeightForWorldSpace;
   
    [Header("Defencer Types")]
    [SerializeField] GameObject fireDefender;
    [SerializeField] GameObject toxicDefender;
    [SerializeField] GameObject electricDefender;
    [SerializeField] GameObject deceleratorDefender;
    [SerializeField] GameObject technologicalDefender;
    
    Vector3 selectedDefender;
    Vector2 touchedArea;
    Vector2 clickedCanvasPos;
    GameObject uiEnvironment;
    GameObject mainCamera;

    Enemy selectedEnemy;
    Defender[] tempDefender;
    Transform[] paths;
    private bool helpLevelPanelIsActive = false;
    private bool allowShowCanvas = true;
    private bool allowArrowMark = false;
    private bool allowForbiddenAS = false;
    private float timer = 0f;
    private float timerForbiddenAS = 0f;
    // Start is called before the first frame update
    void Start()
    {
        uiEnvironment = FindObjectOfType<UIEnvironment>().gameObject;
        GameObject findPaths = GameObject.FindGameObjectWithTag("Paths");
        paths = new Transform[findPaths.transform.childCount ];
        for (int i = 0; i < findPaths.transform.childCount; i++)
        {
            paths[i] = findPaths.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShowArrowSelection();
        ShowForbiddenArea(touchedArea);
        GestureDetector();
        
    }

    private void StateOfUIManagerCanvas(Transform setUIPosition)
    {
        if (setUIPosition != null)
        {
            if (!mainBackGround.activeInHierarchy )
            {
               
               if(!circleRangeOfDefender.activeInHierarchy && !squareRangeOfDefender.activeInHierarchy)
                {
                    
                    mainBackGround.SetActive(true);
                    selectedPlace.SetActive(true);
                    clickedCanvasPos = determineSelectedPozisition(setUIPosition.position);
                    CheckManuBackGroundPos(clickedCanvasPos);
                    transform.position = clickedCanvasPos;
                    //Debug.Log("uicanvas posizyonu = " + uiCanvas.transform.position);
                }
            }
            else 
            {
                mainBackGround.SetActive(false);
                selectedPlace.SetActive(false);
            }
        }
        else
        {
            mainBackGround.SetActive(false);
            selectedPlace.SetActive(false);
           
        }

    }
    //Yol eklenilecegi zaman bu kodları aktif hale getir.
   /* void OnDrawGizmos()
    {
        Gizmos.color = new UnityEngine.Color(1.0f, 1.0f, 0.0f);
        GameObject findPaths2 = GameObject.FindGameObjectWithTag("Paths");
        Transform [] paths2 = new Transform[findPaths2.transform.childCount];
       // Debug.Log(Camera.main.transform.position);
        for (int i = 0; i < findPaths2.transform.childCount; i++)
        {
            paths2[i] = findPaths2.transform.GetChild(i);
        }
        Rect[] rect = DrawRoadWayRectangles(paths2);
        for (int k = 0; k < rect.Length; k++)
        {
            Gizmos.DrawWireCube(new Vector3(rect[k].center.x, rect[k].center.y, 0.01f),
                new Vector3(rect[k].width, rect[k].height, 0.01f));
        }

        // Debug.Log("rect x konumu = " + rect.X + "y si = " + rect.Y);
       // Vector2 positionOfDefender = new Vector2(6f, 8f);
        //Rect defenderRect = new Rect(positionOfDefender.x - 1f, positionOfDefender.y - 1f, 2f, 2f);
       // defenderRect.Contains(defenderRect.position);
        //Gizmos.DrawWireCube(new Vector3(defenderRect.center.x, defenderRect.center.y, 0.01f),
             //   new Vector3(defenderRect.width, defenderRect.height, 0.01f));
    }*/
    private void GestureDetector()
    {
       
            if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            touchedArea = Camera.main.ScreenToWorldPoint(touch.position);
            
         

            if (touch.phase == TouchPhase.Ended)
            {
                if (!uiEnvironment.GetComponent<UIEnvironment>().getIsShowSettingMenu() &&
                  !uiEnvironment.GetComponent<UIEnvironment>().getIsShowingYNMenu() && !getHelpUIisActive())
                {

                    //Debug.Log("camera icindeki dokunulan nokta = " + touchedArea);
                    Collider2D forbiddenArea = Physics2D.OverlapPoint(touchedArea);
                if(collider1 == forbiddenArea || collider2 == forbiddenArea)
                {
                    allowForbiddenAS = true;
                    timerForbiddenAS = 0f;
                    forbiddenAreaSign.transform.position = new Vector3(touchedArea.x, touchedArea.y, 0);
                    mainBackGround.SetActive(false);
                    selectedPlace.SetActive(false);
                    circleRangeOfDefender.SetActive(false);
                    squareRangeOfDefender.SetActive(false);
                }
                else
                {
                    
                        if (allowShowCanvas)
                        {
                            allowForbiddenAS = false;
                            
                            StateOfUIManagerCanvas(CheckIsOverloapTheRoad());
                        }
                        else
                        {
                            allowShowCanvas = true;
                        }
                    }

                }
                else
                {
                    
                    mainBackGround.SetActive(false);
                    selectedPlace.SetActive(false);
                    circleRangeOfDefender.SetActive(false);
                    squareRangeOfDefender.SetActive(false);
                }
                
            }
        }

    }
    private Transform GetSelectedPlaceTransform(Vector2 touchedArea)
    {
        Defender[] restoreDefenderTransformInfo = FindObjectsOfType<Defender>();
        tempDefender = new Defender[restoreDefenderTransformInfo.Length];
        Transform tempTransform = transform;
         // Debug.Log("restoreDefenderTransformInfo length = " + restoreDefenderTransformInfo.Length);
        if (restoreDefenderTransformInfo.Length > 0)
        {
            if (touchedArea != null)
            {
                for (int k = 0; k < tempDefender.Length; k++)
                {
                    if (Mathf.Abs(touchedArea.x - restoreDefenderTransformInfo[k].transform.position.x) <= 1.2f &&
                        Mathf.Abs(touchedArea.y - restoreDefenderTransformInfo[k].transform.position.y) <= 1.2f)
                    {
                        tempDefender[k] = restoreDefenderTransformInfo[k];
                      //  Debug.Log("dokunulan yerdeki defencerin poz = " + tempDefender[k].transform.position + "index number = " + k);
                    }
                }
                for (int i = 0; i < tempDefender.Length; i++)
                {

                    if (tempDefender[i] != null)
                    {
                        
                  
                       // Debug.Log("tıklanan defender konumu = " + tempDefender[i].transform.position);
                        if(tempDefender[i].GetComponent<CircleCollider2D>() != null)
                        {

                            if (!circleRangeOfDefender.activeInHierarchy)
                            {
                               
                                circleRangeOfDefender.SetActive(true);
                                squareRangeOfDefender.SetActive(false);
                            }
                            selectedDefender = tempDefender[i].transform.position;
                            transform.position = tempDefender[i].transform.position;
                            
                                float radius = tempDefender[i].GetComponent<CircleCollider2D>().radius;
                                circleRangeOfDefender.transform.localScale = new Vector3(radius * 2f, radius * 2, 0f);

                            //Secilen defenderın yarısı kadar ücretle defendiri satar
                            circleRangeOfDefender.GetComponentInChildren<Text>().text = (tempDefender[i].GetComponent<Defender>().GetDefenderCost() / 2).ToString();
                        }else
                        {
                            if(!squareRangeOfDefender.activeInHierarchy)
                            {
                                
                                squareRangeOfDefender.SetActive(true);
                                circleRangeOfDefender.SetActive(false);
                            }
                            selectedDefender = tempDefender[i].transform.position;
                            transform.position = tempDefender[i].transform.position;
                            Vector3 squareSizes = new Vector3(tempDefender[i].GetComponent<BoxCollider2D>().size.x,
                                tempDefender[i].GetComponent<BoxCollider2D>().size.y, 0f);
                            squareRangeOfDefender.transform.localScale = squareSizes;
                   
                            //Secilen defenderın yarısı kadar ücretle defendiri satar
                            squareRangeOfDefender.GetComponentInChildren<Text>().text = (tempDefender[i].GetComponent<Defender>().GetDefenderCost() / 2).ToString();
                        }
                       

                        return null;
                    }
                }
              
               if (circleRangeOfDefender.activeInHierarchy)
                {
                   
                    circleRangeOfDefender.SetActive(false);
                    return null;
                }
                if(squareRangeOfDefender.activeInHierarchy)
                {
                    
                    squareRangeOfDefender.SetActive(false);
                    return null;
                }
              //  Debug.Log("defenderın dokunulan yerde olmaması gerek");
                tempTransform.position = touchedArea;
            }
            else {
              //  Debug.Log("haritaya dokunulmamıs");
                return null; }
            
        }
        else
        {
           // Debug.Log("defender hic yok");
            tempTransform.position = touchedArea;
        }
        return tempTransform;
    }
    private Transform CheckIsOverloapTheRoad()
    {
        Transform tempTransform = GetSelectedPlaceTransform(touchedArea);
        if (tempTransform != null)
        {

            // Debug.Log("paths length = " + paths.Length);

            Rect[] getRect = DrawRoadWayRectangles(paths);

            for (int i = 0; i < getRect.Length; i++)
            {
                if (touchedArea != null)
                {
                    // Debug.Log("dikdortgenlere degme durumu = " + getRect[i].Contains(touchedArea));
                    //Eger tiklanilan alan yola degiyorsa null dondur
                    if (getRect[i].Contains(touchedArea))
                    {
                        circleRangeOfDefender.SetActive(false);
                        squareRangeOfDefender.SetActive(false);
                        return null;
                    }

                }
            }
           
        }
        return tempTransform;
    }
    private Rect[] DrawRoadWayRectangles(Transform[] paths)
    {
        
        Rect[] rect = new Rect[paths.Length];
        for (int i = 0; i < paths.Length; i++)
        {
            if (i != paths.Length - 1)
            {
                int distanceBetweenXAxis = (int)Mathf.Abs((paths[i].transform.position.x - paths[i + 1].transform.position.x));
                int distanceBetweenYAxis = (int)Mathf.Abs((paths[i].transform.position.y - paths[i + 1].transform.position.y));
                int signedDistanceBetweenYAxis = (int)(paths[i].transform.position.y - paths[i + 1].transform.position.y);
                int signedDistanceBetweenXAxis = (int)(paths[i].transform.position.x - paths[i + 1].transform.position.x);
                if (distanceBetweenYAxis <= Mathf.Epsilon)
                {
                    //  Debug.Log("y koordinatları arasındaki fark 0");
                    if (signedDistanceBetweenXAxis < 0)
                    {
                        //  Debug.Log("x koordinatı 0 dan kucuk");
                        if (signedDistanceBetweenXAxis < 0)
                            rect[i] = new Rect(paths[i].transform.position.x - 1f, paths[i].transform.position.y - 1f
            , (Mathf.Abs(distanceBetweenXAxis) + 2f), 2f);
                    }
                    else
                    {
                        //Debug.Log("x koordinatı 0 dan buyuk");
                        rect[i] = new Rect(paths[i + 1].transform.position.x - 1f, paths[i + 1].transform.position.y - 1f
            , (Mathf.Abs(distanceBetweenXAxis) + 2f), 2f);
                    }
                }
                else
                {
                    //X koordinatlari arasindaki fark 0 yada 0 a çok yakın ise
                    // Debug.Log("y koordinatları arasındaki fark 0 dan büyük");
                    if (signedDistanceBetweenYAxis < 0)
                    {
                        rect[i] = new Rect(paths[i].transform.position.x - 1f, paths[i].transform.position.y - 1f
            , 2f, (Mathf.Abs(distanceBetweenYAxis) + 2f));

                    }
                    else
                    {
                        rect[i] = new Rect(paths[i + 1].transform.position.x - 1f, paths[i + 1].transform.position.y - 1f
           , 2f, (Mathf.Abs(distanceBetweenYAxis) + 2f));
                    }
                }
            }
        }
        return rect;
    }

    public void InstantiateFireDefender()
    {
        if (mainBackGround.activeInHierarchy)
        {

            if(PurchaseDefender(fireDefender.GetComponent<Defender>().GetDefenderCost()))
            {
                GameObject fire = Instantiate(fireDefender, clickedCanvasPos, Quaternion.identity) as GameObject;
                uiEnvironment.GetComponent<UIEnvironment>().SetCoinText(-fireDefender.GetComponent<Defender>().GetDefenderCost());
                //allowShowCanvas = false;
            }
                
                
                //Debug.Log("cevrilen x pozisyonu = " + clickedCanvasPos.x + "y posiz  = " + clickedCanvasPos.y);
            }
            
        
    }
    public void InstantiateElectricDefender()
    {
     
        if (mainBackGround.activeInHierarchy)
        {
            if (PurchaseDefender(electricDefender.GetComponent<Defender>().GetDefenderCost()))
            {
                GameObject fire = Instantiate(electricDefender, clickedCanvasPos, Quaternion.identity);
                uiEnvironment.GetComponent<UIEnvironment>().SetCoinText(-electricDefender.GetComponent<Defender>().GetDefenderCost());
                //allowShowCanvas = false;
                //Debug.Log("cevrilen x pozisyonu = " + clickedCanvasPos.x + "y posiz  = " + clickedCanvasPos.y);
            }
        }
    }
    public void InstantiateToxicDefender()
    {
        if (mainBackGround.activeInHierarchy)
        {
            if (PurchaseDefender(toxicDefender.GetComponent<Defender>().GetDefenderCost()))
            {
                GameObject fire = Instantiate(toxicDefender, clickedCanvasPos, Quaternion.identity);
                uiEnvironment.GetComponent<UIEnvironment>().SetCoinText(-toxicDefender.GetComponent<Defender>().GetDefenderCost());
               // allowShowCanvas = false;
            }
          //  Debug.Log("cevrilen x pozisyonu = " + clickedCanvasPos.x + "y posiz  = " + clickedCanvasPos.y);
        }
        
    }
    public void InstantiateDeleceratorDefender()
    {
        if (mainBackGround.activeInHierarchy)
        {
            if (PurchaseDefender(deceleratorDefender.GetComponent<Defender>().GetDefenderCost()))
            {
                GameObject fire = Instantiate(deceleratorDefender, clickedCanvasPos, Quaternion.identity);
                uiEnvironment.GetComponent<UIEnvironment>().SetCoinText(-deceleratorDefender.GetComponent<Defender>().GetDefenderCost());
                // allowShowCanvas = false;
            }
            //  Debug.Log("cevrilen x pozisyonu = " + clickedCanvasPos.x + "y posiz  = " + clickedCanvasPos.y);
        }
    }public void InstantiateTechnoDefender()
    {
        if (mainBackGround.activeInHierarchy)
        {
            if (PurchaseDefender(technologicalDefender.GetComponent<Defender>().GetDefenderCost()))
            {
                GameObject techno = Instantiate(technologicalDefender, clickedCanvasPos, Quaternion.identity);
                uiEnvironment.GetComponent<UIEnvironment>().SetCoinText(-technologicalDefender.GetComponent<Defender>().GetDefenderCost());
                // allowShowCanvas = false;
            }
            //  Debug.Log("cevrilen x pozisyonu = " + clickedCanvasPos.x + "y posiz  = " + clickedCanvasPos.y);
        }
    }
    /*Burada ilk yapilan tıklanılan bolge direk olarak int e cevrilmektedir. Direk olarak int e cevirdigimiz bazı
       sikintilara yol acıyor. Ilk olarak defenderlar arasindaki mesafe kontrol ediliyor. Secili olan bolgenin yarisi 
       defendirin ustunde gorunmemesi icin 350 satirdan 435 e kadar kod blogu yazilmistir. 435 den methodun 
       bitimine kadarda yol temas kontrolu yapılıyor. Tiklanilan bolgede yol 
       dikdörtgeni ile defender dikdörtgeni ayri ayri cizilir. 

       @param roadCheckRect cizilirken x ile y -1 birim kaydirilmistir.
       Cunki cizilme mantigi en alt sola goredir ve böyle yaparak iki dikdortgenin birbirine temas etmesi saglanir.
       @param defenderCheckRect ise dokunulan yerde defendir temasini kontrol etmek icin cizilen dikdortgendir.
       @return Vector2 tiklanilan yer bos ise selectedPlace GameObjectine aktarılır degil ise tiklanilan yerde
       selectedPlace ile mainBackGround fonksiyonları set.Active(false) yapılır.
       
       */
    private Vector2 determineSelectedPozisition(Vector2 clickedCanvasPos)
    {
      
        if (mainBackGround.activeInHierarchy)
        {
            if(clickedCanvasPos != null)
            {
                
                Vector2 convertedVectorPosition = new Vector2((int)clickedCanvasPos.x, (int)clickedCanvasPos.y);
                Rect roadCheckRect = new Rect(convertedVectorPosition.x - 1f, convertedVectorPosition.y - 1f, 2f, 2f);
                Rect defenderCheckRect = new Rect(convertedVectorPosition.x, convertedVectorPosition.y, 2f, 2f);
                Rect[] getRect = DrawRoadWayRectangles(paths);
               // Debug.Log("cizilen 1 e 2  birimlik kare poz = " +  defenderRect.position.x + defenderRect.position.y);
                Defender[] restoreDefenderTransformInfo = FindObjectsOfType<Defender>();
               // Debug.Log(" 1 pozision of defender x = " + positionOfDefender.x);
                bool xYonunuBirKereKaydir = true;
                bool yYonunuBirKereKaydir = true;

                for (int k = 0; k < restoreDefenderTransformInfo.Length; k++)
                {
                    if (restoreDefenderTransformInfo[k] != null)
                    {
                        //Tıklanilan bolge direk int e cevrildigi icin kusuratlı degerlerin olusma ihtimali sıfır. Bu yuzden 
                        // x ekseni kontrolu yapılırkan direk +1 e yani tiklanilan yer defenderin tam 1 birim saginda olmasi 
                        //kosulu ve y lerinde ayni koordinata (+ ve - fark etmez) olup olmadigi bakilir. else if kismindada
                        // bu islemin tam tersi yapilir buda direk int e cevirmenin vermis oldugu dez avantaji ortadan kaldirir.
                        if (defenderCheckRect.position.x - restoreDefenderTransformInfo[k].transform.position.x == 1f &&
                            Mathf.Abs(defenderCheckRect.position.y - restoreDefenderTransformInfo[k].transform.position.y) <= 1f)
                        {
                            if (xYonunuBirKereKaydir)
                            {
                                convertedVectorPosition.x += 1f;
                                //  Debug.Log("x + yonunde 1 birim kaydı");
                                defenderCheckRect = new Rect(convertedVectorPosition.x, convertedVectorPosition.y, 2f, 2f);
                                roadCheckRect = new Rect(convertedVectorPosition.x - 1f, convertedVectorPosition.y - 1f, 2f, 2f);
                                xYonunuBirKereKaydir = false;
                            }

                            for (int m = 0; m < restoreDefenderTransformInfo.Length; m++)
                            {


                                if (m != k)
                                {

                                    if (defenderCheckRect.Contains(restoreDefenderTransformInfo[m].transform.position))
                                    {
                                        mainBackGround.SetActive(false);
                                        selectedPlace.SetActive(false);
                                        return new Vector2(-3f, -3f);
                                    }

                                }

                            }
                        }
                        else if (defenderCheckRect.position.y - restoreDefenderTransformInfo[k].transform.position.y == 1f &&
                           Mathf.Abs(defenderCheckRect.position.x - restoreDefenderTransformInfo[k].transform.position.x) <= 1f)
                        {
                            if (yYonunuBirKereKaydir)
                            {
                                convertedVectorPosition.y += 1f;
                              //  Debug.Log("y + yonunde 1 birim kaydı");
                                defenderCheckRect = new Rect(convertedVectorPosition.x, convertedVectorPosition.y, 2f, 2f);
                                roadCheckRect = new Rect(convertedVectorPosition.x - 1f, convertedVectorPosition.y - 1f, 2f, 2f);
                                xYonunuBirKereKaydir = false;
                            }

                            for (int m = 0; m < restoreDefenderTransformInfo.Length; m++)
                            {


                                if (m != k)
                                {

                                    if (defenderCheckRect.Contains(restoreDefenderTransformInfo[m].transform.position))
                                    {
                                        mainBackGround.SetActive(false);
                                        selectedPlace.SetActive(false);
                                        return new Vector2(-3f, -3f);
                                    }

                                }
                            }
                        }
                    }
                }
              //  Debug.Log("cizilen 2 birimlik kare degistirilmis poz = " + roadCheckRect.position.x + roadCheckRect.position.y);
              // Secilen bolgenin yol ile temasin kontrol eden kod kisminin baslangici
              /*Yol kontrol kismindada int e direk cevirdigimiz icin defendiri yolun sol tarafina ve yukari tarafina
               (yol ile bos arazinin birbiri ile kesistigi noktalar icin gecerlidir.) koydugumuz zaman secilen kismin yarisi 
               yolun uzerinde gorunmesi durumu gerceklesir. Burada bu iki durum icin tiklanilan yere göre ya x +1 birim kaydirilir
               yada y +1 birim kaydirilir ve enson tiklanilan bolge yol ile temas edip ama bu iki kosuluda saglamiyorsa (yani else 
               kisminda) gerekli objeler false yapilip Vector3 null dondurelemedigi icin alakasiz yerde return edilir.*/
                for (int i = 0; i < getRect.Length; i++)
                {
                    if (convertedVectorPosition != null)
                    {
                        
                        if (getRect[i].Overlaps(roadCheckRect))
                        {
                           
                            if (getRect[i].x + getRect[i].width == convertedVectorPosition.x)
                            {

                                convertedVectorPosition.x += 1f;
                                roadCheckRect.x += 1f;
                                /*Bu kisim tiklanilan yer yolun kenari olup x koordinati otelendikten sonra bir daha yola denk
                                 * gelip gelmedigini kontrol etmek icin yapilmistir.
                                 */
                                for (int k = 0; k < getRect.Length; k++)
                                {
                                    if (getRect[k].Overlaps(roadCheckRect))
                                    {

                                        mainBackGround.SetActive(false);
                                        selectedPlace.SetActive(false);
                                        return new Vector2(-3f, -3f);
                                    }

                                }
                                for (int j = 0; j < restoreDefenderTransformInfo.Length; j++)
                                {
                                    if (restoreDefenderTransformInfo[j] != null)
                                    {
                                        if (Mathf.Abs(convertedVectorPosition.x - restoreDefenderTransformInfo[j].transform.position.x) <= 1f &&
                            Mathf.Abs(convertedVectorPosition.y - restoreDefenderTransformInfo[j].transform.position.y) <= 1f)
                                        {
                                            mainBackGround.SetActive(false);
                                            selectedPlace.SetActive(false);
                                            
                                            return new Vector2(-3f, -3f);
                                        }
                                    }
                                }
                            }
                            else if (getRect[i].y + getRect[i].height == convertedVectorPosition.y)
                            {
                              
                                convertedVectorPosition.y += 1f;
                                roadCheckRect.y += 1f;
                              
                                /*Bu kisim tiklanilan yer yolun kenari olup y koordinati otelendikten sonra bir daha yola denk
                               * gelip gelmedigini kontrol etmek icin yapilmistir.
                               */
                                for (int k = 0; k < getRect.Length; k++)
                                {
                                    if (getRect[k].Overlaps(roadCheckRect))
                                    {
                                      
                                        mainBackGround.SetActive(false);
                                        selectedPlace.SetActive(false);
                                        
                                        return new Vector2(-3f, -3f);
                                    }
                                    
                                }
                                    for (int j = 0; j < restoreDefenderTransformInfo.Length; j++)
                                    {
                                    if (restoreDefenderTransformInfo[j] != null)
                                    {
                                        if (Mathf.Abs(convertedVectorPosition.x - restoreDefenderTransformInfo[j].transform.position.x) <= 1f &&
                           Mathf.Abs(convertedVectorPosition.y - restoreDefenderTransformInfo[j].transform.position.y) <= 1f)
                                        {
                                            mainBackGround.SetActive(false);
                                            selectedPlace.SetActive(false);
                                           
                                            return new Vector2(-3f, -3f);
                                        }
                                    }
                                }
                            }else
                            {
                                
                                mainBackGround.SetActive(false);
                                selectedPlace.SetActive(false);
                              
                                return new Vector2(-3f, -3f);
                            }
                            
                        }
                       
                    }
                    
                }
               

                return convertedVectorPosition;
            }
            else
            {
                mainBackGround.SetActive(false);
                return new Vector2(-3f, -3f);
            }
           
        }
        else
        {
            return new Vector2(-3f, -3f);
        }

    }
    private void CheckManuBackGroundPos(Vector2 checkUISelectionPoz)
    {
        //Debug.Log("tiklanilan yerin bilgisi" + checkUISelectionPoz + "cameranin pozisyonu" + Camera.main.transform.position);
       // Debug.Log("ortho yari size" + Camera.main.orthographicSize);
       
        if ((checkUISelectionPoz.x -3) < (Camera.main.transform.position.x - Camera.main.transform.position.x) )
        {
            
            mainBackGround.GetComponent<RectTransform>().localPosition = new Vector2( 2, 0);
        }else if((Camera.main.transform.position.x - Camera.main.transform.position.x) + cameraWidhtForWorldSpace < checkUISelectionPoz.x + 3)
        {
            mainBackGround.GetComponent<RectTransform>().localPosition = new Vector2( -2, 0);
        }
        if((checkUISelectionPoz.y +2) > (Camera.main.transform.position.y - Camera.main.orthographicSize) + Camera.main.orthographicSize *2)
        {
            
            mainBackGround.GetComponent<RectTransform>().localPosition = new Vector2(0, -2);
        }else if((checkUISelectionPoz.y - 2) < (Camera.main.transform.position.y - Camera.main.orthographicSize))
        {
          
            mainBackGround.GetComponent<RectTransform>().localPosition = new Vector2(0, 2);
        }
        if((checkUISelectionPoz.x - 3) >= (Camera.main.transform.position.x - Camera.main.transform.position.x) &&
            (Camera.main.transform.position.x - Camera.main.transform.position.x) + cameraWidhtForWorldSpace >= checkUISelectionPoz.x + 3 &&
            (checkUISelectionPoz.y + 2) <= ((Camera.main.transform.position.y - Camera.main.orthographicSize) + Camera.main.orthographicSize * 2) &&
            (checkUISelectionPoz.y - 2) >= (Camera.main.transform.position.y - Camera.main.orthographicSize))
            {
            mainBackGround.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        }


    }
    private bool PurchaseDefender(int defenderCost)
    {
        int getBalance = uiEnvironment.GetComponent<UIEnvironment>().GetCoinText();
      //  Debug.Log("hesaptaki bakiye =" + getBalance);
        if(getBalance < defenderCost)
        {
           // Debug.Log("hesapta yeterli bakiyeniz yok");
            return false;
        }
        else
        {

            return true;
        }
    }

    public void SellDefender()
    {
        if(selectedDefender != null)
        {
            Defender[] canDestroyable = FindObjectsOfType<Defender>();
            for(int i =0; i< canDestroyable.Length; i++)
            {
                if(canDestroyable[i].transform.position == selectedDefender)
                {
                    squareRangeOfDefender.SetActive(false);
                    circleRangeOfDefender.SetActive(false);

                    mainBackGround.SetActive(false);
                    selectedPlace.SetActive(false);
                    allowShowCanvas = false;
                    uiEnvironment.GetComponent<UIEnvironment>().SetCoinText(canDestroyable[i].GetDefenderCost()/2);
                    if (canDestroyable[i].GetComponent<StableDefender>() != null)
                    {
                        // Debug.Log("stabledefender projectile canDestroyProjectile true olmas gerek");
                        canDestroyable[i].GetComponent<StableDefender>().DestroyProjectile();
                    } else if (canDestroyable[i].GetComponent<RotatebleDefender>() != null)
                    {
                        //Debug.Log("RotatebleDefender projectile canDestroyProjectile true olmas gerek");
                        canDestroyable[i].GetComponent<RotatebleDefender>().DestroyProjectile();
                    } else if (canDestroyable[i].GetComponent<TechnologicDefender>() != null)
                    {
                        canDestroyable[i].GetComponent<TechnologicDefender>().DestroyProjectile(); }
                    //TODO technologic defender icinde else if eklenecek cunku o bu siniflara sahip degil.
                    Destroy(canDestroyable[i].gameObject);
                }
            }
        }
    }
    public void EnableArrowSelection(Enemy target)
    {
        if(target != null)
        {
            selectedEnemy = target;
            SetArrowSelection(true);
            timer = 0f;

        }
       
    }
    private void ShowArrowSelection()
    {
        float maxTime = 1.1f;
        if(GetArrowSelection())
        {
            if(selectedEnemy != null)
                {
                if (!arrow.activeInHierarchy)
                {
                arrow.SetActive(true);
                mainBackGround.SetActive(false);
                selectedPlace.SetActive(false);

                    //Debug.Log("bu fonk calisti");


                }
                else
            {
                
                    if (timer <= maxTime)
                    {
                        mainBackGround.SetActive(false);
                        selectedPlace.SetActive(false);
                        circleRangeOfDefender.SetActive(false);
                        squareRangeOfDefender.SetActive(false);
                        transform.position = selectedEnemy.transform.position;
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        timer = 0f;
                        arrow.SetActive(false);
                        SetArrowSelection(false);
                    }
                }

            }
            else
            {
                arrow.SetActive(false);
                SetArrowSelection(false);
            }
        }
        else
        {
            arrow.SetActive(false);
        }
    }
    public bool GetArrowSelection()
    { return allowArrowMark; }
    public void SetArrowSelection(bool state)
    {
        allowArrowMark = state;
    }
    public bool getHelpUIisActive()
    {
        return helpLevelPanelIsActive;
    }
    public void SetHelpLevelPanelIsActive(bool state)
    {
        helpLevelPanelIsActive = state;
    }
    public void DisableAllowShowCanvas()
    {
        allowShowCanvas = false;
        
    }
    private void ShowForbiddenArea(Vector2 touchedArea)
    {
        if(allowForbiddenAS)
        {
            if(timerForbiddenAS >= 1f)
            {
                timerForbiddenAS = 0f;
                forbiddenAreaSign.SetActive(false);
                allowForbiddenAS = false;
            }
            else
            {
                forbiddenAreaSign.SetActive(true);
                
                timerForbiddenAS += Time.deltaTime;
            }
        }
        else
        {
            timerForbiddenAS = 0f;
            forbiddenAreaSign.SetActive(false);
        }
    }
    public void SetAllowForbiddenAS(bool state)
    {
        allowForbiddenAS = state;
    }
}
    

