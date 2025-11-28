using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class PlayerCombat : MonoBehaviour
    {
        public float detectRadius = 8f;          // 적 탐지 반경
        public LayerMask enemyMask;              // Enemy 레이어
        public MainSenceEnemy currentTarget;     // 현재 타겟
        public EnemyTargetUI targetUI;           // UI 프리팹 참조
        public static PlayerCombat instance;

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

        void DetectNearestEnemy()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, enemyMask);

            if (hits.Length == 0)
            {
                ClearTarget();
                return;
            }

            float minDist = Mathf.Infinity;
            MainSenceEnemy nearest = null;

            foreach (Collider col in hits)
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = col.GetComponent<MainSenceEnemy>();
                }
            }

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
