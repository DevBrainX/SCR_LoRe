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
    Green,
    Blue,
    Yellow,
    Gray,
    MAX
}

public class BoxData
{
    public int index;
    public BoxType type;
    public int categoryIndex;
    public int spriteIndex;
    public int colorIndex;
    public float angle;
    public float scale;

    public BoxData()
    {
        index = -1;
        type = BoxType.None;
        categoryIndex = -1;
        spriteIndex = -1;
        colorIndex = -1;
        angle = 0f;
        scale = 0f;
    }

    public void SetData(BoxData _data)
    {
        index = _data.index;
        type = _data.type;
        categoryIndex = _data.categoryIndex;
        spriteIndex = _data.spriteIndex;
        colorIndex = _data.colorIndex;
        angle = _data.angle;
        scale = _data.scale;
    }
}