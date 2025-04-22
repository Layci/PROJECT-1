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
        public int TotalWaveCount => waves.Count;

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
            }

            currentWaveIndex = waveIndex;
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
                // 스폰 위치가 부족하면 마지막 위치 재사용
                int spawnIndex = Mathf.Min(i, spawnPoints.Length - 1);

                GameObject enemyPrefab = wave.enemiesToSpawn[i];
                Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
            }

            currentWaveIndex = waveIndex;
        }*/

        public void StartNextWave()
        {
            SpawnWave(currentWaveIndex + 1);
        }
    }
}
