using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private Rigidbody2D m_Object; // �̵��� ������Ʈ (�÷��̾�)
    private RopeHook ropeHook;
    public Rigidbody2D target; // Ÿ�� ������Ʈ (�ٰ� ����� �Է��ʿ�)
    private Vector2 hookPosition; // ���� ���� ��ġ
    private float weight => m_Object.mass;
    private float targetWeight => target.mass;
    public bool isHeavier => (target!=null) ? weight > targetWeight : false;
    public float moveSpd = 15f;

    public bool isPulled; // true�Ͻ�, pull �ߵ�
    void Awake()
    {
        ropeHook = GameObject.FindGameObjectWithTag("Hook").GetComponent<RopeHook>();
        m_Object = this.gameObject.GetComponent<Rigidbody2D>();
    }
    // Ÿ�� ����� �ߵ��ؾ��ϴ� �Լ�
    public void Set(Rigidbody2D target,Vector2 hookPos) 
    {
        this.target = target;
        hookPosition = hookPos;
    }


    // �÷��̾� ���󰡱�
    private void pullObject(Rigidbody2D moveObj, Vector3 hookPos)
    {
        Vector3 dir = (hookPos - moveObj.transform.position).normalized;
        moveObj.MovePosition(moveObj.transform.position + dir * moveSpd * Time.fixedDeltaTime);

    }

    // ���� �����Ҷ� �ʼ������� ȣ��
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
        //�÷��̾ �������, �÷��̾ ����
        
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
