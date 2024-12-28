using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class yellow_bird : MonoBehaviour
{
    public AudioClip dead;
    public AudioClip dash;
    AudioSource aud;
    
    public float checkDuration = 3f; // 움직임 감지를 위한 대기 시간
    public float velocityThreshold = 1f; // 움직임을 감지할 속도 임계값
    private float stationaryTime = 0f; // 정지된 시간
    private float distanceFromStart;
    
    public Sprite[] sprites; // 스프라이트 배열
    private SpriteRenderer spriteRenderer;
    
    private Vector3 startPos;
    private Rigidbody2D rb;
    private List<Collider2D> colliders;
    
    Animator animator;

    private bool canSpeed = true;
    
    public ScoreCal scorecal;
    private bool trigger = true;
    
    public Canvas _canvas;  // 점수가 생성될 Canvas 참조
    public ObjScore _objScore;  // 점수를 위한 점수 컴포넌트를 참조할 변수
    public Camera ui_camera; // UI 렌더링에 사용할 카메라

    private int cost = 0;

    private string colorcode = "FF3E39";
    
    void Start()
    {
        aud = GetComponent<AudioSource>();
        
        GameObject cal = GameObject.Find("ScoreCal");
        scorecal = cal.GetComponent<ScoreCal>();
        
        Canvas can = FindObjectOfType<Canvas>();
        _canvas = can.GetComponent<Canvas>();
        
        Camera[] cameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in cameras)
        {
            if (cam.name == "ui camera")  // 원하는 카메라 이름을 지정
            {
                // 원하는 카메라 처리
                ui_camera = cam;
                break;
            }
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
        
        GameObject sc = Instantiate(_objScore.gameObject, _canvas.transform);
        // 새 점수를 캔버스 위에 인스턴스화

        _objScore = sc.GetComponent<ObjScore>();
        //하고, 점수 컴포넌트를 가져와서 저장

        _objScore.UpdateOwner(this.transform, ui_camera);
        // 점수의 소유자와 UI 카메라를 업데이트 (아까 점수스크립트에 있던 함수)
        
        colliders = new List<Collider2D>(GetComponents<Collider2D>());
        // 현재 객체에 부착된 모든 Collider2D 컴포넌트를 colliders 리스트에 추가
    }

    void Update()
    {
        distanceFromStart = Vector3.Distance(transform.position, startPos);
        

        if (distanceFromStart > 1.51f)
        {
            spriteRenderer.sprite = sprites[1];
                
            if (rb.velocity.magnitude < velocityThreshold)
            {
                spriteRenderer.sprite = sprites[3];
                stationaryTime += Time.deltaTime; // 정지된 시간 증가
                if (stationaryTime >= checkDuration)
                {
                    animator.enabled = true;
                    this.animator.SetTrigger("DestroyTrigger");
                    ObjDestroy();
                }
            }
        }
        else
        {
            stationaryTime = 0f; // 움직임이 감지되면 정지 시간 초기화
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (distanceFromStart > 1.51f && canSpeed)
            {
                this.aud.PlayOneShot(dash);
                canSpeed = false;
                spriteRenderer.sprite = sprites[2];
                rb.velocity *= 3f;
            }
        }
    }

    private void ObjDestroy()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero; // 속도 초기화
        rb.angularVelocity = 0f;
                    
        // colliders가 null이 아니고 비어있지 않은지 확인
        if (colliders != null && colliders.Count > 0)
        {
            foreach (Collider2D col in colliders)
            {
                col.enabled = false;
            }
        }
        
        if (trigger)
        {
            this.aud.PlayOneShot(dead);
            cost = scorecal.MinusScore(gameObject.tag, trigger);
            _objScore.UpdateScoreText(cost.ToString(), colorcode);
            scorecal.CheckTrigger(trigger);
        }
        trigger = false;
        Destroy(this.gameObject, 2f);
    }
    
    void OffSprite()
    {
        spriteRenderer.enabled = false;
    }
}
