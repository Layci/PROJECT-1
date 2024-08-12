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
            // ������ ��� ������ ã�� ����Ʈ�� �߰�
            units = new List<IUnit>(FindObjectsOfType<MonoBehaviour>().OfType<IUnit>());

            // �ʱ� �� ����
            StartCoroutine(RunTurns());
        }

        private IEnumerator RunTurns()
        {
            while (true)
            {
                // unitSpeed�� ���� ���ֵ��� ������������ ����
                units.Sort((x, y) => y.UnitSpeed.CompareTo(x.UnitSpeed));

                // �� ������ ������� ���� ����
                foreach (var unit in units)
                {
                    unit.TakeTurn();
                    yield return new WaitForSeconds(1.0f); // �� ������ �����̸� �߰�
                }

                // ���� �ϱ��� ��� (���ϴ� ��� �ð� ����)
                yield return new WaitForSeconds(2.0f);
            }
        }
    }
}