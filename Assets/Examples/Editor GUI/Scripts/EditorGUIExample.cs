using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class EditorGUIExample : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField]
    private float2 m_range;
    [SerializeField]
    private GameObject m_gameObject;
    #endregion
}

[CustomEditor(typeof(EditorGUIExample))]
public class EditorGUIExampleEditor : Editor
{
    #region Private Fields
    private string m_textWithPlaceholder;
    private SerializedProperty m_rangeProperty;
    private SerializedProperty m_gameObjectProperty;
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        m_rangeProperty = serializedObject.FindProperty("m_range");
        m_gameObjectProperty = serializedObject.FindProperty("m_gameObject");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Tuntenfisch.Commons.Editor.EditorGUILayout.ScriptHeaderField(serializedObject);

        KeyCode keyCode = Tuntenfisch.Commons.Editor.EditorGUILayout.TextFieldWithPlaceholder(new GUIContent("Text With Placeholder"), ref m_textWithPlaceholder, "I'm a placeholder");

        switch (keyCode)
        {
            case KeyCode.Escape:
            case KeyCode.Return:
                m_textWithPlaceholder = string.Empty;
                break;
        }

        EditorGUI.BeginChangeCheck();
        SerializedProperty minRangeProperty = m_rangeProperty.FindPropertyRelative("x");
        SerializedProperty maxRangeProperty = m_rangeProperty.FindPropertyRelative("y");
        float minRange = minRangeProperty.floatValue;
        float maxRange = maxRangeProperty.floatValue;
        Tuntenfisch.Commons.Editor.EditorGUILayout.MinMaxRange(new GUIContent("Min Max Range"), ref minRange, ref maxRange, 0.0f, 1.0f);

        if (EditorGUI.EndChangeCheck())
        {
            minRangeProperty.floatValue = minRange;
            maxRangeProperty.floatValue = maxRange;
        }

        Rect rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(2.0f * EditorGUIUtility.singleLineHeight));
        int selectedPopupOption = Tuntenfisch.Commons.Editor.EditorGUI.PropertyFieldWithPopupOptions(rect, new GUIContent(m_gameObjectProperty.displayName), m_gameObjectProperty, -1, new string[] { "Press me" });

        switch (selectedPopupOption)
        {
            case 0:
                Debug.Log("I was pressed.");
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
    #endregion
}