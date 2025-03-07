using UnityEngine;

public class TrainingScene : BaseScene
{
    [SerializeField] GameObject startPage;
    [SerializeField] GameObject roundPage;

    [SerializeField] GameObject questionField;
    [SerializeField] GameObject choiceField;

    [SerializeField] Sprite back_1x6;
    [SerializeField] Sprite back_1x7;
    [SerializeField] Sprite back_3x3;

    void Start()
    {
        index = 2;

        Init();
    }

    public override void Init()
    {
        Managers.Game.trainingScene = this;

        startPage.SetActive(false);
        roundPage.SetActive(false);
        Managers.Game.startPage = startPage;
        Managers.Game.roundPage = roundPage;

        questionField.SetActive(false);
        choiceField.SetActive(false);
        Managers.Game.questionField = questionField;
        Managers.Game.choiceField = choiceField;
    }

    public void SetFieldSprite(GameObject _fieldObj, FieldType _type)
    {
        if (_type == FieldType._1x7)
        {
            _fieldObj.GetComponent<SpriteRenderer>().sprite = back_1x7;
        }
        else if (_type == FieldType._3x3)
        {
            _fieldObj.GetComponent<SpriteRenderer>().sprite = back_3x3;
        }
        else if (_type == FieldType._1x6)
        {
            _fieldObj.GetComponent<SpriteRenderer>().sprite = back_1x6;
        }
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
