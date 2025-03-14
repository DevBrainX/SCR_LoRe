using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Round05 : RoundData
{
    public override void Init()
    {
        base.Init();

        // 현재 라운드에 출제되는 문제의 유형을 담아놓는 리스트 (랜덤하게 뽑기 위함)
        randomQuestionDataList = new List<QuestionData>()
        {
            new QuestionData(0, QuestionPattern.ABC, QuestionCategory.Plus, SpriteType.None, FieldType._1x6, FieldType._1x6),
            //new QuestionData(0, QuestionPattern.ABC, QuestionCategory.Minus, QuestionSpriteType.None, FieldType._1x6, FieldType._1x6),
            
            new QuestionData(0, QuestionPattern.ABC, QuestionCategory.Rotation_180, SpriteType.Realistic, FieldType._1x6, FieldType._1x7),
            new QuestionData(0, QuestionPattern.ABAB, QuestionCategory.Rotation_90, SpriteType.Realistic, FieldType._1x6, FieldType._1x7),
        };

        SetRoundData();
    }
}