using UnityEngine;
using UnityEngine.Purchasing;

namespace Viter.IAPSystem
{
    public class PurchaseHandler : MonoBehaviour
    {
        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            IAPManager.OnPurchaseSuccessfulEvent += OnPurchaseSuccessful;
            IAPManager.OnPurchaseFailedEvent += OnPurchaseFailed;
            IAPManager.OnRestoreCompletedEvent += OnRestoreCompleted;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            IAPManager.OnPurchaseSuccessfulEvent -= OnPurchaseSuccessful;
            IAPManager.OnPurchaseFailedEvent -= OnPurchaseFailed;
            IAPManager.OnRestoreCompletedEvent -= OnRestoreCompleted;
        }

        /// <summary>
        /// Purchase Rewards will be processed from here. You can adjust your code based on your requirements.
        /// </summary>
        /// <param name="productInfo"></param>
        private void OnPurchaseSuccessful(ProductInfo productInfo)
        {
            RewardType rewardType = ((RewardType)productInfo.rewardType);

            switch (rewardType)
            {
                case RewardType.REMOVE_ADS:
                    //Remove ads reward
                    break;
                case RewardType.GEMS:

                    int rewardAmount = productInfo.rewardAmount;
                    //Reward purchase
                    break;

                case RewardType.OTHER:
                    break;
            }
            Product product = IAPManager.Instance.GetProductFromSku(productInfo.productName);
        }

        private void OnPurchaseFailed(string reason)
        {

        }

        private void OnRestoreCompleted(bool result)
        {
            if (result)
            {
            }
            else
            {
            }
        }
    }
}


