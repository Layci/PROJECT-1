using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProJect1
{
    public class CharacterTurnSystem : MonoBehaviour
    {
        private void Start()
        {
            float[] speedArr = new float[] { MeleeCharacterControl.instance.unitSpeed, EnemyControl.instance.unitSpeed, 10, 50, 21};

            // 리스트 linq 정렬 방식
            List<float> container = new List<float>(speedArr);
            container = (from num in container orderby num ascending select num).ToList();
            container.Sort(MySortAlgorithm);
            container.OrderByDescending(x => x);

            // 하이러라키 순서 바꾸기
            /*Transform childItem;
            transform.childCount;
            childItem.transform.SetSiblingIndex(0);*/


            for (float i = 0; i < speedArr.Length; i++)
            {
                Debug.Log(speedArr[(int)i]);
            }

            //Debug.Log(container);

            //InsertionLong(speedArr);
        }

        private int MySortAlgorithm(float a, float b)
        {
            if (a > b)
                return 1;
            else
            {
                if (a == b)
                    return 0;
            }

            return -1;
        }

        void InsertionLong(float[] speedArr)
        {
            float i, j, key;

            for (i = 0; i < speedArr.Length; i++)
            {
                key = speedArr[(int)i];

                for (j = i - 1; (j >= 0) && (speedArr[(int)j] > key); j--)
                {
                    speedArr[(int)(j + 1)] = speedArr[(int)j];
                }

                speedArr[(int)(j + 1)] = key;
            }

            PrintArray(speedArr);
        }

        void PrintArray(float[] speedArr)
        {
            for (float i = 0; i < speedArr.Length; i++)
            {
                Debug.Log(speedArr[(int)i]);
            }
        }
    }
}
