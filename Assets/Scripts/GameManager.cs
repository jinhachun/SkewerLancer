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

    private void Start()
    {
        StartCoroutine(nameof(CreateFoodRoutine));
    }
    void CreateFood()
    {
        FoodStruct foodStruct = foodStructs[UnityEngine.Random.Range(0, Math.Min(foodStructs.Count, skewerLength))];
        var randomX = Random.Range(-20,20);
        var randomY = Random.Range(-10, 10);
        var tmpFood = Instantiate(_foodPrefab,new Vector3(randomX,randomY),Quaternion.identity);
        tmpFood.GetComponent<Food>().Set(foodStruct);
    }
    void NewReciept()
    {
        skewerLength = UnityEngine.Random.Range(2, maxSkewerLength+1);
        for (int i = 0; i < skewerLength; i++)
        {
            FoodStruct foodStruct = foodStructs[UnityEngine.Random.Range(0, Math.Min(foodStructs.Count,skewerLength))];
            recipe.Add(foodStruct._foodType);
        }
    }
    IEnumerator CreateFoodRoutine()
    {
        if (recipe.Count == 0)
        {
            NewReciept();
        }
        while (creatingFood)
        {
            CreateFood();
            yield return new WaitForSeconds(createFoodRate);

        }
        yield return new WaitForSeconds(createFoodRate);
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