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
    Vector2 originPikePos;

    private void Start()
    {
        originPikePos = _lancePike.transform.position ;
        StartCoroutine(nameof(SetRoutine));
    }
    [ContextMenu("RESET")]
    public void ResetSprite()
    {
        List<GameObject> deleteList = new List<GameObject>();
        _lancePike.transform.position = originPikePos;
        int tmpChildCnt = gameObject.transform.childCount;

        for (int i = 1; i < tmpChildCnt; i++)
            deleteList.Add(transform.GetChild(i).gameObject);
        foreach (GameObject obj in deleteList) 
            Destroy(obj);
    }
    [ContextMenu("SET")]
    void Set()
    {
        recipe = GameManager.Instance.recipe;
        int foodCnt = recipe.Count;
        _lancePike.transform.position = (Vector3) originPikePos + new Vector3 (-1.1f*foodCnt-.1f, 0, 0);
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
                Debug.Log("UISkewer Set");
                GameManager.Instance.isRecipeChanged = false;
                Set();
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
