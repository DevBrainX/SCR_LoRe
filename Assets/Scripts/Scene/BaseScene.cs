using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public int index;

    public abstract void Init();

    public abstract void Clear();
}
