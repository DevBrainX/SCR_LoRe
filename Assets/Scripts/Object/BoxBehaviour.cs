using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;


public class BoxBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] SpriteRenderer image;
    [SerializeField] SpriteRenderer outline;
    [SerializeField] TextMeshPro number;

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

    void Start()
    {
        SetOriginalPosition();

        //if (data.type != BoxType.Answer)
        //{
        //    SetImageProperties();
        //}
        SetImageProperties();
    }

    public void SetImageProperties()
    {
        SetSprite();
        SetColor();
        SetRotation();
        SetScale();
        SetNumber();
    }

    public void SetSprite()
    {
        if (data.spriteData.category == -1 || data.spriteData.index == -1)
        {
            image.sprite = null;
        }
        else
        {
            if (data.spriteData.type == QuestionSpriteType.Realistic)
            {
                image.sprite = Managers.Resource.realSpriteList[data.spriteData.category][data.spriteData.index];
            }
            else if (data.spriteData.type == QuestionSpriteType.Abstract)
            {
                image.sprite = Managers.Resource.abstSpriteList[data.spriteData.category][data.spriteData.index];
            }
        }
    }

    public void SetColor()
    {
        switch (data.colorIndex)
        {
            case (int)ColorIndex.Red:
                image.color = new Color(255f / 255f, 99f / 255f, 71f / 255f);
                break;
            case (int)ColorIndex.Blue:
                image.color = new Color(30f / 255f, 144f / 255f, 255f / 255f);
                break;
            case (int)ColorIndex.Yellow:
                image.color = new Color(255f / 255f, 215f / 255f, 0f / 255f);
                break;
            case (int)ColorIndex.Green:
                image.color = new Color(50f / 255f, 205f / 255f, 50f / 255f);
                break;
            default: image.color = Color.white; break; // -1
        }
    }

    public void SetRotation()
    {
        image.transform.localRotation = Quaternion.Euler(0, 0, data.angle);
    }

    public void SetScale()
    {
        image.transform.localScale = new Vector3(100f / 512f * data.scale, 100f / 512f * data.scale, 1f);
    }

    public void SetNumber()
    {
        if (data.number > 0)
        {
            number.gameObject.SetActive(true);
            number.text = (data.number).ToString();
        }
        else
        {
            number.gameObject.SetActive(false);
        }
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

    //public void SetData(BoxData _data)
    //{
    //    data.SetData(_data);
    //}

    void Update()
    {

    }
}
