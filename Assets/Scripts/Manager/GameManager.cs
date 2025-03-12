using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


//public enum ObjectCategory
//{
//    Carnivore = 0,  // 동물-육식
//    Herbivore,      // 동물-초식
//    Fish,           // 해양-물고기
//    MarineMammal,    // 해양-포유류
//}

public class GameManager : MonoBehaviour
{
    public TrainingScene trainingScene;

    [SerializeField] GameObject boxPrefab;

    public GameObject startPage;
    public GameObject roundPage;

    public GameObject questionField;
    public GameObject choiceField;

    //List<SpriteData> spriteDataList;
    public List<SpriteData> prevSpriteDataList;

    List<BoxBehaviour> questionBoxList;
    List<BoxBehaviour> choiceBoxList;
    //List<BoxBehaviour> answerBoxList;

    public BoxBehaviour currentDraggingBox;

    public List<SlotData> answerSlotList;
    public List<SlotData> choiceSlotList;

    int currentChoiceIndex = -1;

    public int currentRound = 1;
    public int currentQuestionCount;
    public int totalQuestionCount;
    public int correctCount;
    public int wrongCount;

    public int currentAnswerDataIndex = 0;

    Coroutine roundCoroutine = null;


    void Awake()
    {
        prevSpriteDataList = new List<SpriteData>();

        questionBoxList = new List<BoxBehaviour>();
        choiceBoxList = new List<BoxBehaviour>();
        //answerBoxList = new List<BoxBehaviour>();

        answerSlotList = new List<SlotData>();
        choiceSlotList = new List<SlotData>();
    }

    void Start()
    {

    }

    public void InitRound()
    {
        roundPage.SetActive(true);

        // 문제박스, 선택지박스 부모 오브젝트 on
        questionField.SetActive(true);
        choiceField.SetActive(true);

        // 코루틴 멈추기
        if (roundCoroutine != null)
        {
            StopCoroutine(roundCoroutine);
            roundCoroutine = null;
        }

        //// 라운드 카운트 변수 세팅
        //currentRound++;
        currentQuestionCount = 0;
        totalQuestionCount = 10; // (임시)
        correctCount = 0;
        wrongCount = 0;
    }

    public void StartRound()
    {
        // 문제박스, 선택지박스 리스트 초기화
        questionBoxList.Clear();
        choiceBoxList.Clear();
        Utils.ClearChild(questionField);
        Utils.ClearChild(choiceField);

        // 선택했던 인덱스 초기화
        currentChoiceIndex = -1;

        answerSlotList.Clear();
        choiceSlotList.Clear();

        // UI 초기화
        Managers.Ui.trainingUi.SetProgressText();
        Managers.Ui.trainingUi.SetCheckText(string.Empty);
        Managers.Ui.trainingUi.SetActiveNextButton(false);

        // 문제 횟수 다 채웠으면 Finish 페이지 로드
        if (currentQuestionCount >= totalQuestionCount)
        {
            GoFinishPage();
        }
        else
        {
            // 라운드 코루틴 시작
            roundCoroutine = StartCoroutine(RoundLoop());
        }

        //Debug.Log("StartRound()");
    }

