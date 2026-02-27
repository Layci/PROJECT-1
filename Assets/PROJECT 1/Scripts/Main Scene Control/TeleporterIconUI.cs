using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class TeleporterIconUI : MonoBehaviour
    {
        public string teleporterId;
        private Button btn;

        private void Awake()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(OnIconClicked);
        }

        public void OnIconClicked()
        {
            UIManager.Instance.OpenTeleportPopup(teleporterId);
        }
    }
}
