using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableObject : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    public AudioClip start;
    public AudioClip sling;
    public AudioClip shoot;
    public AudioClip red;
    public AudioClip black;
    public AudioClip yellow;
    AudioSource aud;
    
    
    private CircleCollider2D myCollider;
    private Vector3 startPosition;
    private Vector3 pullPosition;

    private Camera MainCamera;
    Vector3 cameraOffset = new Vector3(5, 2, -10);

    public List<GameObject> bullet;
    public int bulletIndex;
    private GameObject go;
    private Vector3 pullDirection;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer lineRenderer2;
    [SerializeField] private float maxPullDistance;
    public float currentPower;

    public Vector3 position;
    public int maxStep = 5;
    public float timeStep = 0.05f;
    public float Mass = 1.0f;
    public GameObject Trajectory;
    public List<GameObject> Objects = new List<GameObject>();

    public ArrowButton arrowbutton;
    public DropDown dd;
    

    List<Vector3> PredictTrajectory(Vector3 force, float mass)
    {
        List<Vector3> trajectory = new List<Vector3>();
        //궤적을 저장할 리스트. 
        //매 단계마다 계산된 궤적의 위치들이 차례대로 저장

        Vector3 velocity = force / mass;
        //힘(force)을 질량(mass)으로 나눈 값. 물리학적으로 힘 = 질량 * 가속도 이므로
        //velocity는 새의 초기 속도. 즉, force / mass는 주어진 힘에 의해 발생한 초기 속도
        //근데 매시따로 사용안했으니 1로 통일


        trajectory.Add(position);
        //첫 번째 위치(새가 발사될 때의 초기 위치)를 궤적 리스트에 추가.


        //for문은 maxStep까지 반복. maxStep은 예측할 궤적의 단계 수
        for (int i = 1; i <= maxStep; i++)
        {
            //각 단계에서의 시간이 일정한 간격(timeStep)만큼 증가
            float timeElapsed = timeStep * i;


            // 등가속도 운동 공식
            trajectory.Add(position +
                           velocity * timeElapsed +
                           //시간에 따라 이동한 (초기 속도(velocity)로 이동한) 거리

                           Physics.gravity * (0.5f * timeElapsed * timeElapsed));
            //중력의 영향을 반영. 중력은 일정한 가속도를 주기 때문에,
            //물체가 낙하할 때의 위치를 계산하려면
            //0.5f * timeElapsed^2를 곱해줘야함
        }

        return trajectory;
        //최종적으로 계산된 궤적(trajectory) 리스트를 반환
    }

    private void Awake()
    {
        myCollider = GetComponent<CircleCollider2D>();
        MainCamera = Camera.main;
        position = transform.position;
        aud = GetComponent<AudioSource>();
    }

    void Start()
    {
        this.aud.PlayOneShot(start);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.aud.PlayOneShot(sling);
        
        StartCoroutine(arrowbutton.OutButton());
        StartCoroutine(dd.OutButton());
        
        pullPosition = startPosition = MainCamera.ScreenToWorldPoint(
            new Vector3(eventData.position.x,
                eventData.position.y,
                MainCamera.WorldToScreenPoint(transform.position).z));

        bulletIndex = dd.GetSelectedIndex();

        go = Instantiate(bullet[bulletIndex], startPosition, Quaternion.identity);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.aud.PlayOneShot(shoot);

        if (bulletIndex == 0)
        {
            this.aud.PlayOneShot(red);
        }
        else if (bulletIndex == 1)
        {
            this.aud.PlayOneShot(black);
        }
        else if (bulletIndex == 2)
        {
            this.aud.PlayOneShot(yellow);
        }
        
        Debug.Log("OnPointerUp");
        StartCoroutine(Gomu(go.transform.position));
    }

    IEnumerator Gomu(Vector3 pullPosition)
    {
        myCollider.enabled = false;
        
        float duration = 0.1f; // 이동 시간
        float elapsed = 0.0f; // 경과 시간

        while (elapsed < duration)
        {
            float t = elapsed / duration; // 0~1로 정규화
            Vector3 newPosition = Vector3.Lerp(pullPosition, startPosition, t);

            transform.position = newPosition;
            lineRenderer.SetPosition(1, newPosition);
            lineRenderer2.SetPosition(1, newPosition);
            go.transform.position = newPosition;

            elapsed += Time.deltaTime; // 경과 시간 갱신
            yield return null;
        }

        transform.position = startPosition;
        lineRenderer.SetPosition(1, startPosition);
        lineRenderer2.SetPosition(1, startPosition);
        go.transform.position = startPosition;

        StartCoroutine(Fire(go, pullDirection));

        lineRenderer.SetPosition(1, new Vector2(-0.3f, 0));
        lineRenderer2.SetPosition(1, Vector2.zero);
    }

    IEnumerator Fire(GameObject go, Vector3 pullDirection)
    {
        StartCoroutine(Cameraset());
        go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        go.GetComponent<Rigidbody2D>().AddForce(pullDirection * currentPower, ForceMode2D.Impulse);
        yield return null;

        transform.position = new Vector2(-0.18f, -0.68f + 0.718f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        if (Camera.main != null)
        {
            Vector3 mouseWorldPos = MainCamera.ScreenToWorldPoint(
                new Vector3(eventData.position.x,
                    eventData.position.y,
                    MainCamera.WorldToScreenPoint(transform.position).z));

            //mouseWorldPos.z = transform.position.z;
            pullPosition = mouseWorldPos;


            pullDirection = startPosition - mouseWorldPos;

            Vector2 LinePosition1 = Vector2.zero;
            Vector2 LinePosition2 = Vector2.zero;

            if (pullDirection.magnitude > maxPullDistance)
            {
                pullDirection = pullDirection.normalized * maxPullDistance;
                LinePosition1 = startPosition - pullDirection;
            }
            else
            {
                LinePosition1 = mouseWorldPos;
            }

            transform.position = mouseWorldPos;
            go.transform.position = LinePosition1;
            go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            lineRenderer.useWorldSpace = true;
            lineRenderer2.useWorldSpace = true;
            lineRenderer.SetPosition(1, LinePosition1);
            lineRenderer2.SetPosition(0, new Vector2(LinePosition2.x - 0.3f, 0));
            lineRenderer2.SetPosition(1, LinePosition1);

            currentPower = pullDirection.magnitude * 7.5f;

            List<Vector3> trajectorys = PredictTrajectory(pullDirection * currentPower, Mass);

            foreach (var o in Objects)
            {
                Destroy(o);
            }
            //기존의 예상 경로 객체들(Objects)을 모두 삭제 

            Objects.Clear();
            //Objects.Clear()로 리스트를 비움


            //trajectorys 리스트 순환
            foreach (var trajectory in trajectorys)
            {
                var go = Instantiate(Trajectory, trajectory, Quaternion.identity);
                Objects.Add(go);
                //해당 지점에 객체를 배치한 후 Objects 리스트에 추가
            }



            //Objects 리스트 순환
            foreach (var o in Objects)
            {
                o.SetActive(false);
                //비활성화(SetActive(false))하여 이전 경로 표시 지움
            }

            currentPower = pullDirection.magnitude * 7.5f;

            //PredictTrajectory(transform.forward * Power, Mass)를 다시 호출하여 최신 경로 계산,
            //그 결과를 trajectorys2에 저장
            List<Vector3> trajectorys2 = PredictTrajectory(pullDirection * currentPower, Mass);


            //예측된 경로와 생성된 경로 객체의 개수가 일치하는지 확인
            if (Objects.Count == trajectorys2.Count)
            {
                //맞다면 경로 지점 순환
                for (var index = 0; index < trajectorys2.Count; index++)
                {
                    var trajectory = trajectorys2[index];
                    Objects[index].SetActive(true);
                    //Objects[index]를 활성화
                    Objects[index].transform.position = trajectory;
                    //그 위치를 예측된 경로 지점(trajectory)에 맞게 업데이트
                }
            }
        }
    }
    
    IEnumerator Cameraset()
    {
        while (go != null && go.activeSelf) // 생성된 대포알을 따라가는 카메라
        {
            Vector3 targetPosition = new Vector3(go.transform.position.x, go.transform.position.y, MainCamera.transform.position.z);

            // 부드럽게 이동
            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, targetPosition, Time.deltaTime * 5f);

            yield return null; // 프레임마다 루프 반복
        }
        
        StartCoroutine(arrowbutton.InButton());
        StartCoroutine(dd.InButton());

        // 대포알이 비활성화된 후 카메라를 초기 위치로 이동
        while (Vector3.Distance(MainCamera.transform.position, cameraOffset) > 0.1f)
        {
            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, cameraOffset, Time.deltaTime * 10f);
            yield return null; // 프레임마다 루프 반복
        }

        // 최종적으로 위치를 정확히 설정
        MainCamera.transform.position = cameraOffset;
        myCollider.enabled = true;
    }
}

