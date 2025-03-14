using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
{
    public static bool GetRandomBool()
    {
        int randomNumber = Random.Range(0, 2);
        return randomNumber == 1;
    }

    public static void ShuffleList<T>(this List<T> _list)
    {
        System.Random rng = new System.Random();
        int n = _list.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = _list[n];
            _list[n] = _list[k];
            _list[k] = temp;
        }
    }

    public static void ClearList<T>(this List<T> _list)
    {
        if (_list == null)
        {
            return;
        }

        // 리스트의 개수를 체크
        if (_list.Count > 0)
        {
            // null이 아닌 요소들만 남기기
            _list.RemoveAll(item => item == null);
        }
    }

    public static void ClearChild(GameObject _parentObj)
    {
        for (int i = _parentObj.transform.childCount - 1; i > -1; --i)
        {
            UnityEngine.Object.Destroy(_parentObj.transform.GetChild(i).gameObject);
        }
    }
}