    public IEnumerator RoundLoop()
    {
        // 현재 라운드에 맞게 세팅하기 위한 정보들을 담고있는 클래스
        RoundData currentRoundData = null;

        switch (currentRound)
        {
            case 1: currentRoundData = new Round06(); break;
            case 2: currentRoundData = new Round02(); break;
            case 3: currentRoundData = new Round03(); break;
            case 4: currentRoundData = new Round04(); break;
            case 5: currentRoundData = new Round05(); break;
            default: currentRoundData = new Round04(); break;
        }

        currentRoundData.Init();

        Debug.Log("currentRoundData:" + currentRoundData.questionData.ToString());

        ///////////////////////////////////////////////////////////////

        // question 영역 스프라이트 세팅
        trainingScene.SetFieldSprite(questionField, currentRoundData.questionData.questionFieldType);

        // question 박스 좌표들 담아놓은 리스트
        List<Vector3> questionBoxPosList = new List<Vector3>();
        SetBoxPosList(questionBoxPosList, currentRoundData.questionData.questionFieldType);

        // choiceSlot Data 인덱스 설정용
        int answerSlotCount = 0;

        // 문제 오브젝트 생성
        for (int i = 0; i < currentRoundData.questionBoxDataList.Count; ++i)
        {
            // 문제박스 생성
            GameObject newBox = Instantiate(boxPrefab, questionField.transform);
            newBox.GetComponent<BoxBehaviour>().Init(currentRoundData.questionBoxDataList[i], questionBoxPosList[i]);

            // 문제 박스 리스트에 저장
            questionBoxList.Add(newBox.GetComponent<BoxBehaviour>());

            // 정답 박스 타입일때는 정답 슬롯 리스트에도 저장
            if (newBox.GetComponent<BoxBehaviour>().data.type == BoxType.Answer)
            {
                Vector3 worldPos = questionField.transform.TransformPoint(questionBoxPosList[i]);
                answerSlotList.Add(new SlotData(answerSlotCount, worldPos));
                answerSlotCount++;
            }
        }

        ///////////////////////////////////////////////////////////////

        // choice 영역 스프라이트 세팅
        trainingScene.SetFieldSprite(choiceField, currentRoundData.questionData.choiceFieldType);

        // choice 박스 좌표들 담아놓은 리스트
        List<Vector3> choiceBoxPosList = new List<Vector3>();
        SetBoxPosList(choiceBoxPosList, currentRoundData.questionData.choiceFieldType);

        // choiceSlot Data 인덱스 설정용
        int choiceSlotCount = 0;

        // choice 오브젝트 생성
        for (int i = 0; i < currentRoundData.choiceBoxDataList.Count; ++i)
        {
            // choice 박스 생성 (랜덤하게 섞어놓은 인덱스대로 배치)
            GameObject newBox = Instantiate(boxPrefab, choiceField.transform);
            newBox.GetComponent<BoxBehaviour>().Init(currentRoundData.choiceBoxDataList[i], choiceBoxPosList[i]);

            // 선택 박스 리스트에 저장
            choiceBoxList.Add(newBox.GetComponent<BoxBehaviour>());
            newBox.GetComponent<BoxBehaviour>().slotIndex = i;

            // 선택 슬롯 리스트에도 저장
            Vector3 worldPos = choiceField.transform.TransformPoint(choiceBoxPosList[i]);
            choiceSlotList.Add(new SlotData(choiceSlotCount, worldPos, true));
            choiceSlotCount++;
        }

        yield return null;
    }

