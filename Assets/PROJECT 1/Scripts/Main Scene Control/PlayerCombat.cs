using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class PlayerCombat : MonoBehaviour
    {
        public static PlayerCombat instance;

        public float detectRadius = 8f;          // 적 탐지 반경
        public LayerMask enemyMask;              // Enemy 레이어
        public MainSenceEnemy currentTarget;     // 현재 타겟
        public EnemyTargetUI targetUI;           // UI 프리팹 참조

        Camera cam;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            cam = Camera.main;
        }

        void Update()
        {
            DetectNearestEnemy();
        }


        // 가까운 적 감지
        void DetectNearestEnemy()
        {
            // 적을 감지할 구체 추가
            Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, enemyMask);

            // 적이 감지 안될 시 타겟UI 끄기
            if (hits.Length == 0)
            {
                ClearTarget();
                return;
            }

            float minDist = Mathf.Infinity;
            MainSenceEnemy nearest = null;

            // 적이 구체 안에 있을 시
            foreach (Collider col in hits)
            {
                // 플레이어 위치와 감지된 적의 거리를 저장
                float dist = Vector3.Distance(transform.position, col.transform.position);
                // 플레이어와 적의 거리가 최단거리 보다 가까우면
                if (dist < minDist)
                {
                    // minDist값을 dist로 업데이트
                    minDist = dist;
                    nearest = col.GetComponent<MainSenceEnemy>();
                }
            }

            // 현재 가장 가까운적이 현재 타겟된 적이 아니라면 타겟 적 업데이트
            if (nearest != currentTarget)
                SetTarget(nearest);
        }

        void SetTarget(MainSenceEnemy enemy)
        {
            currentTarget = enemy;

            if (enemy != null)
                targetUI.Show(enemy.transform);
        }

        void ClearTarget()
        {
            currentTarget = null;
            targetUI.Hide();
        }
    }
}
