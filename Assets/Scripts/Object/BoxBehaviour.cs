using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public enum BoxType
{
    Question, // ���� (�̵� �Ұ���)
    Choice, // ������ (����, ���� �� �� ����)
    Answer, // ������ ����ִ� ĭ
}

public enum ObjectCategory
{
    Carnivore = 0,  // ����-����
    Herbivore,      // ����-�ʽ�
    Fish,           // �ؾ�-�����
    MarineMammal,    // �ؾ�-������
}



public class BoxBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public SpriteRenderer image;
    public SpriteRenderer outline;

    int spriteIndex;
    int colorIndex;
    int angleIndex;
    int scaleIndex;

    BoxType type;

    Color color;

    Vector3 oriLocalPos;

    float m_ZCoord;

    BoxCollider2D boxCollider;

    // ����Ŭ�� üũ�� ����
    public float doubleClickTime = 0.3f; // ���� Ŭ������ �ν��� �ִ� �ð�
    private float lastClickTime = 0f;

    void Start()
    {
        m_ZCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        boxCollider = GetComponent<BoxCollider2D>();

        //if (type == BoxType.Choice)
        //{
        //    boxCollider.enabled = true;
        //}
        //else
        //{
        //    boxCollider.enabled = false;
        //}

        transform.localPosition = oriLocalPos;

        image.color = color;
    }

    public void Init(BoxType _type, Color _color, Vector3 _oriLocalPos)
    {
        type = _type;
        oriLocalPos = _oriLocalPos;
        color = _color;

        image.transform.localScale = new Vector3(100f / 512f, 100f / 512f, 1f);

        if (type == BoxType.Answer)
        {
            outline.gameObject.SetActive(true);
        }
        else
        {
            outline.gameObject.SetActive(false);
        }
    }

    public void SetSprite(SpriteRenderer _image)
    {
        image.sprite = _image.sprite;
        image.color = _image.color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (type != BoxType.Choice)
        {
            return;
        }

        // ���콺 ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = m_ZCoord;

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
        mouseWorldPos.z = 0; // 2D ���ӿ����� z ���� 0���� ����

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