    public void SetBoxPosList(List<Vector3> _list, FieldType _type)
    {
        // question 박스 좌표들 담아놓은 리스트
        List<Vector3> boxPosList = null;

        if (_type == FieldType._1x7)
        {
            boxPosList = new List<Vector3>()
            {
                new Vector3(-3.6f, 0, 0),
                new Vector3(-2.4f, 0, 0),
                new Vector3(-1.2f, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(1.2f, 0, 0),
                new Vector3(2.4f, 0, 0),
                new Vector3(3.6f, 0, 0),
            };
        }
        else if (_type == FieldType._1x6)
        {
            boxPosList = new List<Vector3>()
            {
                new Vector3(-3f, 0, 0),
                new Vector3(-1.8f, 0, 0),
                new Vector3(-0.6f, 0, 0),
                new Vector3(0.6f, 0, 0),
                new Vector3(1.8f, 0, 0),
                new Vector3(3f, 0, 0),
            };
        }
        else if (_type == FieldType._3x3)
        {
            boxPosList = new List<Vector3>()
            {
                new Vector3(-1.2f, 1.2f, 0), new Vector3(0, 1.2f, 0), new Vector3(1.2f, 1.2f, 0),
                new Vector3(-1.2f, 0, 0), new Vector3(0, 0, 0), new Vector3(1.2f, 0, 0),
                new Vector3(-1.2f, -1.2f, 0), new Vector3(0, -1.2f, 0), new Vector3(1.2f, -1.2f, 0),
            };
        }
        else if (_type == FieldType._2x2x2)
        {
            boxPosList = new List<Vector3>()
            {
                // 1
                new Vector3(-2.3f, 0.6f, 0), new Vector3(-1.1f, 0.6f, 0),
                new Vector3(-2.3f, -0.6f, 0), new Vector3(-1.1f, -0.6f, 0),
                // 2
                new Vector3(1.1f, 0.6f, 0), new Vector3(2.3f, 0.6f, 0),
                new Vector3(1.1f, -0.6f, 0), new Vector3(2.3f, -0.6f, 0),
            };
        }
        else if (_type == FieldType._2x4)
        {
            boxPosList = new List<Vector3>()
            {
                new Vector3(-1.8f, 0.6f, 0), new Vector3(-0.6f, 0.6f, 0),
                new Vector3(0.6f, 0.6f, 0), new Vector3(1.8f, 0.6f, 0),
                new Vector3(-1.8f, -0.6f, 0), new Vector3(-0.6f, -0.6f, 0),
                new Vector3(0.6f, -0.6f, 0), new Vector3(1.8f, -0.6f, 0),
            };
        }

        _list.AddRange(boxPosList);
    }

    public SlotData GetAnswerSlot()
    {
        // 비어있는 AnswerSlot 중 가장 앞의 것을 리턴
        for (int i = 0; i < answerSlotList.Count; ++i)
        {
            if (answerSlotList[i].hasBox == false)
            {
                return answerSlotList[i];
            }
        }

        // 들어있는 AnswerSlot 중 가장 앞의 것을 리턴
        return answerSlotList[0];
    }

    public SlotData GetEmptyChoiceSlot()
    {
        // 비어있는 ChoiceSlot 중 가장 앞의 것을 리턴
        for (int i = 0; i < choiceSlotList.Count; ++i)
        {
            if (choiceSlotList[i].hasBox == false)
            {
                return choiceSlotList[i];
            }
        }

        // null을 리턴하는 경우는 없음. 있으면 오류.
        return null;
    }

    //public SlotData GetFrontAnswerSlot()
    //{
    //    // 들어있는 AnswerSlot 중 가장 앞의 것을 리턴
    //    return answerSlotList[0];
    //}

    public void InsertInAnswerSlot(BoxBehaviour _box)
    {
        //if (currentChoiceIndex != -1)
        //{
        //    choiceBoxList[currentChoiceIndex].gameObject.SetActive(true);
        //}

        //currentChoiceIndex = choiceBoxList.IndexOf(_box);
        //choiceBoxList[currentChoiceIndex].gameObject.SetActive(false);

        ////answerBox.SetData(_box.data);
        //answerBox.data.spriteData = _box.data.spriteData;
        //answerBox.data.colorIndex = _box.data.colorIndex;
        //answerBox.data.angle = _box.data.angle;
        //answerBox.data.scale = _box.data.scale;
        //answerBox.data.number = _box.data.number;

        //answerBox.SetImageProperties();


        //SlotData answerSlot = GetAnswerSlot();
        //answerBox.SetAnswerData(_box);
        //answerBox.SetImageProperties();
    }

    public void RemoveInAnswerBox()
    {
        // 정답박스에 선택지 들어가있으면 빼기
        if (currentChoiceIndex != -1)
        {
            choiceBoxList[currentChoiceIndex].gameObject.SetActive(true);

            currentChoiceIndex = -1;



            ////answerBox.SetData(new BoxData());
            //answerBox.data.spriteData = new SpriteData();
            //answerBox.data.colorIndex = -1;
            //answerBox.data.angle = 0f;
            //answerBox.data.scale = 1f;
            //answerBox.data.number = -1;

            //answerBox.SetImageProperties();
        }
    }

    public void CheckAnswer()
    {
        if (currentChoiceIndex == -1)
        {
            Managers.Ui.trainingUi.SetCheckText(string.Empty);
            return;
        }

        if (choiceBoxList[currentChoiceIndex].data.index == currentAnswerDataIndex)
        {
            Managers.Ui.trainingUi.SetCheckText("Correct");
            //answerBox.SetOutlineColor(Color.green);
            correctCount++;
        }
        else
        {
            Managers.Ui.trainingUi.SetCheckText("Wrong");
            //answerBox.SetOutlineColor(Color.red);
            wrongCount++;
        }

        currentQuestionCount++;

        Managers.Ui.trainingUi.SetActiveNextButton(true);
        Managers.Ui.trainingUi.SetProgressText();
    }

    void GoFinishPage()
    {
        float currentRoundPercentage = (float)correctCount / (float)totalQuestionCount;

        float nextRoundPercentage = 0.5f;
        float prevRoundPercentage = 0.2f;
        int nextRoundCheck = 0;

        if (currentRoundPercentage >= nextRoundPercentage)
        {
            currentRound++;
            nextRoundCheck = 2; // Next
        }
        else if (currentRoundPercentage >= prevRoundPercentage)
        {
            nextRoundCheck = 1; // Stay
        }
        else
        {
            if (currentRound > 1)
            {
                currentRound--;
                nextRoundCheck = 0; // Prev
            }
            else
            {
                nextRoundCheck = 1; // Stay
            }
        }

        roundPage.SetActive(false);
        Managers.Ui.trainingUi.SetPage(2);
        Managers.Ui.trainingUi.SetFinishText(nextRoundCheck);
    }

    void Update()
    {
        //// 게임씬 일때만
        //if (Managers.Scene.CurrentScene.index == 2)
        //{
        //}
    }
}