using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class MainSceneManager : MonoBehaviour
    {
        public static MainSceneManager Instance;

        private HashSet<string> defeatedEnemies = new HashSet<string>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void MarkEnemyDefeated(string enemyID)
        {
            defeatedEnemies.Add(enemyID);
            Debug.Log($"Àû Ã³Ä¡µÊ: {enemyID}");
        }

        public bool IsEnemyDefeated(string enemyID)
        {
            return defeatedEnemies.Contains(enemyID);
        }
    }
}
