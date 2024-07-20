using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UISkewer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _foodPrefab;
    [SerializeField] SpriteRenderer _lancePike;
    List<FoodType> recipe;

    private void Start()
    {
        StartCoroutine(nameof(SetRoutine));
    }

    [ContextMenu("SET")]
    void Set()
    {
        recipe = GameManager.Instance.recipe;
        int foodCnt = recipe.Count;
        _lancePike.transform.position -= new Vector3 (1.1f*foodCnt+.1f, 0, 0);
        for (int i = 0; i < foodCnt; i++)
        {
            var tmpFood = Instantiate(_foodPrefab, new Vector3(-1.1f*i-.1f, 0, 0), Quaternion.identity);
            tmpFood.transform.SetParent(this.gameObject.transform, false);
            var tmpFoodFood = tmpFood.GetComponent<Food>();
            FoodStruct tmpFoodStruct = GameManager.Instance.foodStructs.Find(
                x => x._foodType == recipe[i]
                );
            var tmpFoodSpriteRenderer = tmpFood.GetComponent<SpriteRenderer>();
            tmpFoodSpriteRenderer.sortingLayerName = "UI";
            tmpFoodSpriteRenderer.sortingOrder = 2;
            tmpFoodStruct._movePattern = MovePoint.NONE;
            tmpFoodFood.Set(tmpFoodStruct);
        }
    }

    IEnumerator SetRoutine()
    {
        while (true){
            if (GameManager.Instance.isRecipeChanged)
            {
                GameManager.Instance.isRecipeChanged = false;
                Set();
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
