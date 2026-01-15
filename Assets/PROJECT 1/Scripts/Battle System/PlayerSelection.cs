using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 아군 타겟
namespace ProJect1
{
    public class PlayerSelection : MonoBehaviour
    {
        public static PlayerSelection instance;
        public List<GameObject> playerList = new List<GameObject>();

        private void Awake()
        {
            instance = this;
        }

        public void RegisterPlayer(GameObject player)
        {
            if (!playerList.Contains(player))
                playerList.Add(player);
        }

        public void UnregisterPlayer(GameObject player)
        {
            if (playerList.Contains(player))
                playerList.Remove(player);
        }

        public List<GameObject> GetAOETargetsByIndex(int centerIndex, int range)
        {
            var targets = new List<GameObject>();

            var turnSystem = TurnSystem.instance;
            if (turnSystem == null) return targets;

            var players = turnSystem.playerCharacters;

            for (int i = 0; i < players.Count; i++)
            {
                if (Mathf.Abs(i - centerIndex) <= range)
                    targets.Add(players[i].gameObject);
            }

            Debug.Log($"[PlayerSelection] GetAOETargetsByIndex 결과: {targets.Count}명");
            return targets;
        }

        public List<GameObject> GetAOETargetsFromEnemy(int range, int centerIndex)
        {
            List<GameObject> targets = new List<GameObject>();
            var turnSystem = FindObjectOfType<TurnSystem>();

            if (turnSystem == null || turnSystem.playerCharacters.Count == 0)
            {
                Debug.LogError("[PlayerSelection] 플레이어 리스트가 비어있습니다!");
                return targets;
            }

            int leftBound = Mathf.Max(0, centerIndex - range);
            int rightBound = Mathf.Min(turnSystem.playerCharacters.Count - 1, centerIndex + range);

            for (int i = leftBound; i <= rightBound; i++)
            {
                var player = turnSystem.playerCharacters[i];
                if (player != null)
                    targets.Add(player.gameObject);
            }
            Debug.Log($"[Enemy] centerIndex = {centerIndex}, range = {range}, leftBound~rightBound = {leftBound}~{rightBound}");
            Debug.Log($"[PlayerSelection] 타겟 인덱스 {centerIndex}, 범위 {range}, 결과 대상 수: {targets.Count}");
            return targets;
        }
    }
}
