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

public enum FieldType
{
    None = -1,
    _1x7,
    _3x3,
    _1x6,
    //_2x4,
    //_2x6,
    //Field_1x4x2,
    //Field_2x2x2,
    //Field_2x2x3,
}

public enum QuestionCategory
{
    None = -1,
    Shape = 0,
    Color,
    Scale,
    Rotation_90,
    Rotation_180,
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
    public QuestionCategory category;
    public QuestionSpriteType spriteType;
    public FieldType questionFieldType;
    public FieldType choiceFieldType;

    public QuestionData(int _index, QuestionPattern _pattern, QuestionCategory _category,
        QuestionSpriteType _spriteType, FieldType _questionFieldType, FieldType _choiceFieldType)
    {
        index = _index;
        pattern = _pattern;
        category = _category;
        spriteType = _spriteType;
        questionFieldType = _questionFieldType;
        choiceFieldType = _choiceFieldType;
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
        return $"Index: {index}, Pattern: {pattern}, Category: {category}, SpriteType: {spriteType}, QuestionField: {questionFieldType}, ChoiceField: {choiceFieldType}";
    }
}
