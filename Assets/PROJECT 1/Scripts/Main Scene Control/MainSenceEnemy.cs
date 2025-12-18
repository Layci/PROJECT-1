using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class MainSenceEnemy : MonoBehaviour
    {
        // 이 적을 때렸을 때 진입할 웨이브 구성
        public List<WaveData> waves;

        public string enemyID;

        void Start()
        {
            if (WorldEnemyState.IsDefeated(enemyID))
            {
                gameObject.SetActive(false);
            }
        }

        public void OnPlayerEnterBattle()
        {
            BattleContext.enemyID = enemyID;
            BattleTransitionManager.Instance.StartBattle(this);
            //BattleSceneLoader.PrepareBattle(this);
        }
    }
}
