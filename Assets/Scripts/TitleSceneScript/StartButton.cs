using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public AudioClip confirm;
    AudioSource aud;

    void Start()
    {
        aud = GetComponent<AudioSource>();
    }
    
    void Update() {
        if (Input.GetKey("escape"))
            Application.Quit();
    }
    
    public void GameStart()
    {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        if (confirm != null)
        {
            aud.PlayOneShot(confirm); // 효과음 재생
            yield return new WaitForSeconds(confirm.length); // 효과음 길이만큼 대기
        }
        
        SceneManager.LoadScene("GameScene"); // 씬 로드
    }
}
