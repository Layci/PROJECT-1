using Project1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProJect1
{
    public class AllySelection : MonoBehaviour
    {
        public static AllySelection instance;

        public int selectedIndex = 0;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            // 힐 준비 상태일 때만 동작
            var cur = TurnSystem.instance.allCharacters[TurnSystem.instance.currentTurnIndex] as BaseCharacterControl;

            if (cur == null) return;

            if (cur.prepareState != AttackPrepareState.Skill)
                return;

            if (!cur.isHealSkill)
                return;

            bool moved = false;

            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedIndex++;
                moved = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                selectedIndex--;
                moved = true;
            }

            if (moved)
            {
                var players = TurnSystem.instance.playerCharacters;

                if (selectedIndex < 0)
                    selectedIndex = players.Count - 1;
                if (selectedIndex >= players.Count)
                    selectedIndex = 0;

                UpdateSelectedAlly();
            }
        }

        public void UpdateSelectedAlly()
        {
            var players = TurnSystem.instance.playerCharacters;
            if (players.Count == 0) return;

            if (selectedIndex < 0 || selectedIndex >= players.Count)
                selectedIndex = 0;

            var cur = TurnSystem.instance.allCharacters[
                TurnSystem.instance.currentTurnIndex
            ] as BaseCharacterControl;

            if (cur == null) return;

            int range = cur.skillAttackRange;

            var targets = GetTargets(range)
                .Select(p => p.transform)
                .ToList();

            AllySelectorUI.instance.ShowTargets(targets);
        }

        public List<BaseUnit> GetTargets(int range)
        {
            var targets = new List<BaseUnit>();
            var players = TurnSystem.instance.playerCharacters;

            int start = Mathf.Max(0, selectedIndex - range);
            int end = Mathf.Min(players.Count - 1, selectedIndex + range);

            for (int i = start; i <= end; i++)
                targets.Add(players[i]);

            return targets;
        }

        public BaseUnit GetAnchorTarget()
        {
            if (TurnSystem.instance.playerCharacters.Count == 0) return null;
            return TurnSystem.instance.playerCharacters[selectedIndex];
        }
    }
}
