using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace ProJect1
{
    public class OnTriggerTarget : MonoBehaviour
    {
        public static OnTriggerTarget instance;

        public GameObject targetObject;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "MeleePlayer":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;

                case "MeleeEnemy":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;
            }
        }
    }
}
