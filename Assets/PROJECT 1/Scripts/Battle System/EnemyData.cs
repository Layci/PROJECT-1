using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string enemyName;
        public float maxHealth;
        public float unitSpeed;
        public float attackPower;
        public float skillAttackPower;
        //public float attackRange;
        public GameObject prefab;
    }
}
