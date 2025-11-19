using Project1;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public List<WaveData> waves; // 여러 웨이브
        public Transform[] spawnPoints; // 스폰 위치들
        public static Action OnWaveSpawned; // 웨이브 스폰 완료 이벤트

        private int currentWaveIndex = 0;
        private bool hasSpawnedInitial = false; // 초기 스폰 중복 방지
        public int TotalWaveCount => waves != null ? waves.Count : 0;
        //public int TotalWaveCount => waves.Count;

        void Start()
        {
            waves = BattleSceneLoader.pendingWaves;

            if (waves == null || waves.Count == 0)
            {
                Debug.LogError("전달받은 웨이브 데이터가 없습니다!");
                return;
            }

            // 초기 웨이브는 한 번만 스폰 (중복 방지 플래그 체크)
            if (!hasSpawnedInitial)
            {
                SpawnWave(currentWaveIndex);
                hasSpawnedInitial = true;
            }
            //SpawnWave(currentWaveIndex);
        }

        public void SpawnWave(int waveIndex)
        {
            if (waves == null || waves.Count == 0)
            {
                Debug.LogWarning("SpawnWave 호출되었지만 waves가 비어있습니다.");
                return;
            }

            // 범위 검사
            if (waveIndex < 0 || waveIndex >= waves.Count)
            {
                Debug.Log("모든 웨이브 완료 또는 잘못된 인덱스입니다!");
                return;
            }

            // 현재 웨이브 인덱스 업데이트
            currentWaveIndex = waveIndex;

            WaveData wave = waves[waveIndex];

            for (int i = 0; i < wave.enemiesToSpawn.Count; i++)
            {
                int spawnIndex = Mathf.Min(i, spawnPoints.Length - 1);
                EnemySpawnInfo info = wave.enemiesToSpawn[i];

                GameObject enemyGO = Instantiate(info.enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
                BaseEnemyControl control = enemyGO.GetComponent<BaseEnemyControl>();
                control.enemyData = info.enemyData;
                control.ApplyEnemyData();

                enemyGO.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            hasSpawnedInitial = true; // 최초 스폰 완료 플래그 (또는 최근 스폰 완료 플래그)

            // 모든 스폰이 끝났음을 알림 (TurnSystem이 이 이벤트를 구독해야 함)
            OnWaveSpawned?.Invoke();
        }

        /*public void SpawnWave(int waveIndex)
        {
            if (waveIndex >= waves.Count)
            {
                Debug.Log("모든 웨이브 완료!");
                return;
            }

            WaveData wave = waves[waveIndex];

            for (int i = 0; i < wave.enemiesToSpawn.Count; i++)
            {
                int spawnIndex = Mathf.Min(i, spawnPoints.Length - 1);
                EnemySpawnInfo info = wave.enemiesToSpawn[i];

                GameObject enemyGO = Instantiate(info.enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
                BaseEnemyControl control = enemyGO.GetComponent<BaseEnemyControl>();
                control.enemyData = info.enemyData;
                control.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                control.ApplyEnemyData();
            }

            currentWaveIndex = waveIndex;
        }*/


        public void StartNextWave()
        {
            SpawnWave(currentWaveIndex + 1);
        }
    }
}
