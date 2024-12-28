using System.Collections;
using UnityEngine;
using TMPro;

public class DropDown : MonoBehaviour
{
    public Camera uiCamera; // UI가 월드 공간일 때 사용
    public TMP_Dropdown dropdown;

    private RectTransform rectTransform;

    private Vector2 outPos; // UI는 2D 공간으로 작업
    private Vector2 inPos;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        rectTransform = GetComponent<RectTransform>();

        // anchoredPosition을 기준으로 위치 설정
        inPos = rectTransform.anchoredPosition;
        outPos = new Vector2(inPos.x, inPos.y + 300); // 화면 위로 이동
    }

    public int GetSelectedIndex()
    {
        return dropdown.value; // 현재 선택된 옵션의 인덱스 반환
    }

    public IEnumerator OutButton()
    {
        float duration = 0.15f; // 이동 시간
        float elapsed = 0.0f;

        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            float t = elapsed / duration; // 0~1로 정규화
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, outPos, t); // 이동

            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = outPos; // 정확한 위치 설정
    }

    public IEnumerator InButton()
    {
        float duration = 0.15f; // 이동 시간
        float elapsed = 0.0f;

        Vector2 startPos = rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            float t = elapsed / duration; // 0~1로 정규화
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, inPos, t); // 이동

            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = inPos; // 정확한 위치 설정
    }
}