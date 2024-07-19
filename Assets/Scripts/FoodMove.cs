using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class FoodMove : MonoBehaviour
{
    Player player;

    Camera mainCamera;

    public MovePoint movePattern;
    public Vector2 movePoint;
    public float moveDuration = 1f;
    public float sleepDuration = 1f;
    public float minMoveRange = 2f;
    public float maxMoveRange = 5f;

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void Start()
    {
        StartCoroutine(nameof(moveAction));
    }
    void Move()
    {
        transform.DOMove(movePoint, moveDuration).SetEase(Ease.InOutCubic);
    }
    void SetMovePoint()
    {
        switch (movePattern)
        {
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
                        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
                        {
                            continue;
                        }
                        else break;
                    }
                    if (roopCnt > 1000) { Destroy(gameObject); }
                    return;
                }
        }
    }
    IEnumerator moveAction()
    {
        Debug.Log("move");
        while (true)
        {
            Vector2 originalPos = transform.position;
            SetMovePoint();
            Move();

            
            yield return new WaitForSeconds(moveDuration + 1);
        }
    }
}
public enum MovePoint { RANDOM,FOLLOW};