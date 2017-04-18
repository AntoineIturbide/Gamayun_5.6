namespace UntitledGames.Transforms
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    ///     Represents the various <see cref="Bounds" /> for <see cref="TransformPro" />.
    ///     Used to display the bounds in the viewport, and by <see cref="TransformPro.Ground" />.
    /// </summary>
    public class TransformProBounds
    {
        private static readonly Bounds empty = new Bounds(Vector3.zero, Vector3.zero);
        private Bounds collider;
        private Vector3 colliderOffset;
        private IEnumerable<Collider> colliders;
        private bool collidersDirty;
        private IEnumerable<MeshFilter> meshFilters;
        private Bounds renderer;
        private Vector3 rendererOffset;
        private IEnumerable<Renderer> renderers;
        private bool renderersDirty;
        private Transform transform;

        /// <summary>
        ///     Creates an instance of <see cref="TransformProBounds" />, and stores a reference to the <see cref="Transform" /> it
        ///     relates to.
        /// </summary>
        /// <param name="transform">The <see cref="Transform" /> this bounds object relates to.</param>
        public TransformProBounds(Transform transform)
        {
            this.transform = transform;
        }

        /// <summary>
        ///     Gets the current world space <see cref="Bounds" /> object for the combined <see cref="UnityEngine.Collider" /> data
        ///     of this transform.
        ///     This includes any <see cref="UnityEngine.Collider" /> components on the transform or it's children.
        ///     The data will only be created the first time the getter is accessed, and only updated if the
        ///     <see cref="collidersDirty" /> flag is set, by calling <see cref="SetDirtyWorld" />.
        /// </summary>
        public Bounds Collider
        {
            get
            {
                // Now we get the list of colliders every time and check to see if it's changed. This should be vastly simpler to keep track of, 
                // at a relatively slightly performance cost.
                Collider[] colliders = this.Transform.GetComponentsInChildren<Collider>();
                if ((this.Colliders == null) ||
                    this.Colliders.Any(x => x == null) ||
                    !colliders.OrderBy(t => t.GetInstanceID()).SequenceEqual(this.Colliders.OrderBy(t => t.GetInstanceID())))
                {
                    this.colliders = colliders;
                    this.collidersDirty = true;
                }

                if (!this.collidersDirty)
                {
                    this.collider.center = this.transform.position + this.colliderOffset;
                    return this.collider;
                }
                this.collidersDirty = false;

                bool found = false;
                foreach (Collider collider in this.Colliders)
                {
                    // Safety net - should not be possible, but causes the entire scene view to go black and a wall of errors.
                    if (collider == null)
                    {
                        this.collidersDirty = true;
                        continue;
                    }

                    if (!found)
                    {
                        this.collider = new Bounds(collider.transform.position, Vector3.zero);
                        found = true;
                    }

                    Bounds bounds = collider.bounds;

                    WheelCollider wheelCollider = collider as WheelCollider;
                    if (wheelCollider != null)
                    {
                        float size = wheelCollider.radius * 2 * wheelCollider.transform.lossyScale.y;
                        bounds = new Bounds(collider.bounds.center, new Vector3(0.1f, size, size));
                    }

                    this.collider.Encapsulate(bounds);
                }

                if (!found)
                {
                    return TransformProBounds.empty;
                }

                this.colliderOffset = this.collider.center - this.Transform.position;

                this.collider.center = this.transform.position + this.colliderOffset;
                return this.collider;
            }
        }

        /// <summary>
        ///     Gets the current offset from the transform position to the center of the <see cref="Collider" /> bounds.
        /// </summary>
        public Vector3 ColliderOffset { get { return this.colliderOffset; } }

        public IEnumerable<Collider> Colliders { get { return this.colliders; } }

        /// <summary>
        ///     Returns true if the currently selected <see cref="Transform" /> has any child colliders. If no
        ///     <see cref="Transform" /> is selected or bounds are disabled false will be returned.
        /// </summary>
        public bool HasColliders { get { return (this.Colliders != null) && this.Colliders.Any(); } }

        /// <summary>
        ///     Returns true if the currently selected <see cref="Transform" /> has any child <see cref="MeshFilter" /> components.
        ///     If no <see cref="Transform" /> is selected or bounds are disabled false will be returned.
        /// </summary>
        public bool HasMeshFilters { get { return (this.MeshFilters != null) && this.MeshFilters.Any(); } }

        /// <summary>
        ///     Returns true if the currently selected <see cref="Transform" /> has any child renderers. If no
        ///     <see cref="Transform" /> is selected or bounds are disabled false will be returned.
        /// </summary>
        public bool HasRenderers { get { return (this.Renderers != null) && this.Renderers.Any(); } }

        /// <summary>
        ///     Gets a list of all <see cref="MeshFilter" /> objects owned by this transform. This includes the current transform
        ///     and and children.
        ///     This data is only created the first time this getter is accessed.
        /// </summary>
        public IEnumerable<MeshFilter> MeshFilters
        {
            get
            {
                MeshFilter[] meshFilters = this.Transform.GetComponentsInChildren<MeshFilter>();
                if ((this.meshFilters == null) ||
                    this.meshFilters.Any(x => x == null) ||
                    !meshFilters.OrderBy(t => t.GetInstanceID()).SequenceEqual(this.meshFilters.OrderBy(t => t.GetInstanceID())))
                {
                    this.meshFilters = meshFilters;
                }

                return this.meshFilters;
            }
        }

        /// <summary>
        ///     Gets the current world space <see cref="Bounds" /> object for the combined <see cref="UnityEngine.Renderer" /> data
        ///     of this
        ///     transform.
        ///     This includes any <see cref="UnityEngine.Renderer" /> components on the transform or it's children.
        ///     The data will only be created the first time the getter is accessed, and only updated if the
        ///     <see cref="renderersDirty" /> flag is set, by calling <see cref="SetDirtyWorld" />.
        /// </summary>
        public Bounds Renderer
        {
            get
            {
                Renderer[] renderers = this.Transform.GetComponentsInChildren<Renderer>();
                if ((this.Renderers == null) ||
                    this.Renderers.Any(x => x == null) ||
                    !renderers.OrderBy(t => t.GetInstanceID()).SequenceEqual(this.Renderers.OrderBy(t => t.GetInstanceID())))
                {
                    this.renderers = renderers;
                    this.renderersDirty = true;
                }

                if (!this.renderersDirty)
                {
                    this.renderer.center = this.transform.position + this.rendererOffset;
                    return this.renderer;
                }
                this.renderersDirty = false;

                bool found = false;
                foreach (Renderer renderer in this.Renderers)
                {
                    // Safety net - should not be possible, but causes the entire scene view to go black and a wall of errors.
                    if (renderer == null)
                    {
                        this.renderersDirty = true;
                        continue;
                    }

                    if (!found)
                    {
                        this.renderer = new Bounds(renderer.transform.position, Vector3.zero);
                        found = true;
                    }

                    this.renderer.Encapsulate(renderer.bounds);
                }
                if (!found)
                {
                    return TransformProBounds.empty;
                }

                this.rendererOffset = this.renderer.center - this.Transform.position;

                this.renderer.center = this.transform.position + this.rendererOffset;
                return this.renderer;
            }
        }

        /// <summary>
        ///     Gets the current offset from the transform position to the center of the <see cref="Renderer" /> bounds.
        /// </summary>
        public Vector3 RendererOffset { get { return this.rendererOffset; } }

        public IEnumerable<Renderer> Renderers { get { return this.renderers; } }

        /// <summary>
        ///     Gets the <see cref="Transform" /> that this bounds object refers to.
        /// </summary>
        public Transform Transform { get { return this.transform; } }

        /// <summary>
        ///     Call this method to flag the object as dirty. The next time you access <see cref="Collider" /> or
        ///     <see cref="Renderer" />, the bounds will be recalculated.
        /// </summary>
        public void SetDirtyWorld()
        {
            this.collidersDirty = true;
            this.renderersDirty = true;
        }
    }
}
