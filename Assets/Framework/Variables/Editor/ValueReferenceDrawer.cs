#nullable enable
using UnityEditor;
using UnityEngine;

namespace Framework.Variables.Editor {
    [CustomPropertyDrawer(typeof(ValueReference<>), true)]
    public class ValueReferenceDrawer : PropertyDrawer {
        private readonly string[] _popupOptions = { "Use Constant", "Use Variable" };
        private GUIStyle? _popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            _popupStyle ??= new GUIStyle(GUI.skin.GetStyle("PaneOptions")) {
                imagePosition = ImagePosition.ImageOnly
            };

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            var useConstantProperty = property.FindPropertyRelative("useConstant");
            var constantProperty = property.FindPropertyRelative("constant");
            var variableProperty = property.FindPropertyRelative("variable");

            var buttonRect = new Rect(position);
            buttonRect.yMin += _popupStyle.margin.top;
            buttonRect.width = _popupStyle.fixedWidth + _popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var result = EditorGUI.Popup(buttonRect, useConstantProperty.boolValue ? 0 : 1, _popupOptions, _popupStyle);
            useConstantProperty.boolValue = result == 0;

            EditorGUI.PropertyField(position, useConstantProperty.boolValue ? constantProperty : variableProperty,
                GUIContent.none);

            if (EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}