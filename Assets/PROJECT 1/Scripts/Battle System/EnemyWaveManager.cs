using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public List<WaveData> waves; // 여러 웨이브
        public Transform[] spawnPoints; // 스폰 위치들

        private int currentWaveIndex = 0;
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

            SpawnWave(currentWaveIndex);
        }

        public void SpawnWave(int waveIndex)
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
                control.ApplyEnemyData();

                enemyGO.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
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
