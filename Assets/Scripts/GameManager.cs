using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public GameObject _foodPrefab;

    public int maxSkewerLength;
    public List<FoodStruct> foodStructs;
    public List<FoodType> recipe;
    public List<FoodType> playerRecipe;

    int skewerLength = 2;
    public float createFoodRate = 2;
    public bool creatingFood = true;

    public int score;
    public bool isRecipeChanged = false;

    private void Start()
    {
        StartRoutine();
    }
    void StartRoutine()
    {
        StartCoroutine(nameof(CreateFoodRoutine));
        StartCoroutine(nameof(ScoreEarnRoutine));
    }
    void CreateFood()
    {
        FoodStruct foodStruct = foodStructs[UnityEngine.Random.Range(0, Math.Min(foodStructs.Count, skewerLength))];
        var randomX = Random.Range(-20,20);
        var randomY = Random.Range(-10, 10);
        var tmpFood = Instantiate(_foodPrefab,new Vector3(randomX,randomY),Quaternion.identity);
        tmpFood.GetComponent<Food>().Set(foodStruct);
    }
    void NewRecipe()
    {
        skewerLength = UnityEngine.Random.Range(2, maxSkewerLength+1);
        for (int i = 0; i < skewerLength; i++)
        {
            FoodStruct foodStruct = foodStructs[UnityEngine.Random.Range(0, Math.Min(foodStructs.Count,skewerLength))];
            recipe.Add(foodStruct._foodType);
        }
        isRecipeChanged = true;
    }
    public void AddPlayerRecipe(FoodType foodType)
    {
        playerRecipe.Add(foodType);
    }
    public bool isRecipeSame()
    {
        int i = 0;
        foreach (var food in playerRecipe)
        {
            if (food == recipe[i])
                i++;
            else
                return false;
        }
        if (i == recipe.Count)
        {
            return true;
        }
        return false;
    }
    public void ResetRecipe(bool isRecipeSame)
    {
        if(isRecipeSame)
            recipe.Clear();
        playerRecipe.Clear();
    }
    IEnumerator CreateFoodRoutine()
    {
        while (true)
        {
            if (recipe.Count == 0)
            {
                NewRecipe();

            }
            if (creatingFood)
            {
                CreateFood();

            }
            yield return new WaitForSeconds(createFoodRate);
        }
    }
    IEnumerator ScoreEarnRoutine()
    {
        while (true)
        {
            score++;
            yield return new WaitForSeconds(1f);
        }
    }
}
public enum FoodType { MEAT, GREENONION, CARROT, CORN, MASHMELLOW, FISH };
[Serializable]
public struct FoodStruct
{
    public FoodType _foodType;
    public Color32 _color;
    public MovePoint _movePattern;
    public float _moveDuration;
}