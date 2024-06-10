using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class Interaction_NPC : MonoBehaviour, IInteractable
    {
        public string Key => "NPC." + gameObject.GetHashCode();
        public string Message => "Talk";

        public void Interact()
        {

        }
    }
}
