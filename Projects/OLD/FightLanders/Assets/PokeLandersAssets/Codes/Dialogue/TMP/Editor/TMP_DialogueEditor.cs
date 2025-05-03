using UnityEditor;
using UnityEngine;
using TMPro.EditorUtilities;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(TMP_Dialogue), true)]
    [CanEditMultipleObjects]
    public class TMP_DialogueEditor : TMP_EditorPanelUI
    {
        SerializedProperty speedProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            speedProp = serializedObject.FindProperty("speed");
        }

        protected override void OnUndoRedo() { }

        protected override void DrawExtraSettings()
        {
            DrawDialogueSettings();

            base.DrawExtraSettings();
        }

        private void DrawDialogueSettings()
        {
            EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(speedProp, new GUIContent("     Default Speed"));
            GUILayout.Space(10);
        }

        protected override bool IsMixSelectionTypes()
        {
            return false;
        }
    }
}
