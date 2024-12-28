using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public AudioClip crack;
    public AudioClip broken;
    AudioSource aud;
    
    public Sprite[] sprites; // 스프라이트 배열
    public int currentIndex = 0; // 현재 인덱스
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private List<Collider2D> colliders;
    
    Animator animator;
    
    public float power; // 파워 변수
    public float powerMultiplier = 1.5f; // 속도를 조정하는 배율
    public int damagelimit = 5; //데미지 임계점
    public int score = 0;

    public ScoreCal scorecal;
    private bool trigger = true;
    
    public Canvas _canvas;  // 점수가 생성될 Canvas 참조
    public ObjScore _objScore;  // 점수를 위한 점수 컴포넌트를 참조할 변수
    public Camera ui_camera; // UI 렌더링에 사용할 카메라

    private string colorcode = "1B9A14";
    
    private void Start()
    {
        aud = GetComponent<AudioSource>();
        
        rb = GetComponent<Rigidbody2D>();
        colliders = new List<Collider2D>(GetComponents<Collider2D>());
        
        // SpriteRenderer 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentIndex]; // 초기 스프라이트 설정
        }
        this.animator = GetComponent<Animator>();
        animator.enabled = false;
        
        GameObject sc = Instantiate(_objScore.gameObject, _canvas.transform);
        // 새 점수를 캔버스 위에 인스턴스화

        _objScore = sc.GetComponent<ObjScore>();
        //하고, 점수 컴포넌트를 가져와서 저장

        _objScore.UpdateOwner(this.transform, ui_camera);
        // 점수의 소유자와 UI 카메라를 업데이트 (아까 점수스크립트에 있던 함수)
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRigidbody = collision.rigidbody; // 충돌 상대의 Rigidbody

        if (otherRigidbody != null)
        {
            // 상대 객체의 속도를 가져옵니다.
            Vector3 otherVelocity = otherRigidbody.velocity;

            // 속도의 크기를 파워로 설정
            power = otherVelocity.magnitude * powerMultiplier;

            changeSprite(power);

        }
        
    }

    void changeSprite(float power)
    {
        int changeStack = (int)power / damagelimit;
        
        if (changeStack >= 1)
        {
            for (int i = 1; i <= changeStack; i++)
            {
                if (currentIndex < sprites.Length - 1)
                {
                    this.aud.PlayOneShot(crack);
                    currentIndex++;
                    spriteRenderer.sprite = sprites[currentIndex];
                }
                else
                {
                    rb.isKinematic = true;
                    rb.velocity = Vector2.zero; // 속도 초기화
                    rb.angularVelocity = 0f;
                    
                    foreach (Collider2D col in colliders)
                    {
                        col.enabled = false;
                    }
                    
                    if (trigger)
                    {
                        this.aud.PlayOneShot(broken);
                        score = scorecal.PlusScore(gameObject.tag, trigger);
                        _objScore.UpdateScoreText(score.ToString(), colorcode);
                    }
                    trigger = false;
                    animator.enabled = true;
                    this.animator.SetTrigger("DestroyTrigger");
                    Destroy(this.gameObject, 2f);
                }
            }
        }
    }

    void OffSprite()
    {
        spriteRenderer.enabled = false;
    }
}
