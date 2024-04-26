using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;


namespace Viter.IAPSystem
{
    /// <summary>
    /// This class controlls ingame purchased and its reward. This sdk typically leverages the offical unity iap sdk and used as wrapper to make workflow
    /// simple that suits the asset store plugin and anyone without developement skills can easily configure this setup.
    /// </summary>
    public class IAPManager : Singleton<IAPManager>, IDetailedStoreListener
    {
        public bool manualInitialization = false;
        private IAPProducts iapManager;
        private bool hasInitialised = false;

        private IStoreController storeController;
        private IExtensionProvider extensionProvider;

        /// Purchase even callbacks.
        public static event Action<ProductInfo> OnPurchaseSuccessfulEvent;
        public static event Action<Product> OnPurchaseSuccess;
        public static event Action<string> OnPurchaseFailedEvent;
        public static event Action<bool> OnRestoreCompletedEvent;
        public static event Action<bool> OnIAPInitializeEvent;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (!manualInitialization)
            {
                Initialise();
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            if (!manualInitialization)
            {
                InitializeUnityIAP();
            }
        }

        /// <summary>
        /// Initialize all IAPs from list of product info.
        /// </summary>
        public void InitializeUnityIAP()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

            foreach (ProductInfo info in iapManager.productInfos)
            {
                IDs ids = new IDs();
                if (info.iOSSku != string.Empty) { ids.Add(info.iOSSku, AppleAppStore.Name); }
                if (info.googleSku != string.Empty) { ids.Add(info.iOSSku, GooglePlay.Name); }
                if (info.amazonSku != string.Empty) { ids.Add(info.iOSSku, AmazonApps.Name); }
                builder.AddProduct(info.productName, (ProductType)info.productType, ids);
            }
            UnityPurchasing.Initialize(this, builder);
        }


        /// <summary>
        /// IAP Initialize success callback from Unity IAP SDK.
        /// </summary>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            storeController = controller;
            extensionProvider = extensions;

            OnIAPInitializeEvent?.Invoke(true);
        }

        /// <summary>
        /// IAP Initialize fail callback from Unity IAP SDK.
        /// </summary>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnIAPInitializeEvent?.Invoke(false);
        }

        /// <summary>
        /// Purchase Success callback from Unity IAP SDK.
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            //~~~~~~~~~~~~~~ You can verify receipt here before processing rewards. ~~~~~~~~~~~~~~//

            ProcessPurchaseRewards(e.purchasedProduct.definition.id);
            return PurchaseProcessingResult.Complete;
        }

        /// <summary>
        /// Purchase fail callback from Unity IAP SDK.
        /// </summary>
        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            OnPurchaseFailure(p.ToString());
        }

        // Start Initialization of IAP SDK.
        public void Initialise()
        {
            if (!hasInitialised)
            {
                iapManager = (IAPProducts)Resources.Load("IAPProducts");
                hasInitialised = true;
            }
        }

        /// <summary>
        /// Returns if IAP SDK has initialized or not.
        /// </summary>
        public bool hasUnityIAPSdkInitialised
        {
            get { return storeController != null && extensionProvider != null; }
        }

        // Returns product at given index on the list.
        public ProductInfo GetProductInfoById(int productIndex)
        {
            if (!hasInitialised)
            {
                Initialise();
            }
            return iapManager.productInfos[productIndex];
        }

        // Returns product with given name.
        public ProductInfo GetProductInfoByName(string productName)
        {
            if (!hasInitialised)
            {
                Initialise();
            }
            ProductInfo productInfo = iapManager.productInfos.ToList().Find(o => o.productName == productName);
            return productInfo;
        }

        public Product GetProductFromSku(string productName)
        {
            Product product = null;
            if (product == null)
            {
                if (storeController == null)
                {
                    Debug.LogError("storeController == null");
                }
                else if (storeController.products == null)
                {
                    Debug.LogError("storeController.products == null");
                }
                else if (storeController.products.WithID(productName) == null)
                {
                    Debug.LogError($"Cant find product with id {productName}");
                }
                else
                {
                    product = storeController.products.WithID(productName);
                }
            }
            return product;
        }


        /// <summary>
        /// Restores all purchased products.
        /// </summary>
        public void RestoreAllProducts()
        {
            if (hasUnityIAPSdkInitialised)
            {
                extensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result, stringRes) =>
                {
                    if (OnRestoreCompletedEvent != null)
                    {
                        OnRestoreCompletedEvent.Invoke(result);
                    }
                });
            }
        }

        /// <summary>
        /// Invokes purchase success event.
        /// </summary>
        public void PurchaseProduct(ProductInfo productInfo)
        {
            if (hasUnityIAPSdkInitialised)
            {
                Product product = storeController.products.WithID(productInfo.productName);
                if (product == null) { }
                else if (!product.availableToPurchase) { }
                else { storeController.InitiatePurchase(product); }
            }
        }

        /// <summary>
        /// Invokes sandbox purchase success event.
        /// </summary>
        public void OnSandboxPurchaseSuccess(ProductInfo productInfo)
        {
            ProcessPurchaseRewards(productInfo.productName);
        }

        /// <summary>
        /// Invokes sandbox purchase fail event.
        /// </summary>
        public void OnSandboxPurchaseFailure()
        {
            OnPurchaseFailure("Sandbox Purchase Failure");
        }

        /// <summary>
        /// Process rewards for the purhcased product.
        /// </summary>
        public void ProcessPurchaseRewards(string productName)
        {
            ProductInfo productInfo = IAPManager.Instance.GetProductInfoByName(productName);
            if (OnPurchaseSuccessfulEvent != null)
            {
                OnPurchaseSuccessfulEvent.Invoke(productInfo);
            }
            Product product = GetProductFromSku(productInfo.productName);
            OnPurchaseSuccess?.Invoke(product);
        }

        /// <summary>
        /// Invokes purchase fail event.
        /// </summary>
        public void OnPurchaseFailure(string reason)
        {
            if (OnPurchaseFailedEvent != null)
            {
                OnPurchaseFailedEvent.Invoke(reason);
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            if (OnIAPInitializeEvent != null)
            {
                OnIAPInitializeEvent.Invoke(false);
            }
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            if (OnPurchaseFailedEvent != null)
            {
                OnPurchaseFailedEvent.Invoke(failureDescription.reason.ToString());
            }
        }
    }
}