using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private Rigidbody2D m_Object; // �̵��� ������Ʈ (�÷��̾�)
    private RopeHook ropeHook;
   
    public Transform target; // Ÿ�� ������Ʈ (�ٰ� ����� �Է��ʿ�)

    private Tweener tweener;
    public float moveSpd = 15f;
    public bool isPull = false;
    void Awake()
    {
        ropeHook = GameObject.FindGameObjectWithTag("Hook").GetComponent<RopeHook>();
        m_Object = this.gameObject.GetComponent<Rigidbody2D>();

    }
    // Ÿ�� ����� �ߵ��ؾ��ϴ� �Լ�
    public void Set(Transform target,Vector2 hookPos) 
    {
        this.target = target;
    }


    // �÷��̾� ���󰡱�
    public void pullObject()
    {
        if (m_Object == null) return;
        if (target == null) return;
        isPull = true;
        //Vector3 dir = (hookPos - moveObj.transform.position).normalized;
        //moveObj.MovePosition(moveObj.transform.position + dir * moveSpd * Time.fixedDeltaTime);
        if (tweener!=null && tweener.IsActive() ) tweener.Kill(); 
        if (target.GetComponent<Rigidbody2D>() != null)
        {
            tweener = this.transform.DOMove(ropeHook.transform.position, 0.2f).SetEase(Ease.InCubic).OnComplete(
                () =>
                {
                    StopPull();
                    ropeHook.gameObject.SetActive(false);
                }
             );
        }
        else
        {
            tweener = this.transform.DOMove(target.transform.position, 0.2f).SetEase(Ease.InCubic).OnComplete(
                () =>
                {
                    target.GetComponent<Food>().HitAction();
                    StopPull();
                    ropeHook.gameObject.SetActive(false);
                }
             );
        }
    }

    // ���� �����Ҷ� �ʼ������� ȣ��
    public void StopPull()
    {
        if (target == null) return;
        target.GetComponent<Collider2D>().isTrigger = false;
        target = null;
        isPull=false;

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
