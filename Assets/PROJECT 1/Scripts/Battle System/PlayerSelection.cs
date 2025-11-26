using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log($"[DEBUG AOE] centerIndex={centerIndex}, leftBound={leftBound}, rightBound={rightBound}, playerCount={turnSystem.playerCharacters.Count}");
            return targets;
        }

        /*public List<GameObject> GetAOETargetsFromEnemy(int range, int enemyIndex)
        {
            List<GameObject> targets = new List<GameObject>();
            var turnSystem = FindObjectOfType<TurnSystem>();

            if (turnSystem == null || turnSystem.playerCharacters.Count == 0)
            {
                Debug.LogError("[PlayerSelection] 플레이어 리스트가 비어있음!");
                return targets;
            }

            // 현재 적의 타겟 중심을 인덱스로 기준
            int leftBound = Mathf.Max(0, enemyIndex - range);
            int rightBound = Mathf.Min(turnSystem.playerCharacters.Count - 1, enemyIndex + range);

            for (int i = leftBound; i <= rightBound; i++)
            {
                var player = turnSystem.playerCharacters[i];
                if (player != null)
                    targets.Add(player.gameObject);
            }

            Debug.Log($"[PlayerSelection] 적 인덱스 {enemyIndex}, 범위 {range}, 결과 대상 수: {targets.Count}");
            return targets;
        }*/
        /*public List<GameObject> GetAOETargets(int range)
        {
            List<GameObject> targets = new List<GameObject>();

            // 단순 예시: 첫 번째 플레이어 기준으로 range만큼의 인덱스 범위를 반환
            int centerIndex = 0; // 나중에 타겟 인덱스 기준으로 변경 가능

            for (int i = -range; i <= range; i++)
            {
                int index = centerIndex + i;
                if (index >= 0 && index < playerList.Count)
                {
                    targets.Add(playerList[index]);
                    Debug.Log("윤곽선");
                }
            }

            return targets;
        }*/

        // 특정 범위(range)만큼 대상 리스트 반환
        /*public List<BaseCharacterControl> GetAOETargets(int range)
        {
            List<BaseCharacterControl> targets = new List<BaseCharacterControl>();

            // 현재 타겟 인덱스를 TurnSystem에서 가져올 수도 있음
            int currentTargetIndex = TurnSystem.instance.currentPlayerTargetIndex;
            var playerList = TurnSystem.instance.playerCharacters;

            if (playerList == null || playerList.Count == 0)
                return targets;

            // 중심 타겟 포함
            targets.Add(playerList[currentTargetIndex]);

            // 좌우 범위 적용
            for (int i = 1; i <= range; i++)
            {
                if (currentTargetIndex - i >= 0)
                    targets.Add(playerList[currentTargetIndex - i]);
                if (currentTargetIndex + i < playerList.Count)
                    targets.Add(playerList[currentTargetIndex + i]);
            }

            return targets;
        }

        public void HighlightAOETargets(int range)
        {
            var targets = GetAOETargets(range);

            foreach (var player in targets)
            {
                if (player != null)
                {
                    // 하이라이트 표시용 UI나 셰이더 등 활성화
                    //player.ShowHighlightEffect(true);
                }
            }
        }*/
    }
}
