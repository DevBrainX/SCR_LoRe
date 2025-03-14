using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindAnyObjectByType<BaseScene>(); } }

    public void LoadScene(int _index)
    {
        CurrentScene.Clear();

        SceneManager.LoadScene(_index);
    }
}
