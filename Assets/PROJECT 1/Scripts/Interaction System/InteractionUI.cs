using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class InteractionUI : MonoBehaviour
    {
        public static InteractionUI Instance { get; private set; } = null;


        public Transform root;
        public InteractionUI_ListItem itemPrefab;

        private List<InteractionUI_ListItem> createdItems = new List<InteractionUI_ListItem>();
        private int selectedIndex = 0;

        private void Awake()
        {
            Instance = this;
            itemPrefab.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void AddInteractionData(IInteractable interactableData)
        {
            var newListItem = Instantiate(itemPrefab, root);
            newListItem.gameObject.SetActive(true);
            
            newListItem.DataKey = interactableData.Key;
            newListItem.Message = interactableData.Message;
            newListItem.InteractionData = interactableData;
            newListItem.IsSelected = false;

            createdItems.Add(newListItem);
        }

        public void RemoveInteractionData(IInteractable interactableData)
        {
            var targetItem = createdItems.Find(x => x.DataKey.Equals(interactableData.Key));
            if(targetItem != null)
            {
                createdItems.Remove(targetItem);
                Destroy(targetItem.gameObject);
            }
        }

        public void SelectPrev()
        {
            if (createdItems.Count > 0)
            {
                if (selectedIndex >= 0 && selectedIndex < createdItems.Count)
                {
                    createdItems[selectedIndex].IsSelected = false;
                }

                selectedIndex--;
                selectedIndex = Mathf.Clamp(selectedIndex, 0, createdItems.Count - 1);
                createdItems[selectedIndex].IsSelected = true;
            }
        }

        public void SelectNext()
        {
            if (createdItems.Count > 0)
            {
                if (selectedIndex >= 0 && selectedIndex < createdItems.Count)
                { 
                    createdItems[selectedIndex].IsSelected = false;
                }

                selectedIndex++;
                selectedIndex = Mathf.Clamp(selectedIndex, 0, createdItems.Count - 1);
                createdItems[selectedIndex].IsSelected = true;
            }
        }

        public void DoInteract()
        {
            if (createdItems.Count > 0 && selectedIndex >= 0)
            {
                createdItems[selectedIndex].InteractionData.Interact();
            }
        }
    }
}
