using UnityEngine;

public class TutorialSceneUi : MonoBehaviour
{
    void Start()
    {
        Managers.Ui.tutorialUi = this;
    }

    public void OnClickBack()
    {
        //Time.timeScale = 1.0f;
        Managers.Scene.LoadScene(0);
    }
}
