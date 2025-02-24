using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IndexPair
{
    public int categoryIndex;
    public int spriteIndex;

    public IndexPair(int _categoryIndex, int _spriteIndex)
    {
        this.categoryIndex = _categoryIndex;
        this.spriteIndex = _spriteIndex;
    }
};

[Serializable] //반드시 필요
public class SpriteList //행에 해당되는 이름
{
    public List<Sprite> spriteList;
}

public class GameManager : MonoBehaviour
{
    public List<SpriteList> imageList;

    List<IndexPair> randomIndexList;


    [SerializeField] GameObject boxPrefab;

    public BoxBehaviour answerBox;
    public Transform questionTrans;
    public Transform choiceTrans;

    public List<BoxBehaviour> questionBoxList;
    public List<BoxBehaviour> choiceBoxList;

    int currentChoiceIndex = -1;

    IEnumerator roundCoroutine;


    void Awake()
    {
        roundCoroutine = RoundLoop();
        questionBoxList = new List<BoxBehaviour>();
        choiceBoxList = new List<BoxBehaviour>();

        randomIndexList = new List<IndexPair>();
    }

    void Start()
    {

    }

    public void InitRound()
    {
        // reset box list
        questionBoxList.Clear();
        choiceBoxList.Clear();
        Utils.ClearList(questionTrans.gameObject);
        Utils.ClearList(choiceTrans.gameObject);

        randomIndexList.Clear();
    }

    public void StartRound()
    {
        InitRound();
        StartCoroutine(roundCoroutine);
    }

    public IEnumerator RoundLoop()
    {
        // UI off


        // 랜덤 인덱스 설정
        IndexPair indexPair;

        // 중복되지 않는 인덱스를 찾을 때까지 반복
        do
        {
            int categoryIndex = Random.Range(0, imageList.Count);
            int spriteIndex = Random.Range(0, imageList[categoryIndex].spriteList.Count);

            indexPair = new IndexPair(categoryIndex, spriteIndex);
        }
        while (randomIndexList.Contains(indexPair));

        // 중복이 아닌 경우 리스트에 추가
        randomIndexList.Add(indexPair);


        // 부모 스프라이트의 크기 가져오기
        SpriteRenderer parentSpriteRenderer = questionTrans.GetComponent<SpriteRenderer>();
        float pixelsPerUnit = parentSpriteRenderer.sprite.pixelsPerUnit;
        float parentWidth = parentSpriteRenderer.sprite.bounds.size.x;

        // 부모의 중앙에서 시작하는 스크린 좌표계의 위치 계산
        float xOffset = (100f / pixelsPerUnit) - (parentWidth / 2);

        // 문제 오브젝트 생성
        Vector3 spawnPos = Vector3.zero;
        for (int i = 0; i < 7; ++i)
        {
            GameObject newBox = Instantiate(boxPrefab, questionTrans);
            Color color = Color.red;

            // 자식 스프라이트의 크기
            float childWidth = 100f / 100f; // (width 100 / screenRate 100)

            spawnPos = new Vector3(xOffset + ((childWidth + 0.2f) * i), 0, 10);

            // 문제박스, 정답박스 구별 생성
            if (i != 6)
            {
                newBox.GetComponent<BoxBehaviour>().Init(BoxType.Question, color, spawnPos);
            }
            else
            {
                newBox.GetComponent<BoxBehaviour>().Init(BoxType.Answer, Color.white, spawnPos);
                answerBox = newBox.GetComponent<BoxBehaviour>();
            }

            questionBoxList.Add(newBox.GetComponent<BoxBehaviour>());
        }

        //// 정답 오브젝트 on 
        //answerBox.gameObject.SetActive(true);
        //spawnPos = new Vector3(spawnPos.x + (1 + 0.2f), spawnPos.y, 10);
        //answerBox.transform.localPosition = transform.TransformPoint(spawnPos); // 로컬좌표를 월드좌표로

        // 부모 스프라이트의 크기 가져오기
        parentSpriteRenderer = choiceTrans.GetComponent<SpriteRenderer>();
        pixelsPerUnit = parentSpriteRenderer.sprite.pixelsPerUnit;
        parentWidth = parentSpriteRenderer.sprite.bounds.size.x;

        // 부모의 중앙에서 시작하는 스크린 좌표계의 위치 계산
        xOffset = (100f / pixelsPerUnit) - (parentWidth / 2);

        // 선택지 오브젝트 생성
        for (int i = 0; i < 7; ++i)
        {
            GameObject newBox = Instantiate(boxPrefab, choiceTrans);
            Color color = SetRandomColor();

            // 자식 스프라이트의 크기
            float childWidth = 100f / 100f; // (width 100 / screenRate 100)

            spawnPos = new Vector3(xOffset + ((childWidth + 0.2f) * i), 0, 10);

            newBox.GetComponent<BoxBehaviour>().Init(BoxType.Choice, color, spawnPos);
            choiceBoxList.Add(newBox.GetComponent<BoxBehaviour>());
        }

        yield return null;
    }

    Color SetRandomColor()
    {
        int randomIndex = Random.Range(0, 4);

        switch (randomIndex)
        {
            case 0: return Color.red;
            case 1: return Color.green;
            case 2: return Color.blue;
            case 3: return Color.red;
            default: return Color.black;
        }
    }

    public void InsertInAnswerBox(BoxBehaviour _box)
    {
        if (currentChoiceIndex != -1)
        {
            choiceBoxList[currentChoiceIndex].gameObject.SetActive(true);
        }

        currentChoiceIndex = choiceBoxList.IndexOf(_box);
        choiceBoxList[currentChoiceIndex].gameObject.SetActive(false);

        answerBox.SetSprite(_box.image);
    }

    public void RemoveInAnswerBox()
    {
        // 정답박스에 선택지 들어가있으면 빼기
        if (currentChoiceIndex != -1)
        {
            choiceBoxList[currentChoiceIndex].gameObject.SetActive(true);

            answerBox.image.sprite = null;
            answerBox.image.color = Color.white;

            currentChoiceIndex = -1;
        }
    }

    void Update()
    {
        //// 게임씬 일때만
        //if (Managers.Scene.CurrentScene.index == 2)
        //{
        //}
    }
}