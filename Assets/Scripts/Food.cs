using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    Player player;
    GameObject lance;
    ObjectMovement objectMovement;

    FoodMove foodMove;

    SpriteRenderer spriteRenderer;

    public FoodType type;
    public Color32 color;
    public bool isTarget;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        foodMove = GetComponent<FoodMove>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        objectMovement = player.GetComponent<ObjectMovement>();
        lance = player.transform.GetChild(0).gameObject;
    }
    void Start()
    {
        spriteRenderer.color = color;
    }

    public void Set(FoodStruct foodStruct)
    {
        this.type = foodStruct._foodType;
        this.color = foodStruct._color;
        foodMove.moveDuration = foodStruct._moveDuration;
        foodMove.movePattern = foodStruct._movePattern;
        foodMove.sleepDuration = foodStruct._sleepDuration;
        foodMove.maxMoveRange = foodStruct._moveRange;
    }
    public void HitAction()
    {
        if (isTarget)
        {
            AttachToSkewer();
        }
        else
        {

        }
    }
    void AttachToSkewer()
    {
        foodMove.isMoving = false;
        foodMove.tweener.Kill();
        GetComponent<Collider2D>().enabled = false;
        this.transform.position = player.GetComponent<PlayerLance>().foodVector();
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = Vector3.zero;
        rigidbody2D.isKinematic = true;
        rigidbody2D.simulated = false;
        this.transform.SetParent(lance.transform, false);
        this.spriteRenderer.sortingLayerName = "UI";
        this.spriteRenderer.sortingOrder = 101;
        this.transform.localScale *= 0.6f;

        GameManager.Instance.foodCnt -= 1;
        GameManager.Instance.AddPlayerRecipe(this.type);
    }
    void WrongFood()
    {
        Destroy(gameObject);
    }

}
