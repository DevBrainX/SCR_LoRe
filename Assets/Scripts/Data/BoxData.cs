using System;

public enum BoxType
{
    None = -1,
    Question = 0, // 문제 (이동 불가능)
    Choice, // 선택지 (정답, 오답 둘 다 가능)
    Answer, // 정답을 집어넣는 칸
}

public enum ColorIndex
{
    White = -1,
    Red = 0,
    Blue,
    Yellow,
    Green,
    MAX
}

public class SpriteData
{
    public QuestionSpriteType type;
    public int category;
    public int index;

    public SpriteData()
    {
        type = QuestionSpriteType.None;
        category = -1;
        index = -1;
    }
}

public class BoxData
{
    public int index;
    public BoxType type;
    public SpriteData spriteData;
    //public QuestionSpriteType spriteType;
    //public int spriteCategoryIndex;
    //public int spriteIndex;
    public int colorIndex;
    public float angle;
    public float scale;
    public int number;

    public BoxData()
    {
        index = -1;
        type = BoxType.None;
        spriteData = new SpriteData();
        //spriteType = QuestionSpriteType.None;
        //spriteCategoryIndex = -1;
        //spriteIndex = -1;
        colorIndex = (int)ColorIndex.White;
        angle = 0f;
        scale = 1f;
        number = -1;
    }

    public BoxData(int _index, BoxType _type, SpriteData _spriteData,
        int _colorIndex = -1, float _angle = 0f, float _scale = 1f, int _number = -1)
    {
        index = _index;
        type = _type;
        spriteData = _spriteData;
        //spriteType = _spriteType;
        //spriteCategoryIndex = _spriteCategoryIndex;
        //spriteIndex = _spriteIndex;
        colorIndex = _colorIndex;
        angle = _angle;
        scale = _scale;
        number = _number;
    }

    //public void SetData(BoxData _data)
    //{
    //    index = _data.index;
    //    type = _data.type;
    //    spriteData = _data.spriteData;
    //    //spriteType = _data.spriteType;
    //    //spriteCategoryIndex = _data.spriteCategoryIndex;
    //    //spriteIndex = _data.spriteIndex;
    //    colorIndex = _data.colorIndex;
    //    angle = _data.angle;
    //    scale = _data.scale;
    //}
}