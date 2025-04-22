using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    [System.Serializable]
    public class WaveData
    {
        public List<EnemySpawnInfo> enemiesToSpawn; // 이 웨이브에서 스폰할 적들
    }

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;     // Aki 프리팹 또는 Sapphi 프리팹
        public EnemyData enemyData;        // Aki 1, Aki 2, Sapphi 1 등
    }
}
