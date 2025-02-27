using TMPro;
using UnityEngine;

public class TrainingSceneUi : MonoBehaviour
{
    [SerializeField] GameObject startPage;
    [SerializeField] GameObject roundPage;

    [SerializeField] GameObject checkButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backButton;

    [SerializeField] TextMeshProUGUI checkText;

    void Start()
    {
        Managers.Ui.trainingUi = this;

        SetActiveStartPage();
    }

    public void SetActiveButton(int _state)
    {
        if (_state == 0) // 처음 시작한 상태
        {
            checkButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else // 정답을 체크한 상태
        {
            checkButton.SetActive(false);
            nextButton.SetActive(true);
        }
    }

    public void SetActiveStartPage()
    {
        startPage.SetActive(true);
        roundPage.SetActive(false);
    }

    public void SetActiveRoundPage()
    {
        startPage.SetActive(false);
        roundPage.SetActive(true);
    }

    public void OnClickStart()
    {
        SetActiveRoundPage();

        Managers.Game.StartRound();
    }

    public void OnClickCheck()
    {
        Managers.Game.CheckAnswer();
    }

    public void OnClickNext()
    {
        Managers.Game.StartRound();
    }

    public void OnClickBack()
    {
        Managers.Scene.LoadScene(0);
    }

    public void SetCheckText(string _text)
    {
        checkText.text = _text;
        //checkText.color = Color.white;
    }
}
