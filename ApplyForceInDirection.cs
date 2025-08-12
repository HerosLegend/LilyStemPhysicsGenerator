using BlindkatStudios.Movement;
using UnityEngine;

namespace BlindkatStudios.Components {
    /// <summary>
    /// Provides functionality to apply force to a 2D object in a specified direction, either based on collision data or
    /// a predefined compass direction.
    /// </summary>
    /// <remarks>This class is designed to be used with Unity's physics system. It requires the object to have
    /// both a <see cref="Collider2D"/> and a <see cref="Rigidbody2D"/> component. The force can be applied in response
    /// to collision or trigger events, or in a predefined compass direction if <c>useCompassDirection</c> is
    /// enabled.</remarks>
    public class ApplyForceInDirection : MonoBehaviour {
        [SerializeField, Tooltip("Use CompassDirection to specify the direction. This will override collision data.")]
        private bool useCompassDirection = false;
        [SerializeField, Tooltip("The direction to apply the force in.")]
        private CompassDirection.Direction direction = CompassDirection.Direction.right;
        [SerializeField, Tooltip("Amount of force to apply.")]
        private float force = 10f;

        private Vector2 forceDirection = Vector2.zero;

        public void Awake() {
            try {
                Collider2D collider = GetComponent<Collider2D>();
            } catch (System.Exception ex) {
                Debug.LogError($"{GetType().Name} encountered an error: {ex.Message}");
            }
            try {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
            } catch (System.Exception ex) {
                Debug.LogError($"{GetType().Name} encountered an error while checking for Rigidbody2D: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies a force to the object based on the direction of the collision. FOR COLLISION EVENTS
        /// <param name="collision">The collision data used to determine the direction of the applied force. Cannot be null.</param>
        public void ApplyForce(Collision2D collision) {
            forceDirection = transform.position - collision.transform.position;
            ApplyForce();
        }
        /// <summary>
        /// Applies a force to the object based on the direction of the collider. FOR TRIGGER EVENTS
        /// </summary>
        /// <param name="collider">The 2D collider to which the force will be applied. Cannot be null.</param>
        public void ApplyForce(Collider2D collider) {
            forceDirection = transform.position - collider.transform.position;
            ApplyForce();
        }

        /// <summary>
        /// Applies a force in the specified direction, either based on a compass direction or a custom force vector.
        /// </summary>
        /// <remarks>If <c>useCompassDirection</c> is <see langword="true"/>, the force is applied using a
        /// predefined compass direction. Otherwise, the force is applied using the normalized value of the
        /// <c>forceDirection</c> vector.</remarks>
        private void ApplyForce() {
            if (useCompassDirection) {
                ApplyForce(CompassDirection.DirectionVectors[(int)direction].normalized);
            } else {
                ApplyForce(forceDirection.normalized);
            }
        }

        /// <summary>
        /// Applies a force to the object's Rigidbody2D in the specified direction.
        /// </summary>
        /// <remarks>This method requires the object to have a <see cref="Rigidbody2D"/> component.  The
        /// force is applied using the <see cref="ForceMode2D.Impulse"/> mode, which applies an instant change in
        /// velocity.</remarks>
        /// <param name="forceDirection">The direction of the force to be applied, represented as a <see cref="Vector2"/>.  The vector should be
        /// normalized to ensure the force magnitude is applied correctly.</param>
        private void ApplyForce(Vector2 forceDirection) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(forceDirection * force, ForceMode2D.Impulse);
        }
    }
}
