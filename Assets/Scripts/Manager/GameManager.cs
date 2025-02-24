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
        Vector2 spawnPos = new Vector2(-5, 0);
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
        spawnPos = new Vector2(-5, 0);
        for (int i = 0; i < 4; ++i)
        {
            GameObject newBox = Instantiate(boxPrefab, choiceTrans);
            Color color = SetRandomColor();
            Vector3 oriPos = new Vector3(spawnPos.x + (3 * i), spawnPos.y, 0);
            newBox.GetComponent<BoxBehaviour>().Init(BoxType.Choice, color, oriPos);
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