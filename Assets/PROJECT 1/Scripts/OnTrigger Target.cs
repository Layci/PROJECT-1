using Project1;
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
            instance = this;
        }

        private void OnTriggerEnter(Collider other)
        {
            /*var character = other.GetComponent<BaseEnemyControl>();
            character.TakeDamage(20);*/

            switch (other.gameObject.tag)
            {
                case "MeleePlayer":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;

                case "FayePlayer":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;

                case "InugamiPlayer":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;

                case "HoshiPlayer":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;

                case "MeleeEnemy":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;

                case "Aki Enemy":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;

                case "AI Enemy":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;

                case "Enemy":
                    targetObject = other.gameObject;
                    Debug.Log(other.gameObject.tag);
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            targetObject = null;
        }
    }
}
