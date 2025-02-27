using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    //public static bool GetRandomBool()
    //{
    //    int randomNumber = Random.Range(0, 2);
    //    return randomNumber == 1;
    //}

    public static void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }

    public static void ClearList(GameObject listObject)
    {
        for (int i = listObject.transform.childCount - 1; i > -1; --i)
        {
            UnityEngine.Object.Destroy(listObject.transform.GetChild(i).gameObject);
        }
    }
}