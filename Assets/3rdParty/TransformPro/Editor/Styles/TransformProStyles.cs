namespace UntitledGames.Transforms
{
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     Holds all of the custom styles used by the system.
    ///     The properties of this class will remain mostly undocumented as it's fairly self explainatory.
    /// </summary>
    public static class TransformProStyles
    {
        private static TransformProStylesButtons buttons;
        private static Color[] colorAxis;
        private static Color colorAxisX = new Color(1, 0.8f, 0.8f);
        private static Color colorAxisY = new Color(0.8f, 1, 0.8f);
        private static Color colorAxisZ = new Color(0.8f, 0.8f, 1);
        private static Color colorClear = new Color(1, 0.7f, 0.7f);
        private static Color colorCollider = new Color(0.5f, 1, 0.5f, 0.75f);
        private static Color colorCopy = new Color(1, 0.8f, 0.6f); //new Color(0.7f, 1, 0.7f);
        private static Color colorHelp = new Color(0.7f, 0.7f, 1);
        private static Color colorLabelSubtle = new Color(0.6f, 0.6f, 0.6f);
        private static Color colorMesh = new Color(1, 0.5f, 0.5f, 0.75f);
        private static Color colorPaste = new Color(0.6f, 1, 0.8f); //new Color(1, 1, 0.7f);
        private static Color colorRenderer = new Color(0.5f, 0.5f, 1, 0.75f);
        private static Color colorSnap = new Color(0.8f, 0.6f, 1);
        private static Color colorSpace = new Color(0.7f, 1, 1);
        private static Texture2D iconPanel;
        private static Texture2D iconPanelDeep;
        private static TransformProStylesIcons icons;
        private static GUIStyle labelCopyright;
        private static GUIStyle labelHeading1;
        private static GUIStyle labelHeading2;
        private static GUIStyle labelHeading3;
        private static GUIStyle labelSmall;
        private static Material materialIconTransparent;
        private static GUIStyle panel;
        private static GUIStyle panelDeep;
        private static string pathRoot;
        private static GUIStyle popup;
        private static GUIStyle popupRightPersonal;
        private static GUIStyle popupRightProfessional;
        private static GUISkin skin;

        public static TransformProStylesButtons Buttons { get { return TransformProStyles.buttons; } }

        public static Color[] ColorAxis
        {
            get
            {
                return TransformProStyles.colorAxis ??
                       (TransformProStyles.colorAxis = new[] {TransformProStyles.ColorAxisX, TransformProStyles.ColorAxisY, TransformProStyles.ColorAxisZ});
            }
        }

        public static Color ColorAxisX { get { return TransformProStyles.colorAxisX; } }

        public static Color ColorAxisY { get { return TransformProStyles.colorAxisY; } }

        public static Color ColorAxisZ { get { return TransformProStyles.colorAxisZ; } }

        public static Color ColorClear { get { return TransformProStyles.colorClear; } }

        public static Color ColorCollider { get { return TransformProStyles.colorCollider; } }

        public static Color ColorCopy { get { return TransformProStyles.colorCopy; } }

        public static Color ColorHelp { get { return TransformProStyles.colorHelp; } }

        public static Color ColorLabelSubtle { get { return TransformProStyles.colorLabelSubtle; } }

        public static Color ColorMesh { get { return TransformProStyles.colorMesh; } }

        public static Color ColorPaste { get { return TransformProStyles.colorPaste; } }

        public static Color ColorRenderer { get { return TransformProStyles.colorRenderer; } }

        public static Color ColorSnap { get { return TransformProStyles.colorSnap; } }

        public static Color ColorSpace { get { return TransformProStyles.colorSpace; } }

        public static Texture2D IconPanel { get { return TransformProStyles.iconPanel ?? (TransformProStyles.iconPanel = TransformProStylesIcons.GetCustomIcon("Panel")); } }
        public static Texture2D IconPanelDeep { get { return TransformProStyles.iconPanelDeep ?? (TransformProStyles.iconPanelDeep = TransformProStylesIcons.GetCustomIcon("PanelDeep")); } }

        public static TransformProStylesIcons Icons { get { return TransformProStyles.icons; } }

        public static GUIStyle LabelCopyright
        {
            get
            {
                return TransformProStyles.labelCopyright ??
                       (TransformProStyles.labelCopyright = new GUIStyle(TransformProStyles.LabelSmall));
            }
        }

        public static GUIStyle LabelHeading1
        {
            get
            {
                return TransformProStyles.labelHeading1 ??
                       (TransformProStyles.labelHeading1 = new GUIStyle(EditorStyles.largeLabel)
                                                           {
                                                               fontSize = 20,
                                                               fontStyle = FontStyle.Bold,
                                                               alignment = TextAnchor.MiddleLeft
                                                           });
            }
        }

        public static GUIStyle LabelHeading2
        {
            get
            {
                return TransformProStyles.labelHeading2 ??
                       (TransformProStyles.labelHeading2 = new GUIStyle(EditorStyles.largeLabel)
                                                           {
                                                               fontSize = 17,
                                                               fontStyle = FontStyle.Bold,
                                                               alignment = TextAnchor.MiddleLeft
                                                           });
            }
        }

        public static GUIStyle LabelHeading3
        {
            get
            {
                return TransformProStyles.labelHeading3 ??
                       (TransformProStyles.labelHeading3 = new GUIStyle(EditorStyles.largeLabel)
                                                           {
                                                               fontSize = 14,
                                                               fontStyle = FontStyle.Bold,
                                                               alignment = TextAnchor.MiddleLeft
                                                           });
            }
        }

        public static GUIStyle LabelSmall
        {
            get
            {
                return TransformProStyles.labelSmall ??
                       (TransformProStyles.labelSmall = new GUIStyle(GUI.skin.label)
                                                        {
                                                            fontSize = 9,
                                                            alignment = TextAnchor.MiddleCenter
                                                        });
            }
        }

        public static Material MaterialIconTransparent
        {
            get
            {
                return TransformProStyles.materialIconTransparent ??
                       (TransformProStyles.materialIconTransparent = new Material(Shader.Find("Unlit/Transparent")));
            }
        }

        public static GUIStyle Panel
        {
            get
            {
                return TransformProStyles.panel ??
                       (TransformProStyles.panel = new GUIStyle(EditorStyles.helpBox)
                                                   {
                                                       normal =
                                                       {
                                                           background = TransformProStyles.IconPanel
                                                       }
                                                   });
            }
        }

        public static GUIStyle PanelDeep
        {
            get
            {
                return TransformProStyles.panelDeep ??
                       (TransformProStyles.panelDeep = new GUIStyle(EditorStyles.helpBox)
                                                       {
                                                           normal =
                                                           {
                                                               background = TransformProStyles.IconPanelDeep
                                                           }
                                                       });
            }
        }

        public static string PathEditor { get { return Path.Combine(TransformProStyles.pathRoot, "Editor"); } }
        public static string PathEditorResources { get { return Path.Combine(TransformProStyles.PathEditor, "Resources"); } }
        public static string PathRoot { get { return TransformProStyles.pathRoot; } }

        public static GUIStyle Popup
        {
            get
            {
                return TransformProStyles.popup ??
                       (TransformProStyles.popup = new GUIStyle(EditorStyles.popup));
            }
        }

        public static GUIStyle PopupRight
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                {
                    return TransformProStyles.popupRightProfessional ??
                           (TransformProStyles.popupRightProfessional = new GUIStyle(EditorStyles.popup)
                                                                        {
                                                                            margin = {left = 0},
                                                                            fixedHeight = 16,
                                                                            normal = {background = TransformProStylesIcons.GetCustomIcon("PopupRight.pro")},
                                                                            active = {background = TransformProStylesIcons.GetCustomIcon("PopupRight-active.pro")},
                                                                            focused = {background = TransformProStylesIcons.GetCustomIcon("PopupRight-focus.pro")}
                                                                        });
                }

                return TransformProStyles.popupRightPersonal ??
                       (TransformProStyles.popupRightPersonal = new GUIStyle(EditorStyles.popup)
                                                                {
                                                                    margin = {left = 0},
                                                                    fixedHeight = 16,
                                                                    normal = {background = TransformProStylesIcons.GetCustomIcon("PopupRight")},
                                                                    active = {background = TransformProStylesIcons.GetCustomIcon("PopupRight-active")},
                                                                    focused = {background = TransformProStylesIcons.GetCustomIcon("PopupRight-focus")}
                                                                });
            }
        }

        public static GUISkin Skin
        {
            get
            {
                if (TransformProStyles.skin != null)
                {
                    return TransformProStyles.skin;
                }

                TransformProStyles.skin = Object.Instantiate(GUI.skin);
                TransformProStyles.skin.button = TransformProStyles.Buttons.Standard.Single;
                return TransformProStyles.skin;
            }
        }

        public static void Initialise()
        {
            string[] guid = AssetDatabase.FindAssets("TransformProPreferences t:MonoScript");
            if (guid.Length > 0)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid.First());
                if (assetPath != null)
                {
                    assetPath = Path.GetDirectoryName(assetPath);
                    if (assetPath != null)
                    {
                        assetPath = Directory.GetParent(assetPath).ToString();
                        TransformProStyles.pathRoot = assetPath;
                        return;
                    }
                }
            }

            Debug.LogError("[<color=red>TransformPro</color>] Could not detemine installation path. Further errors may occur.");
        }

        public static void Load()
        {
            if (TransformProStyles.buttons == null)
            {
                TransformProStyles.buttons = new TransformProStylesButtons();
            }

            if (TransformProStyles.icons == null)
            {
                TransformProStyles.icons = new TransformProStylesIcons();
            }
        }
    }
}
