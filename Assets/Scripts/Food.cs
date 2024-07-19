using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Color32 color;
    public bool isTarget;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        spriteRenderer.color = color;
    }

    void Update()
    {
        
    }
}
