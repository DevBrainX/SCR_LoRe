using UnityEngine;

public class TrainingScene : BaseScene
{
    [SerializeField] Transform questionTrans;
    [SerializeField] Transform choiceTrans;

    void Start()
    {
        index = 2;

        Init();
    }

    public override void Init()
    {
        Managers.Game.questionTrans = questionTrans;
        Managers.Game.choiceTrans = choiceTrans;

        //Managers.Game.Init();

        //Managers.Game.StartRound();

        //Managers.Ui.trainingUi.SetActiveStartPage();

        questionTrans.gameObject.SetActive(false);
        choiceTrans.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    public override void Clear()
    {

    }
}
