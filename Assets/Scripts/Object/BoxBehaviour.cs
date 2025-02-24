using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public enum BoxType
{
    Question, // 문제 (이동 불가능)
    Choice, // 선택지 (정답, 오답 둘 다 가능)
    Answer, // 정답을 집어넣는 칸
}

public enum ObjectCategory
{
    Carnivore = 0,  // 동물-육식
    Herbivore,      // 동물-초식
    Fish,           // 해양-물고기
    MarineMammal,    // 해양-포유류
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

    // 더블클릭 체크용 변수
    public float doubleClickTime = 0.3f; // 더블 클릭으로 인식할 최대 시간
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

        // 마우스 화면 좌표를 월드 좌표로 변환
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

        // 마우스 화면 좌표를 월드 좌표로 변환
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0; // 2D 게임에서는 z 축을 0으로 설정

        BoxBehaviour answerBox = Managers.Game.answerBox;

        // 정답박스에 마우스 들어가있으면 정답박스에 고정, 아니면 원래 위치로
        if (answerBox.GetComponent<BoxCollider2D>().OverlapPoint(mouseWorldPos))
        {
            Managers.Game.InsertInAnswerBox(this);
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
