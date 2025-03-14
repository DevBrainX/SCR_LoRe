using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

//public class AnswerData
//{
//    public SpriteData spriteData;
//    public int index;

//    public AnswerData()
//    {

//    }
//}

public class RoundData
{
    public List<BoxData> questionBoxDataList = null;
    public List<BoxData> choiceBoxDataList = null;

    public QuestionData questionData = null;

    public List<SpriteData> spriteDataList = null;

    public List<BoxData> answerDataList = new List<BoxData>();

    // 현재 라운드에 출제되는 문제의 유형을 담아놓는 리스트 (랜덤하게 뽑기 위함)
    public List<QuestionData> randomQuestionDataList = null;

    public virtual void Init()
    {

    }

    public bool IsEqualIndex(SpriteData _sour, SpriteData _dest)
    {
        if (_sour.category == _dest.category && _sour.index == _dest.index)
        {
            return true;
        }

        return false;
    }

    public int GetRandomColorIndex(int _prevIndex = -1)
    {
        int randomIndex = Random.Range(0, (int)ColorIndex.MAX);

        // randomIndex가 _prevIndex와 같을 경우 반복
        while (_prevIndex >= 0 && randomIndex == _prevIndex)
        {
            randomIndex = Random.Range(0, (int)ColorIndex.MAX);
        }

        return randomIndex;
    }

    public void SetRandomQuestionDataIndex()
    {
        if (Managers.Game.currentQuestionCount == 0)
        {
            // 첫 라운드는 무조건 0번 인덱스
            questionData = randomQuestionDataList[0];
        }
        else
        {
            // 이후 랜덤하게 인덱스 세팅
            int randomQuestionDataIndex = Random.Range(0, randomQuestionDataList.Count);
            questionData = randomQuestionDataList[randomQuestionDataIndex];
        }
    }

