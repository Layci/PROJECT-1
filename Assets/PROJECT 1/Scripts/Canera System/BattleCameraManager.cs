using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class BattleCameraManager : MonoBehaviour
    {
        public static BattleCameraManager Instance;

        [Header("Virtual Cameras")]
        public CinemachineVirtualCamera defaultCam;
        public CinemachineVirtualCamera healCam;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            SetPriority(defaultCam);
        }

        public void SwitchToDefault()
        {
            SetPriority(defaultCam);
        }

        public void SwitchToHeal()
        {
            SetPriority(healCam);
        }

        void SetPriority(CinemachineVirtualCamera target)
        {
            defaultCam.Priority = 0;
            healCam.Priority = 0;

            target.Priority = 10;
        }
    }
}
