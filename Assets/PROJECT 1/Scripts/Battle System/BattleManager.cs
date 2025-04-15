using Project1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProJect1
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager instance;

        public Vector3 startPlayerPos = new Vector3(-3, 0, 2); // ���� ��ġ
        public Vector3 startEnemyPos = new Vector3(-3, 0, 3);
        public float spacing = 2f; // ���� �� ����

        public List<BaseCharacterControl> playerCharacters = new List<BaseCharacterControl>();
        public List<BaseEnemyControl> enemyCharacters = new List<BaseEnemyControl>();

        private void Awake()
        {
            // �̱��� �ʱ�ȭ
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            RefreshUnitLists();
        }

        public void RefreshUnitLists()
        {
            playerCharacters = FindObjectsOfType<BaseCharacterControl>().ToList();
            enemyCharacters = FindObjectsOfType<BaseEnemyControl>().ToList();

            Debug.Log($"[UnitManager] �Ʊ� {playerCharacters.Count}��, ���� {enemyCharacters.Count}�� �߰ߵ�.");
        }

        public void RepositionPlayerUnits()
        {
            List<BaseCharacterControl> alivePlayers = playerCharacters
                .Where(p => p.curHealth > 0)
                .ToList();

            for (int i = 0; i < alivePlayers.Count; i++)
            {
                Vector3 newPos = startPlayerPos + new Vector3(spacing * i, 0, 0);
                alivePlayers[i].transform.position = newPos;
            }
        }

        public void RepositionEnemyUnits()
        {
            List<BaseEnemyControl> aliveEnemies = enemyCharacters
                .Where(e => e.curHealth > 0)
                .ToList();

            for (int i = 0; i < aliveEnemies.Count; i++)
            {
                Vector3 newPos = startEnemyPos + new Vector3(spacing * i, 0, 0);
                aliveEnemies[i].transform.position = newPos;
            }
        }
    }
}
