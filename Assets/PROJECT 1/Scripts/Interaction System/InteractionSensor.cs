using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class InteractionSensor : MonoBehaviour
    {
        // 이 코드랑 같은거임 public bool HasInteractable2 { get { return interactables.Count > 0; } }
        public bool HasInteractable => interactables.Count > 0;

        public List<IInteractable> interactables = new List<IInteractable>();

        public System.Action<IInteractable> OnDetected;
        public System.Action<IInteractable> OnLost;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent(out IInteractable interactable))
            {
                interactables.Add(interactable);

                OnDetected?.Invoke(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.TryGetComponent(out IInteractable interactable))
            {
                interactables.Remove(interactable);

                OnLost?.Invoke(interactable);
            }
        }
    }
}
