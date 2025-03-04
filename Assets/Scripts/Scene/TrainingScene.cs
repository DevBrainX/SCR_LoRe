using UnityEngine;

public class TrainingScene : BaseScene
{
    [SerializeField] GameObject startPage;
    [SerializeField] GameObject roundPage;

    [SerializeField] Transform questionTrans;
    [SerializeField] Transform choiceTrans;

    void Start()
    {
        index = 2;

        Init();
    }

    public override void Init()
    {
        startPage.SetActive(false);
        roundPage.SetActive(false);
        Managers.Game.startPage = startPage;
        Managers.Game.roundPage = roundPage;

        questionTrans.gameObject.SetActive(false);
        choiceTrans.gameObject.SetActive(false);
        Managers.Game.questionTrans = questionTrans;
        Managers.Game.choiceTrans = choiceTrans;
    }

    //public void SetActiveStartPage()
    //{
    //    startPage.SetActive(true);
    //    roundPage.SetActive(false);
    //}

    //public void SetActiveRoundPage()
    //{
    //    startPage.SetActive(false);
    //    roundPage.SetActive(true);
    //}

    void Update()
    {

    }

    public override void Clear()
    {

    }
}
