using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;

    public GameObject answerBox;
    public Transform questionTrans;
    public Transform choiceTrans;

    public List<BoxBehaviour> questionBoxList;
    public List<BoxBehaviour> choiceBoxList;

    IEnumerator roundCoroutine;

    void Awake()
    {
        roundCoroutine = RoundLoop();
        questionBoxList = new List<BoxBehaviour>();
        choiceBoxList = new List<BoxBehaviour>();
    }

    void Start()
    {

    }

    public void StartRound()
    {
        StartCoroutine(roundCoroutine);
    }

    public IEnumerator RoundLoop()
    {
        // reset box list
        questionBoxList.Clear();
        choiceBoxList.Clear();
        Utils.ClearList(questionTrans.gameObject);
        Utils.ClearList(choiceTrans.gameObject);

        // UI off


        // 문제 오브젝트 on
        Vector3 spawnPos = new Vector3(-5, 0, 0);
        for (int i = 0; i < 4; ++i)
        {
            GameObject newBox = Instantiate(boxPrefab, questionTrans);
            //Color color = SetRandomColor();
            Vector3 oriPos = new Vector3(spawnPos.x + (3 * i), spawnPos.y, 0);
            newBox.GetComponent<BoxBehaviour>().Init(BoxType.Question, Color.red, oriPos);
            questionBoxList.Add(newBox.GetComponent<BoxBehaviour>());
        }

        // 정답 오브젝트 on 
        answerBox.SetActive(true);

        // 선택지 오브젝트 on
        //float width = 920;
        //float height = 200;
        //float spawnWidth = 100;

        // 부모 스프라이트의 크기 가져오기
        SpriteRenderer parentSpriteRenderer = choiceTrans.GetComponent<SpriteRenderer>();
        float pixelsPerUnit = parentSpriteRenderer.sprite.pixelsPerUnit;
        float parentWidth = parentSpriteRenderer.sprite.bounds.size.x;
        

        // 부모의 중앙에서 시작하는 스크린 좌표계의 위치 계산
        float xOffset = (100f / pixelsPerUnit) - (parentWidth / 2);

        // spawnPos = new Vector3(parentWidth, 0, 0);

        for (int i = 0; i < 7; ++i)
        {
            GameObject newBox = Instantiate(boxPrefab, choiceTrans);
            Color color = SetRandomColor();

            // 자식 스프라이트의 크기
            float childWidth = newBox.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

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

    void Update()
    {
        //// 게임씬 일때만
        //if (Managers.Scene.CurrentScene.index == 2)
        //{
        //}
    }
}