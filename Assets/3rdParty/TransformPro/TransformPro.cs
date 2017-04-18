namespace UntitledGames.Transforms
{
    using UnityEngine;

    /// <summary>
    ///     Provides the runtime features for the system. All main functionality can be found here.
    ///     More information can be found at http://transformpro.untitledgam.es
    ///     TODO: Convert to non-static class and allow a reference to a specific transform to be held
    /// </summary>
    public static partial class TransformPro
    {
        private static readonly string version = "v1.2.5";
        private static TransformProBounds bounds;
        private static bool calculateBounds = true;
        private static Vector3 clipboardPosition = Vector3.zero;
        private static Quaternion clipboardRotation = Quaternion.identity;
        private static Vector3 clipboardScale = Vector3.one;
        private static Vector3 displayPosition;
        private static Vector3 displayRotation;
        private static Vector3 displayScale;
        private static Transform transform;

        /// <summary>
        ///     Gets the <see cref="TransformProBounds" /> object for the current <see cref="Transform" />.
        /// </summary>
        public static TransformProBounds Bounds { get { return TransformPro.bounds; } }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to calculate overall bounds for transforms.
        ///     When setting this value to true, if the current <see cref="Transform" /> does not contain a
        ///     <see cref="TransformProBounds" /> object, one will be created.
        /// </summary>
        public static bool CalculateBounds
        {
            get { return TransformPro.calculateBounds; }
            set
            {
                TransformPro.calculateBounds = value;
                if (value && (TransformPro.transform != null) && (TransformPro.bounds == null))
                {
                    TransformPro.bounds = new TransformProBounds(TransformPro.transform);
                }
            }
        }

        /// <summary>
        ///     The current <see cref="Transform" /> is currently able to have its position changed.
        /// </summary>
        public static bool CanChangePosition { get { return TransformPro.Transform != null; } }

        /// <summary>
        ///     The current <see cref="Transform" /> is currently able to have its rotation changed.
        /// </summary>
        public static bool CanChangeRotation { get { return TransformPro.Transform != null; } }

        /// <summary>
        ///     The current <see cref="Transform" /> is currently able to have its scale changed.
        /// </summary>
        public static bool CanChangeScale { get { return (TransformPro.Transform != null) && (TransformPro.Space != TransformProSpace.World); } }

        public static Vector3 ClipboardPosition { get { return TransformPro.clipboardPosition; } set { TransformPro.clipboardPosition = value; } }

        public static Quaternion ClipboardRotation { get { return TransformPro.clipboardRotation; } set { TransformPro.clipboardRotation = value; } }

        public static Vector3 ClipboardScale { get { return TransformPro.clipboardScale; } set { TransformPro.clipboardScale = value; } }

        /// <summary>
        ///     Returns true if the currently selected <see cref="Transform" /> has any child transforms. If no
        ///     <see cref="Transform" /> is selected false will be returned.
        /// </summary>
        public static bool HasChildren { get { return (TransformPro.Transform != null) && (TransformPro.Transform.childCount > 0); } }

        /// <summary>
        ///     Returns true if the currently selected <see cref="Transform" /> or any of its children contain colliders. If no
        ///     <see cref="Transform" /> is selected or bounds are disabled false will be returned.
        /// </summary>
        public static bool HasColliders { get { return (TransformPro.Transform != null) && (TransformPro.Bounds != null) && TransformPro.Bounds.HasColliders; } }

        /// <summary>
        ///     Returns true if the currently selected <see cref="Transform" /> or any of its children contain
        ///     <see cref="MeshFilter" /> components. If no <see cref="Transform" /> is selected or bounds are disabled false will
        ///     be returned.
        /// </summary>
        public static bool HasMeshFilters { get { return (TransformPro.Transform != null) && (TransformPro.Bounds != null) && TransformPro.Bounds.HasMeshFilters; } }

        /// <summary>
        ///     Returns true if the currently selected <see cref="Transform" /> or any of its children contain renderers. If no
        ///     <see cref="Transform" /> is selected or bounds are disabled false will be returned.
        /// </summary>
        public static bool HasRenderers { get { return (TransformPro.Transform != null) && (TransformPro.Bounds != null) && TransformPro.Bounds.HasRenderers; } }

        /// <summary>
        ///     Gets or sets the <see cref="Vector3" /> Position of the current object, in the current
        ///     <see cref="TransformProSpace" />.
        ///     If you attempt to update a position when <see cref="CanChangePosition" /> is false, the request will be ignored.
        /// </summary>
        public static Vector3 Position
        {
            get
            {
                TransformPro.TryGetPosition();
                return TransformPro.displayPosition;
            }
            set { TransformPro.TrySetPosition(value); }
        }

        public static Vector3 PositionLocal
        {
            get { return TransformPro.transform.localPosition; }
            set
            {
                TransformPro.transform.localPosition = value;
                TransformPro.TryGetPosition();
            }
        }

        public static Vector3 PositionWorld
        {
            get { return TransformPro.transform.position; }
            set
            {
                TransformPro.transform.position = value;
                TransformPro.TryGetPosition();
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Quaternion" /> Rotation of the current object, in the current
        ///     <see cref="TransformProSpace" />.
        ///     If you attempt to update a rotation when <see cref="CanChangeRotation" /> is false, the request will be ignored.
        ///     Setting this property will update the bounds as required.
        /// </summary>
        public static Quaternion Rotation
        {
            get
            {
                switch (TransformPro.space)
                {
                    case TransformProSpace.Local:
                        return TransformPro.Transform.localRotation;
                    case TransformProSpace.World:
                        return TransformPro.Transform.rotation;
                }
                return Quaternion.identity;
            }
            set { TransformPro.TrySetRotation(value); }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Vector3" /> Euler Rotation of the current object, in the current
        ///     <see cref="TransformProSpace" />.
        ///     If you attempt to update a rotation when <see cref="CanChangeRotation" /> is false, the request will be ignored.
        ///     Setting this property will update the bounds as required.
        /// </summary>
        public static Vector3 RotationEuler
        {
            get { return TransformPro.displayRotation; }
            set
            {
                if (TransformPro.TrySetRotation(value))
                {
                    TransformPro.displayRotation = value;
                }
            }
        }

        public static Quaternion RotationLocal { get { return TransformPro.transform.localRotation; } set { TransformPro.transform.localRotation = value; } }

        public static Quaternion RotationWorld { get { return TransformPro.transform.rotation; } set { TransformPro.transform.rotation = value; } }

        /// <summary>
        ///     Gets or sets the <see cref="Vector3" /> Scale of the current object, in the current
        ///     <see cref="TransformProSpace" />.
        ///     If you attempt to update a scale when <see cref="CanChangeScale" /> is false, the request will be ignored.
        ///     Setting this property will update the bounds as required.
        /// </summary>
        public static Vector3 Scale
        {
            get
            {
                TransformPro.TryGetScale();
                return TransformPro.displayScale;
            }
            set
            {
                if (TransformPro.TrySetScale(value))
                {
                    TransformPro.SetBoundsDirtyWorld();
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current <see cref="Transform" /> managed by <see cref="TransformPro" />.
        ///     Setting this property will update the bounds as required.
        /// </summary>
        public static Transform Transform
        {
            get { return TransformPro.transform; }
            set
            {
                if (TransformPro.transform != value)
                {
                    TransformPro.transform = value;
                    if (TransformPro.transform == null)
                    {
                        TransformPro.bounds = null;
                    }
                    else
                    {
                        if (TransformPro.calculateBounds)
                        {
                            TransformPro.bounds = new TransformProBounds(TransformPro.transform);
                        }
                    }
                    TransformPro.UpdateDisplayTransform();
                }
            }
        }

        /// <summary>
        ///     Gets a string containing the current version of <see cref="TransformPro" />.
        /// </summary>
        public static string Version { get { return TransformPro.version; } }

        /// <summary>
        ///     Clones the GameObject for the currently selected Transform, retaining the same name and parent data.
        ///     The new transform will be automatically selected allowing for fast simple scene creation.
        /// </summary>
        /// <returns>The newly created transform.</returns>
        public static Transform Clone()
        {
            if (TransformPro.Transform == null)
            {
                Debug.LogWarning("[<color=red>TransformPro</color>] No transform selected.\nCannot clone.");
                return null;
            }

            GameObject gameObjectOld = TransformPro.Transform.gameObject;
            Transform transformOld = gameObjectOld.transform;

            GameObject gameObjectNew = Object.Instantiate(gameObjectOld);
            gameObjectNew.name = gameObjectOld.name; // Get rid of the (Clone)(Clone)(Clone)(Clone) madness

            Transform transformNew = gameObjectNew.transform;
            transformNew.SetParent(transformOld.parent);
            transformNew.localPosition = transformOld.localPosition;
            transformNew.localRotation = transformOld.localRotation;
            transformNew.localScale = transformOld.localScale;

            TransformPro.Transform = transformNew;
            return transformNew;
        }

        /// <summary>
        ///     Copy the current position display values.
        /// </summary>
        public static void CopyPosition()
        {
            TransformPro.ClipboardPosition = TransformPro.Position;
        }

        /// <summary>
        ///     Copy the current rotation display values.
        /// </summary>
        public static void CopyRotation()
        {
            TransformPro.ClipboardRotation = TransformPro.Rotation;
        }

        /// <summary>
        ///     Copy the current scale display values.
        /// </summary>
        public static void CopyScale()
        {
            TransformPro.ClipboardScale = TransformPro.Scale;
        }

        public static void LookAt(Vector3 position)
        {
            TransformPro.Transform.LookAt(position);
        }

        public static void LookAtClipboard()
        {
            TransformPro.LookAt(TransformPro.ClipboardPosition);
        }

        /// <summary>
        ///     Pastes the copied position to the curent <see cref="Transform" />.
        /// </summary>
        public static void PastePosition()
        {
            TransformPro.Position = TransformPro.ClipboardPosition;
        }

        /// <summary>
        ///     Pastes the copied rotation to the curent <see cref="Transform" />.
        /// </summary>
        public static void PasteRotation()
        {
            TransformPro.Rotation = TransformPro.ClipboardRotation;
        }

        /// <summary>
        ///     Pastes the copied scale to the curent <see cref="Transform" />.
        /// </summary>
        public static void PasteScale()
        {
            TransformPro.Scale = TransformPro.ClipboardScale;
        }

        /// <summary>
        ///     Resets the position, rotation and scale to default values.
        ///     If <see cref="Space" /> is set to <see cref="TransformProSpace.World" />, this will return the object
        ///     to the
        ///     origin.
        ///     If <see cref="Space" /> is set to <see cref="TransformProSpace.Local" />, this will return the object
        ///     to the
        ///     parent. If the object has no parent it will return to the origin.
        /// </summary>
        public static void Reset()
        {
            TransformPro.ResetPosition();
            TransformPro.ResetRotation();
            TransformPro.ResetScale();
        }

        /// <summary>
        ///     Resets the position to default values.
        ///     If <see cref="Space" /> is set to <see cref="TransformProSpace.World" />, this will return the object
        ///     to the
        ///     origin position.
        ///     If <see cref="Space" /> is set to <see cref="TransformProSpace.Local" />, this will return the object
        ///     to the
        ///     parent position. If the object has no parent it will return to the origin position.
        /// </summary>
        public static void ResetPosition()
        {
            TransformPro.Position = Vector3.zero;
        }

        /// <summary>
        ///     Resets the rotation to default values.
        ///     If <see cref="Space" /> is set to <see cref="TransformProSpace.World" />, this will return the object
        ///     to
        ///     world rotation, i.e. world Z is forward.
        ///     If <see cref="Space" /> is set to <see cref="TransformProSpace.Local" />, this will return the object
        ///     to
        ///     the parents rotation. If the object has no parent it will return to world rotation.
        /// </summary>
        public static void ResetRotation()
        {
            TransformPro.RotationEuler = Vector3.zero;
        }

        /// <summary>
        ///     Resets the scale to default values. The current scale system is always relative to the parent.
        /// </summary>
        public static void ResetScale()
        {
            TransformPro.Scale = Vector3.one;
        }

        /// <summary>
        ///     Forces the bounds to update. This helper method will ignore any calls made when the bounds are disabled.
        ///     Largely for internal use, but you can call this manually if you make changes to an actively tracked transform.
        /// </summary>
        public static void SetBoundsDirtyWorld()
        {
            if (TransformPro.Bounds != null)
            {
                TransformPro.Bounds.SetDirtyWorld();
            }
        }

        /// <summary>
        ///     Primarily for internal use. Reset the current values to the values held on the transform.
        /// </summary>
        public static void UpdateFromScene()
        {
            if (TransformPro.transform == null)
            {
                return;
            }

            TransformPro.UpdateDisplayTransform();
            TransformPro.SetBoundsDirtyWorld();
            TransformPro.transform.hasChanged = false;
        }

        /// <summary>
        ///     Helper function for comparing two <see cref="Vector3" /> values. Used to help reduce axis drift.
        /// </summary>
        /// <param name="v1">The left hand <see cref="Vector3" />.</param>
        /// <param name="v2">The right hand <see cref="Vector3" />.</param>
        /// <returns>A <see cref="bool" /> indicating if the two values are approximately the same.</returns>
        private static bool ApproximatelyEquals(Vector3 v1, Vector3 v2)
        {
            return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y) && Mathf.Approximately(v1.z, v2.z);
        }

        /// <summary>
        ///     Helper function for comparing two <see cref="Quaternion" /> values. Used to help reduce axis drift.
        /// </summary>
        /// <param name="q1">The left hand <see cref="Quaternion" />.</param>
        /// <param name="q2">The right hand <see cref="Quaternion" />.</param>
        /// <returns>A <see cref="bool" /> indicating if the two values are approximately the same.</returns>
        private static bool ApproximatelyEquals(Quaternion q1, Quaternion q2)
        {
            return Mathf.Approximately(q1.x, q2.x) && Mathf.Approximately(q1.y, q2.y) && Mathf.Approximately(q1.z, q2.z) && Mathf.Approximately(q1.w, q2.w);
        }

        /// <summary>
        ///     A helper function to get a <see cref="Quaternion" /> value from a <see cref="Matrix4x4" />.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix4x4" /> to get the <see cref="Quaternion" /> from.</param>
        /// <returns>The <see cref="Quaternion" /> contained with the <see cref="Matrix4x4" />.</returns>
        private static Quaternion GetQuaternion(Matrix4x4 matrix)
        {
            return Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        }

        /// <summary>
        ///     Gets the display position value out of the object, where possible.
        /// </summary>
        /// <returns>A <see cref="bool" /> value indicating if the operation was a success.</returns>
        private static bool TryGetPosition()
        {
            if (TransformPro.Transform == null)
            {
                return false;
            }

            switch (TransformPro.space)
            {
                default:
                    Debug.LogError(string.Format("[<color=red>TransformPro</color>] Space mode {0} not handled!", TransformPro.space));
                    return false;
                case TransformProSpace.Local:
                    if (!TransformPro.ApproximatelyEquals(TransformPro.displayPosition, TransformPro.Transform.localPosition))
                    {
                        TransformPro.displayPosition = TransformPro.Transform.localPosition;
                        return true;
                    }
                    return false;
                case TransformProSpace.World:
                    if (!TransformPro.ApproximatelyEquals(TransformPro.displayPosition, TransformPro.Transform.position))
                    {
                        TransformPro.displayPosition = TransformPro.Transform.position;
                        return true;
                    }
                    return false;
            }
        }

        /// <summary>
        ///     Gets the display rotation value out of the object, where possible.
        ///     Note this function is only invoked once after the <see cref="Transform" /> is updated.
        ///     Euler angles are tracked after that to prevent gimbal lock.
        /// </summary>
        /// <returns>A <see cref="bool" /> value indicating if the operation was a success.</returns>
        private static bool TryGetRotation()
        {
            if (TransformPro.transform == null)
            {
                return false;
            }

            switch (TransformPro.space)
            {
                default:
                    Debug.LogError(string.Format("[<color=red>TransformPro</color>] Space mode {0} not handled!", TransformPro.space));
                    return false;
                case TransformProSpace.Local:
                    TransformPro.displayRotation = TransformPro.transform.localEulerAngles;
                    return true;
                case TransformProSpace.World:
                    TransformPro.displayRotation = TransformPro.transform.eulerAngles;
                    return true;
            }
        }

        /// <summary>
        ///     Gets the display scale value out of the object, where possible.
        /// </summary>
        /// <returns>A <see cref="bool" /> value indicating if the operation was a success.</returns>
        private static bool TryGetScale()
        {
            if (TransformPro.transform == null)
            {
                return false;
            }

            switch (TransformPro.space)
            {
                default:
                    Debug.LogError(string.Format("[<color=red>TransformPro</color>] Space mode {0} not handled!", TransformPro.space));
                    return false;
                case TransformProSpace.Local:
                    if (!TransformPro.ApproximatelyEquals(TransformPro.displayScale, TransformPro.Transform.localScale))
                    {
                        TransformPro.displayScale = TransformPro.Transform.localScale;
                        return true;
                    }
                    return false;
                case TransformProSpace.World:
                    if (!TransformPro.ApproximatelyEquals(TransformPro.displayScale, TransformPro.Transform.lossyScale))
                    {
                        TransformPro.displayScale = TransformPro.Transform.lossyScale;
                        return true;
                    }
                    return false;
            }
        }

        private static bool TrySetPosition(Vector3 position)
        {
            if (!TransformPro.CanChangePosition)
            {
                return false;
            }
            // WARN: Must be careful with preventing values from being set.
            if (TransformPro.ApproximatelyEquals(TransformPro.displayPosition, position))
            {
                return false;
            }

            TransformPro.displayPosition = position;
            switch (TransformPro.space)
            {
                default:
                    Debug.LogError(string.Format("[<color=red>TransformPro</color>] Space mode {0} not handled!", TransformPro.space));
                    return false;
                case TransformProSpace.Local:
                    //TransformPro.Transform.localPosition = TransformPro.SnapPosition(position);
                    TransformPro.Transform.localPosition = position;
                    TransformPro.Transform.hasChanged = false;
                    return true;
                case TransformProSpace.World:
                    //TransformPro.Transform.position = TransformPro.SnapPosition(position);
                    TransformPro.Transform.position = position;
                    TransformPro.Transform.hasChanged = false;
                    return true;
            }
        }

        private static bool TrySetRotation(Vector3 value)
        {
            if (!TransformPro.CanChangeRotation)
            {
                return false;
            }

            TransformPro.SetBoundsDirtyWorld();
            switch (TransformPro.space)
            {
                default:
                    Debug.LogError(string.Format("[<color=red>TransformPro</color>] Space mode {0} not handled!", TransformPro.space));
                    return false;
                case TransformProSpace.Local:
                    TransformPro.Transform.localEulerAngles = value;
                    TransformPro.Transform.hasChanged = false;
                    return true;
                case TransformProSpace.World:
                    TransformPro.Transform.eulerAngles = value;
                    TransformPro.Transform.hasChanged = false;
                    return true;
            }
        }

        private static bool TrySetRotation(Quaternion rotation)
        {
            if (!TransformPro.CanChangeRotation)
            {
                return false;
            }

            TransformPro.SetBoundsDirtyWorld();
            switch (TransformPro.space)
            {
                default:
                    Debug.LogError(string.Format("[<color=red>TransformPro</color>] Space mode {0} not handled!", TransformPro.space));
                    return false;
                case TransformProSpace.Local:
                    TransformPro.Transform.localRotation = rotation;
                    TransformPro.Transform.hasChanged = false;
                    TransformPro.displayRotation = TransformPro.Transform.localEulerAngles;
                    return true;
                case TransformProSpace.World:
                    TransformPro.Transform.rotation = rotation;
                    TransformPro.Transform.hasChanged = false;
                    TransformPro.displayRotation = TransformPro.Transform.eulerAngles;
                    return true;
            }
        }

        private static bool TrySetScale(Vector3 scale)
        {
            if (!TransformPro.CanChangeScale)
            {
                return false;
            }
            // WARN: Must be careful with preventing values from being set.
            if (TransformPro.ApproximatelyEquals(TransformPro.displayScale, scale))
            {
                return false;
            }

            TransformPro.displayScale = scale;
            switch (TransformPro.space)
            {
                default:
                    Debug.LogError(string.Format("[<color=red>TransformPro</color>] Space mode {0} not handled!", TransformPro.space));
                    return false;
                case TransformProSpace.Local:
                    TransformPro.Transform.localScale = scale;
                    TransformPro.Transform.hasChanged = false;
                    return true;
                case TransformProSpace.World:
                    // TODO: Approximate the lossy value, convert it to a local scale and set it
                    return false;
            }
        }

        /// <summary>
        ///     Ensures the display values are correct for the current transform, in the current geometry space.
        ///     This is invoked automatically to make sure that change tracking is accurate.
        /// </summary>
        private static void UpdateDisplayTransform()
        {
            TransformPro.TryGetPosition();
            TransformPro.TryGetRotation();
            TransformPro.TryGetScale();
        }
    }
}
