using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ClearUI : MonoBehaviour
{
    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    public AudioClip end;
    AudioSource aud;
    
    private RectTransform rectTransform;
    private Vector2 inPos;
    private Vector2 startPos;
    public int totalscore;

    public Sprite golden_egg;

    public GameObject button;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("RectTransform 컴포넌트가 없습니다!");
            return;
        }
        
        startPos = rectTransform.anchoredPosition;
        Debug.Log(startPos);
        inPos = new Vector2(startPos.x, startPos.y - 2160);
        
        aud = GetComponent<AudioSource>();
    }
    
    public IEnumerator ClearEnter()
    {
        float duration = 0.5f; // 이동 시간
        float elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            float t = elapsed / duration; // 0~1로 정규화
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, inPos, t); // 이동

            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = inPos; // 정확한 위치 설정

        StartCoroutine(egg_1());
    }
    
    public IEnumerator egg_1()
    {
        yield return new WaitForSeconds(1f);
        
        this.aud.PlayOneShot(one);

        // 3번째 자식의 1번째 이미지 자식의 색 변경
        Transform thirdChild = transform.GetChild(2); // 자식 인덱스는 0부터 시작
        Transform firstImageChild = thirdChild.GetChild(0);
        Image imageComponent = firstImageChild.GetComponent<Image>();

        if (imageComponent != null)
        {
            if (golden_egg != null)
            {
                imageComponent.sprite = golden_egg; // 스프라이트 교체
            }
            else
            {
                Debug.LogError("새 스프라이트가 지정되지 않았습니다!");
            }
        }
        else
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다!");
        }

        // 4번째 자식의 1번째 텍스트 자식의 색 변경
        Transform fourthChild = transform.GetChild(3);
        Transform firstTextChild = fourthChild.GetChild(0);
        TextMeshProUGUI textComponent = firstTextChild.GetComponent<TextMeshProUGUI>();

        if (textComponent != null)
        {
            textComponent.color = Color.green; // 원하는 색상으로 변경
        }
        else
        {
            Debug.LogError("TextMeshProUGUI 컴포넌트를 찾을 수 없습니다!");
        }

        if (totalscore >= 20000)
        {
            StartCoroutine(egg_2());
        }
        else
        {
            StartCoroutine(Finish());
        }
    }
    
    public IEnumerator egg_2()
    {
        yield return new WaitForSeconds(1f);
        
        this.aud.PlayOneShot(two);

        // 3번째 자식의 2번째 이미지 자식변경
        Transform thirdChild = transform.GetChild(2); // 자식 인덱스는 0부터 시작
        Transform secondImageChild = thirdChild.GetChild(1);
        Image imageComponent = secondImageChild.GetComponent<Image>();

        if (imageComponent != null)
        {
            if (golden_egg != null)
            {
                imageComponent.sprite = golden_egg; // 스프라이트 교체
            }
            else
            {
                Debug.LogError("새 스프라이트가 지정되지 않았습니다!");
            }
        }
        else
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다!");
        }

        // 4번째 자식의 2번째 텍스트 자식의 색 변경
        Transform fourthChild = transform.GetChild(3);
        Transform secondTextChild = fourthChild.GetChild(1);
        TextMeshProUGUI textComponent = secondTextChild.GetComponent<TextMeshProUGUI>();

        if (textComponent != null)
        {
            textComponent.color = Color.green; // 원하는 색상으로 변경
        }
        else
        {
            Debug.LogError("TextMeshProUGUI 컴포넌트를 찾을 수 없습니다!");
        }

        if (totalscore >= 30000)
        {
            StartCoroutine(egg_3());
        }
        else
        {
            StartCoroutine(Finish());
        }
    }
    
    public IEnumerator egg_3()
    {
        yield return new WaitForSeconds(1f);
        
        this.aud.PlayOneShot(three);

        // 3번째 자식의 3번째 이미지 자식의 색 변경
        Transform thirdChild = transform.GetChild(2); // 자식 인덱스는 0부터 시작
        Transform thirdImageChild = thirdChild.GetChild(2);
        Image imageComponent = thirdImageChild.GetComponent<Image>();

        if (imageComponent != null)
        {
            if (golden_egg != null)
            {
                imageComponent.sprite = golden_egg; // 스프라이트 교체
            }
            else
            {
                Debug.LogError("새 스프라이트가 지정되지 않았습니다!");
            }
        }
        else
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다!");
        }

        // 4번째 자식의 3번째 텍스트 자식의 색 변경
        Transform fourthChild = transform.GetChild(3);
        Transform thirdTextChild = fourthChild.GetChild(2);
        TextMeshProUGUI textComponent = thirdTextChild.GetComponent<TextMeshProUGUI>();

        if (textComponent != null)
        {
            textComponent.color = Color.green; // 원하는 색상으로 변경
        }
        else
        {
            Debug.LogError("TextMeshProUGUI 컴포넌트를 찾을 수 없습니다!");
        }
        
        StartCoroutine(Finish());
    }

    IEnumerator Finish()
    {
        yield return new WaitForSeconds(1f);
        this.aud.PlayOneShot(end);
        yield return new WaitForSeconds(end.length);

        button.SetActive(true);
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
