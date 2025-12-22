using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class ProjectileEffectBase : MonoBehaviour
    {
        public RFX1_Target rfxTarget;

        public void Play(Vector3 startPos, BaseUnit target)
        {
            transform.position = startPos;
            rfxTarget.Target = target.gameObject;
            gameObject.SetActive(true);
        }
    }
}
