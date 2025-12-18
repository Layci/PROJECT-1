using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class WorldEnemyState : MonoBehaviour
    {
        private static HashSet<string> defeatedEnemies = new();

        // 적 처치 판정
        public static void MarkDefeated(string enemyID)
        {
            defeatedEnemies.Add(enemyID);
            Debug.Log($"적 처치됨: {enemyID}");
        }

        // 적이 처치 됐는지 감지
        public static bool IsDefeated(string enemyID)
        {
            return defeatedEnemies.Contains(enemyID);
        }
    }
}
