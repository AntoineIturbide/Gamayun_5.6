// Copyright(c) 2017 Untitled Games   | Developed by: Chris Bellini                    | http://untitledgam.es/contact
// http://transformpro.untitledgam.es | http://transformpro.untitledgam.es/quick-start | http://transformpro.untitledgam.es/api

namespace UntitledGames.Transforms
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     Handles the Snapping Editor GUI.
    /// </summary>
    public static class TransformProEditorSnapping
    {
        private const int ButtonWidth = 27;

        /// <summary>
        ///     Draw the Snap GUI.
        ///     TODO: Seperate this into a seperate module.
        /// </summary>
        public static void DrawGUI()
        {
            GUILayout.BeginHorizontal();

            GUI.color = TransformProStyles.ColorSnap;

            GUI.enabled = TransformPro.CanChangePosition && TransformPro.CanChangeRotation;
            GUIContent snapTransformContent = new GUIContent(TransformProStyles.Icons.Snap, TransformProStrings.SystemLanguage.TooltipSnapTransform);
            if (GUILayout.Button(snapTransformContent, TransformProStyles.Buttons.IconLarge.Left, GUILayout.Width(TransformProEditorSnapping.ButtonWidth)))
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Snap");
                TransformPro.SnapPositionGrid = TransformProPreferences.SnapPositionGrid;
                TransformPro.SnapRotationGrid = TransformProPreferences.SnapRotationGrid;
                TransformPro.Snap();
            }

            GUI.enabled = TransformPro.CanChangePosition;
            GUIContent snapPositionContent = new GUIContent(TransformProStyles.Icons.Position, TransformProStrings.SystemLanguage.TooltipSnapPosition);
            if (GUILayout.Button(snapPositionContent, TransformProStyles.Buttons.IconLarge.Middle, GUILayout.Width(TransformProEditorSnapping.ButtonWidth)))
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Snap Position");
                TransformPro.SnapPositionGrid = TransformProPreferences.SnapPositionGrid;
                TransformPro.Position = TransformPro.SnapPosition(TransformPro.Position);
            }

            GUI.enabled = TransformPro.CanChangePosition;
            GUIContent snapRotationContent = new GUIContent(TransformProStyles.Icons.Rotation, TransformProStrings.SystemLanguage.TooltipSnapRotation);
            if (GUILayout.Button(snapRotationContent, TransformProStyles.Buttons.IconLarge.Right, GUILayout.Width(TransformProEditorSnapping.ButtonWidth)))
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Snap Rotation");
                TransformPro.SnapRotationGrid = TransformProPreferences.SnapRotationGrid;
                TransformPro.RotationEuler = TransformPro.SnapRotationEuler(TransformPro.RotationEuler);
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();
        }
    }
}