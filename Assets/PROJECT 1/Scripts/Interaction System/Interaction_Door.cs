using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Project1
{
    public class Interaction_Door : MonoBehaviour, IInteractable
    {
        public Vector3 openRotation;
        public Vector3 closeRotation;

        public bool isOpen = false;
        public PlayableDirector cinemaDirector;
        public List<GameObject> OnActiveGameObjectsWhenOpen;

        public string Key => "Door." + gameObject.GetHashCode();
        public string Message => isOpen ? "Close" : "Open";

        private bool isFirstInteract = true;

        void Start()
        {
            cinemaDirector.stopped += (PlayableDirector controller) => 
            {
                controller.gameObject.SetActive(false);
            };
        }

        public void Interact()
        {
            isOpen = !isOpen;

            if (isFirstInteract)
            {
                isFirstInteract = false;
                OnActiveGameObjectsWhenOpen.ForEach(go => go.SetActive(true));
            }
        }

        private void Update()
        {
            if (isOpen)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(openRotation), Time.deltaTime * 5f);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(closeRotation), Time.deltaTime * 5f);
            }
        }
    }
}
