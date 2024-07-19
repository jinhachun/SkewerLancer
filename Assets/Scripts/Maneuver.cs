using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Overlays;
using UnityEngine;

public class Maneuver : MonoBehaviour
{
    RopeHook ropeHook;
    ObjectMovement objectMovement;

    LineRenderer line;
    public Color32 lineStartColor;
    public Color32 lineEndColor;

    Transform hook;

    
    float cutOffTime = 10;
    float timeLimit;

    [SerializeField] [Tooltip("로프가 오브젝트에 부착됬을 때 플레이어와 이 변수 이상 떨어지면 로프가 끊어집니다.")]
    float attachMaxDis;
    public Vector3 GetHookPosition() { return hook.position; }

    [SerializeField] [Tooltip("로프가 날아가는 속도")]
    public float ropeSpeed;


    [SerializeField] [Tooltip("로프 사거리")]
    public float ropeMaxDis;

    public float ropeMinDis;

    Vector2 mousedir;

    public bool IsRopeAction { get { return isRopeAction; } private set { isRopeAction = value; } }
    bool isRopeAction;          // 로프를 발사했는가?
    bool isRopeMax;             // 로프의 최대 거리에 도달 했는가?
    public bool isRopeAttach;   // 로프가 어딘가에 부착됬는가?

    
    public bool canTighten;
    public bool isEndCreateRope;

    Rigidbody2D rigidBody;

    public Vector3 pullGoalPosition;
    Vector3 initialRopeDir;



    float sumOfPositionPerFrame;

    private void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Hook");
        
        ropeHook = obj.GetComponent<RopeHook>();
        line =obj.GetComponent<LineRenderer>();
        hook = obj.GetComponent<Transform>();

        objectMovement = this.GetComponent<ObjectMovement>();

        rigidBody = this.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        line.positionCount = 2;
        line.startWidth = 0.5f;
        line.endWidth = 0.05f; 
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        line.startColor = lineStartColor;
        line.endColor = lineEndColor;
        line.useWorldSpace = true;
        

        isRopeAction = false;
        isRopeAttach = false;
        isRopeMax = false;

        ropeHook.gameObject.SetActive(false);
    }

    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if (Input.GetKey(KeyCode.Mouse0) && isRopeAttach)   // 로프 능력
        {
            RopeAbility();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))          // 로프 발사
        {
            if (!isRopeAction)
            {
                RopeShot();
            }
            else if (!isRopeAttach)
            {
                RopeShot();
            }
        } 
        else if(Input.GetKeyUp(KeyCode.Mouse0) && isRopeAttach) // 로프 컴백
        {
            isRopeMax = true;
        }

        if (isRopeAction && !isRopeMax && !isRopeAttach)
        {
            hook.Translate(mousedir.normalized * Time.deltaTime * ropeSpeed);
            hook.Translate(mousedir.normalized * Time.deltaTime * ropeSpeed);
            hook.Translate(mousedir.normalized * Time.deltaTime * ropeSpeed);
            if (Vector2.Distance(transform.position, hook.position) > ropeMaxDis)
            {
                isRopeMax = true;
            }
        }
        if (isRopeAttach)
        {
            isRopeMax = !isRopeMax ? ropeHook.collideTarget==CollideTarget.NONE:true;
            if(ropeHook.targetObject!=null)
                PullTarget();

            float distance = Vector2.Distance(transform.position , hook.position);
                
        }
        if (isRopeMax)
        {
            RopeComeBack();
        }

    }

    // 로프 발사
    void RopeShot()
    {
        //Debug.Log("로프 발사");
        hook.gameObject.SetActive(true);

        hook.position = transform.position;
        mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        ropeHook.collideTarget = CollideTarget.NONE;

        isRopeAction = true;
        isRopeMax = false;

        initialRopeDir = mousedir;

        timeLimit = Time.time + cutOffTime;
    }

    // 로프 능력
    void RopeAbility()
    {
        if (ropeHook.targetObject == null) return;
        
        switch (ropeHook.collideTarget)
        {
 
            case (CollideTarget.PULL):
                {
                    //Debug.Log("로프 능력");
                    objectMovement.isPulled = true;
                    if (objectMovement.isHeavier)
                    {
                        Vector2 comebackVec = transform.position - hook.position;
                        hook.Translate(comebackVec.normalized * Time.deltaTime * ropeSpeed);
                    }
                    return;
                }
            case (CollideTarget.NONE):
                {
                    isRopeMax = true;
                    return;
                }
        }
        RopeComeBack();
    }
    void PullTarget()
    {
        GameObject targetObject = ropeHook.targetObject;
        Vector2 hookPosition = ropeHook.hookPosition;

        objectMovement.Set(targetObject.GetComponent<Rigidbody2D>(), hookPosition);
    }

    // 로프 돌아오기
    public void RopeComeBack()
    {
        //Debug.Log("로프 컴백");
        isRopeAction = false;
        isRopeAttach = false;
        //objectMovement.StopPull();

        
            hook.gameObject.SetActive(false);
            isRopeMax = false;
    }

    // 로프 끊기
    public void RopeCutOff()
    {
        Debug.Log("로프 끊기");
        hook.gameObject.SetActive(false);
        RopeComeBack();
    }


}