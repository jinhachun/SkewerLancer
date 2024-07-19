using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private Rigidbody2D m_Object; // 이동할 오브젝트 (플레이어)
    private RopeHook ropeHook;
    public Rigidbody2D target; // 타겟 오브젝트 (줄과 연결시 입력필요)
    private Vector2 hookPosition; // 줄을 날린 위치
    private float weight => m_Object.mass;
    private float targetWeight => target.mass;
    public bool isHeavier => (target!=null) ? weight > targetWeight : false;
    public float moveSpd = 15f;

    public bool isPulled; // true일시, pull 발동
    void Awake()
    {
        ropeHook = GameObject.FindGameObjectWithTag("Hook").GetComponent<RopeHook>();
        m_Object = this.gameObject.GetComponent<Rigidbody2D>();
    }
    // 타겟 연결시 발동해야하는 함수
    public void Set(Rigidbody2D target,Vector2 hookPos) 
    {
        this.target = target;
        hookPosition = hookPos;
    }


    // 플레이어 날라가기
    private void pullObject(Rigidbody2D moveObj, Vector3 hookPos)
    {
        Vector3 dir = (hookPos - moveObj.transform.position).normalized;
        moveObj.MovePosition(moveObj.transform.position + dir * moveSpd * Time.fixedDeltaTime);

    }

    // 당기기 중지할때 필수적으로 호출
    public void StopPull()
    {
        if (target == null) return;
        target.GetComponent<Collider2D>().isTrigger = false;
        m_Object.velocity /= 2;
        target.gameObject.SetActive(false);
        hookPosition = Vector3.zero;
        isPulled = false;
        target = null;

    }
    public void FixedUpdate()
    {
        if(m_Object ==null) return;
        if (target == null) return;
        if (!isPulled) return;
        if (Vector2.Distance(this.transform.position, hookPosition) < 1.5f)
        {
            StopPull();
            return;
        }
        if (!isHeavier) {
            pullObject(m_Object, hookPosition);
        }
        //플레이어가 가벼우면, 플레이어가 날라감
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(target == null) return; 
        if(collision == null) return;
        if (collision.gameObject == target.gameObject) 
        {
            StopPull();
        }
    }

}
