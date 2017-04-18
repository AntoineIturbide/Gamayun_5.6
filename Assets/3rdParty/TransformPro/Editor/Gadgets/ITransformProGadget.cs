namespace UntitledGames.Transforms
{
    using UnityEditor;
    using UnityEngine;

    public interface ITransformProGadget
    {
        bool Enabled { get; }
        float Height { get; }
        int Sort { get; }
        void OnSceneGUI(SceneView sceneView, TransformProEditorGadgets gadgets, Rect rect);
    }
}
