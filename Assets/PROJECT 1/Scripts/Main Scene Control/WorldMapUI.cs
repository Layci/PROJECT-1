using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class WorldMapUI : MonoBehaviour
    {
        public static WorldMapUI Instance;

        public Dictionary<string, Teleporter> teleporterDict = new Dictionary<string, Teleporter>();
        public GameObject player;
        Teleporter t;

        private void Awake()
        {
            Instance = this;

            // 모든 Teleporter 자동 탐색 후 등록
            Teleporter[] teleporters = FindObjectsOfType<Teleporter>();

            foreach (var t in teleporters)
            {
                if (!teleporterDict.ContainsKey(t.teleporterId))
                    teleporterDict.Add(t.teleporterId, t);
            }
        }

        public void TeleportTo(string teleporterId)
        {
            if (!teleporterDict.ContainsKey(teleporterId))
            {
                Debug.LogError($"Teleporter ID '{teleporterId}' not found!");
                return;
            }

            Teleporter t = teleporterDict[teleporterId];
            UIManager.Instance.CloseTopUI();
            StartCoroutine(TeleportRoutine(t));
        }

        private IEnumerator TeleportRoutine(Teleporter t)
        {
            // 1. 입력 차단 (선택)
            MainPlayerControl.instance.inputBlocked = true;

            // 2. 페이드 아웃
            yield return ScreenFadeManager.Instance.FadeOut();

            // 3. 위치 이동
            player.transform.position = t.teleportPoint.position;

            // 4. 한 프레임 대기 (안정성)
            yield return null;

            // 5. 페이드 인
            yield return ScreenFadeManager.Instance.FadeIn();

            // 6. 입력 복구
            MainPlayerControl.instance.inputBlocked = false;
        }
    }
}
