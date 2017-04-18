// Copyright(c) 2017 Untitled Games   | Developed by: Chris Bellini                    | http://untitledgam.es/contact
// http://transformpro.untitledgam.es | http://transformpro.untitledgam.es/quick-start | http://transformpro.untitledgam.es/api

namespace UntitledGames.Transforms
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     Handles the Clipboard Editor GUI.
    /// </summary>
    public static class TransformProEditorClipboard
    {
        private const int ButtonWidth = 27;
        private static Rect rectLookAt;

        /// <summary>
        ///     Draws the Copy / Paste Editor GUI.
        /// </summary>
        public static void DrawGUI()
        {
            GUILayout.BeginHorizontal();

            GUI.color = Event.current.shift ? TransformProStyles.ColorPaste : TransformProStyles.ColorCopy;

            GUIContent contentTransform = new GUIContent(TransformProStyles.Icons.Clipboard, TransformProStrings.SystemLanguage.TooltipCopyPasteTransform);
            if (GUILayout.Button(contentTransform, TransformProStyles.Buttons.IconLarge.Left, GUILayout.Width(TransformProEditorClipboard.ButtonWidth)))
            {
                if (Event.current.shift || (Event.current.button == 1))
                {
                    if (TransformPro.CanChangePosition || TransformPro.CanChangeRotation || TransformPro.CanChangeScale)
                    {
                        Undo.RecordObject(TransformPro.Transform, "TransformPro Paste Transform");
                        TransformPro.PastePosition();
                        TransformPro.PasteRotation();
                        TransformPro.PasteScale();
                    }
                    else
                    {
                        Debug.LogWarning("[<color=red>TransformPro</color>] Could not paste full transform.");
                    }
                }
                else
                {
                    TransformPro.CopyPosition();
                    TransformPro.CopyRotation();
                    TransformPro.CopyScale();
                }
            }

            GUIContent contentPosition = new GUIContent(TransformProStyles.Icons.Position, TransformProStrings.SystemLanguage.TooltipCopyPastePosition);
            if (GUILayout.Button(contentPosition, TransformProStyles.Buttons.IconLarge.Middle, GUILayout.Width(TransformProEditorClipboard.ButtonWidth)))
            {
                if (Event.current.shift || (Event.current.button == 1))
                {
                    if (TransformPro.CanChangePosition)
                    {
                        Undo.RecordObject(TransformPro.Transform, "TransformPro Paste Position");
                        TransformPro.PastePosition();
                    }
                    else
                    {
                        Debug.LogWarning("[<color=red>TransformPro</color>] Could not paste position.");
                    }
                }
                else
                {
                    TransformPro.CopyPosition();
                }
            }

            GUIContent contentRotation = new GUIContent(TransformProStyles.Icons.Rotation, TransformProStrings.SystemLanguage.TooltipCopyPasteRotation);
            if (GUILayout.Button(contentRotation, TransformProStyles.Buttons.IconLarge.Middle, GUILayout.Width(TransformProEditorClipboard.ButtonWidth)))
            {
                if (Event.current.shift || (Event.current.button == 1))
                {
                    if (TransformPro.CanChangeRotation)
                    {
                        Undo.RecordObject(TransformPro.Transform, "TransformPro Paste Rotate");
                        TransformPro.PasteRotation();
                    }
                    else
                    {
                        Debug.LogWarning("[<color=red>TransformPro</color>] Could not paste rotation.");
                    }
                }
                else
                {
                    TransformPro.CopyRotation();
                }
            }

            GUIContent contentScale = new GUIContent(TransformProStyles.Icons.Scale, TransformProStrings.SystemLanguage.TooltipCopyPasteScale);
            if (GUILayout.Button(contentScale, TransformProStyles.Buttons.IconLarge.Right, GUILayout.Width(TransformProEditorClipboard.ButtonWidth)))
            {
                if (Event.current.shift || (Event.current.button == 1))
                {
                    if (TransformPro.CanChangeScale)
                    {
                        Undo.RecordObject(TransformPro.Transform, "TransformPro Paste Scale");
                        TransformPro.PasteScale();
                    }
                    else
                    {
                        Debug.LogWarning("[<color=red>TransformPro</color>] Could not paste scale.");
                    }
                }
                else
                {
                    TransformPro.CopyScale();
                }
            }

            GUI.color = TransformProStyles.ColorPaste;

            GUIContent contentLookAt = new GUIContent(TransformProStyles.Icons.LookAt, TransformProStrings.SystemLanguage.TooltipLookAt);
            TransformProEditorClipboard.rectLookAt = EditorGUILayout.GetControlRect(false, GUILayout.Width(TransformProEditorClipboard.ButtonWidth));
            if (GUI.Button(TransformProEditorClipboard.rectLookAt, contentLookAt, TransformProStyles.Buttons.IconLarge.Single))
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Look At");
                TransformPro.LookAtClipboard();
            }

            GUILayout.EndHorizontal();

            GUI.color = Color.white;
        }
    }
}