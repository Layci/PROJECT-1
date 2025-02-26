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

        // 새로운 캐릭터가 추가될 때 리스트 갱신
        public void UpdateCharacterLists()
        {
            players.Clear();
            enemies.Clear();

            players.AddRange(FindObjectsOfType<BaseCharacterControl>());
            enemies.AddRange(FindObjectsOfType<BaseEnemyControl>());

            // X축 기준 정렬
            players.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
            enemies.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        }

        // 특정 플레이어에게 버프 적용
        public void ApplyBuffToPlayer(BaseCharacterControl player, Buff buff)
        {
            if (!players.Contains(player))
                return;

            player.AddBuff(buff);
            Debug.Log($"{player.name}에게 {buff.buffName} 버프 적용!");
        }

        // 특정 적에게 버프 적용
        public void ApplyBuffToEnemy(BaseEnemyControl enemy, Buff buff)
        {
            if (!enemies.Contains(enemy))
                return;

            enemy.AddBuff(buff);
            Debug.Log($"{enemy.name}에게 {buff.buffName} 버프 적용!");
        }

        // 전투 도중 새로 추가된 적 처리
        public void OnEnemySpawned(BaseEnemyControl newEnemy)
        {
            enemies.Add(newEnemy);
            enemies.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        }

        // 버프가 종료될 때 제거 (턴이 끝날 때 체크)
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
