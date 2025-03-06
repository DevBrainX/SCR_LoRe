using System;
using System.Collections;
using System.Collections.Generic;
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
    List<BoxData> prevBoxDataList;

    public List<BoxBehaviour> questionBoxList;
    public List<BoxBehaviour> choiceBoxList;

    int currentChoiceIndex = -1;

    public int currentRound = 1;
    public int currentQuestionCount;
    public int totalQuestionCount;
    public int correctCount;
    public int wrongCount;

    List<int> questionDataIndex;
    int answerDataIndex = 0;

    Coroutine roundCoroutine = null;


    void Awake()
    {
        //questionBoxDataList = new List<BoxData>();
        //choiceBoxDataList = new List<BoxData>();

        questionBoxList = new List<BoxBehaviour>();
        choiceBoxList = new List<BoxBehaviour>();

        //boxDataList = new List<BoxData>();
        prevBoxDataList = new List<BoxData>();
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
        totalQuestionCount = 5; // (임시)
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

        Debug.Log("StartRound()");
    }

    public IEnumerator RoundLoop()
    {
        // 현재 라운드에 출제되는 문제의 유형 정보
        QuestionData currentQuestionData = null;

        // 현재 문제의 타입 세팅 (AA,AB 등등)
        if (currentRound == 1)
        {
            // 현재 라운드에 출제되는 문제의 유형을 담아놓는 리스트 (랜덤하게 뽑기 위함)
            List<QuestionData> randomQuestionDataList = new List<QuestionData>()
            {
                new QuestionData(QuestionPattern.AAAA, QuestionMatrixType.Matrix_1x7, QuestionCategory.Shape, QuestionSpriteType.Realistic),
                new QuestionData(QuestionPattern.ABAB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Shape, QuestionSpriteType.Realistic),
                new QuestionData(QuestionPattern.ABC, QuestionMatrixType.Matrix_3x3, QuestionCategory.Shape, QuestionSpriteType.Realistic),
                new QuestionData(QuestionPattern.ABAB, QuestionMatrixType.Matrix_3x3, QuestionCategory.Shape, QuestionSpriteType.Realistic),
            };

            if (currentQuestionCount == 0)
            {
                // 첫 라운드는 무조건 0번 인덱스
                currentQuestionData = randomQuestionDataList[0];
            }
            else
            {
                // 이후 랜덤하게 인덱스 세팅
                int randomQuestionDataIndex = Random.Range(0, randomQuestionDataList.Count);
                currentQuestionData = randomQuestionDataList[randomQuestionDataIndex];
            }
        }
        //else if(currentRound == 2)
        //{
        //    // 첫 라운드는 무조건 
        //    if (currentQuestionCount == 0)
        //    {
        //        currentQuestionType = QuestionType.AAAA;
        //    }
        //    else
        //    {
        //        int randomQuestionType = Random.Range(0, 3);
        //        currentQuestionType = (QuestionType)randomQuestionType;
        //    }
        //}
        else
        {
            List<QuestionData> randomQuestionDataList = new List<QuestionData>()
            {
                new QuestionData(QuestionPattern.AAAA, QuestionMatrixType.Matrix_1x7, QuestionCategory.Color, QuestionSpriteType.Abstract),
                new QuestionData(QuestionPattern.ABAB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Color, QuestionSpriteType.Abstract),
                new QuestionData(QuestionPattern.AABB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Color, QuestionSpriteType.Abstract),
                //new QuestionData(QuestionPattern.AAAA, QuestionMatrixType.Matrix_3x3, QuestionCategory.Color, QuestionSpriteType.Abstract),
                new QuestionData(QuestionPattern.ABAB, QuestionMatrixType.Matrix_3x3, QuestionCategory.Color, QuestionSpriteType.Abstract),
                //new QuestionData(QuestionPattern.AABB, QuestionMatrixType.Matrix_3x3, QuestionCategory.Color, QuestionSpriteType.Abstract),
            };

            if (currentQuestionCount == 0)
            {
                currentQuestionData = randomQuestionDataList[0];
            }
            else
            {
                int randomQuestionDataIndex = Random.Range(0, randomQuestionDataList.Count);
                currentQuestionData = randomQuestionDataList[randomQuestionDataIndex];
            }
        }

        // (임시)
        Debug.Log(currentQuestionData.ToString());

        ///////////////////////////////////////////////////////////////

        // 랜덤 인덱스를 저장할 BoxData
        List<BoxData> boxDataList = new List<BoxData>();
        BoxData data = null;

        for (int i = 0; i < 7; ++i)
        {
            // 중복되지 않는 인덱스가 있는지?
            bool isUnique = false;

            // 중복되지 않는 인덱스를 찾을 때까지 반복
            while (!isUnique)
            {
                data = new BoxData();
                data.index = i;

                if (currentQuestionData.spriteType == QuestionSpriteType.Realistic)
                {
                    data.spriteType = QuestionSpriteType.Realistic;
                    data.spriteCategoryIndex = Random.Range(0, Managers.Resource.realSpriteList.Count);
                    data.spriteIndex = Random.Range(0, Managers.Resource.realSpriteList[data.spriteCategoryIndex].Count);
                    // 사실적 이미지 일때는 흰색, 추상적 이미지 일때는 랜덤
                    data.colorIndex = -1;
                }
                else
                {
                    data.spriteType = QuestionSpriteType.Abstract;
                    data.spriteCategoryIndex = Random.Range(0, Managers.Resource.abstSpriteList.Count);
                    data.spriteIndex = Random.Range(0, Managers.Resource.abstSpriteList[data.spriteCategoryIndex].Count);
                    data.colorIndex = Random.Range(0, (int)ColorIndex.MAX);
                }

                isUnique = true;

                // 이전 박스 리스트에 있는 요소인지 중복 여부 확인
                foreach (var data2 in prevBoxDataList)
                {
                    if (IsEqualIndex(data, data2))
                    {
                        // 중복된것 발견하면 break하고 다시 while문 처음으로 돌아감
                        isUnique = false;
                        break;
                    }
                }

                // 현재 박스 리스트에 있는 요소인지 중복 여부 확인
                if (isUnique)
                {
                    foreach (var data2 in boxDataList)
                    {
                        if (IsEqualIndex(data, data2))
                        {
                            // 중복된것 발견하면 break하고 다시 while문 처음으로 돌아감
                            isUnique = false;
                            break;
                        }
                    }
                }
            }

            // 중복이 아닌 경우 리스트에 추가
            boxDataList.Add(data);
        }

        //// (임시) 디버깅
        //for (int i = 0; i < 7; ++i)
        //{
        //    Debug.Log(boxDataList[i].categoryIndex + "," + boxDataList[i].spriteIndex);
        //}

        // 이전 박스 데이터 옮겨담기 (중복 방지)
        prevBoxDataList.Clear();
        prevBoxDataList.AddRange(boxDataList);

        ///////////////////////////////////////////////////////////////

        // 정답 위치 인덱스
        int answerIndex = 0;

        // 문제 타입에 따른 questionDataIndex, answerDataIndex 세팅
        switch (currentQuestionData.pattern)
        {
            // 문제, 정답 데이터 세팅
            case QuestionPattern.AAAA: // 1x7
                questionDataIndex = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
                answerIndex = 6;
                break;

            case QuestionPattern.ABAB: // 1x7, 3x3
                if (currentQuestionData.matrixType == QuestionMatrixType.Matrix_1x7)
                {
                    questionDataIndex = new List<int> { 0, 1, 0, 1, 0, 1, 0 };
                    answerIndex = 6;
                }
                else if (currentQuestionData.matrixType == QuestionMatrixType.Matrix_3x3)
                {
                    questionDataIndex = new List<int> { 0, 1, 0, 1, 0, 1, 0, 1, 0 };

                    if (Utils.GetRandomBool())
                        answerIndex = 3;
                    else
                        answerIndex = 5;
                }
                break;

            case QuestionPattern.AABB: // 1x7
                questionDataIndex = new List<int> { 0, 0, 1, 1, 0, 0, 1 };
                answerIndex = 6;
                break;

            case QuestionPattern.ABC: // 3x3
                questionDataIndex = new List<int> { 0, 1, 2, 0, 1, 2, 0, 1, 2 };

                if (Utils.GetRandomBool())
                    answerIndex = 1;
                else
                    answerIndex = 7;
                break;

                //case QuestionPattern.AAB:
                //    questionDataIndex = new List<int> { 0, 0, 1 };
                //    answerDataIndex = 0;
                //    break;
        }

        // 정답 데이터의 인덱스는 정답 List 인덱스 번호
        answerDataIndex = questionDataIndex[answerIndex];

        // 
        List<BoxData> questionBoxDataList = new List<BoxData>();
        List<BoxData> choiceBoxDataList = new List<BoxData>();

        // question 개수 설정
        int questionCount = 0;
        if (currentQuestionData.matrixType == QuestionMatrixType.Matrix_1x7)
            questionCount = 7;
        else if (currentQuestionData.matrixType == QuestionMatrixType.Matrix_3x3)
            questionCount = 9;

        // question 박스 데이터 리스트 세팅
        for (int i = 0; i < questionCount; ++i)
        {
            data = new BoxData();

            // 문제 데이터 패턴에 따른 문제 인덱스 세팅
            data.SetData(boxDataList[questionDataIndex[i]]);

            // 정답 위치 인덱스인지 확인
            if (i == answerIndex)
            {
                data.type = BoxType.Answer;
            }
            else
            {
                data.type = BoxType.Question;
            }

            questionBoxDataList.Add(data);
        }


        ///////////////////////////////////////////////////////////////

        // question 영역 스프라이트 세팅
        trainingScene.SetQuestionFieldSprite(currentQuestionData.matrixType);

        // question 영역 스프라이트 크기
        float questionSpriteWidth = questionField.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float questionSpriteHeight = questionField.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        // question 영역을 스크린 좌표계로 환산
        float xOffset = 1 - (questionSpriteWidth / 2);
        float yOffset = 1 - (questionSpriteHeight / 2);

        // 박스간의 간격
        float spacing = 0.2f;

        // 문제 오브젝트 생성
        Vector3 spawnPos = Vector3.zero;
        for (int i = 0; i < questionCount; ++i)
        {
            // 자식 스프라이트의 크기
            float childWidth = 100f / 100f; // (width 100 / screenRate 100)

            // 1x7 행렬일때 배치 세팅
            if (currentQuestionData.matrixType == QuestionMatrixType.Matrix_1x7)
            {
                // 간격에 맞춰서 생성 위치 세팅
                spawnPos = new Vector3(xOffset + ((childWidth + spacing) * i), yOffset, 0);
            }
            // 3x3 행렬일때 배치 세팅
            else if (currentQuestionData.matrixType == QuestionMatrixType.Matrix_3x3)
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
            newBox.GetComponent<BoxBehaviour>().Init(questionBoxDataList[i], spawnPos);

            // 정답박스 타입일때는 지정
            if (newBox.GetComponent<BoxBehaviour>().data.type == BoxType.Answer)
            {
                answerBox = newBox.GetComponent<BoxBehaviour>();
            }

            questionBoxList.Add(newBox.GetComponent<BoxBehaviour>());
        }


        ///////////////////////////////////////////////////////////////


        // choice 개수 설정
        int choiceCount = 7;

        // choice 박스 데이터 리스트 세팅
        for (int i = 0; i < choiceCount; ++i)
        {
            data = new BoxData();

            // AA 형태일때는 정답데이터 인덱스로 문제 만들기
            data.SetData(boxDataList[i]);

            data.type = BoxType.Choice;

            choiceBoxDataList.Add(data);
        }

        // choice 리스트 요소 셔플 (랜덤하게 섞음)
        Utils.ShuffleList(choiceBoxDataList);


        ///////////////////////////////////////////////////////////////


        // choice 영역 스프라이트 크기
        float choiceSpriteWidth = 9.2f;

        // choice 영역을 스크린 좌표계로 환산
        xOffset = 1 - (choiceSpriteWidth / 2);

        // choice 오브젝트 생성
        for (int i = 0; i < choiceCount; ++i)
        {
            // 자식 스프라이트의 크기
            float childWidth = 100f / 100f; // (width 100 / screenRate 100)

            // 간격에 맞춰서 생성 위치 세팅
            spawnPos = new Vector3(xOffset + ((childWidth + 0.2f) * i), 0, 0);

            // choice 박스 생성 (랜덤하게 섞어놓은 인덱스대로 배치)
            GameObject newBox = Instantiate(boxPrefab, choiceField.transform);
            newBox.GetComponent<BoxBehaviour>().Init(choiceBoxDataList[i], spawnPos);
            choiceBoxList.Add(newBox.GetComponent<BoxBehaviour>());
        }

        yield return null;
    }

    public bool IsEqualIndex(BoxData _sour, BoxData _dest)
    {
        if (_sour.spriteCategoryIndex == _dest.spriteCategoryIndex
            && _sour.spriteIndex == _dest.spriteIndex)
        {
            return true;
        }

        return false;
    }

    int GetRandomColorIndex()
    {
        int randomIndex = Random.Range(0, (int)ColorIndex.MAX);
        return randomIndex;
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
        answerBox.data.spriteType = _box.data.spriteType;
        answerBox.data.spriteCategoryIndex = _box.data.spriteCategoryIndex;
        answerBox.data.spriteIndex = _box.data.spriteIndex;
        answerBox.data.colorIndex = _box.data.colorIndex;
        answerBox.data.angle = _box.data.angle;
        answerBox.data.scale = _box.data.scale;

        answerBox.SetSprite();
        answerBox.SetColor();
    }

    public void RemoveInAnswerBox()
    {
        // 정답박스에 선택지 들어가있으면 빼기
        if (currentChoiceIndex != -1)
        {
            choiceBoxList[currentChoiceIndex].gameObject.SetActive(true);

            currentChoiceIndex = -1;

            //answerBox.SetData(new BoxData());
            answerBox.data.spriteCategoryIndex = -1;
            answerBox.data.spriteIndex = -1;
            answerBox.data.colorIndex = -1;
            answerBox.data.angle = 0;
            answerBox.data.scale = 0;

            answerBox.SetSprite();
            answerBox.SetColor();
        }
    }

    public void CheckAnswer()
    {
        if (currentChoiceIndex == -1)
        {
            Managers.Ui.trainingUi.SetCheckText(string.Empty);
            return;
        }

        if (choiceBoxList[currentChoiceIndex].data.index == answerDataIndex)
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