namespace UntitledGames.Transforms
{
    using UnityEditor;

    [InitializeOnLoad]
    public static class TransformProEditorLoader
    {
        static TransformProEditorLoader()
        {
            TransformProStyles.Initialise();
            TransformProPreferences.Load();

            // Coming soon - customisable scene view gadgets
            /*
            if (TransformProEditorGadgets.Instance == null)
            {
                TransformProEditorGadgets.Create();
            }

            if (TransformProEditorGadgets.Instance == null)
            {
                Debug.LogWarning("[<color=red>TransformPro</color>] Could not create gadget manager.");
                return;
            }

            EditorApplication.delayCall += TransformProEditorGadgets.Instance.Setup;
            */
        }
    }
}
