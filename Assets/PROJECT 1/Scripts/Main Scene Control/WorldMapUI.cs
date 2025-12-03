using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class WorldMapUI : MonoBehaviour
    {
        public static WorldMapUI Instance;

        private Dictionary<string, Teleporter> teleporterDict = new Dictionary<string, Teleporter>();
        public GameObject player;

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

            player.transform.position = t.teleportPoint.position;
        }
    }
}
