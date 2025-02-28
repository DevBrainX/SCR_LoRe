using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public enum BoxType
{
    Question, // ���� (�̵� �Ұ���)
    Choice, // ������ (����, ���� �� �� ����)
    Answer, // ������ ����ִ� ĭ
}

public class BoxBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public SpriteRenderer image;
    public SpriteRenderer outline;

    [HideInInspector] public BoxData data;

    BoxType type;

    Vector3 oriLocalPos;

    // ����Ŭ�� üũ�� ����
    public float doubleClickTime = 0.3f; // ���� Ŭ������ �ν��� �ִ� �ð�
    private float lastClickTime = 0f;

    public void Init(BoxType _type, BoxData _data, Vector3 _oriLocalPos)
    {
        type = _type;
        data = _data;
        oriLocalPos = _oriLocalPos;

        // ����ڽ� �϶��� �׵θ� on
        if (type == BoxType.Answer)
        {
            outline.gameObject.SetActive(true);
            SetOutlineColor(Color.gray);
        }
        else
        {
            outline.gameObject.SetActive(false);
        }
    }

    public void SetData(BoxData _data)
    {
        data.SetData(_data);
    }

    void Start()
    {
        SetOriginalPosition();

        if (type != BoxType.Answer)
        {
            SetSprite();
        }

        SetColor();
        SetScale();
    }

    public void SetSprite()
    {
        image.sprite = Managers.Game.imageList[data.categoryIndex].spriteList[data.spriteIndex];
    }

    public void SetColor()
    {
        switch (data.colorIndex)
        {
            case 0: image.color = Color.red; break;
            case 1: image.color = Color.green; break;
            case 2: image.color = Color.blue; break;
            case 3: image.color = Color.yellow; break;
            case 4: image.color = Color.gray; break;
            default: image.color = Color.white; break;
        }
    }

    public void SetScale()
    {
        image.transform.localScale = new Vector3(100f / 512f, 100f / 512f, 1f);
    }

    public void SetOutlineColor(Color _color)
    {
        outline.color = _color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (type != BoxType.Choice)
        {
            return;
        }

        // ���콺 ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = transform.position.z;

        transform.position = mouseWorldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (type != BoxType.Choice)
        {
            return;
        }

        // ���콺 ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = transform.position.z;  //0; // 2D ���ӿ����� z ���� 0���� ����

        BoxBehaviour answerBox = Managers.Game.answerBox;

        // ����ڽ��� ���콺 �������� ����ڽ��� ����, �ƴϸ� ���� ��ġ��
        if (GetComponent<BoxCollider2D>().OverlapPoint(answerBox.transform.position))
        {
            Managers.Game.InsertInAnswerBox(this);
        }

        SetOriginalPosition();
    }

    void OnMouseUp()
    {
        // ���� �ð��� ������ Ŭ�� �ð� ���� ���
        if (Time.time - lastClickTime <= doubleClickTime)
        {
            // ���� Ŭ�� ����
            OnDoubleClick();
        }
        else
        {
            // ���� Ŭ������ �ν�
            lastClickTime = Time.time;
        }
    }

    private void OnDoubleClick()
    {
        if (type == BoxType.Answer)
        {
            Managers.Game.RemoveInAnswerBox();
        }
        else if (type == BoxType.Choice)
        {
            Managers.Game.InsertInAnswerBox(this);
        }
    }

    public void SetOriginalPosition()
    {
        transform.localPosition = oriLocalPos;
    }

    void Update()
    {

    }
}
