using UnityEngine;

public class TrainingScene : BaseScene
{
    //[SerializeField] UiManager uiManager;
    //[SerializeField] ObjectManager objectManager;
    //[SerializeField] Player player;
    //[SerializeField] RingGauge ringGauge;

    //[SerializeField] BoxBehaviour answerBox;
    [SerializeField] Transform questionTrans;
    [SerializeField] Transform choiceTrans;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        index = 2;

        //Managers.Instance.SetUiManager(uiManager);
        //Managers.Instance.SetObjectManager(objectManager);

        //Managers.Game.answerBox = answerBox;
        Managers.Game.questionTrans = questionTrans;
        Managers.Game.choiceTrans = choiceTrans;

        //Managers.Game.Init();

        Managers.Game.StartRound();
    }

    void Update()
    {

    }

    public override void Clear()
    {

    }
}
