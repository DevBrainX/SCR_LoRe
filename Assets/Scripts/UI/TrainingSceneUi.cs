using TMPro;
using UnityEngine;

public class TrainingSceneUi : MonoBehaviour
{
    //
    [SerializeField] GameObject startPage;
    [SerializeField] GameObject roundPage;
    [SerializeField] GameObject finishPage;

    [SerializeField] TextMeshProUGUI roundText;
    
    //
    [SerializeField] GameObject checkButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backButton;

    [SerializeField] TextMeshProUGUI checkText;
    [SerializeField] TextMeshProUGUI progressText;

    //
    [SerializeField] TextMeshProUGUI finishText;

    void Start()
    {
        Managers.Ui.trainingUi = this;

        SetPage(0);
    }

    public void SetPage(int _page)
    {
        if (_page == 0) // startPage
        {
            startPage.SetActive(true);
            roundPage.SetActive(false);
            finishPage.SetActive(false);

            SetRoundText();
        }
        else if (_page == 1) // roundPage
        {
            startPage.SetActive(false);
            roundPage.SetActive(true);
            finishPage.SetActive(false);
        }
        else if (_page == 2) // finishPage
        {
            startPage.SetActive(false);
            roundPage.SetActive(false);
            finishPage.SetActive(true);
        }
    }

    public void SetActiveNextButton(bool _hasCheckAnswer)
    {
        if (_hasCheckAnswer == false) // 정답 체크 안한 상태
        {
            checkButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else // 정답 체크한 상태
        {
            checkButton.SetActive(false);
            nextButton.SetActive(true);
        }
    }

    public void OnClickStart()
    {
        SetPage(1);

        Managers.Game.InitRound();
        Managers.Game.StartRound();
    }

    public void OnClickCheck()
    {
        Managers.Game.CheckAnswer();
    }

    public void OnClickNextQuestion()
    {
        Managers.Game.StartRound();
    }

    public void OnClickBack()
    {
        Managers.Scene.LoadScene(0);
    }

    public void OnClickNextRound()
    {
        SetPage(0);
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

    public void SetFinishText(int _state)
    {
        if (_state == 0)
        {
            finishText.text = string.Format("좀 더 노력하세요.\n이전 라운드로 계속 진행합니다.");
        }
        else if(_state == 1)
        {
            finishText.text = string.Format("좀 더 노력하세요.\n현재 라운드로 계속 진행합니다.");
        }
        else if (_state == 2)
        {
            finishText.text = string.Format("잘 하셨습니다.\n다음 라운드로 계속 진행합니다.");
        }
    }
}
