using System.Collections.Generic;
using System.Linq;
using Tuntenfisch.Commons.Coupling.Scriptables.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Editor
{
    public abstract class ScriptableEditor : UnityEditor.Editor
    {
        // Workaround to get width. Taken from https://forum.unity.com/threads/editorguilayout-get-width-of-inspector-window-area.82068/#post-7155460.
        private float Width
        {
            get
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                if (Event.current.type == EventType.Repaint)
                {
                    m_width = GUILayoutUtility.GetLastRect().width;
                }
                return m_width;
            } 
        }

        private Dictionary<GameObject, Reference> m_references = new Dictionary<GameObject, Reference>();
        private GUIContent m_hasAccessFlagIcon;
        private GUIStyle m_centeredBoldLabelStyle;
        private float m_width;

        protected virtual void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            m_hasAccessFlagIcon = EditorGUIUtility.isProSkin ? EditorGUIUtility.IconContent("d_Valid@2x") : EditorGUIUtility.IconContent("Valid@2x");
        }

        protected virtual void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Commons.Editor.EditorGUI.ScriptHeaderField(serializedObject);
            DisplayProperties();
            DisplayFindInSceneButton();
            DisplayReferences();
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void DisplayProperties();

        private void OnSceneGUI(SceneView sceneView)
        {
            foreach ((GameObject gameObject, Reference reference) in from pair in m_references where pair.Key != null select pair)
            {
                Commons.Editor.Handles.ColoredLabel(gameObject.transform.position, $"<b>{gameObject.name}</b>\n<i>{reference.AccessFlags} Access</i>", reference.LabelColor);
            }
        }

        protected void DisplayFindInSceneButton()
        {
            if (GUILayout.Button($"Find {ObjectNames.NicifyVariableName(nameof(GameObject))}s in Scene referencing {target.name}"))
            {
                FindVariableReferences();
                SceneView.RepaintAll();
            }
        }

        private void FindVariableReferences()
        {
            m_references.Clear();

            foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                FindSceneReferencesRecursive(rootGameObject);
            }
        }

        private void FindSceneReferencesRecursive(GameObject parentGameObject)
        {
            foreach (Component component in parentGameObject.GetComponents<Component>())
            {
                SerializedProperty property = new SerializedObject(component).GetIterator();

                while (property.NextVisible(true))
                {
                    if (property.propertyType != SerializedPropertyType.ObjectReference || property.objectReferenceValue != target)
                    {
                        continue;
                    }

                    AccessFlags accessFlags = GetAccessFlags(property);

                    if (m_references.TryGetValue(parentGameObject, out Reference variableReference))
                    {
                        variableReference.AccessFlags |= accessFlags;
                    }
                    else
                    {
                        m_references.Add(parentGameObject, new Reference { AccessFlags = accessFlags });
                    }
                }
            }

            foreach (Transform childTransform in parentGameObject.transform)
            {
                FindSceneReferencesRecursive(childTransform.gameObject);
            }
        }

        protected AccessFlags GetAccessFlags(SerializedProperty property)
        {
            AccessHintAttribute accessHintAttribute = Commons.Editor.EditorGUI.GetAttribute<AccessHintAttribute>(property);

            return accessHintAttribute != null ? accessHintAttribute.AccessFlags : AccessFlags.Unkown;
        }

        protected void DisplayReferences()
        {
            if (m_references.Count != 0)
            {
                if (m_centeredBoldLabelStyle == null)
                {
                    m_centeredBoldLabelStyle = new GUIStyle(GUI.skin.label);
                    m_centeredBoldLabelStyle.alignment = TextAnchor.MiddleCenter;
                    m_centeredBoldLabelStyle.fontStyle = FontStyle.Bold;
                }
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayoutOption smallColumnWidth = GUILayout.Width(0.25f * Width);
                GUILayoutOption largeColumnWidth = GUILayout.Width(0.5f * Width);
                GUILayoutOption rowHeight = GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(ObjectNames.NicifyVariableName(nameof(GameObject)), m_centeredBoldLabelStyle, largeColumnWidth, rowHeight);
                GUILayout.Label(nameof(AccessFlags.Read), m_centeredBoldLabelStyle, smallColumnWidth, rowHeight);
                GUILayout.Label(nameof(AccessFlags.Write), m_centeredBoldLabelStyle, smallColumnWidth, rowHeight);
                EditorGUILayout.EndHorizontal();

                foreach ((GameObject gameObject, Reference reference) in from pair in m_references where pair.Key != null select pair)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(GUIContent.none, gameObject, typeof(GameObject), false, largeColumnWidth, rowHeight);
                    GUILayout.Label((reference.AccessFlags & AccessFlags.Read) == AccessFlags.Read ? m_hasAccessFlagIcon : GUIContent.none, m_centeredBoldLabelStyle, smallColumnWidth, rowHeight);
                    GUILayout.Label((reference.AccessFlags & AccessFlags.Write) == AccessFlags.Write ? m_hasAccessFlagIcon : GUIContent.none, m_centeredBoldLabelStyle, smallColumnWidth, rowHeight);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
        }

        private class Reference
        {
            public AccessFlags AccessFlags
            {
                get => m_accessFlags;

                set
                {
                    m_labelColor = GetLabelColor(value);
                    m_accessFlags = value;
                }
            }

            public Color LabelColor => m_labelColor;

            private AccessFlags m_accessFlags;
            private Color m_labelColor;

            private Color GetLabelColor(AccessFlags accessFlags)
            {
                return accessFlags switch
                {
                    AccessFlags.Read => new Color(1.0f, 0.0f, 0.0f, 0.75f),
                    AccessFlags.Write => new Color(1.0f, 0.65f, 0.0f, 0.75f),
                    _ => new Color(0.0f, 1.0f, 0.0f, 0.75f),
                };
            }
        }
    }
}