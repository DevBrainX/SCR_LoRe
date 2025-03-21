﻿using System.Collections.Generic;

public class Round06 : RoundData
{
    public override void Init()
    {
        base.Init();

        // 현재 라운드에 출제되는 문제의 유형을 담아놓는 리스트 (랜덤하게 뽑기 위함)
        randomQuestionDataList = new List<QuestionData>()
        {
            new QuestionData(0, QuestionPattern.None, QuestionCategory.Group, SpriteType.Abstract, FieldType._2x2x2, FieldType._2x4),
        };

        SetRoundData();
    }
}