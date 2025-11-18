using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProJect1
{
    public class BattleSceneLoader
    {
        public static List<WaveData> pendingWaves;

        public static void LoadBattle(MainSenceEnemy enemy)
        {
            pendingWaves = enemy.waves;   // 메인씬 적의 웨이브 가져오기
            SceneManager.LoadScene("BattleScene");
        }
    }
}
