using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Project1
{
    public class GameManager : MonoBehaviour
    {
        private List<IUnit> units;

        private void Start()
        {
            // 씬에서 모든 유닛을 찾고 리스트에 추가
            units = new List<IUnit>(FindObjectsOfType<MonoBehaviour>().OfType<IUnit>());

            // 초기 턴 실행
            StartCoroutine(RunTurns());
        }

        private IEnumerator RunTurns()
        {
            while (true)
            {
                // unitSpeed에 따라 유닛들을 내림차순으로 정렬
                units.Sort((x, y) => y.UnitSpeed.CompareTo(x.UnitSpeed));

                // 각 유닛이 순서대로 턴을 수행
                foreach (var unit in units)
                {
                    unit.TakeTurn();
                    yield return new WaitForSeconds(1.0f); // 턴 사이의 딜레이를 추가
                }

                // 다음 턴까지 대기 (원하는 대기 시간 설정)
                yield return new WaitForSeconds(2.0f);
            }
        }
    }
}