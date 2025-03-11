using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public class BoxBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] SpriteRenderer image;
    [SerializeField] SpriteRenderer outline;
    [SerializeField] TextMeshPro number;

    public BoxData data;
    public bool isInAnswerSlot = false;
    public int slotIndex = -1; // 현재 인덱스 몇번의 슬롯에 들어있는지?

    //Vector3 oriLocalPos;

    // 드래그 이동 관련 변수
    private Vector3 initialPos;
    private Vector3 dragStartPos;

    // 더블클릭 체크용 변수
    public float doubleClickTime = 0.3f; // 더블 클릭으로 인식할 최대 시간
    private float lastClickTime = 0f;

    public void Init(BoxData _data, Vector3 _pos)
    {
        data = _data;
        //oriLocalPos = _oriLocalPos;
        initialPos = _pos;

        // 정답박스 일때는 테두리 on
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

    private void Start()
    {
        //SetOriginalPosition();
        transform.localPosition = initialPos;

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

        // 드래그 시작 시 오브젝트의 현재 위치를 저장
        initialPos = transform.position;

        // 마우스 시작 위치 저장
        dragStartPos = Camera.main.ScreenToWorldPoint(eventData.position);
        dragStartPos.z = initialPos.z; // Z축을 현재 오브젝트의 Z축으로 설정
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (data.type != BoxType.Choice)
        {
            return;
        }

        // 현재 마우스 위치 계산 (마우스 화면 좌표를 월드 좌표로 변환)
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        currentMousePos.z = initialPos.z;

        // 드래그 이동량 계산
        Vector3 delta = currentMousePos - dragStartPos;
        delta.z -= 0.1f;

        // 오브젝트 위치 업데이트
        transform.position = initialPos + delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (data.type != BoxType.Choice)
        {
            return;
        }

        // 마우스 화면 좌표를 월드 좌표로 변환
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        currentMousePos.z = transform.position.z;

        //BoxBehaviour answerBox = Managers.Game.GetFrontAnswerBox();

        //// 정답박스에 마우스 들어가있으면 정답박스에 고정, 아니면 원래 위치로
        //if (answerBox.IsOverlapPoint(mouseWorldPos))
        //{2
        //    Managers.Game.InsertInAnswerBox(this);
        //}

        //SetOriginalPosition();
    }

    void OnMouseUp()
    {

    }

    void OnMouseDown()
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
        //if (data.type == BoxType.Answer)
        //{
        //    Managers.Game.RemoveInAnswerBox();
        //}

        if (data.type == BoxType.Choice)
        {
            // 슬롯에 들어있지않을때 -> Answer Slot으로 이동
            if (!isInAnswerSlot)
            {
                SlotData answerSlot = Managers.Game.GetAnswerSlot();
                transform.position = answerSlot.pos;
                answerSlot.hasBox = true;

                Managers.Game.choiceSlotList[slotIndex].hasBox = false;
                isInAnswerSlot = true;
                slotIndex = answerSlot.index;
            }
            // 슬롯에 들어있을때 -> Choice Slot으로 이동
            else
            {
                SlotData choiceSlot = Managers.Game.GetEmptyChoiceSlot();
                transform.position = choiceSlot.pos;
                choiceSlot.hasBox = true;

                Managers.Game.answerSlotList[slotIndex].hasBox = false;
                isInAnswerSlot = false;
                slotIndex = choiceSlot.index;
            }
        }
    }

    public bool IsOverlapPoint(Vector2 _point)
    {
        if (GetComponent<BoxCollider2D>().OverlapPoint(_point))
        {
            return true;
        }

        return false;
    }

    //public void SetOriginalPosition()
    //{
    //    transform.localPosition = oriLocalPos;
    //}

    //public void SetData(BoxData _data)
    //{
    //    data.SetData(_data);
    //}

    public void SetEmptyData()
    {
        data.SetEmptyData();
    }

    public void SetAnswerData(BoxBehaviour _box)
    {
        data.SetAnswerData(_box.data);
    }

    void Update()
    {

    }
}
