using TMPro;
using UnityEngine;

public class TrainingSceneUi : MonoBehaviour
{
    [SerializeField] GameObject startPage;
    [SerializeField] GameObject roundPage;

    [SerializeField] TextMeshProUGUI roundText;

    [SerializeField] GameObject checkButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backButton;

    [SerializeField] TextMeshProUGUI checkText;
    [SerializeField] TextMeshProUGUI progressText;

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

        SetRoundText();
    }

    public void SetActiveRoundPage()
    {
        startPage.SetActive(false);
        roundPage.SetActive(true);
    }

    public void OnClickStart()
    {
        SetActiveRoundPage();

        Managers.Game.InitRound();
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

    public void SetRoundText()
    {
        roundText.text = string.Format("Round {0}", Managers.Game.currentRound);
    }

    public void SetCheckText(string _text)
    {
        if(_text == string.Empty)
        {
            checkText.gameObject.SetActive(false);
        }
        else
        {
            checkText.gameObject.SetActive(true);
            checkText.text = _text;
        }
    }

    public void SetProgressText()
    {
        progressText.text = string.Format("Total: {0}\nCorrect: {1}\nWrong: {2}", 
            Managers.Game.totalQuestionCount, Managers.Game.correctCount, Managers.Game.wrongCount);
    }
}
