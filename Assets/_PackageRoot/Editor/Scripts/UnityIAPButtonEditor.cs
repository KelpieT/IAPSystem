#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Viter.IAPSystem
{
    [CustomEditor(typeof(UnityIAPButton))]
	public class UnityIAPButtonEditor : Editor
	{
		private bool cache = false;
		UnityIAPButton unityIAPButton;
		IAPProducts iapProducts;

		string[] allProducts;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			serializedObject.Update();

			if (!cache)
			{
				cache = true;
				unityIAPButton = (UnityIAPButton)target;
				iapProducts = (IAPProducts)Resources.Load("IAPProducts");
				allProducts = new string[iapProducts.productInfos.Length];

				int index = 0;
				foreach (ProductInfo product in iapProducts.productInfos)
				{
					allProducts[index] = product.productName;
					index++;
				}
			}


			GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
			labelStyle.fontStyle = FontStyle.Bold;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Button Type : ", labelStyle, GUILayout.MaxWidth(120));
			unityIAPButton.buttonType = EditorGUILayout.Popup(unityIAPButton.buttonType, new string[] { "RESTORE", "PURCHASE" });
			EditorGUILayout.EndHorizontal();

			if (unityIAPButton.buttonType == 1)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Product Name : ", labelStyle, GUILayout.MaxWidth(120));
				unityIAPButton.inAppProductIndex = EditorGUILayout.Popup(unityIAPButton.inAppProductIndex, allProducts);
				EditorGUILayout.EndHorizontal();

			}
			if (GUILayout.Button("Prepare for save"))
			{
				EditorUtility.SetDirty(unityIAPButton);
			}
			EditorGUILayout.HelpBox("You don't need to add On Click() event explicitly. It will be handled automatically. Purchase completion responce and rewards will be handled from IAPManagert.cs", MessageType.Info);
		}
	}
}
#endif