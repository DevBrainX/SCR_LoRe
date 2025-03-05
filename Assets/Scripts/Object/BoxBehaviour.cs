using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;


public class BoxBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SpriteRenderer image;
    public SpriteRenderer outline;

    [HideInInspector] public BoxData data;

    Vector3 oriLocalPos;

    // �巡�� �̵� ���� ����
    private Vector3 initialPosition;
    private Vector3 dragStartPosition;

    // ����Ŭ�� üũ�� ����
    public float doubleClickTime = 0.3f; // ���� Ŭ������ �ν��� �ִ� �ð�
    private float lastClickTime = 0f;

    public void Init(BoxData _data, Vector3 _oriLocalPos)
    {
        data = _data;
        oriLocalPos = _oriLocalPos;

        // ����ڽ� �϶��� �׵θ� on
        if (data.type == BoxType.Answer)
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

        if (data.type != BoxType.Answer)
        {
            SetSprite();
        }

        SetColor();
        SetScale();
    }

    public void SetSprite()
    {
        if (data.spriteCategoryIndex == -1 || data.spriteIndex == -1)
        {
            image.sprite = null;
        }
        else
        {
            if (data.spriteType == QuestionSpriteType.Realistic)
            {
                image.sprite = Managers.Resource.realSpriteList[data.spriteCategoryIndex][data.spriteIndex];
            }
            else if (data.spriteType == QuestionSpriteType.Abstract)
            {
                image.sprite = Managers.Resource.abstSpriteList[data.spriteCategoryIndex][data.spriteIndex];
            }
        }
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
            default: image.color = Color.white; break; // -1
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (data.type != BoxType.Choice)
        {
            return;
        }

        // �巡�� ���� �� ������Ʈ�� ���� ��ġ�� ����
        initialPosition = transform.position;

        // ���콺 ���� ��ġ ����
        dragStartPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        dragStartPosition.z = initialPosition.z; // Z���� ���� ������Ʈ�� Z������ ����
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (data.type != BoxType.Choice)
        {
            return;
        }

        // ���� ���콺 ��ġ ��� (���콺 ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ)
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        currentMousePosition.z = initialPosition.z;

        // �巡�� �̵��� ���
        Vector3 delta = currentMousePosition - dragStartPosition;
        delta.z -= 0.1f;

        // ������Ʈ ��ġ ������Ʈ
        transform.position = initialPosition + delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (data.type != BoxType.Choice)
        {
            return;
        }

        // ���콺 ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = transform.position.z;

        BoxBehaviour answerBox = Managers.Game.answerBox;

        // ����ڽ��� ���콺 �������� ����ڽ��� ����, �ƴϸ� ���� ��ġ��
        if (answerBox.GetComponent<BoxCollider2D>().OverlapPoint(mouseWorldPos))
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
        if (data.type == BoxType.Answer)
        {
            Managers.Game.RemoveInAnswerBox();
        }
        else if (data.type == BoxType.Choice)
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
