using UnityEngine;

public class LobbySceneUi : MonoBehaviour
{
    void Start()
    {

    }

    public void OnClickStartTutotial()
    {
        //Time.timeScale = 1.0f;
        Managers.Scene.LoadScene(1);
    }

    public void OnClickStartTraining()
    {
        //Time.timeScale = 1.0f;
        Managers.Scene.LoadScene(2);
    }

    //public void OnClickBack()
    //{
    //    // Managers.Audio.PlayAudio(Sound.ButtonClick, false);
    //    gameExplainPopUpUi.SetActive(false);
    //}

    //public void OnClickQuit()
    //{
    //    // Managers.Audio.PlayAudio(Sound.ButtonClick, false);
    //    Managers.Quit();
    //}
}