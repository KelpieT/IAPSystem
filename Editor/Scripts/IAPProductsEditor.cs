using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Viter.IAPSystem
{
    [CustomEditor(typeof(IAPProducts))]
    public class IAPProductsEditor : Editor
    {
        private bool cache = false;
        private SerializedProperty allInAppProducts;
        IAPProducts iapProducts;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!cache)
            {
                iapProducts = (IAPProducts)target;
                allInAppProducts = serializedObject.FindProperty("productInfos");
                cache = true;
            }

            if (allInAppProducts != null)
            {
                ShowArrayProperty(allInAppProducts);
            }

            serializedObject.ApplyModifiedProperties();

            // if (GUI.changed)
            if (GUILayout.Button("Prepare for save"))
            {
                EditorUtility.SetDirty(iapProducts);
            }
        }

        public void ShowArrayProperty(SerializedProperty list, string label = "Product ")
        {

            {
                if (list.arraySize > 0)
                {
                    int indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 1;

                    GUILayout.Space(5);
                    for (int i = 0; i < list.arraySize; i++)
                    {
                        GUILayout.Space(5);
                        EditorGUILayout.BeginHorizontal();

                        SerializedProperty productName = list.GetArrayElementAtIndex(i).FindPropertyRelative("productName");
                        if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20f)))
                        {
                            list.InsertArrayElementAtIndex(i);
                        }

                        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20f)))
                        {
                            list.DeleteArrayElementAtIndex(i);
                            return;
                        }
                        EditorGUILayout.EndHorizontal();

                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel = indentLevel + 1;
                            EditorGUILayout.BeginVertical();

                            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                            labelStyle.fontStyle = FontStyle.Bold;

                            // BeginBox();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Product Name : ", labelStyle, GUILayout.MaxWidth(200));
                            productName.stringValue = EditorGUILayout.TextField(productName.stringValue);
                            EditorGUILayout.EndHorizontal();
                            // EndBox();

                            // BeginBox();
                            GUILayout.Space(5);
                            SerializedProperty overrideStoreIds = list.GetArrayElementAtIndex(i).FindPropertyRelative("overrideStoreIds");
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Override Store Specific IDs : ", labelStyle, GUILayout.MaxWidth(200));
                            overrideStoreIds.boolValue = EditorGUILayout.Toggle(overrideStoreIds.boolValue);
                            EditorGUILayout.EndHorizontal();

                            labelStyle.fontStyle = FontStyle.Normal;
                            if (overrideStoreIds.boolValue)
                            {
                                int subIndentLevel = EditorGUI.indentLevel;
                                EditorGUI.indentLevel = (subIndentLevel + 1);

                                EditorGUILayout.BeginHorizontal();

                                SerializedProperty iOSSku = list.GetArrayElementAtIndex(i).FindPropertyRelative("iOSSku");
                                EditorGUILayout.LabelField("iOS SKU : ", labelStyle, GUILayout.MaxWidth(200));
                                iOSSku.stringValue = EditorGUILayout.TextField(iOSSku.stringValue);
                                EditorGUILayout.EndHorizontal();


                                EditorGUILayout.BeginHorizontal();
                                SerializedProperty googleSku = list.GetArrayElementAtIndex(i).FindPropertyRelative("googleSku");
                                EditorGUILayout.LabelField("Google SKU : ", labelStyle, GUILayout.MaxWidth(200));
                                googleSku.stringValue = EditorGUILayout.TextField(googleSku.stringValue);
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                SerializedProperty amazonSku = list.GetArrayElementAtIndex(i).FindPropertyRelative("amazonSku");
                                EditorGUILayout.LabelField("Amazon SKU : ", labelStyle, GUILayout.MaxWidth(200));
                                amazonSku.stringValue = EditorGUILayout.TextField(amazonSku.stringValue);
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                SerializedProperty samsungSku = list.GetArrayElementAtIndex(i).FindPropertyRelative("samsungSku");
                                EditorGUILayout.LabelField("Samsung SKU : ", labelStyle, GUILayout.MaxWidth(200));
                                samsungSku.stringValue = EditorGUILayout.TextField(samsungSku.stringValue);
                                EditorGUILayout.EndHorizontal();
                            }
                            GUILayout.Space(5);
                            // EndBox();

                            // BeginBox();
                            EditorGUILayout.BeginHorizontal();
                            SerializedProperty productType = list.GetArrayElementAtIndex(i).FindPropertyRelative("productType");
                            EditorGUILayout.LabelField("Product Type : ", labelStyle, GUILayout.MaxWidth(200));
                            // productType.intValue = EditorGUILayout.Popup (productType.intValue, new string[]{"CONSUMABLE", "NON CONSUMABLE", "SUBSCRIPTION"});
                            productType.intValue = EditorGUILayout.Popup(productType.intValue, new string[] { "CONSUMABLE", "NON CONSUMABLE" });
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            SerializedProperty rewardType = list.GetArrayElementAtIndex(i).FindPropertyRelative("rewardType");
                            EditorGUILayout.LabelField("Reward Type : ", labelStyle, GUILayout.MaxWidth(200));
                            rewardType.intValue = EditorGUILayout.Popup(rewardType.intValue, new string[] { "REMOVE ADS", "GEMS", "OTHER" });
                            EditorGUILayout.EndHorizontal();

                            RewardType thisRewardType = (RewardType)rewardType.intValue;

                            if (thisRewardType == RewardType.GEMS)
                            {
                                EditorGUILayout.BeginHorizontal();
                                SerializedProperty rewardAmount = list.GetArrayElementAtIndex(i).FindPropertyRelative("rewardAmount");
                                EditorGUILayout.LabelField("Gems Amount : ", labelStyle, GUILayout.MaxWidth(200));
                                rewardAmount.intValue = EditorGUILayout.IntField(rewardAmount.intValue);
                                EditorGUILayout.EndHorizontal();
                            }
                            // EndBox();

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUI.indentLevel = indentLevel;
                }
            }

            GUILayout.Space(2);
            GUI.backgroundColor = Color.grey;
            GUIStyle style2 = new GUIStyle(GUI.skin.button);
            style2.richText = true;
            style2.fixedHeight = 30;

            if (GUILayout.Button(new GUIContent("<b>Add Product</b>"), style2))
            {
                list.arraySize += 1;
            }
            GUILayout.Space(2);
            GUI.backgroundColor = Color.white;
            DrawScriptingDefineSymbol();
        }

        void DrawScriptingDefineSymbol()
        {
            GUI.enabled = !EditorApplication.isCompiling;
            GUI.backgroundColor = Color.grey;
            GUIStyle style2 = new GUIStyle(GUI.skin.button);
            style2.richText = true;
            style2.fontStyle = FontStyle.Bold;
            style2.fixedHeight = 30;

            GUI.backgroundColor = Color.white;
        }

        void DrawSDKDetection()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.HelpBox("Unity IAP SDK not detected, Please import Unity IAP SDK to make in-app purchases. ", MessageType.Warning, true);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("You can download Unity IAP SDK from", GUILayout.MaxWidth(210));

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.red;

            if (GUILayout.Button("Here.", labelStyle))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/add-ons/services/billing/unity-iap-68207");
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}
