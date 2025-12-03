using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class Teleporter : MonoBehaviour
    {
        public string teleporterId;   // 월드맵 아이콘과 연결될 고유 ID
        public Transform teleportPoint;  // 실제 순간이동될 위치

        private void OnDrawGizmos()
        {
            // 씬에서 위치 확인용
            Gizmos.color = Color.cyan;
            if (teleportPoint != null)
                Gizmos.DrawSphere(teleportPoint.position, 0.3f);
        }
    }
}
