using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI tempText;
    public ScoreCal scorecal;
    
    void Start()
    {
        // 해당 게임 오브젝트에서 TextMeshProUGUI 컴포넌트를 찾음
        tempText = GetComponent<TextMeshProUGUI>();

        if (tempText != null)
        {
            // 텍스트 내용 변경
            tempText.text = "SCORE: 5000";
        }
        else
        {
            Debug.LogError("TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        tempText.text = $"SCORE: {scorecal.score}";
    }
}
