using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class LegacyTextUnityIAPButtonView : UnityIAPButtonView
{
    [SerializeField] private Text txtPrice;
    [SerializeField] private Text txtTitle;
    [SerializeField] private Text txtDescription;
    
    public override void SetView(ProductMetadata productMetadata)
    {
        if (txtPrice != null)
        {
            txtPrice.text = productMetadata.localizedPriceString;
        }

        if (txtTitle != null)
        {
            txtTitle.text = productMetadata.localizedTitle;
        }

        if (txtDescription != null)
        {
            txtDescription.text = productMetadata.localizedDescription;
        }
    }
}
