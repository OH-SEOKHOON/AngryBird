using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexButton : MonoBehaviour
{
    public AudioClip select;
    AudioSource aud;
    
    public GameObject[] objects; // 배열에 게임 오브젝트를 연결
    public Button leftButton;   // 좌측 버튼
    public Button rightButton;  // 우측 버튼
    private int currentIndex = 0; // 현재 활성화된 게임 오브젝트의 인덱스

    void Start()
    {
        aud = GetComponent<AudioSource>();
        
        // 버튼 클릭 이벤트 연결
        leftButton.onClick.AddListener(SwitchLeft);
        rightButton.onClick.AddListener(SwitchRight);

        // 초기화 - 첫 번째 오브젝트만 활성화
        UpdateObjects();
    }

    // 왼쪽 버튼 클릭 처리
    public void SwitchLeft()
    {
        this.aud.PlayOneShot(select);
        
        currentIndex = (currentIndex - 1 + objects.Length) % objects.Length; // 인덱스를 반대로 순환
        UpdateObjects();
    }

    // 오른쪽 버튼 클릭 처리
    public void SwitchRight()
    {
        this.aud.PlayOneShot(select);
        currentIndex = (currentIndex + 1) % objects.Length; // 인덱스를 순환
        UpdateObjects();
    }

    // 오브젝트 상태 갱신
    private void UpdateObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == currentIndex); // 현재 인덱스의 오브젝트만 활성화
        }
    }
}
