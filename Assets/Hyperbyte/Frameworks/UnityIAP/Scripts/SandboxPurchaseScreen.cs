
using UnityEngine;
using UnityEngine.UI;

namespace Viter.IAPSystem
{
    /// <summary>
    /// Sandbox purchase will be used while Unity IAP SDK setup is not completed.
    /// </summary>
    public class SandboxPurchaseScreen : MonoBehaviour
    {
        public Text txtProductDescription;
        ProductInfo currentProduct = null;

        /// <summary>
        /// Initializes the purchase request.
        /// </summary>
        public void InitialiseSandboxPurchase(ProductInfo productInfo)
        {
            txtProductDescription.text = "Purchasing " + productInfo.productName;
            currentProduct = productInfo;
        }

        /// <summary>
        /// Close button listener.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Returns purchase success responce.
        /// </summary>
        public void SendSuccessResponce()
        {
            IAPManager.Instance.OnSandboxPurchaseSuccess(currentProduct);
            Destroy(gameObject);
        }

        /// <summary>
        /// Returns purchase fail responce.
        /// </summary>
        public void SendFailureResponce()
        {
            IAPManager.Instance.OnSandboxPurchaseFailure();
            Destroy(gameObject);
        }
    }
}