    // spriteDataList 에 랜덤하게 스프라이트 데이터를 생성해서 저장해놓음
    public void SetRandomSpriteDataList()
    {
        // 데이터 컨테이너 초기화
        spriteDataList.ClearList();
        spriteDataList = new List<SpriteData>();

        // 랜덤 BoxData의 스프라이트 인덱스를 저장
        SpriteData data = null;

        // Choice Field Type에 따라서 랜덤 스프라이트 개수 설정
        int _count = -1;
        if (questionData.choiceFieldType == FieldType._1x6)
            _count = 6;
        else if (questionData.choiceFieldType == FieldType._1x7)
            _count = 7;
        else if (questionData.choiceFieldType == FieldType._3x3)
            _count = 5;
        else if (questionData.choiceFieldType == FieldType._2x4)
            _count = 8;
        else
            _count = 1;

        for (int i = 0; i < _count; ++i)
        {
            // 중복되지 않는 인덱스가 있는지?
            bool isUnique = false;

            // 중복되지 않는 인덱스를 찾을 때까지 반복
            while (!isUnique)
            {
                data = new SpriteData();

                if (questionData.spriteType == SpriteType.Realistic)
                {
                    data.type = SpriteType.Realistic;
                    data.category = Random.Range(0, Managers.Resource.realSpriteList.Count);
                    data.index = Random.Range(0, Managers.Resource.realSpriteList[data.category].Count);
                }
                else
                {
                    data.type = SpriteType.Abstract;
                    data.category = Random.Range(0, Managers.Resource.abstSpriteList.Count);
                    data.index = Random.Range(0, Managers.Resource.abstSpriteList[data.category].Count);
                }

                isUnique = true;

                // 이전 박스 리스트에 있는 요소인지 중복 여부 확인
                foreach (var data2 in Managers.Game.prevSpriteDataList)
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
                    foreach (var data2 in spriteDataList)
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
            spriteDataList.Add(data);
        }

        //// (임시) 디버깅
        //for (int i = 0; i < spriteDataList.count; ++i)
        //{
        //    Debug.Log(spriteDataList[i].category + "," + spriteDataList[i].index);
        //}

        // 이전 박스 데이터 옮겨담기 (중복 방지)
        Managers.Game.prevSpriteDataList.Clear();
        Managers.Game.prevSpriteDataList.AddRange(spriteDataList);
    }

    public void SetRoundData()
    {
        // randomQuestionDataList에 들어있는 QuestionData 중에 하나 뽑음
        SetRandomQuestionDataIndex();

        // 문제데이터에 사용될 스프라이트 인덱스 랜덤하게 세팅
        SetRandomSpriteDataList();

        // 정답의 색상
        int answerColor = GetRandomColorIndex();

        // 정답 데이터 리스트 세팅
        answerDataList.Clear();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        // 문제 데이터 리스트, 선택지 데이터 리스트 세팅

        // AAAA Shape Realistic _1x7 _1x7
        if (questionData.pattern == QuestionPattern.AAAA
            && questionData.category == QuestionCategory.Shape
            && questionData.spriteType == SpriteType.Realistic
            && questionData.questionFieldType == FieldType._1x7 && questionData.choiceFieldType == FieldType._1x7)
        {
            questionBoxDataList = new List<BoxData>
            {
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Answer, new SpriteData()),
            };

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(BoxType.Choice, spriteDataList[0]),
                new BoxData(BoxType.Choice, spriteDataList[1]),
                new BoxData(BoxType.Choice, spriteDataList[2]),
                new BoxData(BoxType.Choice, spriteDataList[3]),
                new BoxData(BoxType.Choice, spriteDataList[4]),
                new BoxData(BoxType.Choice, spriteDataList[5]),
                new BoxData(BoxType.Choice, spriteDataList[6]),
            };

            // 정답 데이터 세팅
            SetAnswerData(0);
        }
        // ABAB Shape Realistic _1x7 _1x7
        else if (questionData.pattern == QuestionPattern.ABAB
            && questionData.category == QuestionCategory.Shape
            && questionData.spriteType == SpriteType.Realistic
            && questionData.questionFieldType == FieldType._1x7 && questionData.choiceFieldType == FieldType._1x7)
        {
            questionBoxDataList = new List<BoxData>
            {
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Question, spriteDataList[1]),
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Question, spriteDataList[1]),
                new BoxData(BoxType.Question, spriteDataList[0]),
                new BoxData(BoxType.Question, spriteDataList[1]),
                new BoxData(BoxType.Answer, new SpriteData()),
            };

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(BoxType.Choice, spriteDataList[0]),
                new BoxData(BoxType.Choice, spriteDataList[1]),
                new BoxData(BoxType.Choice, spriteDataList[2]),
                new BoxData(BoxType.Choice, spriteDataList[3]),
                new BoxData(BoxType.Choice, spriteDataList[4]),
                new BoxData(BoxType.Choice, spriteDataList[5]),
                new BoxData(BoxType.Choice, spriteDataList[6]),
            };

            SetAnswerData(0);
        }
        // ABC Shape Realistic _3x3 _1x7
        else if (questionData.pattern == QuestionPattern.ABC
            && questionData.category == QuestionCategory.Shape
            && questionData.spriteType == SpriteType.Realistic
            && questionData.questionFieldType == FieldType._3x3 && questionData.choiceFieldType == FieldType._1x7)
        {
            // 50% 확률로 정답칸 위치 인덱스 설정 (1 or 7)
            if (Utils.GetRandomBool())
            {
                questionBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Answer, new SpriteData()),
                    new BoxData(BoxType.Question, spriteDataList[2]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[2]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[2]),
                };
            }
            else
            {
                questionBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[2]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[2]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Answer, new SpriteData()),
                    new BoxData(BoxType.Question, spriteDataList[2]),
                };
            }

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(BoxType.Choice, spriteDataList[0]),
                new BoxData(BoxType.Choice, spriteDataList[1]),
                new BoxData(BoxType.Choice, spriteDataList[2]),
                new BoxData(BoxType.Choice, spriteDataList[3]),
                new BoxData(BoxType.Choice, spriteDataList[4]),
                new BoxData(BoxType.Choice, spriteDataList[5]),
                new BoxData(BoxType.Choice, spriteDataList[6]),
            };

            SetAnswerData(1);
        }
        // ABA Shape Realistic _3x3 _1x7
        else if (questionData.pattern == QuestionPattern.ABA
            && questionData.category == QuestionCategory.Shape
            && questionData.spriteType == SpriteType.Realistic
            && questionData.questionFieldType == FieldType._3x3 && questionData.choiceFieldType == FieldType._1x7)
        {
            // 50% 확률로 정답칸 위치 인덱스 설정 (3 or 5)
            if (Utils.GetRandomBool())
            {
                questionBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Answer, new SpriteData()),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                };
            }
            else
            {
                questionBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Answer, new SpriteData()),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                    new BoxData(BoxType.Question, spriteDataList[1]),
                    new BoxData(BoxType.Question, spriteDataList[0]),
                };
            }

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(BoxType.Choice, spriteDataList[0]),
                new BoxData(BoxType.Choice, spriteDataList[1]),
                new BoxData(BoxType.Choice, spriteDataList[2]),
                new BoxData(BoxType.Choice, spriteDataList[3]),
                new BoxData(BoxType.Choice, spriteDataList[4]),
                new BoxData(BoxType.Choice, spriteDataList[5]),
                new BoxData(BoxType.Choice, spriteDataList[6]),
            };

            SetAnswerData(1);
        }

        /*

        // AAAA Color Abstract _1x7 _1x7
        else if (questionData.pattern == QuestionPattern.AAAA
            && questionData.category == QuestionCategory.Color
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._1x7 && questionData.choiceFieldType == FieldType._1x7)
        {
            questionBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Question, spriteDataList[0], 0),
                    new BoxData(BoxType.Question, spriteDataList[0], 0),
                    new BoxData(BoxType.Question, spriteDataList[0], 0),
                    new BoxData(BoxType.Question, spriteDataList[0], 0),
                    new BoxData(BoxType.Question, spriteDataList[0], 0),
                    new BoxData(BoxType.Question, spriteDataList[0], 0),
                    new BoxData(BoxType.Answer, new SpriteData()),
                };

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Choice, spriteDataList[0], 0),
                    new BoxData(BoxType.Choice, spriteDataList[0], 1),
                    new BoxData(BoxType.Choice, spriteDataList[0], 2),
                    new BoxData(BoxType.Choice, spriteDataList[1], 0),
                    new BoxData(BoxType.Choice, spriteDataList[1], GetRandomColorIndex()),
                    new BoxData(BoxType.Choice, spriteDataList[2], GetRandomColorIndex()),
                    new BoxData(BoxType.Choice, spriteDataList[3], GetRandomColorIndex()),
                };

            answerDataIndex = 0;
        }
        // ABAB Color Abstract _1x7 _1x7
        else if (questionData.pattern == QuestionPattern.ABAB
            && questionData.category == QuestionCategory.Color
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._1x7 && questionData.choiceFieldType == FieldType._1x7)
        {
            questionBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Question, spriteDataList[0], 2),
                    new BoxData(BoxType.Question, spriteDataList[0], 3),
                    new BoxData(BoxType.Question, spriteDataList[0], 2),
                    new BoxData(BoxType.Question, spriteDataList[0], 3),
                    new BoxData(BoxType.Question, spriteDataList[0], 2),
                    new BoxData(BoxType.Question, spriteDataList[0], 3),
                    new BoxData(BoxType.Answer, new SpriteData()),
                };

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Choice, spriteDataList[0], 2),
                    new BoxData(BoxType.Choice, spriteDataList[0], 3),
                    new BoxData(BoxType.Choice, spriteDataList[0], 0),
                    new BoxData(BoxType.Choice, spriteDataList[1], 2),
                    new BoxData(BoxType.Choice, spriteDataList[1], GetRandomColorIndex()),
                    new BoxData(BoxType.Choice, spriteDataList[2], GetRandomColorIndex()),
                    new BoxData(BoxType.Choice, spriteDataList[3], GetRandomColorIndex()),
                };

            answerDataIndex = 0;
        }
        // AABB Color Abstract _1x7 _1x7
        else if (questionData.pattern == QuestionPattern.AABB
            && questionData.category == QuestionCategory.Color
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._1x7 && questionData.choiceFieldType == FieldType._1x7)
        {
            questionBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Question, spriteDataList[0], 1),
                    new BoxData(BoxType.Question, spriteDataList[0], 1),
                    new BoxData(BoxType.Question, spriteDataList[0], 2),
                    new BoxData(BoxType.Question, spriteDataList[0], 2),
                    new BoxData(BoxType.Question, spriteDataList[0], 1),
                    new BoxData(BoxType.Question, spriteDataList[0], 1),
                    new BoxData(BoxType.Answer, new SpriteData()),
                };

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(BoxType.Choice, spriteDataList[0], 1),
                    new BoxData(BoxType.Choice, spriteDataList[0], 2),
                    new BoxData(BoxType.Choice, spriteDataList[0], 0),
                    new BoxData(BoxType.Choice, spriteDataList[1], 1),
                    new BoxData(BoxType.Choice, spriteDataList[1], 2),
                    new BoxData(BoxType.Choice, spriteDataList[2], GetRandomColorIndex()),
                    new BoxData(BoxType.Choice, spriteDataList[3], GetRandomColorIndex()),
                };

            answerDataIndex = 1;
        }
        // ABC Color Abstract _3x3 _1x7
        else if (questionData.pattern == QuestionPattern.ABC
            && questionData.category == QuestionCategory.Color
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._3x3 && questionData.choiceFieldType == FieldType._1x7)
        {
            // 50% 확률로 정답칸 위치 인덱스 설정 (1 or 7)
            if (Utils.GetRandomBool())
            {
                questionBoxDataList = new List<BoxData>
                    {
                        new BoxData(0, BoxType.Question, spriteDataList[0], 1),
                        new BoxData(0, BoxType.Answer, new SpriteData()),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 3),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 3),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 3),
                    };
            }
            else
            {
                questionBoxDataList = new List<BoxData>
                    {
                        new BoxData(0, BoxType.Question, spriteDataList[0], 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 3),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 3),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 1),
                        new BoxData(0, BoxType.Answer, new SpriteData()),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 3),
                    };
            }

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Choice, spriteDataList[0], 1),
                    new BoxData(1, BoxType.Choice, spriteDataList[0], 2),
                    new BoxData(2, BoxType.Choice, spriteDataList[0], 3),
                    new BoxData(3, BoxType.Choice, spriteDataList[1], 1),
                    new BoxData(4, BoxType.Choice, spriteDataList[1], 2),
                    new BoxData(5, BoxType.Choice, spriteDataList[2], GetRandomColorIndex()),
                    new BoxData(6, BoxType.Choice, spriteDataList[3], GetRandomColorIndex()),
                };

            answerDataIndex = 1;
        }
        // ABA Color Abstract _3x3 _1x7
        else if (questionData.pattern == QuestionPattern.ABA
            && questionData.category == QuestionCategory.Color
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._3x3 && questionData.choiceFieldType == FieldType._1x7)
        {
            // 50% 확률로 정답칸 위치 인덱스 설정 (3 or 5)
            if (Utils.GetRandomBool())
            {
                questionBoxDataList = new List<BoxData>
                    {
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                        new BoxData(0, BoxType.Answer, new SpriteData()),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                    };
            }
            else
            {
                questionBoxDataList = new List<BoxData>
                    {
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                        new BoxData(0, BoxType.Answer, new SpriteData()),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 2),
                        new BoxData(0, BoxType.Question, spriteDataList[0], 0),
                    };
            }

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Choice, spriteDataList[0], 0),
                    new BoxData(1, BoxType.Choice, spriteDataList[0], 2),
                    new BoxData(2, BoxType.Choice, spriteDataList[0], 3),
                    new BoxData(3, BoxType.Choice, spriteDataList[1], 0),
                    new BoxData(4, BoxType.Choice, spriteDataList[1], 2),
                    new BoxData(5, BoxType.Choice, spriteDataList[2], GetRandomColorIndex()),
                    new BoxData(6, BoxType.Choice, spriteDataList[3], GetRandomColorIndex()),
                };

            answerDataIndex = 1;
        }
        // ABAB Scale Abstract _1x7 _1x7
        else if (questionData.pattern == QuestionPattern.ABAB
            && questionData.category == QuestionCategory.Scale
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._1x7 && questionData.choiceFieldType == FieldType._1x7)
        {
            questionBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(0, BoxType.Answer, new SpriteData()),
                };

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Choice, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(1, BoxType.Choice, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(2, BoxType.Choice, spriteDataList[1], answerColor, 0, 1),
                    new BoxData(3, BoxType.Choice, spriteDataList[1], answerColor, 0, 1.5f),
                    new BoxData(4, BoxType.Choice, spriteDataList[2], GetRandomColorIndex(), 0, 1),
                    new BoxData(5, BoxType.Choice, spriteDataList[3], GetRandomColorIndex(), 0, 1.5f),
                    new BoxData(6, BoxType.Choice, spriteDataList[4], GetRandomColorIndex(), 0, 1),
                };

            answerDataIndex = 0;
        }
        // AABB Scale Abstract _1x7 _1x7
        else if (questionData.pattern == QuestionPattern.AABB
            && questionData.category == QuestionCategory.Scale
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._1x7 && questionData.choiceFieldType == FieldType._1x7)
        {
            questionBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Answer, new SpriteData()),
                };

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Choice, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(1, BoxType.Choice, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(2, BoxType.Choice, spriteDataList[1], answerColor, 0, 1),
                    new BoxData(3, BoxType.Choice, spriteDataList[1], answerColor, 0, 1.5f),
                    new BoxData(4, BoxType.Choice, spriteDataList[2], GetRandomColorIndex(), 0, 1),
                    new BoxData(5, BoxType.Choice, spriteDataList[3], GetRandomColorIndex(), 0, 1.5f),
                    new BoxData(6, BoxType.Choice, spriteDataList[4], GetRandomColorIndex(), 0, 1),
                };

            answerDataIndex = 1;
        }
        // AAB Scale Abstract _1x7 _1x7
        else if (questionData.pattern == QuestionPattern.AAB
            && questionData.category == QuestionCategory.Scale
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._1x7 && questionData.choiceFieldType == FieldType._1x7)
        {
            questionBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(0, BoxType.Answer, new SpriteData()),
                };

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Choice, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(1, BoxType.Choice, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(2, BoxType.Choice, spriteDataList[1], answerColor, 0, 1),
                    new BoxData(3, BoxType.Choice, spriteDataList[1], answerColor, 0, 1.5f),
                    new BoxData(4, BoxType.Choice, spriteDataList[2], GetRandomColorIndex(), 0, 1),
                    new BoxData(5, BoxType.Choice, spriteDataList[3], GetRandomColorIndex(), 0, 1.5f),
                    new BoxData(6, BoxType.Choice, spriteDataList[4], GetRandomColorIndex(), 0, 1),
                };

            answerDataIndex = 0;
        }
        // ABA Scale Abstract _3x3 _1x7
        else if (questionData.pattern == QuestionPattern.ABA
            && questionData.category == QuestionCategory.Scale
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._3x3 && questionData.choiceFieldType == FieldType._1x7)
        {
            // 50% 확률로 정답칸 위치 인덱스 설정 (3 or 5)
            if (Utils.GetRandomBool())
            {
                questionBoxDataList = new List<BoxData>
                    {
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Answer, new SpriteData()),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    };
            }
            else
            {
                questionBoxDataList = new List<BoxData>
                    {
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Answer, new SpriteData()),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                    };
            }

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Choice, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(1, BoxType.Choice, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(2, BoxType.Choice, spriteDataList[0], answerColor, 0, 0.5f),
                    new BoxData(3, BoxType.Choice, spriteDataList[1], answerColor, 0, 1),
                    new BoxData(4, BoxType.Choice, spriteDataList[1], answerColor, 0, 1.5f),
                    new BoxData(5, BoxType.Choice, spriteDataList[2], GetRandomColorIndex(), 0, 1),
                    new BoxData(6, BoxType.Choice, spriteDataList[3], GetRandomColorIndex(), 0, 1),
                };

            answerDataIndex = 0;
        }
        // ABC Scale Abstract _3x3 _1x7
        else if (questionData.pattern == QuestionPattern.ABC
            && questionData.category == QuestionCategory.Scale
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._3x3 && questionData.choiceFieldType == FieldType._1x7)
        {
            // 50% 확률로 정답칸 위치 인덱스 설정 (1 or 7)
            if (Utils.GetRandomBool())
            {
                questionBoxDataList = new List<BoxData>
                    {
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Answer, new SpriteData()),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 0.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 0.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 0.5f),
                    };
            }
            else
            {
                questionBoxDataList = new List<BoxData>
                    {
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 0.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 0.5f),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 1.5f),
                        new BoxData(0, BoxType.Answer, new SpriteData()),
                        new BoxData(0, BoxType.Question, spriteDataList[0], answerColor, 0, 0.5f),
                    };
            }

            choiceBoxDataList = new List<BoxData>
                {
                    new BoxData(0, BoxType.Choice, spriteDataList[0], answerColor, 0, 1.5f),
                    new BoxData(1, BoxType.Choice, spriteDataList[0], answerColor, 0, 1),
                    new BoxData(2, BoxType.Choice, spriteDataList[0], answerColor, 0, 0.5f),
                    new BoxData(3, BoxType.Choice, spriteDataList[1], answerColor, 0, 1),
                    new BoxData(4, BoxType.Choice, spriteDataList[1], answerColor, 0, 1.5f),
                    new BoxData(5, BoxType.Choice, spriteDataList[2], GetRandomColorIndex(), 0, 1),
                    new BoxData(6, BoxType.Choice, spriteDataList[3], GetRandomColorIndex(), 0, 1),
                };

            answerDataIndex = 1;
        }
        // ABC Plus None _1x6 _1x6
        else if (questionData.pattern == QuestionPattern.ABC
            && questionData.category == QuestionCategory.Plus
            && questionData.spriteType == SpriteType.None
            && questionData.questionFieldType == FieldType._1x6 && questionData.choiceFieldType == FieldType._1x6)
        {
            // 정답 숫자(연산 문제용)
            int answerNumber = Random.Range(20, 41);
            int numberDifference = Random.Range(1, 4);

            questionBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 5)),
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 4)),
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 3)),
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 2)),
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 1)),
                new BoxData(0, BoxType.Answer, new SpriteData()),
            };

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 2)),
                new BoxData(1, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 1)),
                new BoxData(2, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber),
                new BoxData(3, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 1)),
                new BoxData(4, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 2)),
                new BoxData(5, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 3)),
            };

            answerDataIndex = 2;
        }
        // ABC Minus None _1x6 _1x6
        else if (questionData.pattern == QuestionPattern.ABC
            && questionData.category == QuestionCategory.Minus
            && questionData.spriteType == SpriteType.None
            && questionData.questionFieldType == FieldType._1x6 && questionData.choiceFieldType == FieldType._1x6)
        {
            // 정답 숫자(연산 문제용)
            int answerNumber = Random.Range(20, 41);
            int numberDifference = Random.Range(1, 4);

            questionBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 5)),
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 4)),
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 3)),
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 2)),
                new BoxData(0, BoxType.Question, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 1)),
                new BoxData(0, BoxType.Answer, new SpriteData()),
            };

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 2)),
                new BoxData(1, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber - (numberDifference * 1)),
                new BoxData(2, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber),
                new BoxData(3, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 1)),
                new BoxData(4, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 2)),
                new BoxData(5, BoxType.Choice, new SpriteData(), -1, 0, 1, answerNumber + (numberDifference * 3)),
            };

            answerDataIndex = 2;
        }
        // ABC Rotation_180 Realistic _1x6 _1x7
        else if (questionData.pattern == QuestionPattern.ABC
            && questionData.category == QuestionCategory.Rotation_180
            && questionData.spriteType == SpriteType.Realistic
            && questionData.questionFieldType == FieldType._1x6 && questionData.choiceFieldType == FieldType._1x7)
        {
            float rotateAngle = 180f;

            questionBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, 0),
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, rotateAngle),
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, 0),
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, rotateAngle),
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, 0),
                new BoxData(0, BoxType.Answer, new SpriteData()),
            };

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Choice, spriteDataList[0], -1, 0),
                new BoxData(1, BoxType.Choice, spriteDataList[0], -1, rotateAngle),
                new BoxData(2, BoxType.Choice, spriteDataList[1], -1, 0),
                new BoxData(3, BoxType.Choice, spriteDataList[1], -1, rotateAngle),
                new BoxData(4, BoxType.Choice, spriteDataList[2], -1, 0),
                new BoxData(5, BoxType.Choice, spriteDataList[3], -1, rotateAngle),
                new BoxData(6, BoxType.Choice, spriteDataList[4], -1, rotateAngle),
            };

            answerDataIndex = 1;
        }
        // ABAB Rotation_90 Realistic _1x6 _1x7
        else if (questionData.pattern == QuestionPattern.ABAB
            && questionData.category == QuestionCategory.Rotation_90
            && questionData.spriteType == SpriteType.Realistic
            && questionData.questionFieldType == FieldType._1x6 && questionData.choiceFieldType == FieldType._1x7)
        {
            float rotateAngle = 90f;

            questionBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, 0),
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, rotateAngle * 1),
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, rotateAngle * 2),
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, rotateAngle * 3),
                new BoxData(0, BoxType.Question, spriteDataList[0], -1, 0),
                new BoxData(0, BoxType.Answer, new SpriteData()),
            };

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Choice, spriteDataList[0], -1, 0),
                new BoxData(1, BoxType.Choice, spriteDataList[0], -1, rotateAngle * 1),
                new BoxData(2, BoxType.Choice, spriteDataList[0], -1, rotateAngle * 2),
                new BoxData(3, BoxType.Choice, spriteDataList[0], -1, rotateAngle * 3),
                new BoxData(4, BoxType.Choice, spriteDataList[1], -1, 0),
                new BoxData(5, BoxType.Choice, spriteDataList[1], -1, rotateAngle * 1),
                new BoxData(6, BoxType.Choice, spriteDataList[2], -1, rotateAngle * 2),
            };

            answerDataIndex = 1;
        }
        // None Group Abstract _2x2x2 _2x4
        else if (questionData.pattern == QuestionPattern.None
            && questionData.category == QuestionCategory.Group
            && questionData.spriteType == SpriteType.Abstract
            && questionData.questionFieldType == FieldType._2x2x2 && questionData.choiceFieldType == FieldType._2x4)
        {
            questionBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Answer, new SpriteData()),
                new BoxData(0, BoxType.Answer, new SpriteData()),
                new BoxData(0, BoxType.Answer, new SpriteData()),
                new BoxData(0, BoxType.Answer, new SpriteData()),

                new BoxData(0, BoxType.Answer, new SpriteData()),
                new BoxData(0, BoxType.Answer, new SpriteData()),
                new BoxData(0, BoxType.Answer, new SpriteData()),
                new BoxData(0, BoxType.Answer, new SpriteData()),
            };

            choiceBoxDataList = new List<BoxData>
            {
                new BoxData(0, BoxType.Choice, spriteDataList[0], GetRandomColorIndex()),
                new BoxData(0, BoxType.Choice, spriteDataList[1], GetRandomColorIndex()),
                new BoxData(0, BoxType.Choice, spriteDataList[2], GetRandomColorIndex()),
                new BoxData(0, BoxType.Choice, spriteDataList[3], GetRandomColorIndex()),
                new BoxData(0, BoxType.Choice, spriteDataList[4], GetRandomColorIndex()),
                new BoxData(0, BoxType.Choice, spriteDataList[5], GetRandomColorIndex()),
                new BoxData(0, BoxType.Choice, spriteDataList[6], GetRandomColorIndex()),
                new BoxData(0, BoxType.Choice, spriteDataList[7], GetRandomColorIndex()),
            };

            answerDataIndex = 0;
        }
        
        */

        else
        {
            Debug.Log("Failed SetRoundData()");
        }

        // choice 리스트 요소 셔플 (랜덤하게 섞음)
        choiceBoxDataList.ShuffleList();

        // 정답 인덱스 세팅
        //Managers.Game.currentAnswerDataIndex = answerDataIndex;
    }

    public void SetAnswerData(int _index)
    {
        BoxData data = new BoxData();
        data.SetAnswerData(choiceBoxDataList[_index]);
        answerDataList.Add(data);
    }
}

