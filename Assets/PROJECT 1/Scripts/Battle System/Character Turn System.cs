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
            // unitSpeed ���� �������� ��ü ����Ʈ�� �ʱ�ȭ
            Dictionary<float, System.Func<IEnumerator>> speedActions = new Dictionary<float, System.Func<IEnumerator>>
            {

            { BaseCharacterControl.instance.unitSpeed, () => BaseCharacterControl.instance.ExecuteAction() },
            { BaseEnemyControl.instance.unitSpeed, () => BaseEnemyControl.instance.ExecuteAction() }

            };

            // unitSpeed ���� ����Ʈ�� ��ȯ�Ͽ� ������������ ����
            List<float> speedList = speedActions.Keys.ToList();
            speedList.Sort((a, b) => b.CompareTo(a));

            // ���ĵ� ������� �ڷ�ƾ�� ����� �� ��ũ��Ʈ�� ���������� ����
            StartCoroutine(ExecuteActionsInTurn(speedList, speedActions));

            /*// ���ĵ� ������� �ش� ��ũ��Ʈ ����
            foreach (float speed in speedList)
            {
                Debug.Log("Executing action for unitSpeed: " + speed);
                speedActions[speed].Invoke(); // �� �ӵ��� �´� �׼� ����
            }*/


            /*float[] speedArr = new float[] { MeleeCharacterControl.instance.unitSpeed, EnemyControl.instance.unitSpeed};

            // ����Ʈ linq ���� ���

            // �迭�� ����Ʈ�� ��ȯ
            List<float> speedList = speedArr.ToList();

            // ����Ʈ�� ������������ ����
            speedList.Sort((a, b) => b.CompareTo(a));

            // ���ĵ� ����Ʈ�� �ٽ� �迭�� ��ȯ
            speedArr = speedList.ToArray();

            // ���ĵ� �迭�� ����Ͽ� Ȯ��
            foreach (float speed in speedArr)
            {
                Debug.Log(speed);
            }*/

            // ���̷���Ű ���� �ٲٱ�
            /*Transform childItem;
            transform.childCount;
            childItem.transform.SetSiblingIndex(0);*/
        }

        private IEnumerator ExecuteActionsInTurn(List<float> speedList, Dictionary<float, System.Func<IEnumerator>> speedActions)
        {
            foreach (float speed in speedList)
            {
                Debug.Log("Executing action for unitSpeed: " + speed);
                yield return StartCoroutine(speedActions[speed].Invoke()); // �� �ӵ��� �´� �׼� ������ ��ٸ�
            }
        }
    }
}
