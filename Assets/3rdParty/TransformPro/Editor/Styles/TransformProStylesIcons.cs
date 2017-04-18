namespace UntitledGames.Transforms
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class TransformProStylesIcons
    {
        private TransformProStylesIconsSkinPair clipboard;
        private TransformProStylesIconsSkinPair cog;
        private TransformProStylesIconsSkinPair collider;
        private TransformProStylesIconsSkinPair drop;
        private TransformProStylesIconsSkinPair editVector1;
        private TransformProStylesIconsSkinPair editVector3;
        private TransformProStylesIconsSkinPair gameObject;
        private TransformProStylesIconsSkinPair ground;
        private TransformProStylesIconsSkinPair help;
        private TransformProStylesIconsSkinPair logo;
        private TransformProStylesIconsSkinPair lookAt;
        private TransformProStylesIconsSkinPair meshFilter;
        private TransformProStylesIconsSkinPair position;
        private TransformProStylesIconsSkinPair random;
        private TransformProStylesIconsSkinPair renderer;
        private TransformProStylesIconsSkinPair rotation;
        private TransformProStylesIconsSkinPair scale;
        private TransformProStylesIconsSkinPair snap;
        private TransformProStylesIconsSkinPair toolHandleCenter;
        private TransformProStylesIconsSkinPair toolHandleGlobal;
        private TransformProStylesIconsSkinPair toolHandleLocal;
        private TransformProStylesIconsSkinPair toolHandlePivot;
        private TransformProStylesIconsSkinPair transform;

        public TransformProStylesIcons()
        {
            this.clipboard = TransformProStylesIcons.GetCustomIcons("Clipboard");
            this.cog = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("icons/d_SettingsIcon.png"));
            this.collider = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetComponentIcon<CapsuleCollider>());
            this.drop = TransformProStylesIcons.GetCustomIcons("Drop");
            this.editVector1 = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetCustomIcon("EditVector1"));
            this.editVector3 = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetCustomIcon("EditVector3"));
            this.gameObject = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetComponentIcon<GameObject>());
            this.ground = TransformProStylesIcons.GetCustomIcons("Ground");
            this.help = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("_Help"));
            this.logo = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetComponentIcon<Transform>());
            this.lookAt = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("ViewToolOrbit"), TransformProStylesIcons.GetBuiltInIcon("ViewToolOrbit On"));
            this.meshFilter = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetComponentIcon<Mesh>());
            this.position = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("MoveTool"), TransformProStylesIcons.GetBuiltInIcon("MoveTool on"));
            this.random = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetCustomIcon("Random"));
            this.renderer = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetComponentIcon<MeshRenderer>());
            this.rotation = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("RotateTool"), TransformProStylesIcons.GetBuiltInIcon("RotateTool on"));
            this.scale = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("ScaleTool"), TransformProStylesIcons.GetBuiltInIcon("ScaleTool on"));
            this.snap = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("GridIcon"));
            this.toolHandleCenter = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("d_ToolHandleCenter"));
            this.toolHandleGlobal = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("d_ToolHandleGlobal"));
            this.toolHandleLocal = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("d_ToolHandleLocal"));
            this.toolHandlePivot = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("d_ToolHandlePivot"));
            this.transform = new TransformProStylesIconsSkinPair(TransformProStylesIcons.GetBuiltInIcon("Transform Icon"));
        }

        public Texture Clipboard { get { return this.clipboard.Texture; } }
        public Texture Cog { get { return this.cog.Texture; } }
        public Texture Collider { get { return this.collider.Texture; } }
        public Texture Drop { get { return this.drop.Texture; } }
        public Texture EditVector1 { get { return this.editVector1.Texture; } }
        public Texture EditVector3 { get { return this.editVector3.Texture; } }
        public Texture GameObject { get { return this.gameObject.Texture; } }
        public Texture Ground { get { return this.ground.Texture; } }
        public Texture Help { get { return this.help.Texture; } }
        public Texture Logo { get { return this.logo.Texture; } }
        public Texture LookAt { get { return this.lookAt.Texture; } }
        public Texture MeshFilter { get { return this.meshFilter.Texture; } }
        public Texture Position { get { return this.position.Texture; } }
        public Texture Random { get { return this.random.Texture; } }
        public Texture Renderer { get { return this.renderer.Texture; } }
        public Texture Rotation { get { return this.rotation.Texture; } }
        public Texture Scale { get { return this.scale.Texture; } }
        public Texture Snap { get { return this.snap.Texture; } }
        public Texture ToolHandleCenter { get { return this.toolHandleCenter.Texture; } }
        public Texture ToolHandleGobal { get { return this.toolHandleGlobal.Texture; } }
        public Texture ToolHandleLocal { get { return this.toolHandleLocal.Texture; } }
        public Texture ToolHandlePivot { get { return this.toolHandlePivot.Texture; } }
        public Texture Transform { get { return this.transform.Texture; } }

        /// <summary>
        ///     Loads an icon from the built in resources.
        /// </summary>
        /// <param name="icon">The name of the built in icon.</param>
        /// <returns>The <see cref="Texture" /> to display.</returns>
        public static Texture GetBuiltInIcon(string icon)
        {
            return EditorGUIUtility.Load(icon) as Texture;
        }

        /// <summary>
        ///     Gets the <see cref="Texture" /> used by the standard editor components.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component" /> to find the icon for.</typeparam>
        /// <returns>The <see cref="Texture" /> representing the <see cref="Component" />.</returns>
        public static Texture GetComponentIcon<T>()
        {
            return EditorGUIUtility.ObjectContent(null, typeof(T)).image;
        }

        /// <summary>
        ///     Loads a custom icon from the editor resources.
        /// </summary>
        /// <param name="icon">The icon name to be loaded.</param>
        /// <returns>The custom icon to be displayed.</returns>
        public static Texture2D GetCustomIcon(string icon)
        {
            string assetPath = Path.Combine(TransformProStyles.PathEditorResources, "TransformPro-" + icon + ".png");
            return (Texture2D) EditorGUIUtility.Load(assetPath);
        }

        /// <summary>
        ///     Gets a pair of custom icons to support both personal and professional skins.
        /// </summary>
        /// <param name="icon">
        ///     The icon name to be loaded. The Personal skin uses TransformPro-{name}.png and the Professional skin
        ///     uses TransformPro-{name}.pro.png.
        /// </param>
        /// <returns>A pair of icons to be used. The current skin is automatically detected.</returns>
        public static TransformProStylesIconsSkinPair GetCustomIcons(string icon)
        {
            string iconPersonal = Path.Combine(TransformProStyles.PathEditorResources, "TransformPro-" + icon + ".png");
            string iconProfessional = Path.Combine(TransformProStyles.PathEditorResources, "TransformPro-" + icon + ".pro.png");

#if UNITY_5_3_OR_NEWER
            Texture texturePersonal = AssetDatabase.LoadAssetAtPath<Texture>(iconPersonal);
            Texture textureProfessional = AssetDatabase.LoadAssetAtPath<Texture>(iconProfessional);
#else
            Texture texturePersonal = AssetDatabase.LoadAssetAtPath(iconPersonal, typeof(Texture)) as Texture;
            Texture textureProfessional = AssetDatabase.LoadAssetAtPath(iconProfessional, typeof(Texture)) as Texture;
#endif

            return new TransformProStylesIconsSkinPair(texturePersonal, textureProfessional);
        }
    }
}
