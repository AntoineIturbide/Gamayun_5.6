// Copyright(c) 2017 Untitled Games   | Developed by: Chris Bellini                    | http://untitledgam.es/contact
// http://transformpro.untitledgam.es | http://transformpro.untitledgam.es/quick-start | http://transformpro.untitledgam.es/api

namespace UntitledGames.Transforms
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     Handles the Grounding Editor GUI.
    /// </summary>
    public static class TransformProEditorGrounding
    {
        private const int ButtonWidth = 27;

        /// <summary>
        ///     Draw the GUI for the Grounding module.
        ///     TODO: Seperate this into a seperate module.
        /// </summary>
        public static void DrawGUI()
        {
            GUILayout.BeginHorizontal();

            GUI.color = Color.white;
            GUI.enabled = TransformPro.CanChangePosition && TransformPro.CalculateBounds;

            GUIContent dropLabel = new GUIContent(TransformProStyles.Icons.Drop, TransformProStrings.SystemLanguage.TooltipDrop);
            if (GUILayout.Button(dropLabel, TransformProStyles.Buttons.IconLarge.Left, GUILayout.Width(TransformProEditorGrounding.ButtonWidth)))
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Drop");
                if (!TransformPro.Drop())
                {
                    Debug.LogWarning("[<color=red>TransformPro</color>] Grounding attempt failed.\nCould not find a collider underneath the transform.");
                }
            }

            GUIContent groundLabel = new GUIContent(TransformProStyles.Icons.Ground, TransformProStrings.SystemLanguage.TooltipGround);
            if (GUILayout.Button(groundLabel, TransformProStyles.Buttons.IconLarge.Right, GUILayout.Width(TransformProEditorGrounding.ButtonWidth)))
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Ground");
                if (!TransformPro.Ground())
                {
                    Debug.LogWarning("[<color=red>TransformPro</color>] Grounding attempt failed.\nCould not find a collider underneath the transform.");
                }
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();
        }
    }
}