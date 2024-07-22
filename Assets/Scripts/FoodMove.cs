using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Cinemachine.Examples;


public class FoodMove : MonoBehaviour
{
    Player player;

    Camera mainCamera;
    RaycastHit2D hit;

    public MovePoint movePattern;
    public Vector2 movePoint;
    public float moveDuration = 1f;
    public float sleepDuration = 1f;
    public float minMoveRange = 2f;
    public float maxMoveRange = 5f;
    public bool isMoving;

    Vector2 moveDirection;

    public Tweener tweener;
    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void Start()
    {
        StartCoroutine(nameof(moveAction));
        moveDirection = Vector2.right;
    }
    private void Update()
    {
    }
    void Move()
    {
        tweener = (transform.DOMove(movePoint, moveDuration).SetEase(Ease.InOutCubic)).OnComplete(MoveComplete);
    }
    void MoveComplete()
    {
        switch (movePattern)
        {
            case (MovePoint.BOUNCE):
                {
                    moveDirection = Quaternion.Euler(0, 0, 90) * moveDirection;
                    return;
                }
        } 
    }
    void SetMovePoint()
    {
        switch (movePattern)
        {
            case (MovePoint.NONE):
                {
                    movePoint = this.transform.position;
                    isMoving = false;
                    return;
                }
            case (MovePoint.FOLLOW):
                {
                    var minX = Mathf.Min(transform.position.x, player.transform.position.x + 1);
                    var minY = Mathf.Min(transform.position.y, player.transform.position.y + 1);
                    var maxX = Mathf.Max(transform.position.x, player.transform.position.x + 1);
                    var maxY = Mathf.Max(transform.position.y, player.transform.position.y + 1);
                    var followX = Random.Range(minX,maxX);
                    var followY = Random.Range(minY,maxY);
                    movePoint = new Vector2(followX,followY);
                    return;
                }
            case (MovePoint.RANDOM):
                {
                    if (minMoveRange > maxMoveRange) return;
                    int roopCnt = 0;
                    while (roopCnt < 1000) {
                        var minX = transform.position.x - Random.Range(0, maxMoveRange + 1);
                        var minY = transform.position.y - Random.Range(0, maxMoveRange + 1);
                        var maxX = transform.position.x + Random.Range(0, maxMoveRange + 1);
                        var maxY = transform.position.y + Random.Range(0, maxMoveRange + 1);
                        var randomX = Random.Range(minX, maxX);
                        var randomY = Random.Range(minY, maxY);
                        movePoint = new Vector2(randomX, randomY);

                        if (Vector2.Distance(transform.position, movePoint) < minMoveRange)
                            continue;

                        Vector3 viewPos = mainCamera.WorldToViewportPoint(movePoint);
                        if (viewPos.x < 0.15 || viewPos.x > 0.85 || viewPos.y < 0.15 || viewPos.y > 0.85)
                        {
                            continue;
                        }
                        else break;
                    }
                    if (roopCnt > 100) { 
                        Destroy(gameObject); 
                    }
                    return;
                }
            case(MovePoint.BOUNCE):
                {
                    var moveDirection = new Vector2(1,1);
                    Debug.DrawRay(transform.position, moveDirection * 20f);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection,  20f);
                    if (hit.collider != null)
                    {
                        
                        Vector2 hitPoint = hit.point;
                        movePoint = hitPoint;

                        Vector2 normal = hit.normal;
                        moveDirection = Vector2.Reflect(moveDirection, normal);
                        
                    }
                    return;
                }
        }
    }
    IEnumerator moveAction()
    {
        Debug.Log("move");
        int roopCnt = 0;
        while (isMoving)
        {
            if (roopCnt > 300) {
                Debug.Log("음식이동 무한루프");
                break;
            }
            Debug.Log("음식 이동");
            roopCnt++;
            SetMovePoint();
            if (isMoving)
                Move();
            else break;

            
            yield return new WaitForSeconds(moveDuration + 1);
        }
        yield break; 
    }
}
public enum MovePoint { NONE,RANDOM,FOLLOW,BOUNCE};