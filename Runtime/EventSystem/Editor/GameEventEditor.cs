#if UNITY_EDITOR
namespace PushForward.EventSystem.Editor
{
    using UnityEditor;
    using UnityEngine;
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
    public class EventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameEvent gameEvent = this.target as GameEvent;
            if (GUILayout.Button("Raise") && gameEvent != null)
            { gameEvent.Raise(); }
        }
    }
}
#endif