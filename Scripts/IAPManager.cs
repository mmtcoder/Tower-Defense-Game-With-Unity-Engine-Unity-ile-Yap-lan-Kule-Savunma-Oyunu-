using UnityEngine.Purchasing;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public Slider paymentSlider;
    public Text amountText;
    

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private string donateFiftyCent = "one_tl_donate";
    private string donateOneDolar = "three_tl_donate";
    private string donateTwoDolar = "five_tl_donate";

    private const string LANGUAGE_PREFS_NAME = "LanguageKey";
    private const string LANGUAGE_TURKISH = "LanguageTurkish";
    private const string LANGUAGE_ENGLISH = "LanguageEnglish";

    private Text donateText;

    // Product identifiers for all products capable of being purchased: 
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.





    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(donateFiftyCent, ProductType.Consumable);
        builder.AddProduct(donateOneDolar, ProductType.Consumable);
        builder.AddProduct(donateTwoDolar, ProductType.Consumable);


        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }
    public void DonateFiftyCentButton()
    {
        BuyProductID(donateFiftyCent);
    }
    public void DonateOneDolarButton()
    {
        BuyProductID(donateOneDolar);
    }
    public void DonateTwoDolarButton()
    {
        BuyProductID(donateTwoDolar);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        if(String.Equals(args.purchasedProduct.definition.id,donateFiftyCent,StringComparison.Ordinal))
        {
            if(donateText != null)
            {
                if (LANGUAGE_TURKISH.Equals(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
                {
                    donateText.text = "1 TL desteğiniz gerceklesti. Desteğin için çok teşekkür ederim.";
                }
                else
                {
                    donateText.text = "Your support of 1 dolar has been executed. Thank you for your support.";
                }
            }
            
           
        }
        else if (String.Equals(args.purchasedProduct.definition.id, donateOneDolar, StringComparison.Ordinal))
        {
            if(donateText != null)
            {
                if (LANGUAGE_TURKISH.Equals(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
                {
                    donateText.text = "3 TL desteğiniz gerceklesti. Desteğin için çok teşekkür ederim.";
                }
                else
                {
                    donateText.text = "Your support of 1 dolar has been executed. Thank you for your support.";
                }
            }
            
        }
        else if (String.Equals(args.purchasedProduct.definition.id, donateTwoDolar, StringComparison.Ordinal))
        {
            if(donateText != null)
            {
                if (LANGUAGE_TURKISH.Equals(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
                {
                    donateText.text = "5 TL desteğiniz gerceklesti. Desteğin için çok teşekkür ederim :)";
                }
                else
                {
                    donateText.text = "Your support of 1 dolar has been executed. Thank you for your support :)";
                }
            }
                
        }
        else
        {
            Debug.Log("Satin alma basarisiz");
        }
       
        // A consumable product has been purchased by this user.


        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                //Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
               // Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
           // Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
           // Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
           /* // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });*/
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
           // Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
       // Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
       // Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
       // Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }


    // Start is called before the first frame update
    void Start()
    {
       
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
       
    }

    private void Update()
    {
        if(paymentSlider != null)
        {
            if(paymentSlider.value == 0f)
            {
                if(LANGUAGE_TURKISH.Equals(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
                {
                    amountText.text = "1TL DESTEKLE";
                }
                else
                {
                    amountText.text = "1$ DONATE";
                }
                
            }
            else if(paymentSlider.value == 1f)
            {
                if (LANGUAGE_TURKISH.Equals(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
                {
                    amountText.text = "3TL DESTEKLE";
                }
                else
                {
                    amountText.text = "1$ DONATE";
                }
            }
            else if (paymentSlider.value == 2f)
            {
                if (LANGUAGE_TURKISH.Equals(PlayerPrefs.GetString(LANGUAGE_PREFS_NAME)))
                {
                    amountText.text = "5TL DESTEKLE";
                }
                else
                {
                    amountText.text = "1$ DONATE";
                }
            }
        }
        donateText = GameObject.FindGameObjectWithTag("DonateText").GetComponent<Text>();
    }

    public void DonateButton()
    {
        if(paymentSlider.value == 0)
        {
            DonateFiftyCentButton();
        }
        else if (paymentSlider.value == 1)
        {
            DonateOneDolarButton();
        }
        else if (paymentSlider.value == 2)
        {
            DonateTwoDolarButton();
        }
    }
}
