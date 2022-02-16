using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Audio.Editor
{
    [CustomEditor(typeof(AudioClipCollection), editorForChildClasses: true)]
    public class AudioClipCollectionEditor : UnityEditor.Editor
    {
        #region Private Fields
        [SerializeField]
        private AudioSource m_audioPreviewSource;
        #endregion

        #region Unity Callbacks
        public void OnEnable()
        {
            m_audioPreviewSource = EditorUtility.CreateGameObjectWithHideFlags("Audio Preview Source", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        }

        public void OnDisable()
        {
            DestroyImmediate(m_audioPreviewSource.gameObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

            if (GUILayout.Button("Play"))
            {
                ((AudioClipCollection)target).Play(m_audioPreviewSource);
            }
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}