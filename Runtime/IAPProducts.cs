
using UnityEngine;

/// <summary>
/// This scriptable class instance contains list of all in game iap skus. This can be
/// configured from Viter -> Unity IAP Settings menu item.
/// </summary>

namespace Viter.IAPSystem
{
    public class IAPProducts : ScriptableObject
    {
        public ProductInfo[] productInfos;
    }

    [System.Serializable]
    public class ProductInfo
    {
        // ID of product.
        public string productName;

        // If store sku is different then product name then enable override.
        public bool overrideStoreIds = false;

        // iOS Sku.
        public string iOSSku;

        // Google Sku.
        public string googleSku;

        // Amazon Sku.
        public string amazonSku;

        // Samsung Sku.
        public string samsungSku;

        // Product type : CONSUMABLE, NON CONSUMABLE, SUBSCRIPTION
        public int productType;

        // Type of reward.
        public int rewardType;

        // Amount of reward if reward type is GEMS.
        public int rewardAmount;
    }

    // Type of reward associated with product.
    public enum RewardType
    {
        REMOVE_ADS,
        GEMS,
        OTHER
    }
}
