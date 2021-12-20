using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Sets.Editor
{
    [CustomPropertyDrawer(typeof(RuntimeSet<>), useForChildren: true)]
    public class RuntimeSetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Commons.Editor.EditorGUI.GenericObjectField(position, property, fieldInfo.FieldType, label);
        }
    }
}