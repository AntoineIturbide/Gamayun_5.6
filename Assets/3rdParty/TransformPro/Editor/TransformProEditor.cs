namespace UntitledGames.Transforms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using Random = System.Random;
    using UnityObject = UnityEngine.Object;

    /// <summary>
    ///     Extends the default Unity Transform inspector to add extra features.
    ///     More information can be found at http://transformpro.untitledgam.es
    /// </summary>
//[CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    public class TransformProEditor : Editor
    {
        /// <summary>
        ///     The random number provider for the rotation randomisation.
        /// </summary>
        private static Random random;

        /// <summary>
        ///     Should the scale edit 3 seperate axis or a single value
        /// </summary>
        private static bool singleScale = true;

#if !UNITY_5_3_OR_NEWER
        private IEnumerable<UnityObject> previousSelection;
#endif

        public static Camera Camera
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    if (SceneView.currentDrawingSceneView != null)
                    {
                        if (SceneView.currentDrawingSceneView.camera != null)
                        {
                            return SceneView.currentDrawingSceneView.camera;
                        }
                    }
                }
#endif
                return Camera.main;
            }
        }

        /// <summary>
        ///     A <see cref="System.Random" /> instance used to set random data in the editor.
        ///     <see cref="UnityEngine.Random" /> is not used to avoid polluting and procedural generation that may be being used
        ///     elsewhere.
        /// </summary>
        public static Random Random { get { return TransformProEditor.random ?? (TransformProEditor.random = new Random(DateTime.Now.GetHashCode())); } }

        /// <summary>
        ///     Should the scale edit 3 seperate axis or a single value
        /// </summary>
        public static bool SingleScale { get { return TransformProEditor.singleScale; } set { TransformProEditor.singleScale = value; } }

        /// <summary>
        ///     Shows a form containing the preferences UI. Doesn't use the default Editor preferences due to being unable to
        ///     reflect the internal structs used to define preferences tabs.
        /// </summary>
        public static void ShowPreferences()
        {
            TransformProPreferences preferences = EditorWindow.GetWindow<TransformProPreferences>();
            preferences.Show();
        }

        /// <summary>
        ///     Draw the Inspector. This is currently static and paramterless due to the static design of TransformPro - this is
        ///     likely to change in a future version.
        /// </summary>
        private static void InspectorGUI()
        {
            TransformProStyles.Load();

            GUISkin resetSkin = GUI.skin;
            GUI.skin = TransformProStyles.Skin;

            EditorGUILayout.BeginHorizontal();

            // Preferences cog (move to prefs class?
            GUIContent preferencesContent = new GUIContent(TransformProStyles.Icons.Cog, TransformProStrings.SystemLanguage.TooltipPreferences);
            GUI.contentColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            if (GUILayout.Button(preferencesContent, TransformProStyles.Buttons.Icon.Single, GUILayout.Width(28)))
            {
                TransformProEditor.ShowPreferences();
            }
            GUI.contentColor = Color.white;

            // Bounds visualisation toggle controls
            TransformProEditorBounds.DrawGUI();

            // Space mode controls
            TransformProEditorHandles.DrawGUI();

            // Reset button main
            GUI.color = TransformProStyles.ColorClear;
            GUIContent resetContent = new GUIContent("Reset", TransformProStrings.SystemLanguage.TooltipResetTransform);
            if (GUILayout.Button(resetContent, TransformProStyles.Buttons.Icon.Single, GUILayout.Width(60)))
            {
                Undo.RecordObject(TransformPro.Transform, "TransformPro Reset");
                TransformPro.Reset();
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            // Position
            TransformProEditorVector3.Data positionData = TransformProEditorVector3.Draw("Position",
                                                                                         TransformPro.Position,
                                                                                         TransformPro.CanChangePosition,
                                                                                         TransformProPreferences.AdvancedPosition,
                                                                                         TransformPro.ResetPosition,
                                                                                         "0",
                                                                                         TransformProStrings.SystemLanguage.TooltipResetPosition);
            if (positionData.Changed)
            {
                TransformPro.Position = positionData.Value;
            }
            TransformProPreferences.AdvancedPosition = positionData.Foldout;
            if (TransformProPreferences.AdvancedPosition)
            {
                TransformProEditorAdvanced.DrawPositionGUI();

                // ------------------------------------------------------------------------------------------------------------------------------------------
                GUILayout.Space(5); // space only added to advanced control
            }

            // Rotation
            TransformProEditorVector3.Data rotationData = TransformProEditorVector3.Draw("Rotation",
                                                                                         TransformPro.RotationEuler,
                                                                                         TransformPro.CanChangeRotation,
                                                                                         TransformProPreferences.AdvancedRotation,
                                                                                         TransformPro.ResetRotation,
                                                                                         "0",
                                                                                         TransformProStrings.SystemLanguage.TooltipResetRotation);
            if (rotationData.Changed)
            {
                TransformPro.RotationEuler = rotationData.Value;
            }
            TransformProPreferences.AdvancedRotation = rotationData.Foldout;
            if (TransformProPreferences.AdvancedRotation)
            {
                TransformProEditorAdvanced.DrawRotationGUI();

                // ------------------------------------------------------------------------------------------------------------------------------------------
                GUILayout.Space(5); // space only added to advanced control
            }

            // Scale
            TransformProEditorVector3.DrawScale(TransformPro.Scale);
            if (TransformProPreferences.AdvancedScale)
            {
                // stuff

                // ------------------------------------------------------------------------------------------------------------------------------------------
                GUILayout.Space(5);
            }

            EditorGUILayout.BeginHorizontal();

            // Copy & Paste
            TransformProEditorClipboard.DrawGUI();

            // Snap
            TransformProEditorSnapping.DrawGUI();

            // Grounding
            TransformProEditorGrounding.DrawGUI();

            EditorGUILayout.EndHorizontal();

            GUI.skin = resetSkin;
        }

        private static void OnSceneClipboard()
        {
            // TODO: Replace this with proper color support
            Color colorClipboard = TransformProStyles.ColorCopy;
            colorClipboard.r = colorClipboard.r * colorClipboard.r * colorClipboard.r;
            colorClipboard.g = colorClipboard.g * colorClipboard.g * colorClipboard.g;
            colorClipboard.b = colorClipboard.b * colorClipboard.b * colorClipboard.b;
            //colorClipboard.a = 0.25f;

            Quaternion rotation = TransformPro.ClipboardRotation * Quaternion.LookRotation(Vector3.up);
            Vector3 rotationEuler = TransformPro.ClipboardRotation * Vector3.up;

            Vector3 position = TransformPro.ClipboardPosition;
            Vector3 positionTop = position + (rotationEuler * 0.15f);

            float size = (HandleUtility.WorldToGUIPoint(position) - HandleUtility.WorldToGUIPoint(positionTop)).magnitude / 12f;

            Handles.color = colorClipboard;
            Handles.DrawSolidDisc(position, rotationEuler, 0.1f);
#if UNITY_5_5_OR_NEWER
            Handles.ConeHandleCap(0, positionTop, rotation, 0.1f, EventType.Repaint);
#else
            Handles.ConeCap(0, positionTop, rotation, 0.1f);
#endif
            Handles.DrawAAPolyLine(Texture2D.whiteTexture, size, position, positionTop);
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
#if !UNITY_5_3_OR_NEWER
            IEnumerable<UnityObject> selection = Selection.objects.OrderBy(x => x.GetInstanceID());
            if ((this.previousSelection == null) || !selection.SequenceEqual(this.previousSelection))
            {
                this.SelectionChanged();
            }
            this.previousSelection = selection;
#endif

            IEnumerable<Transform> transforms = this.targets.Cast<Transform>().ToArray();
            int transformCount = transforms.Count();

            switch (transformCount)
            {
                case 0:
                    Debug.LogError("[<color=red>TransformPro</color>] Attempted to inspect transforms without any target.");
                    break;
                case 1:
                    TransformPro.Transform = transforms.First();
                    if (TransformPro.Transform.hasChanged)
                    {
                        TransformPro.UpdateFromScene();
                    }
                    TransformProEditor.InspectorGUI();
                    break;
                default:
                    Tools.hidden = false;
                    base.OnInspectorGUI();
                    break;
            }
        }

        /// <summary>
        ///     Redraw the GUI if the modifier keys has changed. This allows buttons to change color when the shift key is pressed
        ///     to reflect alternative actions.
        /// </summary>
        private void ModifierKeysChanged()
        {
            this.Repaint();
        }

        /// <summary>
        ///     Deregister events.
        /// </summary>
        private void OnDisable()
        {
#if UNITY_5_3_OR_NEWER
            Selection.selectionChanged -= this.SelectionChanged;
#endif

            Undo.undoRedoPerformed -= this.UndoRedoPerformed;
            EditorApplication.modifierKeysChanged -= this.ModifierKeysChanged;
        }

        /// <summary>
        ///     Ensure everything is loaded and register events.
        /// </summary>
        private void OnEnable()
        {
            // why doesnt prefs handle this check? expose a force bool if needed
            if (!TransformProPreferences.AreLoaded)
            {
                TransformProPreferences.Load();
            }

#if UNITY_5_3_OR_NEWER
            Selection.selectionChanged += this.SelectionChanged;
#endif

            Undo.undoRedoPerformed += this.UndoRedoPerformed;
            EditorApplication.modifierKeysChanged += this.ModifierKeysChanged;
        }

        /// <summary>
        ///     Draw overlays to the scene.
        /// </summary>
        private void OnSceneGUI()
        {
            if (TransformPro.Transform == null)
            {
                return;
            }

            TransformProEditorGrid.DrawHandles();
            TransformPro.Bounds.DrawHandles();
            TransformProEditorHandles.DrawHandles();

            TransformProEditor.OnSceneClipboard();

            // Coming soon, gadgets, auto snap, visible grid..
            /*
            Handles.BeginGUI();
            TransformProEditorGrid.DrawSceneGUI();
            TransformProEditorSceneBase.DrawSceneGUI();
            Handles.EndGUI();
            */
        }

        /// <summary>
        ///     <see cref="Selection.selectionChanged" /> delegate. Ensures the current selection is being edited.
        /// </summary>
        private void SelectionChanged()
        {
            TransformPro.Transform = Selection.activeTransform;
            TransformProEditorHandles.UpdatePivot();
        }

        /// <summary>
        ///     <see cref="Undo.undoRedoPerformed" /> delegate. Ensures the bounds are recalculated when Undo or Redo are used.
        /// </summary>
        private void UndoRedoPerformed()
        {
            TransformPro.SetBoundsDirtyWorld();
        }
    }
}
