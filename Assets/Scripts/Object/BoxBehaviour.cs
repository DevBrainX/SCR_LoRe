using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public enum BoxType
{
    Question, // 문제 (이동 불가능)
    Choice, // 선택지 (정답, 오답 둘 다 가능)
    Answer, // 정답을 집어넣는 칸
}

public class BoxBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public SpriteRenderer image;
    public SpriteRenderer outline;

    [HideInInspector] public BoxData data;

    BoxType type;

    Vector3 oriLocalPos;

    // 더블클릭 체크용 변수
    public float doubleClickTime = 0.3f; // 더블 클릭으로 인식할 최대 시간
    private float lastClickTime = 0f;

    public void Init(BoxType _type, BoxData _data, Vector3 _oriLocalPos)
    {
        type = _type;
        data = _data;
        oriLocalPos = _oriLocalPos;

        // 정답박스 일때는 테두리 on
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

        // 마우스 화면 좌표를 월드 좌표로 변환
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

        // 마우스 화면 좌표를 월드 좌표로 변환
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = transform.position.z;  //0; // 2D 게임에서는 z 축을 0으로 설정

        BoxBehaviour answerBox = Managers.Game.answerBox;

        // 정답박스에 마우스 들어가있으면 정답박스에 고정, 아니면 원래 위치로
        if (GetComponent<BoxCollider2D>().OverlapPoint(answerBox.transform.position))
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
