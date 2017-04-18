namespace UntitledGames.Transforms
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     The main settings for <see cref="TransformPro" />. These are saved using the standard <see cref="EditorPrefs" />
    ///     system.
    ///     This class extends <see cref="EditorWindow" /> to allow us to pop it up as a seperate form from the cog icon.
    /// </summary>
    public class TransformProPreferences : EditorWindow
    {
        private static bool advancedPosition;
        private static bool advancedRotation;
        private static bool advancedScale;
        private static bool areLoaded;
        private static bool enableBounds;

        private static bool enableGadgets;

        //private static Vector3 resetPosition;
        //private static Vector3 resetRotation;
        //private static Vector3 resetScale;
        private static Vector2 scrollPosition;

        private static bool showColliderBounds;
        private static bool showMeshFilterBounds;
        private static bool showRendererBounds;
        private static Vector3 snapPositionGrid;
        private static Vector3 snapPositionOrigin;
        private static Vector3 snapRotationGrid;
        private static Vector3 snapRotationOrigin;

        /// <summary>
        ///     A value indicating whether the advanced position panel is currently open.
        /// </summary>
        public static bool AdvancedPosition
        {
            get { return TransformProPreferences.advancedPosition; }
            set
            {
                TransformProPreferences.advancedPosition = value;
                EditorPrefs.SetBool("advancedPosition", value);
            }
        }

        /// <summary>
        ///     A value indicating whether the advanced rotation panel is currently open.
        /// </summary>
        public static bool AdvancedRotation
        {
            get { return TransformProPreferences.advancedRotation; }
            set
            {
                TransformProPreferences.advancedRotation = value;
                EditorPrefs.SetBool("advancedRotation", value);
            }
        }

        /// <summary>
        ///     A value indicating whether the advanced scale panel is currently open.
        /// </summary>
        public static bool AdvancedScale
        {
            get { return TransformProPreferences.advancedScale; }
            set
            {
                TransformProPreferences.advancedScale = value;
                EditorPrefs.SetBool("advancedScale", value);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the preferences are currently loaded.
        /// </summary>
        public static bool AreLoaded { get { return TransformProPreferences.areLoaded; } }

        /// <summary>
        ///     Gets or sets a values indicating whether to calculate bounds for the selected <see cref="Transform" /> objects.
        ///     Setting this value to true may cause bounds to be recalculated immediately.
        /// </summary>
        public static bool EnableBounds
        {
            get { return TransformProPreferences.enableBounds; }
            set
            {
                TransformProPreferences.enableBounds = value;
                EditorPrefs.SetBool("enableBounds", value);

                TransformPro.CalculateBounds = value;
                SceneView.RepaintAll();
            }
        }

        public static bool EnableGadgets
        {
            get { return TransformProPreferences.enableGadgets; }
            set
            {
                TransformProPreferences.enableGadgets = value;
                EditorPrefs.SetBool("enableGadgets", value);

                SceneView.RepaintAll();
            }
        }

        /// <summary>Gets or sets a value indicating whether <see cref="Collider" /> bounds should be shown in the viewport.</summary>
        public static bool ShowColliderBounds
        {
            get { return TransformProPreferences.showColliderBounds; }
            set
            {
                TransformProPreferences.showColliderBounds = value;
                EditorPrefs.SetBool("showColliderBounds", value);
            }
        }

        /// <summary>Gets or sets a value indicating whether <see cref="Mesh" /> bounds should be shown in the viewport.</summary>
        public static bool ShowMeshFilterBounds
        {
            get { return TransformProPreferences.showMeshFilterBounds; }
            set
            {
                TransformProPreferences.showMeshFilterBounds = value;
                EditorPrefs.SetBool("showMeshFilterBounds", value);
            }
        }

        /// <summary>Gets or sets a value indicating whether <see cref="Renderer" /> bounds should be shown in the viewport.</summary>
        public static bool ShowRendererBounds
        {
            get { return TransformProPreferences.showRendererBounds; }
            set
            {
                TransformProPreferences.showRendererBounds = value;
                EditorPrefs.SetBool("showRendererBounds", value);
            }
        }

        /// <summary>Gets the snap positioning grid.</summary>
        public static Vector3 SnapPositionGrid
        {
            get { return TransformProPreferences.snapPositionGrid; }
            set
            {
                TransformProPreferences.snapPositionGrid = value;
                TransformProPreferences.SaveVector3("snapPositionGrid", value);
            }
        }

        /// <summary>Gets the X axis component of the snap positioning origin.</summary>
        public static Vector3 SnapPositionOrigin
        {
            get { return TransformProPreferences.snapPositionOrigin; }
            set
            {
                TransformProPreferences.snapPositionOrigin = value;
                TransformProPreferences.SaveVector3("snapPositionOrigin", value);
            }
        }

        /// <summary>Gets the X axis component of the snap rotation grid.</summary>
        public static Vector3 SnapRotationGrid
        {
            get { return TransformProPreferences.snapRotationGrid; }
            set
            {
                TransformProPreferences.snapRotationGrid = value;
                TransformProPreferences.SaveVector3("snapRotationGrid", value);
            }
        }

        /// <summary>Gets the X axis component of the snap rotation origin.</summary>
        public static Vector3 SnapRotationOrigin
        {
            get { return TransformProPreferences.snapRotationOrigin; }
            set
            {
                TransformProPreferences.snapRotationOrigin = value;
                TransformProPreferences.SaveVector3("snapRotationOrigin", value);
            }
        }

        /// <summary>
        ///     Loads the settings and applies them.
        /// </summary>
        public static void Load()
        {
            TransformProPreferences.advancedPosition = EditorPrefs.GetBool("advancedPosition", false);
            TransformProPreferences.advancedRotation = EditorPrefs.GetBool("advancedRotation", false);
            TransformProPreferences.advancedScale = EditorPrefs.GetBool("advancedScale", false);

            TransformProPreferences.snapPositionGrid = TransformProPreferences.LoadVector3("snapPositionGrid", Vector3.one);
            TransformProPreferences.snapPositionOrigin = TransformProPreferences.LoadVector3("snapPositionOrigin", Vector3.zero);
            TransformProPreferences.snapRotationGrid = TransformProPreferences.LoadVector3("snapRotationGrid", new Vector3(90, 90, 90));
            TransformProPreferences.snapRotationOrigin = TransformProPreferences.LoadVector3("snapRotationOrigin", Vector3.zero);

            TransformProPreferences.enableBounds = EditorPrefs.GetBool("enableBounds", true);
            TransformProPreferences.enableGadgets = EditorPrefs.GetBool("enableGadgets", true);

            TransformProPreferences.showRendererBounds = EditorPrefs.GetBool("showRendererBounds", true);
            TransformProPreferences.showColliderBounds = EditorPrefs.GetBool("showColliderBounds", true);
            TransformProPreferences.showMeshFilterBounds = EditorPrefs.GetBool("showMeshFilterBounds", true);

            // Apply all settings to the transform pro.
            TransformPro.SnapPositionGrid = TransformProPreferences.SnapPositionGrid;
            TransformPro.SnapPositionOrigin = TransformProPreferences.SnapPositionOrigin;
            TransformPro.SnapRotationGrid = TransformProPreferences.SnapRotationGrid;
            TransformPro.SnapRotationOrigin = TransformProPreferences.SnapRotationOrigin;
            TransformPro.CalculateBounds = TransformProPreferences.EnableBounds;

            TransformProPreferences.areLoaded = true;
        }

        /// <summary>
        ///     Draws a standard <see cref="Vector3" /> field, but with fixes to prevent it dropping onto two lines in the
        ///     preferences window.
        /// </summary>
        /// <param name="label">The label to show for the field.</param>
        /// <param name="value">The value to show for the field.</param>
        /// <returns>The updated value.</returns>
        private static Vector3 DrawPreferencesField(string label, Vector3 value)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            Rect rectLabel = rect;
            rectLabel.width = EditorGUIUtility.labelWidth;
            GUI.Label(rectLabel, label);
            Rect rectField = rect;
            rectField.xMin += EditorGUIUtility.labelWidth;
            return EditorGUI.Vector3Field(rectField, GUIContent.none, value);
        }

        /// <summary>
        ///     Draws a standard <see cref="bool" /> field, but with fixes to prevent it dropping onto two lines in the preferences
        ///     window.
        /// </summary>
        /// <param name="label">The label to show for the field.</param>
        /// <param name="value">The value to show for the field.</param>
        /// <returns>The updated value.</returns>
        private static bool DrawPreferencesField(string label, bool value)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            Rect rectLabel = rect;
            rectLabel.width = EditorGUIUtility.labelWidth;
            GUI.Label(rectLabel, label);
            Rect rectField = rect;
            rectField.xMin += EditorGUIUtility.labelWidth;
            return EditorGUI.Toggle(rectField, value);
        }

        private static Vector3 LoadVector3(string name, Vector3 vector3)
        {
            vector3.x = EditorPrefs.GetFloat(name + ".x", vector3.x);
            vector3.y = EditorPrefs.GetFloat(name + ".y", vector3.y);
            vector3.z = EditorPrefs.GetFloat(name + ".z", vector3.z);
            return vector3;
        }

        /// <summary>
        ///     Draws the preferences GUI. The main portion of the interfaces is found here, and this method is decorated with
        ///     <see cref="UnityEditor.PreferenceItem" /> so that it appears in the default preferences window.
        /// </summary>
        [PreferenceItem("TransformPro")]
        private static void OnPreferencesGUI()
        {
            if (!TransformProPreferences.AreLoaded)
            {
                TransformProPreferences.Load();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("(c) 2017 Untitled Games", TransformProStyles.LabelCopyright);
            GUILayout.Label(TransformPro.Version);
            EditorGUILayout.EndHorizontal();

            // ------------------------------------------------------------------------------------------------------------------------------------------
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Snapping", TransformProStyles.LabelHeading2, GUILayout.Height(24));
            GUI.color = TransformProStyles.ColorClear;
            if (GUILayout.Button("Reset"))
            {
                TransformProPreferences.SnapPositionGrid = Vector3.one;
                TransformProPreferences.SnapPositionOrigin = Vector3.zero;
                TransformProPreferences.SnapRotationGrid = new Vector3(90, 90, 90);
                TransformProPreferences.SnapRotationOrigin = Vector3.zero;
            }
            GUI.color = Color.white;
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Position", TransformProStyles.LabelHeading3, GUILayout.Height(19));
            TransformProPreferences.SnapPositionGrid = TransformProPreferences.DrawPreferencesField("Size", TransformProPreferences.SnapPositionGrid);
            TransformProPreferences.SnapPositionOrigin = TransformProPreferences.DrawPreferencesField("Origin", TransformProPreferences.SnapPositionOrigin);

            EditorGUILayout.LabelField("Rotation", TransformProStyles.LabelHeading3, GUILayout.Height(19));
            TransformProPreferences.SnapRotationGrid = TransformProPreferences.DrawPreferencesField("Size", TransformProPreferences.SnapRotationGrid);
            TransformProPreferences.SnapRotationOrigin = TransformProPreferences.DrawPreferencesField("Origin", TransformProPreferences.SnapRotationOrigin);

            // ------------------------------------------------------------------------------------------------------------------------------------------
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Bounds", TransformProStyles.LabelHeading2, GUILayout.Height(24));
            EditorGUILayout.HelpBox("Enabling bounds will allow TransformPro to calculate the maximum extents of the current object, including its children. " +
                                    "This enables world and local space outlining in the scene view, as well as features such as Drop and Ground. " +
                                    //"Advanced scaling options such as 'relative fit' also require this option to be enabled. " +
                                    "Enabling this introduces a small performance overhead on selecting a new transform.",
                                    MessageType.Info);
            TransformProPreferences.EnableBounds = TransformProPreferences.DrawPreferencesField("Enable", TransformProPreferences.EnableBounds);

            /*
            // ------------------------------------------------------------------------------------------------------------------------------------------
            GUILayout.Space(5);
            EditorGUILayout.HelpBox("Gadgets are small helper panels that appear in the scene view that contain common functionality such as nudges. They " +
                                    "are currently a beta features.\n\n" +
                                    "In a later version you will be able to select and order a list of gadgets and quick toggle the entire display from the " +
                                    "standard TransformPro interface.",
                                    MessageType.Info);
            TransformProPreferences.EnableGadgets = TransformProPreferences.DrawPreferencesField("Enable", TransformProPreferences.EnableGadgets);
            */
        }

        private static void SaveVector3(string name, Vector3 vector3)
        {
            EditorPrefs.SetFloat(name + ".x", vector3.x);
            EditorPrefs.SetFloat(name + ".y", vector3.y);
            EditorPrefs.SetFloat(name + ".z", vector3.z);
        }

        /// <summary>
        ///     Sets the window <see cref="EditorWindow.titleContent" />.
        /// </summary>
        private void OnEnable()
        {
#if UNITY_5_3_OR_NEWER
            TransformProStyles.Load();
            this.titleContent = new GUIContent("TransformPro", TransformProStyles.Icons.Logo);
#else
            this.title = "TransformPro";
#endif

            this.minSize = new Vector2(240, 400);
        }

        /// <summary>
        ///     Draws the stand alone version of the preferences window.
        ///     Adds a replacement header, then calls <see cref="OnPreferencesGUI" />.
        /// </summary>
        private void OnGUI()
        {
            TransformProPreferences.scrollPosition = EditorGUILayout.BeginScrollView(TransformProPreferences.scrollPosition);

            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(64));
            Rect rectIcon = rect;
            rectIcon.width = 64;
            rect.xMin += 70;
            EditorGUI.DrawPreviewTexture(rectIcon, TransformProStyles.Icons.Logo, TransformProStyles.MaterialIconTransparent);
            GUI.Label(rect, "TransformPro", TransformProStyles.LabelHeading1);

            TransformProPreferences.OnPreferencesGUI();

            EditorGUILayout.EndScrollView();
        }
    }
}
