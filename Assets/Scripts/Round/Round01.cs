using System.Collections.Generic;

public class Round01 : RoundData
{
    public override void Init()
    {
        base.Init();

        // 현재 라운드에 출제되는 문제의 유형을 담아놓는 리스트 (랜덤하게 뽑기 위함)
        randomQuestionDataList = new List<QuestionData>()
        {
            new QuestionData(0, QuestionPattern.AAAA, QuestionCategory.Shape, SpriteType.Realistic, FieldType._1x7, FieldType._1x7),
            new QuestionData(1, QuestionPattern.ABAB, QuestionCategory.Shape, SpriteType.Realistic, FieldType._1x7, FieldType._1x7),
            new QuestionData(2, QuestionPattern.ABC, QuestionCategory.Shape, SpriteType.Realistic, FieldType._3x3, FieldType._1x7),
            new QuestionData(3, QuestionPattern.ABA, QuestionCategory.Shape, SpriteType.Realistic, FieldType._3x3, FieldType._1x7),

            //new QuestionData(0, QuestionPattern.AAAA, QuestionMatrixType.Matrix_1x7, QuestionCategory.Shape, QuestionSpriteType.Realistic),
            //new QuestionData(1, QuestionPattern.ABAB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Shape, QuestionSpriteType.Realistic),
            //new QuestionData(2, QuestionPattern.ABC, QuestionMatrixType.Matrix_3x3, QuestionCategory.Shape, QuestionSpriteType.Realistic),
            //new QuestionData(3, QuestionPattern.ABA, QuestionMatrixType.Matrix_3x3, QuestionCategory.Shape, QuestionSpriteType.Realistic),
        };

        SetRoundData();
    }
}