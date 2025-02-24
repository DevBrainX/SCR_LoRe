using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public enum BoxType
{
    Question, // 문제 (이동 불가능)
    Choice, // 선택지 (정답, 오답 둘 다 가능)
    //Answer, // 정답을 집어넣는 칸
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

    // 더블클릭 체크용 변수
    public float doubleClickTime = 0.3f; // 더블 클릭으로 인식할 최대 시간
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
        // 마우스 화면 좌표를 월드 좌표로 변환
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = m_ZCoord;

        transform.position = mouseWorldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 마우스 화면 좌표를 월드 좌표로 변환
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0; // 2D 게임에서는 z 축을 0으로 설정

        GameObject answerBox = Managers.Game.answerBox;

        // 정답박스에 마우스 들어가있으면 정답박스에 고정, 아니면 원래 위치로
        if (answerBox.GetComponent<BoxCollider2D>().OverlapPoint(mouseWorldPos))
        {
            InsertInAnswerBox();
        }

        SetOriginalPosition();
    }

    void OnMouseUp()
    {
        // 현재 시간과 마지막 클릭 시간 차이 계산
        if (Time.time - lastClickTime <= doubleClickTime)
        {
            // 더블 클릭 감지
            OnDoubleClick();
        }
        else
        {
            // 단일 클릭으로 인식
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

        //// 정답박스에 이미 들어있는지 확인
        //if (answerObject.GetComponent<AnswerBox>().insertedBox != null)
        //{
        //    answerObject.GetComponent<AnswerBox>().insertedBox.gameObject.SetActive(true);
        //}

        // 정답박스에 이미 선택지 오브젝트 들어가있으면 다시 활성화
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
