using Project1;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProJect1
{
    public class CharacterTurnSystem : MonoBehaviour
    {
        public bool nextTurn = false;

        private void Start()
        {
            // unitSpeed 값을 가져오는 객체 리스트를 초기화
            Dictionary<float, System.Func<IEnumerator>> speedActions = new Dictionary<float, System.Func<IEnumerator>>
            {

            { BaseCharacterControl.instance.unitSpeed, () => BaseCharacterControl.instance.ExecuteAction() },
            { BaseEnemyControl.instance.unitSpeed, () => BaseEnemyControl.instance.ExecuteAction() }

            };

            // unitSpeed 값을 리스트로 변환하여 내림차순으로 정렬
            List<float> speedList = speedActions.Keys.ToList();
            speedList.Sort((a, b) => b.CompareTo(a));

            // 정렬된 순서대로 코루틴을 사용해 각 스크립트를 순차적으로 실행
            StartCoroutine(ExecuteActionsInTurn(speedList, speedActions));

            /*// 정렬된 순서대로 해당 스크립트 실행
            foreach (float speed in speedList)
            {
                Debug.Log("Executing action for unitSpeed: " + speed);
                speedActions[speed].Invoke(); // 각 속도에 맞는 액션 실행
            }*/


            /*float[] speedArr = new float[] { MeleeCharacterControl.instance.unitSpeed, EnemyControl.instance.unitSpeed};

            // 리스트 linq 정렬 방식

            // 배열을 리스트로 변환
            List<float> speedList = speedArr.ToList();

            // 리스트를 내림차순으로 정렬
            speedList.Sort((a, b) => b.CompareTo(a));

            // 정렬된 리스트를 다시 배열로 변환
            speedArr = speedList.ToArray();

            // 정렬된 배열을 출력하여 확인
            foreach (float speed in speedArr)
            {
                Debug.Log(speed);
            }*/

            // 하이러라키 순서 바꾸기
            /*Transform childItem;
            transform.childCount;
            childItem.transform.SetSiblingIndex(0);*/
        }

        private IEnumerator ExecuteActionsInTurn(List<float> speedList, Dictionary<float, System.Func<IEnumerator>> speedActions)
        {
            foreach (float speed in speedList)
            {
                Debug.Log("Executing action for unitSpeed: " + speed);
                yield return StartCoroutine(speedActions[speed].Invoke()); // 각 속도에 맞는 액션 실행을 기다림
            }
        }
    }
}
