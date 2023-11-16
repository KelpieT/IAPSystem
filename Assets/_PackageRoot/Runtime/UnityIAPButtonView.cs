using UnityEngine;
using UnityEngine.Purchasing;

public abstract class UnityIAPButtonView : MonoBehaviour
{
    public abstract void SetView(ProductMetadata productMetadata);
}
