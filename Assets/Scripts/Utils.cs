using UnityEngine;

public static class Utils
{
    //public static bool GetRandomBool()
    //{
    //    int randomNumber = Random.Range(0, 2);
    //    return randomNumber == 1;
    //}

    public static void ClearList(GameObject listObject)
    {
        for (int i = listObject.transform.childCount - 1; i > -1; --i)
        {
            UnityEngine.Object.Destroy(listObject.transform.GetChild(i).gameObject);
        }
    }
}