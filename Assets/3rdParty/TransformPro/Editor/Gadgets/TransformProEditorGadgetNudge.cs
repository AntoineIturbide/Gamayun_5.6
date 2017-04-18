namespace TransformPro.Transforms
{
    using UnityEditor;
    using UnityEngine;
    using UntitledGames.Transforms;

    public class TransformProEditorGadgetNudge : ITransformProGadget
    {
        /// <inheritdoc />
        public bool Enabled { get { return TransformPro.CanChangePosition || TransformPro.CanChangeRotation; } }

        /// <inheritdoc />
        public float Height { get { return (TransformPro.CanChangePosition ? 50 : 0) + (TransformPro.CanChangeRotation ? 50 : 0); } }

        /// <inheritdoc />
        public int Sort { get { return 1; } }

        /// <inheritdoc />
        public void OnSceneGUI(SceneView sceneView, TransformProEditorGadgets gadgets, Rect rect)
        {
            float y = 0;

            if (TransformPro.CanChangePosition)
            {
                y = TransformProEditorGadgetNudge.DrawHeading(rect, y, TransformProStyles.Icons.Position);
            }

            if (TransformPro.CanChangeRotation)
            {
                y = TransformProEditorGadgetNudge.DrawHeading(rect, y, TransformProStyles.Icons.Rotation);
            }
        }

        private static float DrawHeading(Rect rect, float y, Texture icon)
        {
            Rect headingRect = new Rect(rect.x, rect.y + y, 18, 18);
            GUI.DrawTexture(headingRect, icon);
            float width = (rect.width - 18) / 3;
            headingRect.x += 18;

            headingRect.width = width;
            GUI.color = TransformProStyles.ColorAxisX;
            GUI.Label(headingRect, "X", TransformProStyles.LabelSmall);

            headingRect.x += width;
            GUI.color = TransformProStyles.ColorAxisY;
            GUI.Label(headingRect, "Y", TransformProStyles.LabelSmall);

            headingRect.x += width;
            GUI.color = TransformProStyles.ColorAxisZ;
            GUI.Label(headingRect, "Z", TransformProStyles.LabelSmall);

            return y + 18;
        }
    }
}
