using System;
using System.Reflection;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public class PropertyPrompt : EditorWindow
    {
        #region Public Properties
        public SerializedProperty Property
        {
            get => m_property;

            set
            {
                m_property = value;
                FieldInfo fieldInfo = m_property.GetFieldInfo();
                m_displayName = new GUIContent($"{ObjectNames.NicifyVariableName(fieldInfo.FieldType.Name)} {m_property.displayName}");
            }
        }
        #endregion

        #region Public Fields

        public Action<SerializedProperty> OnInputReceived;
        public string Message;
        public string OkButtonLabel;
        public string CancelButtonLabel;
        #endregion

        #region Private Fields
        private SerializedProperty m_property;
        private GUIContent m_displayName;
        #endregion

        #region Unity Callbacks
        private void OnGUI()
        {
            bool close = false;

            UnityEditor.EditorGUILayout.BeginVertical();

            if (!string.IsNullOrEmpty(Message))
            {
                UnityEditor.EditorGUILayout.HelpBox(Message, MessageType.Info);
            }
            Property.serializedObject.Update();
            UnityEditor.EditorGUILayout.PropertyField(Property, m_displayName);
            Property.serializedObject.ApplyModifiedProperties();
            UnityEditor.EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(OkButtonLabel))
            {
                OnInputReceived?.Invoke(Property);
                close = true;
            }

            if (GUILayout.Button(CancelButtonLabel))
            {
                close = true;
            }
            UnityEditor.EditorGUILayout.EndHorizontal();
            UnityEditor.EditorGUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                minSize = new float2(minSize.x, rect.height + 2.0f * UnityEditor.EditorGUIUtility.standardVerticalSpacing);
                maxSize = new float2(maxSize.x, minSize.y);
            }

            if (close)
            {
                Close();
            }
        }
        #endregion

        #region Public Methods
        public static void Show(GUIContent title, SerializedProperty property, Action<SerializedProperty> OnInputReceived, string message = null, string okButtonLabel = "Ok", string cancelButtonLabel = "Cancel")
        {
            PropertyPrompt inputPrompt = CreateInstance<PropertyPrompt>();
            inputPrompt.titleContent = title;
            inputPrompt.Property = property;
            inputPrompt.OnInputReceived = OnInputReceived;
            inputPrompt.Message = message;
            inputPrompt.OkButtonLabel = okButtonLabel;
            inputPrompt.CancelButtonLabel = cancelButtonLabel;
            EditorApplication.delayCall += inputPrompt.ShowModal;
        }
        #endregion
    }
}