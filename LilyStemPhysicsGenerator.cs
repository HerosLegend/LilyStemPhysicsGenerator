using UnityEngine;
using BlindkatStudios.Components;

namespace BlindkatStudios.Generators {
    /// <summary>
    /// Provides functionality to dynamically generate and manage physics-related components on child objects of a
    /// GameObject, such as colliders, rigidbodies, and joints.
    /// </summary>
    /// <remarks>This class is designed to automate the setup of physics components for child objects in a
    /// hierarchical structure. It can be used to add, configure, and remove components such as <see
    /// cref="BoxCollider2D"/>, <see cref="Rigidbody2D"/>, <see cref="HingeJoint2D"/>, and custom components like
    /// <c>ApplyForceInDirection</c> and <c>TriggerEvents</c>.  The generated components are configured based on the
    /// serialized fields, such as <c>boxColliderSize</c>, <c>useLimitsOnHinge</c>, <c>hingeLowerLimit</c>, and
    /// <c>hingeUpperLimit</c>.</remarks>
    public class LilyStemPhysicsGenerator : MonoBehaviour {
        [Header("Component Settings")]
        [SerializeField, Tooltip("The box collider size generated on all bones.")]
        private Vector2 boxColliderSize = new Vector2(1f, 0.5f);
        [SerializeField, Tooltip("Should the HingeJoint2D use limits?")]
        private bool useLimitsOnHinge = true;
        [SerializeField, Tooltip("The lower limit to use on the HingeJoint2D")]
        private float hingeLowerLimit = -30f;
        [SerializeField, Tooltip("The higher limit to use on the HingeJoint2D")]
        private float hingeUpperLimit = 30f;

        /// <summary>
        /// Adds and configures a set of components on all child GameObjects of the current object.
        /// </summary>
        /// <remarks>This method iterates through all child objects of the current GameObject (excluding
        /// the root object itself) and adds the following components to each child: <see cref="BoxCollider2D"/>, <see
        /// cref="Rigidbody2D"/>, <see cref="HingeJoint2D"/>, <c>ApplyForceInDirection</c>, and <c>TriggerEvents</c>. 
        /// The components are then configured with specific properties, such as enabling triggers on the  <see
        /// cref="BoxCollider2D"/> and setting gravity scale on the <see cref="Rigidbody2D"/>.</remarks>
        public void GenerateComponentsOnChildren() {
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if (child == transform) continue; // Skip the root object

                child.gameObject.AddComponent<BoxCollider2D>();
                child.gameObject.AddComponent<Rigidbody2D>();
                child.gameObject.AddComponent<HingeJoint2D>();
                child.gameObject.AddComponent<ApplyForceInDirection>();
                child.gameObject.AddComponent<TriggerEvents>();

                // Configure BoxCollider2D
                BoxCollider2D boxCollider = child.gameObject.GetComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;
                boxCollider.size = boxColliderSize;

                // Configure Rigidbody2D
                Rigidbody2D rb = child.gameObject.GetComponent<Rigidbody2D>();
                rb.gravityScale = -1f;

                // Configure HingeJoint2D
                HingeJoint2D hinge = child.gameObject.GetComponent<HingeJoint2D>();
                hinge.autoConfigureConnectedAnchor = true;
                hinge.connectedBody = child.parent.GetComponent<Rigidbody2D>();
                hinge.useLimits = useLimitsOnHinge;
                hinge.limits = new JointAngleLimits2D {
                    min = hingeLowerLimit,
                    max = hingeUpperLimit
                };
            }
        }

        /// <summary>
        /// Updates the trigger event listeners for all child transforms of the current object.
        /// </summary>
        /// <remarks>This method recursively iterates through all child transforms of the current object,
        /// excluding the root transform, and updates their trigger event listeners. Hidden or inactive child objects
        /// are also included in the iteration.</remarks>
        private void UpdateTriggerEventListeners() {
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if (child == transform) continue; // Skip the root object
                UpdateTriggerEventListeners(child);
            }
        }

        /// <summary>
        /// Initializes the component and ensures that all child objects have their trigger event listeners updated.
        /// </summary>
        /// <remarks>This is needed to be done on Awake so the Event has reference to the TriggerEvents GameObject. 
        /// You may not see this properly updated in the inspector but it does work.</remarks>
        private void Awake() {
            UpdateTriggerEventListeners(); // Ensure all children have the TriggerEvents component updated on start
        }

        /// <summary>
        /// Updates the trigger event listeners for the specified child transform.
        /// </summary>
        /// <remarks>This method ensures that the child object has a <see cref="TriggerEvents"/> component
        /// and updates its <c>onTriggerEnter</c> event listeners. Existing listeners are cleared, and a new listener is
        /// added to apply a force to colliding objects using the <see cref="ApplyForceInDirection"/> component on the
        /// child object.</remarks>
        /// <param name="child">The child <see cref="Transform"/> whose trigger event listeners will be updated. Must not be null.</param>
        private void UpdateTriggerEventListeners(Transform child) {
            try {
                // Ensure the child has a TriggerEvents component
                TriggerEvents triggerEvents = child.gameObject.GetComponent<TriggerEvents>();
                triggerEvents.onTriggerEnter.RemoveAllListeners(); // Clear existing listeners
                triggerEvents.onTriggerEnter.AddListener((Collider2D collider) => {
                    ApplyForceInDirection applyForce = child.gameObject.GetComponent<ApplyForceInDirection>();
                    applyForce.ApplyForce(collider);
                });
            } catch (System.Exception ex) {
                Debug.LogError($"Error updating TriggerEvents for {child.name}: {ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Removes all specified components from the child objects of the current GameObject.
        /// </summary>
        /// <remarks>This method iterates through all child objects of the current GameObject, including
        /// inactive ones,  and removes the following components if they exist: <see cref="BoxCollider2D"/>, <see
        /// cref="HingeJoint2D"/>,  <see cref="Rigidbody2D"/>, <see cref="ApplyForceInDirection"/>, and <see
        /// cref="TriggerEvents"/>.  The root GameObject itself is excluded from this operation.</remarks>
        public void RemoveAllComponents() {
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if (child == transform) continue; // Skip the root object
                DestroyImmediate(child.gameObject.GetComponent<BoxCollider2D>());
                DestroyImmediate(child.gameObject.GetComponent<HingeJoint2D>());
                DestroyImmediate(child.gameObject.GetComponent<Rigidbody2D>());
                DestroyImmediate(child.gameObject.GetComponent<ApplyForceInDirection>());
                DestroyImmediate(child.gameObject.GetComponent<TriggerEvents>());
            }
        }
    }
}

