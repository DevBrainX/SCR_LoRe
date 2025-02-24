using UnityEngine;

public class TrainingSceneUi : MonoBehaviour
{
    void Start()
    {

    }

    public void OnClickBack()
    {
        //Time.timeScale = 1.0f;
        Managers.Scene.LoadScene(0);
    }
}
