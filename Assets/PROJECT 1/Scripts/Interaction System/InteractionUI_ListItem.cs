using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class InteractionUI_ListItem : MonoBehaviour
    {
        public string DataKey
        {
            set => key = value;
            get => key;
        }

        public string Message
        {
            set => text.text = value;
        }

        public bool IsSelected
        {
            set => selection.SetActive(value);
        }

        public IInteractable InteractionData
        {
            set => interactionData = value;
            get => interactionData;
        }

        public GameObject selection;
        public TMPro.TextMeshProUGUI text;

        private IInteractable interactionData;
        private string key;
    }
}
