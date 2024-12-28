using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    public Camera uiCamera;
    
    private RectTransform rectTransform;

    private Vector2 outPos; // UI는 2D 공간으로 작업
    private Vector2 inPos;

    // 화면 밖 위치 계산
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // 화면 밖의 X 위치 계산 (예: 화면 바깥으로 150만큼 이동)
        inPos = rectTransform.anchoredPosition;
        outPos = new Vector2(inPos.x + 300, inPos.y);  // 화면 왼쪽 밖
    }

    public IEnumerator OutButton()
    {
        float duration = 0.15f; // 이동 시간
        float elapsed = 0.0f; // 경과 시간

        Vector2 startPos = rectTransform.anchoredPosition; // 시작 위치 (스크린 좌표)

        // 버튼을 화면 밖으로 이동
        while (elapsed < duration)
        {
            float t = elapsed / duration; // 0~1로 정규화
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, outPos, t); // 스크린 좌표 기준으로 이동

            elapsed += Time.deltaTime; // 경과 시간 갱신
            yield return null;
        }
        rectTransform.anchoredPosition = outPos; // 정확한 위치로 설정
    }

    public IEnumerator InButton()
    {
        float duration = 0.15f; // 이동 시간
        float elapsed = 0.0f; // 경과 시간

        Vector2 startPos = rectTransform.anchoredPosition; // 시작 위치 (스크린 좌표)

        // 버튼을 화면 안으로 이동
        while (elapsed < duration)
        {
            float t = elapsed / duration; // 0~1로 정규화
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, inPos, t); // 스크린 좌표 기준으로 이동

            elapsed += Time.deltaTime; // 경과 시간 갱신
            yield return null;
        }
        rectTransform.anchoredPosition = inPos; // 정확한 위치로 설정
    }
}