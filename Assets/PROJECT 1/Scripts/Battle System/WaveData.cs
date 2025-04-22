using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    [System.Serializable]
    public class WaveData
    {
        public List<EnemySpawnInfo> enemiesToSpawn; // �� ���̺꿡�� ������ ����
    }

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;     // Aki ������ �Ǵ� Sapphi ������
        public EnemyData enemyData;        // Aki 1, Aki 2, Sapphi 1 ��
    }
}
