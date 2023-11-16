using UnityEngine;
using UnityEditor;

namespace Viter.IAPSystem
{
    public class UnityIAPEditorMenu : MonoBehaviour
    {
        #region MobileAdsSetting
        [MenuItem("Viter/Unity IAP Settings", false, 14)]
        public static void GenerateMobileAdsManager()
        {
            string assetPath = "Assets/Viter/Resources";
            string assetName = "IAPProducts.asset";

            IAPProducts asset;

            if (!System.IO.Directory.Exists(assetPath))
            {
                System.IO.Directory.CreateDirectory(assetPath);
            }

            if (System.IO.File.Exists(assetPath + "/" + assetName))
            {
                asset = (IAPProducts)(Resources.Load(System.IO.Path.GetFileNameWithoutExtension(assetName)));
            }
            else
            {
                asset = ScriptableObject.CreateInstance<IAPProducts>();
                AssetDatabase.CreateAsset(asset, assetPath + "/" + assetName);
                AssetDatabase.SaveAssets();
            }

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
        #endregion

        // [InitializeOnLoad]
        // public class AutorunNew
        // {
        //     static AutorunNew() {
        //         EditorApplication.update += RunOnce;
        //         AssetDatabase.importPackageCompleted += ImportPackageStartedCallback;	
        //     }

        //     static void RunOnce() {
        //         EditorApplication.update -= RunOnce;
        //     }
        // }
    }
}
