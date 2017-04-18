// Copyright(c) 2017 Untitled Games   | Developed by: Chris Bellini                    | http://untitledgam.es/contact
// http://transformpro.untitledgam.es | http://transformpro.untitledgam.es/quick-start | http://transformpro.untitledgam.es/api

namespace UntitledGames.Transforms
{
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     Draws the 3 main Vector3 based editor controls.
    /// </summary>
    public static class TransformProEditorVector3
    {
        /// <summary>
        ///     Draws the main Vector3 fields.
        ///     TODO: Replace this with a seperate module system to allow for the differences more easily.
        /// </summary>
        /// <param name="label">The label for the Vector3. (Position, Rotation or Scale)</param>
        /// <param name="value">The current value to display.</param>
        /// <param name="enabled">Can you edit this Vector 3 currently?</param>
        /// <param name="foldout">Is the advanced panel opened?</param>
        /// <param name="reset">An action to execute if the reset button is pressed.</param>
        /// <param name="resetLabel">The label to display on the reset button. (0 or 1)</param>
        /// <param name="resetTooltip">The tooltip to display on the reset button.</param>
        /// <returns>
        ///     A struct containing the updated vector along with flags to show if the value has been changed or the advanced
        ///     panel opened.
        /// </returns>
        public static Data Draw(string label, Vector3 value, bool enabled, bool foldout, Action reset, string resetLabel, string resetTooltip)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            EditorGUI.BeginChangeCheck();
            GUI.enabled = enabled;
            rect.width -= 24;

            Rect labelRect = new Rect(rect) {width = 60};
            GUI.Label(labelRect, label);
            Rect controlRect = new Rect(rect) {x = labelRect.xMax};
            controlRect.width -= labelRect.width;
            value = EditorGUI.Vector3Field(controlRect, GUIContent.none, value);
            bool changed = false;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(TransformPro.Transform, string.Format("TransformPro {0}", label));
                changed = true;
            }

            Rect resetRect = rect;
            resetRect.xMin = resetRect.xMax + 4;
            resetRect.width = 20;

            GUI.color = TransformProStyles.ColorClear;
            GUIContent resetLabelContent = new GUIContent(resetLabel, resetTooltip);
            if (GUI.Button(resetRect, resetLabelContent, TransformProStyles.Buttons.Standard.Single))
            {
                Undo.RecordObject(TransformPro.Transform, string.Format("TransformPro Reset {0}", label));
                reset();
            }
            GUI.color = Color.white;
            GUI.enabled = true;
            foldout = EditorGUI.Foldout(rect, foldout, "");

            return new Data(value, changed, foldout);
        }

        public static void DrawScale(Vector3 value)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            GUI.enabled = TransformPro.CanChangeScale;
            rect.width -= 24;

            Rect labelRect = new Rect(rect) {width = 44};
            GUI.Label(labelRect, "Scale");

            float valueSingle = (value.x + value.y + value.z) / 3.0f;
            bool single = TransformProEditor.SingleScale;
            if (!Mathf.Approximately(value.x, value.y) || !Mathf.Approximately(value.x, value.z))
            {
                single = false;
                GUI.color = Color.gray;
            }
            else
            {
                GUI.color = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            }

            Rect singleRect = new Rect(labelRect) {x = labelRect.xMax, width = 16};
            GUIContent singleContent = new GUIContent(single ? TransformProStyles.Icons.EditVector1 : TransformProStyles.Icons.EditVector3);
            EditorGUI.BeginChangeCheck();
            single = GUI.Toggle(singleRect, single, singleContent, GUIStyle.none);
            if (EditorGUI.EndChangeCheck())
            {
                TransformProEditor.SingleScale = single;
            }
            GUI.color = Color.white;

            Rect controlRect = new Rect(rect) {x = singleRect.xMax};
            controlRect.width -= labelRect.width + singleRect.width;
            EditorGUI.BeginChangeCheck();
            if (single)
            {
                controlRect.x += 13;
                controlRect.width -= 13;
                valueSingle = EditorGUI.FloatField(controlRect, GUIContent.none, valueSingle);
                value = new Vector3(valueSingle, valueSingle, valueSingle);
            }
            else
            {
                value = EditorGUI.Vector3Field(controlRect, GUIContent.none, value);
            }
            bool changed = false;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Scale");
                changed = true;
            }

            Rect resetRect = rect;
            resetRect.xMin = resetRect.xMax + 4;
            resetRect.width = 20;

            GUI.color = TransformProStyles.ColorClear;
            GUIContent resetLabelContent = new GUIContent("1", TransformProStrings.SystemLanguage.TooltipResetScale);
            if (GUI.Button(resetRect, resetLabelContent, TransformProStyles.Buttons.Standard.Single))
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Reset Scale");
                value = Vector3.one;
                changed = true;
            }
            GUI.color = Color.white;
            GUI.enabled = true;

            //TransformProEditor.AdvancedScale = EditorGUI.Foldout(rect, TransformProEditor.AdvancedScale, "");

            if (changed)
            {
                TransformPro.Scale = value;
            }
        }


        /// <summary>
        ///     A struct containing the data for one of the main Vector3 values.
        /// </summary>
        public struct Data
        {
            private readonly Vector3 value;
            private readonly bool changed;
            private readonly bool foldout;

            /// <summary>
            ///     Creates a new instance of the <see cref="Data" /> struct.
            /// </summary>
            /// <param name="value">The current value of the <see cref="Vector3" />.</param>
            /// <param name="changed">A value indicating whether the <see cref="Vector3" /> was changed this frame.</param>
            /// <param name="foldout">A value indicating whether the advanced panel is open.</param>
            public Data(Vector3 value, bool changed, bool foldout)
            {
                this.value = value;
                this.changed = changed;
                this.foldout = foldout;
            }

            /// <summary>
            ///     The current value of the <see cref="Vector3" />.
            /// </summary>
            public Vector3 Value { get { return this.value; } }

            /// <summary>
            ///     A value indicating whether the <see cref="Vector3" /> was changed this frame.
            /// </summary>
            public bool Foldout { get { return this.foldout; } }

            /// <summary>
            ///     A value indicating whether the advanced panel is open.
            /// </summary>
            public bool Changed { get { return this.changed; } }
        }
    }
}