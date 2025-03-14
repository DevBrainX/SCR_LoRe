using System;
using static UnityEditor.U2D.ScriptablePacker;

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
    public SpriteType type;
    public int category;
    public int index;

    public SpriteData()
    {
        type = SpriteType.None;
        category = -1;
        index = -1;
    }

    public void SetData(SpriteData _data)
    {
        type = _data.type;
        category = _data.category;
        index = _data.index;
    }

    public bool IsSameData(SpriteData _data)
    {
        if (type == _data.type
            && category == _data.category
            && index == _data.index)
        {
            return true;
        }

        return false;
    }
}

public class BoxData
{
    public BoxType type;

    public SpriteData spriteData;
    public int colorIndex;
    public float angle;
    public float scale;
    public int number;

    public BoxData()
    {
        type = BoxType.None;

        SetEmptyData();
    }

    public BoxData(BoxType _type, SpriteData _spriteData,
        int _colorIndex = -1, float _angle = 0f, float _scale = 1f, int _number = -1)
    {
        type = _type;

        spriteData = _spriteData;
        colorIndex = _colorIndex;
        angle = _angle;
        scale = _scale;
        number = _number;
    }

    public void SetEmptyData()
    {
        //type = _data.type;

        spriteData = new SpriteData();
        colorIndex = (int)ColorIndex.White;
        angle = 0f;
        scale = 1f;
        number = -1;
    }

    public void SetAnswerData(BoxData _data)
    {
        //type = _data.type;

        spriteData = new SpriteData();
        spriteData.SetData(_data.spriteData);
        colorIndex = _data.colorIndex;
        angle = _data.angle;
        scale = _data.scale;
        number = _data.number;
    }

    public bool IsSameData(BoxData _data)
    {
        if (spriteData.IsSameData(_data.spriteData)
            && colorIndex == _data.colorIndex
            && angle == _data.angle
            && scale == _data.scale
            && number == _data.number)
        {
            return true;
        }

        return false;
    }
}