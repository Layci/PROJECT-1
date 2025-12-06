using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProJect1
{
    public class PreventDuplicateToggle : MonoBehaviour
    {
        public Toggle toggle;

        void Start()
        {
            // 초기 상태 = 켜짐(true)
            toggle.isOn = PartyFormationManager.Instance.preventDuplicate;

            toggle.onValueChanged.AddListener((value) =>
            {
                PartyFormationManager.Instance.preventDuplicate = value;
                Debug.Log("중복 선택 방지: " + (value ? "켜짐" : "꺼짐"));
            });
        }
    }
}
