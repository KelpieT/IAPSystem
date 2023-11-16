using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;

public class TmpUnityIAPButtonView : UnityIAPButtonView
{
    [SerializeField] private TMP_Text txtPrice;
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtDescription;

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
