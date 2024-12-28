using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCal : MonoBehaviour
{
    public int score = 5000;
    public int combo = 0;
    public int enemyscore;
    public int cost;
    
    public string[] targetTags = { "small_pig", "iron_pig", "old_pig", "big_pig" }; // 검사할 태그
    public Vector2 boxSize = new Vector2(40f, 40f); // 박스 크기 (OverlapBox 사용 시)
    
    public ClearUI clearui;

    public int PlusScore(string tag, bool trigger)
    {
        if (trigger)
        {
            enemyscore = 0;
        
            switch (tag)
            {
                case "wood":
                    enemyscore = 500;
                    break;
                case "small_pig":
                    enemyscore = 1000;
                    break;
                case "iron_pig":
                    enemyscore = 2000;
                    break;
                case "old_pig":
                    enemyscore = 3000;
                    break;
                case "big_pig":
                    enemyscore = 5000;
                    break;
            }
            combo++;
        
            score += enemyscore * combo;
        
            Debug.Log($"{tag} 격파! 현재콤보:{combo}, 현재점수:{score}");
        }

        return combo * enemyscore;
    }
    
    public int MinusScore(string tag, bool trigger)
    {
        if (trigger)
        {
            cost = 0;
        
            switch (tag)
            {
                case "red_bird":
                    cost = -500;
                    break;
                case "black_bird":
                    cost = -1500;
                    break;
                case "yellow_bird":
                    cost = -2500;
                    break;
            }
            combo = 0;
        
            score += cost;
        
            Debug.Log($"새 고용비:{cost}, 현재점수:{score}");
        }

        return cost;
    }
    
    public void CheckTrigger(bool trigger)
    {
        if (trigger)
        {
            // 현재 위치를 기준으로 지정된 크기의 박스 영역 내의 모든 Collider를 탐지
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);;

            bool hasTargetTag = false;

            // 탐지된 Collider들 중에서 특정 태그를 가진 개체가 있는지 확인
            foreach (var collider in colliders)
            {
                // collider의 태그가 targetTags 배열에 있는지 확인
                foreach (var tag in targetTags)
                {
                    if (collider.CompareTag(tag))
                    {
                        hasTargetTag = true;
                        break;
                    }
                }

                if (hasTargetTag)
                    break;
            }

            // 특정 태그를 가진 개체가 없으면 클리어 메서드 실행
            if (!hasTargetTag)
            {
                PlayClear();
            }
        }
    }
    
    void PlayClear()
    {
        clearui.totalscore = score;
        StartCoroutine(clearui.ClearEnter());
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;  // 영역 색상 설정
        Gizmos.DrawWireCube(transform.position, new Vector3(boxSize.x, boxSize.y, 0f)); // 2D 영역에 맞게 3D로 그리기
    }
}
