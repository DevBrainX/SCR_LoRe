using UnityEngine;
using UnityEngine.Rendering;

//public class AnswerBox2 : MonoBehaviour
//{
//    [SerializeField] SpriteRenderer image;

//    public int currentChoiceIndex; // 현재 정답박스에 들어있는 선택지의 인덱스

//    // 더블클릭 체크용 변수
//    private float doubleClickTime = 0.3f; // 더블 클릭으로 인식할 최대 시간
//    private float lastClickTime = 0f;

//    //private void Start()
//    //{
//    //    currentChoiceIndex = -1;
//    //}

//    //public void SetSprite(SpriteRenderer _image)
//    //{
//    //    image.sprite = _image.sprite;
//    //    image.color = _image.color;
//    //}

//    //private void OnMouseUp()
//    //{
//    //    // 현재 시간과 마지막 클릭 시간 차이 계산
//    //    if (Time.time - lastClickTime <= doubleClickTime)
//    //    {
//    //        // 더블 클릭 감지
//    //        OnDoubleClick();
//    //    }
//    //    else
//    //    {
//    //        // 단일 클릭으로 인식
//    //        lastClickTime = Time.time;
//    //    }
//    //}

//    //private void OnDoubleClick()
//    //{
//    //    // 정답박스에 선택지 들어가있으면 빼기
//    //    if(currentChoiceIndex != -1)
//    //    {
//    //        Managers.Game.choiceBoxList[currentChoiceIndex].gameObject.SetActive(true);

//    //        image.sprite = null;
//    //        image.color = Color.white;

//    //        currentChoiceIndex = -1;
//    //    }

//    //    //Debug.Log("더블 클릭 감지됨!");
//    //}
//}