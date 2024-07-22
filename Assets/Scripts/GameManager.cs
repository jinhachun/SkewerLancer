using System;
using System.Collections;
using System.Collections.Generic;
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
    public static void DestroyGameManager()
    {
        Destroy(instance.gameObject);
    }

    public bool inGame;
    public GameObject _foodPrefab;

    public List<FoodStruct> foodStructs;
    public Queue<FoodStruct> foodStructQueue;

    public Dictionary<FoodStruct, int> foodStructCnt;
    Dictionary<FoodStruct, int> foodStructSpawnPercentage;

    public List<FoodType> recipe;
    public List<FoodType> playerRecipe;

    [SerializeField]
    int maxLevel = 8;
    [SerializeField]
    int minLevel = 2;
    [SerializeField]
    int testCombo = 0;
    public int maxSkewerLength => 2+ Math.Min(combo/4,5);
    public int skewerLength = 2;

    public int foodCnt = 0;
    public int maxFoodCnt = 20;

    public float createFoodRate = 2;
    public bool creatingFood = true;

    public int score;
    public int combo;
    private int levelUpScore = 5000;
    public int level = 1;
    public bool isRecipeChanged = false;

    public int scoreRate => (10 * combo);
    public int scoreEatRate => skewerLength * combo * 50;

    public float hpLossRate => Math.Max(0.35f - (0.025f * (level)), 0.2f);





    private void Start()
    {
        inGame = true;
        Time.timeScale = 1f;
        foodStructCnt = new Dictionary<FoodStruct, int>();
        foodStructSpawnPercentage = new Dictionary<FoodStruct, int>();
        foodStructCnt.Add(foodStructs[0], 0);
        foodStructCnt.Add(foodStructs[1], 0);
        StartRoutine();

       
    }
    void StartRoutine()
    {
        StartCoroutine(nameof(CreateFoodRoutine));
        StartCoroutine(nameof(ScoreEarnRoutine));
    }
    void CreateFood()
    {

        var i = Math.Min(foodStructs.Count - 1, maxSkewerLength - 1);
        if (!foodStructCnt.ContainsKey(foodStructs[i]))
            foodStructCnt.Add(foodStructs[i], 0);
        UpdateFoodStructCnt();
        FoodStruct foodStruct = RandomSpawnTarget();
        foreach (KeyValuePair<FoodStruct, int> keyValuePair in foodStructCnt)
        {
            if (foodCnt!=0 && keyValuePair.Value == 0)
            {
                foodStruct = keyValuePair.Key;
            }
        }
        foodStructCnt[foodStruct]++;
        var randomX = Random.Range(-15,15);
        var randomY = Random.Range(-7, 7);
        var tmpFood = Instantiate(_foodPrefab,new Vector3(randomX,randomY),Quaternion.identity);
        tmpFood.GetComponent<Food>().Set(foodStruct);
    }
    void UpdateFoodStructCnt()
    {
        foodStructSpawnPercentage.Clear();
        int max = 0;
        foreach (var keyValue in foodStructCnt)
        {
            max = Math.Max(max, keyValue.Value);
        }
        foreach (var keyValue in foodStructCnt)
        {
            foodStructSpawnPercentage.Add(keyValue.Key, max - keyValue.Value);
        }

    }
    FoodStruct RandomSpawnTarget()
    {
        List<FoodStruct> tmpFoodStructs = new List<FoodStruct>();
        foreach(var keyValue in foodStructSpawnPercentage)
        {
            Debug.Log(keyValue.Key+" "+keyValue.Value);
            for(int i=0;i<keyValue.Value; i++)
            {
                tmpFoodStructs.Add(keyValue.Key);
            }
        }
        if (tmpFoodStructs.Count > 0)
            return tmpFoodStructs[Random.Range(0, tmpFoodStructs.Count)];
        else 
            return foodStructs[UnityEngine.Random.Range(0, Math.Min(foodStructs.Count, skewerLength))]; ;
    }
    void NewRecipe()
    {
        //var = foodStructCnt.Keys[Random.Range(0,foodStructCnt.Keys.Count)]
        skewerLength = UnityEngine.Random.Range(2, maxSkewerLength+1);
        for (int i = 0; i < skewerLength; i++)
        {
            FoodStruct foodStruct = foodStructs[UnityEngine.Random.Range(0, Math.Min(foodStructs.Count,skewerLength))];
            recipe.Add(foodStruct._foodType);
        }
        isRecipeChanged = true;
    }
    void SetHighScore()
    {
        int highscore = PlayerPrefs.GetInt("HIGHSCORE", 0);
        if (highscore < score)
        {
            PlayerPrefs.SetInt("HIGHSCORE", score);
        }
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
    public void FoodAttach(FoodStruct foodStruct)
    {
        foodCnt -= 1;
        foodStructCnt[foodStruct]--;
        AddPlayerRecipe(foodStruct._foodType);
    }
    public void EarnScore()
    {
        score += scoreEatRate;
        combo++;
        SetHighScore();
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
                if (foodCnt < maxFoodCnt)
                {
                    foodCnt++;
                    CreateFood();
                }

            }
            yield return new WaitForSeconds(createFoodRate);
        }
    }
    IEnumerator ScoreEarnRoutine()
    {
        while (true)
        {
            score += scoreRate;
            SetHighScore();
            if (score != 0 && score>levelUpScore)
            {
                level++;
                levelUpScore *= 2;
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
    public float _sleepDuration;
    public float _minMoveRange;
    public float _maxMoveRange;

}