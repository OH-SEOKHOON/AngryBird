using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToButton : MonoBehaviour
{
    public AudioClip back;
    AudioSource aud;
    
    public RectTransform uiElement; // 이동할 UI 요소
    public Vector2 offScreenPosition; // 화면 밖 초기 위치
    public Vector2 onScreenPosition; // 화면 안 목표 위치
    public float animationDuration = 0.5f; // 애니메이션 지속 시간

    private bool isOnScreen = false; // 현재 UI가 화면 안에 있는지 여부
    private Coroutine animationCoroutine; // 코루틴 중복 방지용

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        
        // UI 요소를 화면 밖 초기 위치로 설정
        if (uiElement != null)
        {
            uiElement.anchoredPosition = offScreenPosition;
        }
    }

    public void ToggleUI()
    {
        this.aud.PlayOneShot(back);
        
        if (uiElement == null) return;

        // 기존 코루틴이 실행 중이면 중지
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        // 새 코루틴 실행
        if (isOnScreen)
        {
            animationCoroutine = StartCoroutine(AnimateUI(uiElement, onScreenPosition, offScreenPosition, animationDuration));
        }
        else
        {
            animationCoroutine = StartCoroutine(AnimateUI(uiElement, offScreenPosition, onScreenPosition, animationDuration));
        }

        isOnScreen = !isOnScreen; // 상태 반전
    }

    private IEnumerator AnimateUI(RectTransform target, Vector2 startPos, Vector2 endPos, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            target.anchoredPosition = Vector2.Lerp(startPos, endPos, t); // 위치 보간
            yield return null;
        }

        target.anchoredPosition = endPos; // 최종 위치 고정
        animationCoroutine = null; // 코루틴 해제
    }
}
