
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Purchasing;


namespace Viter.IAPSystem
{
    /// <summary>
    /// Any UI Button with attached this script will work as IAP Button and request a IAP Purchase of selected SKU from the dropdown selection. 
    /// Also this script acts as restore iap button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UnityIAPButton : MonoBehaviour
    {
        public event Action OnRestoreIapsPressed;
        public event Action<Product> OnPurchasePressed;
        // Button type - Purchase or Restore.
        public int buttonType = 1;

        // Which product sku to be assigned to this button.
        public int inAppProductIndex = 0;

        // Info of the producrt.
        private ProductInfo thisProduct;

        // IAP title text.
        [SerializeField] private UnityIAPButtonView unityIAPButtonView;

        [SerializeField] private Button thisButton;

        // Button has initialized or not.
        private bool hasInitialized = false;


        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            Invoke(nameof(InitIAPProduct), 0.1F);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            thisButton.onClick.RemoveListener(OnPurchaseButtonPressed);
        }

        /// <summary>
        ///  Fetrched IAP product and assign to button which listener.
        /// </summary>
        private void InitIAPProduct()
        {
            thisProduct = IAPManager.Instance.GetProductInfoById(inAppProductIndex);
            thisButton.onClick.AddListener(OnPurchaseButtonPressed);

            if (!hasInitialized)
            {
                if (IAPManager.Instance.hasUnityIAPSdkInitialised)
                {
                    Product product = IAPManager.Instance.GetProductFromSku(thisProduct.productName);

                    if (product != null)
                    {

                        unityIAPButtonView?.SetView(product.metadata);
                        hasInitialized = true;
                    }
                }
            }
        }

        // Purchase button click listner.
        private void OnPurchaseButtonPressed()
        {
            if (buttonType == 0)
            {
                OnRestoreIapsPressed?.Invoke();
                IAPManager.Instance.RestoreAllProducts();
            }
            else
            {
                Product product = IAPManager.Instance.GetProductFromSku(thisProduct.productName);
                OnPurchasePressed?.Invoke(product);
                IAPManager.Instance.PurchaseProduct(thisProduct);
            }
        }
    }
}
