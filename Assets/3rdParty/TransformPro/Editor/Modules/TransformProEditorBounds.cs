// Copyright(c) 2017 Untitled Games   | Developed by: Chris Bellini                    | http://untitledgam.es/contact
// http://transformpro.untitledgam.es | http://transformpro.untitledgam.es/quick-start | http://transformpro.untitledgam.es/api

namespace UntitledGames.Transforms
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     Draw a given set of Bounds data to the viewport.
    /// </summary>
    public static class TransformProEditorBounds
    {
        public static void DrawGUI()
        {
            GUI.enabled = TransformPro.CalculateBounds;
            GUI.color = Color.white;
            bool showRendererBoundsOld = TransformProPreferences.ShowRendererBounds;
            bool showColliderBoundsOld = TransformProPreferences.ShowColliderBounds;
            bool showMeshFilterBoundsOld = TransformProPreferences.ShowMeshFilterBounds;
            GUIContent showRendererContent = new GUIContent(TransformProStyles.Icons.Renderer, TransformProStrings.SystemLanguage.TooltipVisualiseRenderers);
            TransformProPreferences.ShowRendererBounds = GUILayout.Toggle(TransformProPreferences.ShowRendererBounds, showRendererContent, TransformProStyles.Buttons.Icon.Left, GUILayout.Width(28));
            GUIContent showColliderContent = new GUIContent(TransformProStyles.Icons.Collider, TransformProStrings.SystemLanguage.TooltipVisualiseColliders);
            TransformProPreferences.ShowColliderBounds = GUILayout.Toggle(TransformProPreferences.ShowColliderBounds, showColliderContent, TransformProStyles.Buttons.Icon.Middle, GUILayout.Width(28));
            GUIContent showMeshesContent = new GUIContent(TransformProStyles.Icons.MeshFilter, TransformProStrings.SystemLanguage.TooltipVisualiseMeshes);
            TransformProPreferences.ShowMeshFilterBounds = GUILayout.Toggle(TransformProPreferences.ShowMeshFilterBounds, showMeshesContent, TransformProStyles.Buttons.Icon.Right, GUILayout.Width(28));
            if ((TransformProPreferences.ShowRendererBounds != showRendererBoundsOld)
                || (TransformProPreferences.ShowColliderBounds != showColliderBoundsOld)
                || (TransformProPreferences.ShowMeshFilterBounds != showMeshFilterBoundsOld))
            {
                SceneView.RepaintAll();
            }
            GUI.enabled = true;
        }

        /// <summary>
        ///     Draw a full set of <see cref="TransformProBounds" /> data to the viewport using the <see cref="Handles" /> system.
        /// </summary>
        /// <param name="bounds">The <see cref="TransformProBounds" /> data to draw.</param>
        public static void DrawHandles(this TransformProBounds bounds)
        {
            if ((bounds == null) || !TransformPro.CalculateBounds)
            {
                return;
            }

            if (TransformProPreferences.ShowRendererBounds)
            {
                Handles.color = TransformProStyles.ColorRenderer;
                TransformProEditorBounds.DrawHandles(TransformPro.Bounds.Renderer.min, TransformPro.Bounds.Renderer.max, 4);
            }

            if (TransformProPreferences.ShowColliderBounds)
            {
                Handles.color = TransformProStyles.ColorCollider;
                TransformProEditorBounds.DrawHandles(TransformPro.Bounds.Collider.min, TransformPro.Bounds.Collider.max, 2);
            }

            if (TransformProPreferences.ShowMeshFilterBounds)
            {
                Handles.color = TransformProStyles.ColorMesh;
                int timeout = 0;
                foreach (MeshFilter meshFilter in bounds.MeshFilters)
                {
                    if ((meshFilter == null) || (meshFilter.sharedMesh == null))
                    {
                        continue;
                    }
                    TransformProEditorBounds.DrawHandles(meshFilter.transform, meshFilter.sharedMesh.bounds.min, meshFilter.sharedMesh.bounds.max, 1);
                    timeout++;

                    if (timeout >= 32)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///     Draws a rotated local space box.
        /// </summary>
        /// <param name="transform">The transform to use to rotate the box.</param>
        /// <param name="a">The smallest local space position values.</param>
        /// <param name="b">The largest local space position values.</param>
        /// <param name="dashSize">The dash size to draw the box with.</param>
        public static void DrawHandles(Transform transform, Vector3 a, Vector3 b, float dashSize)
        {
            Vector3 aaa = transform.localToWorldMatrix.MultiplyPoint(new Vector3(a.x, a.y, a.z));
            Vector3 aab = transform.localToWorldMatrix.MultiplyPoint(new Vector3(a.x, a.y, b.z));
            Vector3 aba = transform.localToWorldMatrix.MultiplyPoint(new Vector3(a.x, b.y, a.z));
            Vector3 abb = transform.localToWorldMatrix.MultiplyPoint(new Vector3(a.x, b.y, b.z));
            Vector3 baa = transform.localToWorldMatrix.MultiplyPoint(new Vector3(b.x, a.y, a.z));
            Vector3 bab = transform.localToWorldMatrix.MultiplyPoint(new Vector3(b.x, a.y, b.z));
            Vector3 bba = transform.localToWorldMatrix.MultiplyPoint(new Vector3(b.x, b.y, a.z));
            Vector3 bbb = transform.localToWorldMatrix.MultiplyPoint(new Vector3(b.x, b.y, b.z));

            TransformProEditorBounds.DrawHandles(aaa, aab, aba, abb, baa, bab, bba, bbb, dashSize);
        }

        /// <summary>
        ///     Draw a world space bounding box.
        /// </summary>
        /// <param name="a">The smallest world space position values.</param>
        /// <param name="b">The largest world space position values.</param>
        /// <param name="dashSize">The dash size to draw the box with.</param>
        public static void DrawHandles(Vector3 a, Vector3 b, float dashSize)
        {
            Vector3 aaa = new Vector3(a.x, a.y, a.z);
            Vector3 aab = new Vector3(a.x, a.y, b.z);
            Vector3 aba = new Vector3(a.x, b.y, a.z);
            Vector3 abb = new Vector3(a.x, b.y, b.z);
            Vector3 baa = new Vector3(b.x, a.y, a.z);
            Vector3 bab = new Vector3(b.x, a.y, b.z);
            Vector3 bba = new Vector3(b.x, b.y, a.z);
            Vector3 bbb = new Vector3(b.x, b.y, b.z);

            TransformProEditorBounds.DrawHandles(aaa, aab, aba, abb, baa, bab, bba, bbb, dashSize);
        }

        /// <summary>
        ///     Draws a rotated box using the <see cref="Handles" /> system.
        /// </summary>
        /// <param name="aaa">A <see cref="Vector3" /> containing the following composite vector: (Low, Low, Low)</param>
        /// <param name="aab">A <see cref="Vector3" /> containing the following composite vector: (Low, Low, High)</param>
        /// <param name="aba">A <see cref="Vector3" /> containing the following composite vector: (Low, High, Low)</param>
        /// <param name="abb">A <see cref="Vector3" /> containing the following composite vector: (Low, High, High)</param>
        /// <param name="baa">A <see cref="Vector3" /> containing the following composite vector: (High, Low, Low)</param>
        /// <param name="bab">A <see cref="Vector3" /> containing the following composite vector: (High, Low, High)</param>
        /// <param name="bba">A <see cref="Vector3" /> containing the following composite vector: (High, High, Low)</param>
        /// <param name="bbb">A <see cref="Vector3" /> containing the following composite vector: (High, High, High)</param>
        /// <param name="dashSize">The dash size to draw the box with.</param>
        private static void DrawHandles(
            Vector3 aaa, Vector3 aab, Vector3 aba, Vector3 abb,
            Vector3 baa, Vector3 bab, Vector3 bba, Vector3 bbb,
            float dashSize)
        {
            // X
            Handles.DrawDottedLine(aaa, baa, dashSize);
            Handles.DrawDottedLine(aba, bba, dashSize);
            Handles.DrawDottedLine(aab, bab, dashSize);
            Handles.DrawDottedLine(abb, bbb, dashSize);
            // Y
            Handles.DrawDottedLine(aaa, aba, dashSize);
            Handles.DrawDottedLine(baa, bba, dashSize);
            Handles.DrawDottedLine(aab, abb, dashSize);
            Handles.DrawDottedLine(bab, bbb, dashSize);
            // Z
            Handles.DrawDottedLine(aaa, aab, dashSize);
            Handles.DrawDottedLine(aba, abb, dashSize);
            Handles.DrawDottedLine(baa, bab, dashSize);
            Handles.DrawDottedLine(bba, bbb, dashSize);
        }
    }
}