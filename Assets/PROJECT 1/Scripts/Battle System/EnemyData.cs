using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string enemyName;
        public Sprite enemyIcon;
        public float maxHealth;
        public float moveSpeed;
        public float attackPower;
        public float skillAttackPower;
        public float unitSpeed;
        public float attackRange;
    }
}
