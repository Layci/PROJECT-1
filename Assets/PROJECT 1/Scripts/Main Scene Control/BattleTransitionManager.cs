using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProJect1
{
    public class BattleTransitionManager : MonoBehaviour
    {
        public static BattleTransitionManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void StartBattle(MainSenceEnemy enemy)
        {
            // 1. 웨이브 데이터 준비
            BattleSceneLoader.PrepareBattle(enemy);

            // 2. 연출 + 씬 전환
            StartCoroutine(BattleEnterRoutine());
        }

        private IEnumerator BattleEnterRoutine()
        {
            // 1. 페이드 아웃
            yield return ScreenFadeManager.Instance.FadeOut();

            // 2. 씬 로드
            AsyncOperation op =
                SceneManager.LoadSceneAsync("BattleScene");
            op.allowSceneActivation = false;

            yield return new WaitForSeconds(0.5f);
            op.allowSceneActivation = true;
        }
    }
}
