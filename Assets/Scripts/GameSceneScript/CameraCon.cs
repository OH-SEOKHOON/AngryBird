using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraCon : MonoBehaviour
{
    public Button arrowButton;
    private Vector3 startPos; // 초기 위치
    public float moveSpeed = 10f; // 카메라 이동 속도
    private bool isMoving = false; // 버튼이 눌려 있는지 여부
    private Coroutine returnCoroutine; // 복귀 코루틴 참조

    private void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isMoving && transform.position.x < 19f)
        {
            // 카메라를 오른쪽(X축)으로 이동
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    // UI 버튼의 Pointer Down 이벤트에 연결
    public void OnButtonDown()
    {
        isMoving = true;

        // 기존 복귀 코루틴이 실행 중이면 중단
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
    }

    // UI 버튼의 Pointer Up 이벤트에 연결
    public void OnButtonUp()
    {
        isMoving = false;
        returnCoroutine = StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        float duration = 0.25f; // 이동 시간
        float elapsed = 0.0f; // 경과 시간

        Vector3 currentPosition = transform.position; // 현재 위치 저장

        while (elapsed < duration)
        {
            // 버튼이 다시 눌리면 즉시 코루틴 중단
            if (isMoving) yield break;

            float t = elapsed / duration; // 0~1로 정규화
            transform.position = Vector3.Lerp(currentPosition, startPos, t);

            elapsed += Time.deltaTime; // 경과 시간 갱신
            yield return null;
        }

        transform.position = startPos; // 최종 위치 설정
    }
}