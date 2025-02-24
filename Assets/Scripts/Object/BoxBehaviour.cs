using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public enum BoxType
{
    Question, // ���� (�̵� �Ұ���)
    Choice, // ������ (����, ���� �� �� ����)
    //Answer, // ������ ����ִ� ĭ
}

public class BoxBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] SpriteRenderer image;

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

        if (type == BoxType.Choice)
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }

        transform.localPosition = oriLocalPos;

        image.color = color;
    }

    public void Init(BoxType _type, Color _color, Vector3 _oriLocalPos)
    {
        type = _type;
        oriLocalPos = _oriLocalPos;
        color = _color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���콺 ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = m_ZCoord;

        transform.position = mouseWorldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ���콺 ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0; // 2D ���ӿ����� z ���� 0���� ����

        GameObject answerBox = Managers.Game.answerBox;

        // ����ڽ��� ���콺 �������� ����ڽ��� ����, �ƴϸ� ���� ��ġ��
        if (answerBox.GetComponent<BoxCollider2D>().OverlapPoint(mouseWorldPos))
        {
            InsertInAnswerBox();
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
        InsertInAnswerBox();
    }

    private void InsertInAnswerBox()
    {
        //GameObject answerObject = Managers.Game.answerBox;

        AnswerBox answerBox = Managers.Game.answerBox.GetComponent<AnswerBox>();

        //// ����ڽ��� �̹� ����ִ��� Ȯ��
        //if (answerObject.GetComponent<AnswerBox>().insertedBox != null)
        //{
        //    answerObject.GetComponent<AnswerBox>().insertedBox.gameObject.SetActive(true);
        //}

        // ����ڽ��� �̹� ������ ������Ʈ �������� �ٽ� Ȱ��ȭ
        if (answerBox.currentChoiceIndex != -1)
        {
            Managers.Game.choiceBoxList[answerBox.currentChoiceIndex].gameObject.SetActive(true);
        }

        //answerObject.GetComponent<AnswerBox>().insertedBox = this;
        answerBox.SetSprite(image);
        answerBox.currentChoiceIndex = Managers.Game.choiceBoxList.IndexOf(this);
        this.gameObject.SetActive(false);
    }

    public void SetOriginalPosition()
    {
        transform.localPosition = oriLocalPos;
    }

    //void OnMouseDrag()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    mousePos.z = m_ZCoord;
    //
    //    transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    //}

    //void OnMouseUp()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    mousePos.z = m_ZCoord;
    //    //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
    //    transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    //}

    void Update()
    {

    }
}
