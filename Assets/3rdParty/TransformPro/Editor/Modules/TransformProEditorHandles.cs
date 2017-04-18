namespace UntitledGames.Transforms
{
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     Draws overriden pivots to the scene GUI.
    /// </summary>
    public static class TransformProEditorHandles
    {
        private static Quaternion helperRotation = Quaternion.identity;

        //private static Vector3 helperRotationEulerNew;
        //private static Vector3 helperRotationEulerOld;
        private static string[] optionsPivot;

        private static string[] optionsSpace;

        public static Quaternion HelperRotation { get { return TransformProEditorHandles.helperRotation; } }

        public static string[] OptionsPivot { get { return TransformProEditorHandles.optionsPivot ?? (TransformProEditorHandles.optionsPivot = Enum.GetNames(typeof(TransformProPivot))); } }

        public static string[] OptionsSpace { get { return TransformProEditorHandles.optionsSpace ?? (TransformProEditorHandles.optionsSpace = Enum.GetNames(typeof(TransformProSpace))); } }

        public static void DrawGUI()
        {
            TransformProEditorHandles.DrawSpaceGUI();
            TransformProEditorHandles.DrawPivotGUI();
        }

        public static void DrawHandles()
        {
            switch (Tools.current)
            {
                default:
                    Tools.hidden = false;
                    break;
                /*
            case Tool.Move:
                Tools.hidden = true;
                Vector3 handlePosition = TransformPro.HandlePosition;
                EditorGUI.BeginChangeCheck();
                handlePosition = Handles.DoPositionHandle(handlePosition, TransformPro.HandleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(TransformPro.Transform, "TransformPro Move");
                    TransformPro.PositionWorld = handlePosition;
                }
                break;
            case Tool.Rotate:
                Tools.hidden = true;
                TransformProEditorHandles.helperRotation = TransformPro.HandleRotation;
                EditorGUI.BeginChangeCheck();
                TransformProEditorHandles.helperRotation = Handles.DoRotationHandle(TransformProEditorHandles.helperRotation, TransformPro.HandlePosition);
                if ((Event.current.type == EventType.Used) && (TransformProEditorHandles.helperRotation == TransformPro.HandleRotation))
                {
                    TransformProEditorHandles.helperRotationEulerOld = Vector3.zero;
                }
                if (EditorGUI.EndChangeCheck())
                {
                    if (TransformProEditorHandles.helperRotation != TransformPro.HandleRotation)
                    {
                        Undo.RecordObject(TransformPro.Transform, "TransformPro Rotate");
                        TransformProEditorHandles.helperRotationEulerNew = TransformProEditorHandles.helperRotation.eulerAngles;
                        Vector3 eulerDelta = TransformProEditorHandles.helperRotationEulerNew - TransformProEditorHandles.helperRotationEulerOld;
                        if (TransformPro.Space == TransformProSpace.Local)
                        {
                            TransformPro.RotationWorld = TransformProEditorHandles.helperRotation;
                        }
                        else
                        {
                            TransformPro.Transform.Rotate(TransformPro.HandleRotation * Vector3.right, eulerDelta.x, Space.World);
                            TransformPro.Transform.Rotate(TransformPro.HandleRotation * Vector3.up, eulerDelta.y, Space.World);
                            TransformPro.Transform.Rotate(TransformPro.HandleRotation * Vector3.forward, eulerDelta.z, Space.World);
                        }
                        TransformProEditorHandles.helperRotationEulerOld = TransformProEditorHandles.helperRotationEulerNew;
                    }
                }
                break;
                */
            }
        }

        public static void DrawPivotGUI()
        {
            GUI.color = TransformProStyles.ColorSpace;
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            GUI.enabled = TransformPro.Pivot != TransformProPivot.Object;
            GUIContent centerContent = new GUIContent(TransformProStyles.Icons.GameObject);
            if (GUILayout.Button(centerContent, TransformProStyles.Buttons.Icon.Left, GUILayout.Width(20)))
            {
                TransformPro.Pivot = TransformProPivot.Object;
            }
            /*
            GUI.enabled = TransformPro.Pivot != TransformProPivot.Center;
            GUIContent pivotContent = new GUIContent(TransformProStyles.Icons.ToolHandleCenter);
            if (GUILayout.Button(pivotContent, TransformProStyles.Buttons.Icon.Middle, GUILayout.Width(20)))
            {
                TransformPro.Pivot = TransformProPivot.Center;
            }
            */

            GUI.enabled = true;
            EditorGUILayout.Popup((int) TransformPro.Pivot, TransformProEditorHandles.OptionsPivot, TransformProStyles.PopupRight);

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }

            EditorGUILayout.EndHorizontal();

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.y -= 16;
            GUI.color = TransformProStyles.ColorLabelSubtle;
            GUI.Label(rect, "Pivot", TransformProStyles.LabelSmall);
            GUI.color = Color.white;
        }

        public static void DrawSpaceGUI()
        {
            GUI.color = TransformProStyles.ColorSpace;
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            GUI.enabled = TransformPro.Space != TransformProSpace.Local;
            GUIContent localContent = new GUIContent(TransformProStyles.Icons.ToolHandleLocal, TransformProStrings.SystemLanguage.TooltipSpaceLocal);
            if (GUILayout.Button(localContent, TransformProStyles.Buttons.Icon.Left, GUILayout.Width(20)))
            {
                TransformPro.Space = TransformProSpace.Local;
                TransformProEditorHandles.UpdatePivot();
            }
            GUI.enabled = TransformPro.Space != TransformProSpace.World;
            GUIContent worldContent = new GUIContent(TransformProStyles.Icons.ToolHandleGobal, TransformProStrings.SystemLanguage.TooltipSpaceWorld);
            if (GUILayout.Button(worldContent, TransformProStyles.Buttons.Icon.Middle, GUILayout.Width(20)))
            {
                TransformPro.Space = TransformProSpace.World;
                TransformProEditorHandles.UpdatePivot();
            }

            GUI.enabled = true;
            TransformPro.Space = (TransformProSpace) EditorGUILayout.Popup((int) TransformPro.Space, TransformProEditorHandles.OptionsSpace, TransformProStyles.PopupRight);

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }

            EditorGUILayout.EndHorizontal();

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.y -= 16;
            GUI.color = TransformProStyles.ColorLabelSubtle;
            GUI.Label(rect, "Space", TransformProStyles.LabelSmall);
            GUI.color = Color.white;
        }

        public static void UpdatePivot()
        {
            // LOOK: THESE ARE NOT THE SAME THING
            // We should be able top control the scene handles and the transform display independantly.
            // Simply STOP DOING THIS
            // (this only works one way)
            switch (TransformPro.Space)
            {
                default:
                    break;
                /* 
            case TransformProSpace.World:
                Tools.pivotRotation = PivotRotation.Global;
                break;
            case TransformProSpace.Local:
                Tools.pivotRotation = PivotRotation.Local;
                break;
                */
            }
        }
    }
}
