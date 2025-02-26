using System;
using System.Collections.Generic;
using UnityEngine;
using Project1;

namespace Project1
{
    public class BuffManager : MonoBehaviour
    {
        /*private List<BaseCharacterControl> players = new List<BaseCharacterControl>();
        private List<BaseEnemyControl> enemies = new List<BaseEnemyControl>();

        private void Start()
        {
            UpdateCharacterLists();
        }

        // ���ο� ĳ���Ͱ� �߰��� �� ����Ʈ ����
        public void UpdateCharacterLists()
        {
            players.Clear();
            enemies.Clear();

            players.AddRange(FindObjectsOfType<BaseCharacterControl>());
            enemies.AddRange(FindObjectsOfType<BaseEnemyControl>());

            // X�� ���� ����
            players.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
            enemies.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        }

        // Ư�� �÷��̾�� ���� ����
        public void ApplyBuffToPlayer(BaseCharacterControl player, Buff buff)
        {
            if (!players.Contains(player))
                return;

            player.AddBuff(buff);
            Debug.Log($"{player.name}���� {buff.buffName} ���� ����!");
        }

        // Ư�� ������ ���� ����
        public void ApplyBuffToEnemy(BaseEnemyControl enemy, Buff buff)
        {
            if (!enemies.Contains(enemy))
                return;

            enemy.AddBuff(buff);
            Debug.Log($"{enemy.name}���� {buff.buffName} ���� ����!");
        }

        // ���� ���� ���� �߰��� �� ó��
        public void OnEnemySpawned(BaseEnemyControl newEnemy)
        {
            enemies.Add(newEnemy);
            enemies.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        }

        // ������ ����� �� ���� (���� ���� �� üũ)
        public void RemoveExpiredBuffs()
        {
            foreach (var player in players)
            {
                player.RemoveExpiredBuffs();
            }
            foreach (var enemy in enemies)
            {
                enemy.RemoveExpiredBuffs();
            }
        }*/
    }
}
