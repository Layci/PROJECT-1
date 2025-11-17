using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyTargetUI : MonoBehaviour
    {
        public RectTransform uiTransform;
        Camera cam;
        Transform target;

        void Start()
        {
            cam = Camera.main;
            Hide();
        }

        void Update()
        {
            if (target == null) return;

            Vector3 screenPos = cam.WorldToScreenPoint(target.position);
            uiTransform.position = screenPos;
        }

        public void Show(Transform target)
        {
            this.target = target;
            uiTransform.gameObject.SetActive(true);
        }

        public void Hide()
        {
            target = null;
            uiTransform.gameObject.SetActive(false);
        }
    }
}
