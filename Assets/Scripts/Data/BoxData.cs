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
    public int inDataIndex;

    public SpriteData spriteData;
    public int colorIndex;
    public float angle;
    public float scale;
    public int number;

    public BoxData()
    {
        index = -1;
        type = BoxType.None;
        inDataIndex = -1;

        SetEmptyData();
    }

    public BoxData(int _index, BoxType _type, SpriteData _spriteData,
        int _colorIndex = -1, float _angle = 0f, float _scale = 1f, int _number = -1)
    {
        index = _index;
        type = _type;
        inDataIndex = -1;

        spriteData = _spriteData;
        colorIndex = _colorIndex;
        angle = _angle;
        scale = _scale;
        number = _number;
    }

    public void SetEmptyData()
    {
        //index = _data.index;
        //type = _data.type;
        inDataIndex = -1;

        spriteData = new SpriteData();
        colorIndex = (int)ColorIndex.White;
        angle = 0f;
        scale = 1f;
        number = -1;
    }

    public void SetAnswerData(BoxData _data)
    {
        //index = _data.index;
        //type = _data.type;
        inDataIndex = _data.index;

        spriteData = _data.spriteData;
        colorIndex = _data.colorIndex;
        angle = _data.angle;
        scale = _data.scale;
        number = _data.number;
    }
}