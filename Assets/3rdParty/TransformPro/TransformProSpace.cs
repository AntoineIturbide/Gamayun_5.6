// Copyright(c) 2017 Untitled Games   | Developed by: Chris Bellini                    | http://untitledgam.es/contact
// http://transformpro.untitledgam.es | http://transformpro.untitledgam.es/quick-start | http://transformpro.untitledgam.es/api

namespace UntitledGames.Transforms
{
#if UNITY_EDITOR
    using System;
    using UnityEngine;

#else
    using System;
    using UnityEngine;
#endif

    /// <summary>
    ///     Represents the transform space the system operates under.
    /// </summary>
    [Serializable]
    public enum TransformProSpace
    {
        /// <summary>
        ///     Local space operates relative to the selected transform. Forward represents the Z axis of the object.
        /// </summary>
        Local = 0,

        /// <summary>
        ///     World space is the global coordinate system. Forward represents the global Z axis.
        /// </summary>
        World = 1
    }

    public static partial class TransformPro
    {
        private static TransformProPivot pivot = TransformProPivot.Object;
        private static TransformProSpace space = TransformProSpace.Local;

        public static Vector3 HandlePosition { get { return TransformPro.PositionWorld; } }

        public static Quaternion HandleRotation
        {
            get
            {
                switch (TransformPro.Space)
                {
                    default:
                    case TransformProSpace.World:
                        return Quaternion.identity;
                    case TransformProSpace.Local:
                        return TransformPro.RotationWorld;
                }
            }
        }

        public static TransformProPivot Pivot
        {
            get { return TransformPro.pivot; }
            set
            {
                TransformPro.pivot = value;
                TransformPro.UpdateDisplayTransform();
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="TransformProSpace" /> currently being used. If this value is changed all display
        ///     values will be recalculated.
        /// </summary>
        public static TransformProSpace Space
        {
            get { return TransformPro.space; }
            set
            {
                if (TransformPro.space == value)
                {
                    return;
                }
                TransformPro.space = value;

                /*
                switch (value)
                {
                    default:
                    case TransformProSpace.World:
                        Tools.pivotRotation = PivotRotation.Global;
                        break;
                    case TransformProSpace.Local:
                        Tools.pivotRotation = PivotRotation.Local;
                        break;
                }
                */

                TransformPro.UpdateDisplayTransform();
            }
        }
    }
}