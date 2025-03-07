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

    public BoxBehaviour answerBox;

    //List<BoxData> questionBoxDataList;
    //List<BoxData> choiceBoxDataList;

    //List<BoxData> boxDataList;
    //List<BoxData> prevBoxDataList;
    //List<SpriteData> spriteDataList;
    public List<SpriteData> prevSpriteDataList;

    public List<BoxBehaviour> questionBoxList;
    public List<BoxBehaviour> choiceBoxList;

    int currentChoiceIndex = -1;

    public int currentRound = 1;
    public int currentQuestionCount;
    public int totalQuestionCount;
    public int correctCount;
    public int wrongCount;

    //List<int> questionDataIndex;
    public int currentAnswerDataIndex = 0;

    Coroutine roundCoroutine = null;


    void Awake()
    {
        //questionBoxDataList = new List<BoxData>();
        //choiceBoxDataList = new List<BoxData>();

        questionBoxList = new List<BoxBehaviour>();
        choiceBoxList = new List<BoxBehaviour>();

        //boxDataList = new List<BoxData>();
        //prevBoxDataList = new List<BoxData>();
        //spriteDataList = new List<SpriteData>();
        prevSpriteDataList = new List<SpriteData>();
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
        ///////////////////////////////////////////////////////////////


        // 현재 라운드에 맞게 세팅하기 위한 정보들을 담고있는 클래스
        RoundData currentRoundData = null;

        switch (currentRound)
        {
            case 1: currentRoundData = new Round05(); break;
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

        

        // question 영역 스프라이트 크기
        float spriteWidth = questionField.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float spriteHeight = questionField.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        // question 영역을 스크린 좌표계로 환산
        float xOffset = 1 - (spriteWidth / 2);
        float yOffset = 1 - (spriteHeight / 2);

        // 박스간의 간격
        float spacing = 0.2f;

        // 문제 오브젝트 생성
        Vector3 spawnPos = Vector3.zero;
        for (int i = 0; i < currentRoundData.questionBoxDataList.Count; ++i)
        {
            // 자식 스프라이트의 크기
            float childWidth = 100f / 100f; // (width 100 / screenRate 100)

            // 1x6, 1x7 행렬일때 배치 세팅
            if (currentRoundData.questionData.questionFieldType == FieldType._1x7
                || currentRoundData.questionData.questionFieldType == FieldType._1x6)
            {
                // 간격에 맞춰서 생성 위치 세팅
                spawnPos = new Vector3(xOffset + ((childWidth + spacing) * i), yOffset, 0);
            }
            // 3x3 행렬일때 배치 세팅
            else if (currentRoundData.questionData.questionFieldType == FieldType._3x3)
            {
                // 행과 열 계산
                int columns = 3;
                int row = i / columns; // 현재 행
                int col = i % columns; // 현재 열

                // 간격에 맞춰서 생성 위치 세팅
                spawnPos = new Vector3(xOffset + (childWidth + spacing) * col, yOffset + (childWidth + spacing) * row, 0);
            }

            // 문제박스 생성
            GameObject newBox = Instantiate(boxPrefab, questionField.transform);
            newBox.GetComponent<BoxBehaviour>().Init(currentRoundData.questionBoxDataList[i], spawnPos);

            // 정답박스 타입일때는 지정
            if (newBox.GetComponent<BoxBehaviour>().data.type == BoxType.Answer)
            {
                answerBox = newBox.GetComponent<BoxBehaviour>();
            }

            questionBoxList.Add(newBox.GetComponent<BoxBehaviour>());
        }


        ///////////////////////////////////////////////////////////////

        // choice 영역 스프라이트 세팅
        trainingScene.SetFieldSprite(choiceField, currentRoundData.questionData.choiceFieldType);

        // choice 영역 스프라이트 크기
        spriteWidth = choiceField.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        spriteHeight = choiceField.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        // choice 영역을 스크린 좌표계로 환산
        xOffset = 1 - (spriteWidth / 2);
        yOffset = 1 - (spriteHeight / 2);

        // choice 오브젝트 생성
        for (int i = 0; i < currentRoundData.choiceBoxDataList.Count; ++i)
        {
            // 자식 스프라이트의 크기
            float childWidth = 100f / 100f; // (width 100 / screenRate 100)

            // 1x6, 1x7 행렬일때 배치 세팅
            if (currentRoundData.questionData.choiceFieldType == FieldType._1x7
                || currentRoundData.questionData.choiceFieldType == FieldType._1x6)
            {
                // 간격에 맞춰서 생성 위치 세팅
                spawnPos = new Vector3(xOffset + ((childWidth + spacing) * i), yOffset, 0);
            }

            // choice 박스 생성 (랜덤하게 섞어놓은 인덱스대로 배치)
            GameObject newBox = Instantiate(boxPrefab, choiceField.transform);
            newBox.GetComponent<BoxBehaviour>().Init(currentRoundData.choiceBoxDataList[i], spawnPos);
            choiceBoxList.Add(newBox.GetComponent<BoxBehaviour>());
        }

        yield return null;
    }

    public void InsertInAnswerBox(BoxBehaviour _box)
    {
        if (currentChoiceIndex != -1)
        {
            choiceBoxList[currentChoiceIndex].gameObject.SetActive(true);
        }

        currentChoiceIndex = choiceBoxList.IndexOf(_box);
        choiceBoxList[currentChoiceIndex].gameObject.SetActive(false);

        //answerBox.SetData(_box.data);
        answerBox.data.spriteData = _box.data.spriteData;
        //answerBox.data.spriteType = _box.data.spriteType;
        //answerBox.data.spriteCategoryIndex = _box.data.spriteCategoryIndex;
        //answerBox.data.spriteIndex = _box.data.spriteIndex;
        answerBox.data.colorIndex = _box.data.colorIndex;
        answerBox.data.angle = _box.data.angle;
        answerBox.data.scale = _box.data.scale;
        answerBox.data.number = _box.data.number;

        answerBox.SetImageProperties();
    }

    public void RemoveInAnswerBox()
    {
        // 정답박스에 선택지 들어가있으면 빼기
        if (currentChoiceIndex != -1)
        {
            choiceBoxList[currentChoiceIndex].gameObject.SetActive(true);

            currentChoiceIndex = -1;

            //answerBox.SetData(new BoxData());
            answerBox.data.spriteData = new SpriteData();
            //answerBox.data.spriteCategoryIndex = -1;
            //answerBox.data.spriteIndex = -1;
            answerBox.data.colorIndex = -1;
            answerBox.data.angle = 0f;
            answerBox.data.scale = 1f;
            answerBox.data.number = -1;

            answerBox.SetImageProperties();
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
            answerBox.SetOutlineColor(Color.green);
            correctCount++;
        }
        else
        {
            Managers.Ui.trainingUi.SetCheckText("Wrong");
            answerBox.SetOutlineColor(Color.red);
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