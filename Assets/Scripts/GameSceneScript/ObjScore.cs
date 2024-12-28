using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjScore : MonoBehaviour
{
    private Camera camera; //점수를 화면에 그리기 위한 주 카메라
    private Transform owner; //점수가 표시될 대상 객체의 Transform
    private Camera ui_camera; //UI를 렌더링할 때 사용할 카메라
    private TextMeshProUGUI text; //점수를 나타내는 Image 컴포넌트.
    
    private void Awake()
    {
        // 자식 객체의 TextMeshProUGUI 컴포넌트를 가져옴
        text = GetComponentInChildren<TextMeshProUGUI>();
        UpdateTextAlpha(0, Color.white);
        if (text == null)
        {
            Debug.LogError("TextMeshProUGUI 컴포넌트를 찾을 수 없습니다!");
        }
    }
    
    void Start()
    {
        camera = Camera.main;
        //카메라 변수에 메인 카메라 호출
    }

	
    //외부에서 호출될 함수, 외부 객체와 ui카메라를 할당함
    public void UpdateOwner(Transform owner, Camera ui_camera)
    {
        this.owner = owner;
        this.ui_camera = ui_camera;
    }

    void LateUpdate()
    {
    	//객체와 카메라 모두 할당에 성공했을때
        if (owner != null && camera != null)
        {
            Vector3 screenPoint = camera.WorldToScreenPoint(owner.position);
            //대상 오브젝트(owner)의 월드 좌표를 화면 좌표로 변환하여
            //screenPoint에 할당
            
            
            Vector2 localPoint;
            //밑의 함수의 결과를 저장할 변수
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(), screenPoint, ui_camera, out localPoint);
            //화면 좌표를 부모 RectTransform의 로컬 좌표로 변환.
            //이때 ui_camera를 사용하여 UI 카메라에 맞는 좌표를 계산
            
            
            transform.localPosition = localPoint;
            //계산된 로컬 좌표를 HpBar 객체의 위치로 설정
            
        }
    }
    
    public void UpdateScoreText(string newText, string hexColor)
    {
        if (text != null)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#" + hexColor, out color)) // 헥스 코드로 색상 파싱
            {
                UpdateTextAlpha(1, color); // 텍스트가 처음에는 완전히 보이도록 설정하고 색상도 지정
                text.text = newText;
                StartCoroutine(AnimateScoreText(color));
            }
            else
            {
                Debug.LogWarning("잘못된 헥스 코드입니다: " + hexColor);
            }
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI가 설정되지 않았습니다.");
        }
    }
    
    public void UpdateTextAlpha(float alpha, Color color)
    {
        if (text != null)
        {
            color.a = Mathf.Clamp01(alpha); // 알파값은 0~1 사이로 클램핑
            text.color = color;
        }
    }
    
    private IEnumerator AnimateScoreText(Color color)
    {
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        Vector3 initialPosition = rectTransform.localPosition;
        
        // 1.5초 동안 텍스트가 위로 떠오르며 알파값을 줄여서 투명해지도록 애니메이션
        float timeElapsed = 0f;
        float moveDistance = 30f; // 텍스트가 위로 떠오르는 거리
        float duration = 1.5f; // 애니메이션 지속 시간

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            
            // 위치 애니메이션 (위로 떠오르기)
            rectTransform.localPosition = Vector3.Lerp(initialPosition, initialPosition + Vector3.up * moveDistance, t);
            
            // 알파 애니메이션 (서서히 투명해짐)
            UpdateTextAlpha(1 - t, color); // alpha가 0으로 가는 애니메이션, 색상도 함께 변경

            timeElapsed += Time.deltaTime;
            yield return null; // 한 프레임 대기
        }

        // 애니메이션이 끝난 후 텍스트를 완전히 투명하게 만든 후 오브젝트 삭제
        UpdateTextAlpha(0, color); // 알파값을 0으로, 원하는 색상 유지
        Destroy(this.gameObject);
    }
    
}
