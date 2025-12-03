using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class TeleporterIconUI : MonoBehaviour
    {
        public string teleporterId;      // Teleporter와 연결될 ID
        private Button btn;

        private void Awake()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(OnIconClicked);
        }

        void OnIconClicked()
        {
            WorldMapUI.Instance.TeleportTo(teleporterId);
        }
    }
}
