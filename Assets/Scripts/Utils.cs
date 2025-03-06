using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool GetRandomBool()
    {
        int randomNumber = Random.Range(0, 2);
        return randomNumber == 1;
    }

    public static void ShuffleList<T>(List<T> _list)
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

    public static void ClearChild(GameObject _parentObj)
    {
        for (int i = _parentObj.transform.childCount - 1; i > -1; --i)
        {
            UnityEngine.Object.Destroy(_parentObj.transform.GetChild(i).gameObject);
        }
    }
}