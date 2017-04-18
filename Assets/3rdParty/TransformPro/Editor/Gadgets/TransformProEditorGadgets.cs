namespace UntitledGames.Transforms
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public class TransformProEditorGadgets : ScriptableObject, ISerializationCallbackReceiver
    {
        public const float HeightGimbal = 110;
        public const float Padding = 5;

        private static readonly Color colorTab = new Color(0.7f, 0.7f, 0.7f);

        private static readonly Color colorTabSelected = new Color(0.2f, 0.2f, 0.2f);

        //private static readonly Color colorText = new Color(0.7f, 0.7f, 0.7f);

        private static TransformProEditorGadgets instance;

        [SerializeField]
        private ITransformProGadget[] gadgets;

        public static TransformProEditorGadgets Instance
        {
            get
            {
                if (TransformProEditorGadgets.instance != null)
                {
                    return TransformProEditorGadgets.instance;
                }

                TransformProEditorGadgets[] gadgetEditors = Resources.FindObjectsOfTypeAll<TransformProEditorGadgets>();

                if ((gadgetEditors == null) || (gadgetEditors.Length <= 0))
                {
                    return TransformProEditorGadgets.instance;
                }

                TransformProEditorGadgets.instance = gadgetEditors[0];

                for (int i = 1; i < gadgetEditors.Length; i++)
                {
                    Object.DestroyImmediate(gadgetEditors[i]);
                }

                return TransformProEditorGadgets.instance;
            }
            set { TransformProEditorGadgets.instance = value; }
        }

        public IEnumerable<ITransformProGadget> Gadgets { get { return this.gadgets ?? (this.gadgets = this.GetConcrete().ToArray()); } }

        /// <inheritdoc />
        public void OnBeforeSerialize()
        {
        }

        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            TransformProEditorGadgets.instance = this;

            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        }

        public static void Create()
        {
            //TransformProEditorGadgets.instance = ScriptableObject.CreateInstance<TransformProEditorGadgets>();
            //TransformProEditorGadgets.instance.hideFlags = HideFlags.DontSave;
        }

        private static float DrawPanel(float x, float y, float width, float height)
        {
            Rect rect = new Rect(x, y, width, height);
            GUI.color = Color.black;
            GUI.Label(rect, GUIContent.none, TransformProStyles.PanelDeep);
            GUI.color = Color.white;

            return y + height;
        }

        public bool Button(Rect rect, Texture texture, GUIStyle style, bool pressed)
        {
            Event e = Event.current;

            GUI.backgroundColor = pressed ? TransformProEditorGadgets.colorTabSelected : TransformProEditorGadgets.colorTab;
            GUI.Box(rect, texture, style);
            GUI.backgroundColor = Color.white;

            if (!pressed && rect.Contains(e.mousePosition) && (e.rawType == EventType.MouseDown))
            {
                SceneView.RepaintAll();
                return true;
            }

            return false;
        }

        public void Setup()
        {
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;

            TransformProEditorGadgets.instance = this;
            SceneView.RepaintAll();
        }

        private IEnumerable<ITransformProGadget> GetConcrete()
        {
            /*
            Type type = typeof(ITransformProGadget);
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                                               .SelectMany(s => s.GetTypes())
                                               .Where(p => type.IsAssignableFrom(p) && p.IsClass);
            return types.Select(Activator.CreateInstance)
                        .Cast<ITransformProGadget>()
                        .OrderBy(x => x.Sort);
                        */
            return new ITransformProGadget[0];
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!TransformProPreferences.EnableGadgets)
            {
                return;
            }

            Event e = Event.current;
            int controlID = GUIUtility.GetControlID(this.GetHashCode(), FocusType.Passive);

            if ((e.rawType != EventType.Layout) && (e.rawType != EventType.Repaint) && (e.rawType != EventType.MouseDown) && (e.rawType != EventType.MouseUp))
            {
                return;
            }

            TransformProStyles.Load();

            Handles.BeginGUI();

            float x = Screen.width - 100;
            float y = 5;
            float width = 90;

            // Adds a panel around the default gimbal control to improve visibility and help with consistancy
            y = TransformProEditorGadgets.DrawPanel(x, y, width, TransformProEditorGadgets.HeightGimbal);
            y += TransformProEditorGadgets.Padding;

            bool overPanel = false;
            foreach (ITransformProGadget gadget in this.Gadgets)
            {
                if (!gadget.Enabled)
                {
                    continue;
                }

                float nextY = TransformProEditorGadgets.DrawPanel(x, y, width, gadget.Height);

                Rect rect = new Rect(x, y, width, gadget.Height);
                gadget.OnSceneGUI(sceneView, this, rect);

                overPanel |= rect.Contains(e.mousePosition);

                y = nextY + TransformProEditorGadgets.Padding;
            }

            if (overPanel && (e.rawType == EventType.Layout))
            {
                HandleUtility.AddDefaultControl(controlID);
            }
            if (overPanel && ((e.rawType == EventType.MouseDown) || (e.rawType == EventType.MouseUp)))
            {
                e.Use();
            }

            Handles.EndGUI();
        }
    }
}
