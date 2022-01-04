using Tuntenfisch.Commons.Coupling.Scriptables.Editor;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Coupling.Scriptables.Events.Editor
{
    [CustomEditor(typeof(GameEvent))]
    public class EventEditor : ScriptableEditor
    {
        #region Private Variables
        private SerializedProperty m_onGameEventInvokedProperty;
        #endregion

        #region Protected Methods
        protected override void DisplayProperties()
        {
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            GameEvent gameEvent = target as GameEvent;

            if (GUILayout.Button(ObjectNames.NicifyVariableName(nameof(gameEvent.InvokeGameEvent))))
            {
                gameEvent.InvokeGameEvent();
            }
            EditorGUI.EndDisabledGroup();
        }
        #endregion
    }
}