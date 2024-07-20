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

    public bool inGame;
    public GameObject _foodPrefab;

    public List<FoodStruct> foodStructs;
    public Dictionary<FoodStruct, int> foodStructCnt;
    
    public List<FoodType> recipe;
    public List<FoodType> playerRecipe;

    public int maxSkewerLength = 2;
    public int skewerLength = 2;

    public int foodCnt = 0;
    public int maxFoodCnt = 20;

    public float createFoodRate = 2;
    public bool creatingFood = true;

    public int score;
    private int levelUpScore = 1000;
    public bool isRecipeChanged = false;

     

    private void Start()
    {
        foodStructCnt = new Dictionary<FoodStruct, int>();
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
        foreach (KeyValuePair<FoodStruct,int> keyValuePair in foodStructCnt)
        {
            if(keyValuePair.Value == 0)
            {
                foodStruct = keyValuePair.Key;
            }
        }
        if(!foodStructCnt.ContainsKey(foodStruct)) foodStructCnt.Add(foodStruct, 0);
        foodStructCnt[foodStruct] += 1;
       
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
        if (playerRecipe.Count != recipe.Count)
            return false;
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
    public void EarnScore()
    {
        score += skewerLength*100;
    }
    IEnumerator CreateFoodRoutine()
    {
        while (inGame)
        {
            Debug.Log("===CreateFoodRoutine===");
            if (recipe.Count == 0)
            {
                Debug.Log("새 레시피");
                NewRecipe();

            }
            if (creatingFood)
            {
                if (foodCnt < maxFoodCnt)
                {
                    Debug.Log("음식 생성");
                    foodCnt++;
                    CreateFood();
                }

            }
            yield return new WaitForSeconds(createFoodRate);
        }
    }
    IEnumerator ScoreEarnRoutine()
    {
        while (GameManager.Instance.inGame)
        {
            Debug.Log("점수추가");
            score+=(50+skewerLength);
            if(score != 0 && score>levelUpScore)
            {
                levelUpScore *= 2;
                maxSkewerLength++;
                createFoodRate = Math.Max(createFoodRate-0.2f,0.5f);
            }
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