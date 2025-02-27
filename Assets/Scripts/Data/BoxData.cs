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
    public int categoryIndex;
    public int spriteIndex;
    public int colorIndex;
    public float angle;
    public float scale;
    public bool isAnswer;

    public BoxData()
    {
        categoryIndex = -1;
        spriteIndex = -1;
        colorIndex = -1;
        angle = 0f;
        scale = 0f;
        isAnswer = false;
    }

    public void SetData(BoxData _data)
    {
        categoryIndex = _data.categoryIndex;
        spriteIndex = _data.spriteIndex;
        colorIndex = _data.colorIndex;
        angle = _data.angle;
        scale = _data.scale;
        isAnswer = _data.isAnswer;
    }
}