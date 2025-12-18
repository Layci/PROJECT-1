using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ProJect1
{
    public class Teleporter : MonoBehaviour
    {
        public string teleporterId;      // 월드맵 아이콘과 연결될 고유 ID
        public float detectRadius;       // 힐 반경
        public Transform teleportPoint;  // 실제 순간이동될 위치
        public LayerMask playerMask;     // 플레이어 레이어

        private void Update()
        {
            Healplayer();
        }

        private void OnDrawGizmos()
        {
            // 텔레포트 지점 시각화 (투명도 30%)
            if (teleportPoint != null)
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
                Gizmos.DrawSphere(teleportPoint.position, 0.3f);
            }

            // 감지 반경 시각화 (투명도 50%)
            Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
            Gizmos.DrawSphere(transform.position, detectRadius);
        }

        void Healplayer()
        {
            // 감지 구체에 플레이어가 닿으면 힐
            Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, playerMask);
            var partys = PartyFormationManager.Instance.currentParty;
            foreach(var member in partys)
            {
                if (member.currentHP == member.maxHP)
                    return;
            }
            foreach(Collider col in hits)
            {
                PartyFormationManager.Instance.PartyHeal();
                UIManager.Instance.StartCoroutine("ShowHealMessege");
            }
        }
    }
}
