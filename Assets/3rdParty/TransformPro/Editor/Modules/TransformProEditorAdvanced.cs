// Copyright(c) 2017 Untitled Games   | Developed by: Chris Bellini                    | http://untitledgam.es/contact
// http://transformpro.untitledgam.es | http://transformpro.untitledgam.es/quick-start | http://transformpro.untitledgam.es/api

namespace UntitledGames.Transforms
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     Draws the advanced editors when the toggles are opened.
    /// </summary>
    public static class TransformProEditorAdvanced
    {
        /// <summary>
        ///     Draws the position advanced editor GUI.
        /// </summary>
        public static void DrawPositionGUI()
        {
            TransformPro.SnapPositionGrid = TransformProPreferences.SnapPositionGrid;
            TransformPro.SnapPositionOrigin = TransformProPreferences.SnapPositionOrigin;
            TransformPro.SnapRotationGrid = TransformProPreferences.SnapRotationGrid;
            TransformPro.SnapRotationOrigin = TransformProPreferences.SnapRotationOrigin;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(GUIContent.none, GUILayout.Width(30));
            GUILayout.Label("Grid x1", TransformProStyles.LabelSmall);
            GUILayout.Label("Grid x5", TransformProStyles.LabelSmall);
            GUILayout.Label("Grid x10", TransformProStyles.LabelSmall);
            Vector3[] amounts =
            {
                TransformPro.SnapPositionGrid, TransformPro.SnapPositionGrid * 5, TransformPro.SnapPositionGrid * 10
            };
            Vector3[] axisVectors = {Vector3.right, Vector3.up, Vector3.forward};
            GUILayout.Label(GUIContent.none, GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();

            GUI.enabled = TransformPro.CanChangePosition;

            Vector3 position = TransformPro.Position;
            for (int axis = 0; axis < 3; axis++)
            {
                GUI.color = TransformProStyles.ColorAxis[axis];

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(((char) (axis + 88)).ToString(), GUILayout.Width(30));

                //foreach (Vector3 amount in amounts)
                for (int amountIndex = 0; amountIndex < amounts.Length; amountIndex++)
                {
                    Vector3 amount = amounts[amountIndex];
                    int grid = 1;
                    if (amountIndex == 1)
                    {
                        grid = 5;
                    }
                    else if (amountIndex == 2)
                    {
                        grid = 10;
                    }

                    string tooltip = string.Empty;
                    switch (axis)
                    {
                        case 0:
                            tooltip = string.Format(TransformProStrings.SystemLanguage.TooltipQuickPositionX, grid, amount.x);
                            break;
                        case 1:
                            tooltip = string.Format(TransformProStrings.SystemLanguage.TooltipQuickPositionY, grid, amount.y);
                            break;
                        case 2:
                            tooltip = string.Format(TransformProStrings.SystemLanguage.TooltipQuickPositionZ, grid, amount.z);
                            break;
                    }

                    for (int direction = -1; direction <= 1; direction += 2)
                    {
                        GUIContent label = new GUIContent(direction < 0 ? "-" : "+", tooltip);
                        GUIStyle style = direction < 0 ? TransformProStyles.Buttons.Standard.Left : TransformProStyles.Buttons.Standard.Right;
                        if (GUILayout.Button(label, style))
                        {
                            if (Event.current.shift)
                            {
                                // Because the TransformPro focus changes, the position will apply to the new object.
                                Transform newTransform = TransformPro.Clone();
                                Undo.RegisterCreatedObjectUndo(newTransform.gameObject, "TransformPro Quick Update (Clone)");
                                Selection.activeTransform = newTransform;
                            }
                            else
                            {
                                Undo.RecordObject(TransformPro.Transform, "TransformPro Quick Update");
                            }

                            position += amount[axis] * direction * axisVectors[axis];
                        }
                    }
                }

                string tooltipReset = string.Empty;
                switch (axis)
                {
                    case 0:
                        tooltipReset = TransformProStrings.SystemLanguage.TooltipResetPositionX;
                        break;
                    case 1:
                        tooltipReset = TransformProStrings.SystemLanguage.TooltipResetPositionY;
                        break;
                    case 2:
                        tooltipReset = TransformProStrings.SystemLanguage.TooltipResetPositionZ;
                        break;
                }

                GUI.color = TransformProStyles.ColorClear;
                GUIContent resetContent = new GUIContent("0", tooltipReset);
                if (GUILayout.Button(resetContent, TransformProStyles.Buttons.Standard.Single, GUILayout.Width(20)))
                {
                    Undo.RecordObject(TransformPro.Transform, "TransformPro Reset");
                    position[axis] = 0;
                }

                EditorGUILayout.EndHorizontal();
            }

            GUI.color = Color.white;
            GUI.enabled = true;

            TransformPro.Position = position;
        }

        /// <summary>
        ///     Draws the rotation advanced editor GUI.
        /// </summary>
        public static void DrawRotationGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(GUIContent.none, GUILayout.Width(30));
            GUILayout.Label("30", TransformProStyles.LabelSmall);
            GUILayout.Label("45", TransformProStyles.LabelSmall);
            GUILayout.Label("90", TransformProStyles.LabelSmall);
            int[] amounts = {30, 45, 90};
            GUILayout.Label(GUIContent.none, GUILayout.Width(30));
            GUILayout.Label(GUIContent.none, GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();

            GUI.enabled = TransformPro.CanChangeRotation;
            Vector3 euler = TransformPro.RotationEuler;

            for (int axis = 0; axis < 3; axis++)
            {
                GUI.color = TransformProStyles.ColorAxis[axis];

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(((char) (axis + 88)).ToString(), GUILayout.Width(30));

                foreach (int amount in amounts)
                {
                    string tooltip = string.Empty;
                    switch (axis)
                    {
                        case 0:
                            tooltip = string.Format(TransformProStrings.SystemLanguage.TooltipQuickRotationX, amount);
                            break;
                        case 1:
                            tooltip = string.Format(TransformProStrings.SystemLanguage.TooltipQuickRotationY, amount);
                            break;
                        case 2:
                            tooltip = string.Format(TransformProStrings.SystemLanguage.TooltipQuickRotationZ, amount);
                            break;
                    }

                    for (int direction = -1; direction <= 1; direction += 2)
                    {
                        GUIContent label = new GUIContent(direction < 0 ? "-" : "+", tooltip);
                        GUIStyle style = direction < 0 ? TransformProStyles.Buttons.Standard.Left : TransformProStyles.Buttons.Standard.Right;
                        if (GUILayout.Button(label, style))
                        {
                            if (Event.current.shift)
                            {
                                // Because the TransformPro focus changes, the position will apply to the new object.
                                Transform newTransform = TransformPro.Clone();
                                Undo.RegisterCreatedObjectUndo(newTransform.gameObject, "TransformPro Quick Update (Clone)");
                                Selection.activeTransform = newTransform;
                            }
                            else
                            {
                                Undo.RecordObject(TransformPro.Transform, "TransformPro Quick Update");
                            }
                            euler[axis] += amount * direction;
                        }
                    }
                }

                string tooltipRnd = string.Empty;
                string tooltipReset = string.Empty;
                switch (axis)
                {
                    case 0:
                        tooltipRnd = TransformProStrings.SystemLanguage.TooltipResetRotationRndX;
                        tooltipReset = TransformProStrings.SystemLanguage.TooltipResetRotationX;
                        break;
                    case 1:
                        tooltipRnd = TransformProStrings.SystemLanguage.TooltipResetRotationRndY;
                        tooltipReset = TransformProStrings.SystemLanguage.TooltipResetRotationY;
                        break;
                    case 2:
                        tooltipRnd = TransformProStrings.SystemLanguage.TooltipResetRotationRndZ;
                        tooltipReset = TransformProStrings.SystemLanguage.TooltipResetRotationZ;
                        break;
                }

                GUI.color = Color.white;
                GUIContent randomLabel = new GUIContent(TransformProStyles.Icons.Random, tooltipRnd);
                if (GUILayout.Button(randomLabel, TransformProStyles.Buttons.Icon.Single, GUILayout.Width(25)))
                {
                    Undo.RecordObject(TransformPro.Transform, "TransformPro Randomise");
                    euler[axis] = (float) TransformProEditor.Random.NextDouble() * 360;
                }

                GUI.color = TransformProStyles.ColorClear;
                GUIContent resetLabel = new GUIContent("0", tooltipReset);
                if (GUILayout.Button(resetLabel, TransformProStyles.Buttons.Standard.Single, GUILayout.Width(20)))
                {
                    Undo.RecordObject(TransformPro.Transform, "TransformPro Reset");
                    euler[axis] = 0;
                }

                EditorGUILayout.EndHorizontal();
            }

            GUI.color = Color.white;
            GUI.enabled = true;

            TransformPro.RotationEuler = euler;
        }
    }
}