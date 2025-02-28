using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public enum QuestionType
{
    AAAA = 0,
    ABAB,
    MAX
}

public enum ObjectCategory
{
    Carnivore = 0,  // 동물-육식
    Herbivore,      // 동물-초식
    Fish,           // 해양-물고기
    MarineMammal,    // 해양-포유류
}

[Serializable] //반드시 필요
public class SpriteList //행에 해당되는 이름
{
    public List<Sprite> spriteList;
}


public class GameManager : MonoBehaviour
{
    public List<SpriteList> imageList;

    [SerializeField] GameObject boxPrefab;

    //List<BoxData> questionBoxDataList;
    //List<BoxData> choiceBoxDataList;

    public BoxBehaviour answerBox;
    public Transform questionTrans;
    public Transform choiceTrans;

    public List<BoxBehaviour> questionBoxList;
    public List<BoxBehaviour> choiceBoxList;

    int currentChoiceIndex = -1;
    int answerDataIndex = 0;

    Coroutine roundCoroutine = null;


    void Awake()
    {
        //questionBoxDataList = new List<BoxData>();
        //choiceBoxDataList = new List<BoxData>();

        questionBoxList = new List<BoxBehaviour>();
        choiceBoxList = new List<BoxBehaviour>();

        //boxDataList = new List<BoxData>();
    }

    void Start()
    {

    }

    public void StartRound()
    {
        InitRound();

        // 라운드 코루틴 시작
        roundCoroutine = StartCoroutine(RoundLoop());

        Debug.Log("StartRound()");
    }

    public void InitRound()
    {
        // 문제박스, 선택지박스 부모 오브젝트 on
        questionTrans.gameObject.SetActive(true);
        choiceTrans.gameObject.SetActive(true);

        // 코루틴 멈추기
        if (roundCoroutine != null)
        {
            StopCoroutine(roundCoroutine);
            roundCoroutine = null;
        }

        // 문제박스, 선택지박스 리스트 초기화
        questionBoxList.Clear();
        choiceBoxList.Clear();
        Utils.ClearList(questionTrans.gameObject);
        Utils.ClearList(choiceTrans.gameObject);

        //boxDataList.Clear();

        // 선택했던 인덱스 초기화
        currentChoiceIndex = -1;

        // UI 초기화
        Managers.Ui.trainingUi.SetCheckText(string.Empty);

        Managers.Ui.trainingUi.SetActiveButton(0);
    }

    public IEnumerator RoundLoop()
    {
        // UI off


        // 랜덤 인덱스를 저장할 BoxData
        List<BoxData> boxDataList = new List<BoxData>();
        BoxData data;

        // 같은 내용인지?
        bool isUnique;

        for (int i = 0; i < 7; ++i)
        {
            // 중복되지 않는 인덱스를 찾을 때까지 반복
            do
            {
                int categoryIndex = Random.Range(0, imageList.Count);
                int spriteIndex = Random.Range(0, imageList[categoryIndex].spriteList.Count);

                data = new BoxData();
                data.index = i;
                data.categoryIndex = categoryIndex;
                data.spriteIndex = spriteIndex;
                // (임시) 칼라값 랜덤하게
                data.colorIndex = GetRandomColorIndex();

                isUnique = true;

                // 기존 리스트와 비교하여 중복 여부 확인
                foreach (var data2 in boxDataList)
                {
                    if (IsEqualIndex(data, data2))
                    {
                        isUnique = false;
                        break;
                    }
                }

            } while (!isUnique); // 중복되지 않을 때까지 반복

            // 중복이 아닌 경우 리스트에 추가
            boxDataList.Add(data);
        }

        //// (임시) 디버깅
        //for (int i = 0; i < 7; ++i)
        //{
        //    Debug.Log(boxDataList[i].categoryIndex + "," + boxDataList[i].spriteIndex);
        //}


        ///////////////////////////////////////////////////////////////


        // 정답 데이터 세팅
        answerDataIndex = 0;
        //boxDataList[answerDataIndex].isAnswer = true;

        // 
        List<BoxData> questionBoxDataList = new List<BoxData>();
        List<BoxData> choiceBoxDataList = new List<BoxData>();

        // question 개수 설정
        int questionCount = 7;

        // question 박스 데이터 리스트 세팅
        for (int i = 0; i < questionCount; ++i)
        {
            data = new BoxData();

            // AA 형태일때는 정답데이터 인덱스로 문제 만들기
            data.SetData(boxDataList[answerDataIndex]);

            // 마지막 인덱스는 정답박스로 만들기
            if (i == (questionCount - 1))
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

        // question 영역 스프라이트 크기
        float questionSpriteWidth = 9.2f;

        // question 영역을 스크린 좌표계로 환산
        float xOffset = 1 - (questionSpriteWidth / 2);

        // 문제 오브젝트 생성
        Vector3 spawnPos = Vector3.zero;
        for (int i = 0; i < questionCount; ++i)
        {
            // 자식 스프라이트의 크기
            float childWidth = 100f / 100f; // (width 100 / screenRate 100)

            // 간격에 맞춰서 생성 위치 세팅
            spawnPos = new Vector3(xOffset + ((childWidth + 0.2f) * i), 0, 0);

            // 문제박스 생성
            GameObject newBox = Instantiate(boxPrefab, questionTrans);
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

        //// 선택지 인덱스를 랜덤하게 배치할 리스트
        //List<int> randomChoiceIndexList = new List<int>();
        //for (int i = 0; i < choiceCount; ++i)
        //    randomChoiceIndexList.Add(i);

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
            GameObject newBox = Instantiate(boxPrefab, choiceTrans);
            newBox.GetComponent<BoxBehaviour>().Init(choiceBoxDataList[i], spawnPos);
            choiceBoxList.Add(newBox.GetComponent<BoxBehaviour>());
        }

        yield return null;
    }

    public bool IsEqualIndex(BoxData _sour, BoxData _dest)
    {
        if (_sour.categoryIndex == _dest.categoryIndex
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
        answerBox.data.categoryIndex = _box.data.categoryIndex;
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
            answerBox.data.categoryIndex = -1;
            answerBox.data.spriteIndex = -1;
            answerBox.data.colorIndex = -1;
            answerBox.data.angle = 0;
            answerBox.data.scale = 0;

            answerBox.image.sprite = null;
            answerBox.image.color = Color.white;
        }
    }

    public void CheckAnswer()
    {
        if (currentChoiceIndex == -1)
        {
            Managers.Ui.trainingUi.SetCheckText(string.Empty);
            return;
        }

        //if (choiceBoxList[currentChoiceIndex].data.isAnswer == true)
        if (choiceBoxList[currentChoiceIndex].data.index == answerDataIndex)
        {
            Managers.Ui.trainingUi.SetCheckText("Correct");
            answerBox.SetOutlineColor(Color.green);
        }
        else
        {
            Managers.Ui.trainingUi.SetCheckText("Wrong");
            answerBox.SetOutlineColor(Color.red);
        }

        Managers.Ui.trainingUi.SetActiveButton(1);

    }

    void Update()
    {
        //// 게임씬 일때만
        //if (Managers.Scene.CurrentScene.index == 2)
        //{
        //}
    }
}