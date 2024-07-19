using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UISkewer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _foodPrefab;
    [SerializeField] SpriteRenderer _lancePike;
    List<FoodType> recipe;

    void Set()
    {
        recipe = GameManager.Instance.recipe;
        
    }
}
