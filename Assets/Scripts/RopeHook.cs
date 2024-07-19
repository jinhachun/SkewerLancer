using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class RopeHook : MonoBehaviour
{
    public CollideTarget collideTarget;

    [SerializeField]
    string swingTag;

    [SerializeField]
    string maneuverTag;

    Maneuver maneuver;

    public GameObject targetObject;
    public Vector2 hookPosition;

    public FixedJoint2D playerFixedJoint2D;

    Vector3 offset;
    [SerializeField]
    bool isPlayerExit;

    private void Awake()
    {
        maneuver = GameObject.FindGameObjectWithTag("Player").GetComponent<Maneuver>();
        playerFixedJoint2D = maneuver.GetComponent<FixedJoint2D>();
    }

    void Start()
    {
        isPlayerExit = false;
    }

    private void Update()
    {
        if (maneuver.isRopeAttach && targetObject != null)
        {
            transform.position = targetObject.transform.position - offset;

            hookPosition.x = this.transform.position.x;
            hookPosition.y = this.transform.position.y;
        }
    }

    public void DestroyRope()
    {
        Debug.Log("DestroyRope");
        playerFixedJoint2D.enabled = false;
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == null) return;
        if (col.gameObject.CompareTag("Player"))
        {
            if (!maneuver.isRopeAttach)
                isPlayerExit = false;

            if (isPlayerExit)
            {
                isPlayerExit = false;
                maneuver.RopeComeBack();
                this.gameObject.SetActive(false);
                return;
            }
            return;
        }
        if(maneuver.isRopeAttach) return;

        maneuver.isRopeAttach = true;
        targetObject = col.gameObject;
        offset = targetObject.transform.position - transform.position;


        if (col.gameObject.CompareTag(maneuverTag))
        {
            Debug.Log("maneuver");
            hookPosition.x = this.transform.position.x;
            hookPosition.y = this.transform.position.y;

            collideTarget = CollideTarget.PULL; 
            this.gameObject.SetActive(false);
        }
    }
}
public enum CollideTarget { NONE, SWING, PULL, PUSH };
