using UnityEngine;
using UnityEngine.Events;

namespace BlindkatStudios.Components {
    /// <summary>
    /// Provides Unity event hooks for 2D trigger interactions, allowing custom behavior to be executed when trigger
    /// events occur on a <see cref="Collider2D"/> component.
    /// </summary>
    /// <remarks>This class requires a <see cref="Collider2D"/> component to be attached to the same
    /// GameObject. The <see cref="Collider2D.isTrigger"/> property will be automatically set to <see langword="true"/> 
    /// during the <see cref="Awake"/> method to ensure trigger functionality.</remarks>
    public class TriggerEvents : MonoBehaviour {
        [Tooltip("Events to call when Unity OnTriggerEnter is called.")]
        public UnityEvent<Collider2D> onTriggerEnter;
        [Tooltip("Events to call when Unity OnTriggerExit is called.")]
        private UnityEvent<Collider2D> onTriggerExit;
        [Tooltip("Events to call when Unity OnTriggerStay is called.")]
        private UnityEvent<Collider2D> onTriggerStay;

        public void Awake() {
            try {
                Collider2D collider = GetComponent<Collider2D>();
                collider.isTrigger = true;
            } catch {
                Debug.LogError($"{GetType().Name} requires a Collider2D component to function properly.");
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log($"{GetType().Name} triggered by {other.name}");
            onTriggerEnter?.Invoke(other);  
        }

        private void OnTriggerExit2D(Collider2D other) {
            onTriggerExit?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other) {
            onTriggerStay?.Invoke(other);
        }
    }
}

