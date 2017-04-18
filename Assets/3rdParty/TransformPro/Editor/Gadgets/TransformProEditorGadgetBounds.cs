namespace TransformPro.Transforms
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UntitledGames.Transforms;

    public class TransformProEditorGadgetBounds : ITransformProGadget
    {
        private int selectedTab;

        /// <inheritdoc />
        public bool Enabled { get { return false; } }

        /// <inheritdoc />
        public float Height { get { return 85; } }

        /// <inheritdoc />
        public int Sort { get { return 1; } }

        /// <inheritdoc />
        public void OnSceneGUI(SceneView sceneView, TransformProEditorGadgets gadgets, Rect rect)
        {
            //rect.x -= 1;
            //rect.width += 2;
            float tabWidth = rect.width / 3;

            Rect tab = new Rect(rect.x, rect.y, tabWidth, 16);
            if (gadgets.Button(tab, TransformProStyles.Icons.Renderer, TransformProStyles.Buttons.Icon.Left, this.selectedTab == 0))
            {
                this.selectedTab = 0;
            }

            tab.x += tabWidth;
            if (gadgets.Button(tab, TransformProStyles.Icons.Collider, TransformProStyles.Buttons.Icon.Middle, this.selectedTab == 1))
            {
                this.selectedTab = 1;
            }

            tab.x += tabWidth;
            tab.xMax = rect.xMax;
            if (gadgets.Button(tab, TransformProStyles.Icons.MeshFilter, TransformProStyles.Buttons.Icon.Right, this.selectedTab == 2))
            {
                this.selectedTab = 2;
            }

            switch (this.selectedTab)
            {
                case 0:
                    Rect countLabelRect = new Rect(rect.x + 2, rect.y + 20, rect.width - 4, 20);
                    string rendererCount = "No Renderers";
                    if (TransformPro.HasRenderers)
                    {
                        rendererCount = string.Format("{0} ({1})",
                                                      TransformPro.Bounds.Renderers.Count(),
                                                      TransformPro.Bounds.Renderers.GroupBy(x => x.isVisible).First().Count());
                    }
                    GUI.Label(countLabelRect, rendererCount, TransformProStyles.LabelSmall);
                    break;
            }

            GUI.color = Color.white;
        }
    }
}
