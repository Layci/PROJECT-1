using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public List<WaveData> waves; // ���� ���̺�
        public Transform[] spawnPoints; // ���� ��ġ��

        private int currentWaveIndex = 0;
        public int TotalWaveCount => waves.Count;

        public void SpawnWave(int waveIndex)
        {
            if (waveIndex >= waves.Count)
            {
                Debug.Log("��� ���̺� �Ϸ�!");
                return;
            }

            WaveData wave = waves[waveIndex];

            for (int i = 0; i < wave.enemiesToSpawn.Count; i++)
            {
                // ���� ��ġ�� �����ϸ� ������ ��ġ ����
                int spawnIndex = Mathf.Min(i, spawnPoints.Length - 1);

                GameObject enemyPrefab = wave.enemiesToSpawn[i];
                Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
            }

            currentWaveIndex = waveIndex;
        }

        public void StartNextWave()
        {
            SpawnWave(currentWaveIndex + 1);
        }
    }
}
