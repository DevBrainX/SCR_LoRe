using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Round03 : RoundData
{
    public override void Init()
    {
        base.Init();

        // 현재 라운드에 출제되는 문제의 유형을 담아놓는 리스트 (랜덤하게 뽑기 위함)
        randomQuestionDataList = new List<QuestionData>()
        {
            new QuestionData(0, QuestionPattern.ABAB, QuestionCategory.Scale, QuestionSpriteType.Abstract, FieldType._1x7, FieldType._1x7),
            new QuestionData(1, QuestionPattern.AABB, QuestionCategory.Scale, QuestionSpriteType.Abstract, FieldType._1x7, FieldType._1x7),
            new QuestionData(2, QuestionPattern.AAB, QuestionCategory.Scale, QuestionSpriteType.Abstract, FieldType._1x7, FieldType._1x7),
            new QuestionData(3, QuestionPattern.ABC, QuestionCategory.Scale, QuestionSpriteType.Abstract, FieldType._3x3, FieldType._1x7),
            new QuestionData(4, QuestionPattern.ABA, QuestionCategory.Scale, QuestionSpriteType.Abstract, FieldType._3x3, FieldType._1x7),

            //new QuestionData(0, QuestionPattern.ABAB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Scale, QuestionSpriteType.Abstract),
            //new QuestionData(1, QuestionPattern.AABB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Scale, QuestionSpriteType.Abstract),
            //new QuestionData(2, QuestionPattern.AAB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Scale, QuestionSpriteType.Abstract),
            //new QuestionData(3, QuestionPattern.ABC, QuestionMatrixType.Matrix_3x3, QuestionCategory.Scale, QuestionSpriteType.Abstract),
            //new QuestionData(4, QuestionPattern.ABA, QuestionMatrixType.Matrix_3x3, QuestionCategory.Scale, QuestionSpriteType.Abstract),
        };

        SetRoundData();

        /*

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

        // 문제데이터에 사용될 스프라이트 인덱스 랜덤하게 세팅
        SetRandomSpriteDataList(questionData.spriteType, 7);

        // 정답의 색상
        int answerColor = GetRandomColorIndex();

        // 문제 데이터 리스트, 선택지 데이터 리스트 세팅
        if (questionData.index == 0) // ABAB Matrix_1x7
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
        else if (questionData.index == 1) // AABB Matrix_1x7
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
        else if (questionData.index == 2) // AAB Matrix_1x7
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
        else if (questionData.index == 3) // ABC Matrix_3x3
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
        else if (questionData.index == 4) // ABA Matrix_3x3
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
        else
        {
            Debug.Log("Failed Init()");
        }

        // choice 리스트 요소 셔플 (랜덤하게 섞음)
        Utils.ShuffleList(choiceBoxDataList);

        // 정답 인덱스 세팅
        Managers.Game.currentAnswerDataIndex = answerDataIndex;

        */
    }
}