using System.Collections.Generic;

public class Round02 : RoundData
{
    public override void Init()
    {
        base.Init();

        // 현재 라운드에 출제되는 문제의 유형을 담아놓는 리스트 (랜덤하게 뽑기 위함)
        randomQuestionDataList = new List<QuestionData>()
        {
            new QuestionData(0, QuestionPattern.AAAA, QuestionCategory.Color, SpriteType.Abstract, FieldType._1x7, FieldType._1x7),
            new QuestionData(1, QuestionPattern.ABAB, QuestionCategory.Color, SpriteType.Abstract, FieldType._1x7, FieldType._1x7),
            new QuestionData(2, QuestionPattern.AABB, QuestionCategory.Color, SpriteType.Abstract, FieldType._1x7, FieldType._1x7),
            new QuestionData(3, QuestionPattern.ABC, QuestionCategory.Color, SpriteType.Abstract, FieldType._3x3, FieldType._1x7),
            new QuestionData(4, QuestionPattern.ABA, QuestionCategory.Color, SpriteType.Abstract, FieldType._3x3, FieldType._1x7),

            //new QuestionData(0, QuestionPattern.AAAA, QuestionMatrixType.Matrix_1x7, QuestionCategory.Color, QuestionSpriteType.Abstract),
            //new QuestionData(1, QuestionPattern.ABAB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Color, QuestionSpriteType.Abstract),
            //new QuestionData(2, QuestionPattern.AABB, QuestionMatrixType.Matrix_1x7, QuestionCategory.Color, QuestionSpriteType.Abstract),
            //new QuestionData(3, QuestionPattern.ABC, QuestionMatrixType.Matrix_3x3, QuestionCategory.Color, QuestionSpriteType.Abstract),
            //new QuestionData(4, QuestionPattern.ABA, QuestionMatrixType.Matrix_3x3, QuestionCategory.Color, QuestionSpriteType.Abstract),
        };

        SetRoundData();
    }
}