#if UNITY_EDITOR
namespace PushForward.ScriptableObjects.Primitives.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferenceDrawer : PropertyDrawer
    {
        /// <summary>Options to display in the popup to select constant or variable.</summary>
        private readonly string[] popupOptions = { "Use Override", "Use Variable" };

        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.popupStyle ??= new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
                                    { imagePosition = ImagePosition.ImageOnly };

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty useOverride = property.FindPropertyRelative("useOverride");
            SerializedProperty overrideValue = property.FindPropertyRelative("overrideValue");
            SerializedProperty variable = property.FindPropertyRelative("variable");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += this.popupStyle.margin.top;
            buttonRect.width = this.popupStyle.fixedWidth + this.popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, useOverride.boolValue ? 0 : 1, this.popupOptions, this.popupStyle);

            useOverride.boolValue = result == 0;

            EditorGUI.PropertyField(position, useOverride.boolValue ? overrideValue : variable, GUIContent.none);

            if (EditorGUI.EndChangeCheck())
            { property.serializedObject.ApplyModifiedProperties(); }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
#endif