using System;

public enum QuestionPattern
{
    None = -1,
    AAAA = 0,
    ABAB,
    AABB,
    ABA,
    ABC,
    AAB,
}

public enum QuestionMatrixType
{
    None = -1,
    Matrix_1x7 = 0,
    Matrix_3x3,
}

public enum QuestionCategory
{
    None = -1,
    Shape = 0,
    Color,
    Scale,
    Rotation,
    Plus,
    Minus,
}

public enum QuestionSpriteType
{
    None = -1,
    Realistic = 0,
    Abstract,
}

public class QuestionData
{
    public int index;
    public QuestionPattern pattern;
    public QuestionMatrixType matrixType;
    public QuestionCategory category;
    public QuestionSpriteType spriteType;

    public QuestionData(int _index, QuestionPattern _pattern, QuestionMatrixType _matrixType,
        QuestionCategory _category, QuestionSpriteType _spriteType)
    {
        index = _index;
        pattern = _pattern;
        matrixType = _matrixType;
        category = _category;
        spriteType = _spriteType;
    }

    //public void SetData(QuestionData _data)
    //{
    //    pattern = _data.pattern;
    //    matrixType = _data.matrixType;
    //    category = _data.category;
    //    spriteType = _data.spriteType;
    //}

    // 디버깅용 함수
    public override string ToString()
    {
        return $"Index: {index}, Pattern: {pattern}, MatrixType: {matrixType}, Category: {category}, SpriteType: {spriteType}";
    }
}
