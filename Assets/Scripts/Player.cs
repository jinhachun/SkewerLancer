using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] ComboAddTxt _comboAddTxtPrefab;
    [SerializeField] ScoreAddTxt _scoreAddTxtPrefab;
    [SerializeField] UISkewer uiSkewer;
    [SerializeField] GameObject _GameOver;
    PlayerLance playerLance;
    public int hp = 100;
    public int maxHp = 100;
    public int comboTime = 10;
    public int maxComboTime = 10;

    bool eat;

    private void Awake()
    {
        playerLance = GetComponent<PlayerLance>();
    }
    void Start()
    {
        StartCoroutine(nameof(hpLoss));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.playerRecipe.Count == 0) return;
            eat = true;
        }
    }
    private void FixedUpdate()
    {
        if (eat) EatSkewer();        
    }
    void EatSkewer()
    {
        
        
            bool isRecipeSame = GameManager.Instance.isRecipeSame();
            if (isRecipeSame)
            {
                comboTime = maxComboTime;
                GameManager.Instance.EarnScore();
                if(GameManager.Instance.combo == 1)
                {
                    StartCoroutine(nameof(comboPlus));
                }
                uiSkewer.ResetSprite();
                hp = System.Math.Min(hp+20+GameManager.Instance.skewerLength,maxHp);
                var scoreAddTxt = Instantiate(_scoreAddTxtPrefab, this.transform.position, Quaternion.identity);
                scoreAddTxt.Set(GameManager.Instance.scoreEatRate);
                var comboAddTxt = Instantiate(_comboAddTxtPrefab,
                    new Vector2(transform.position.x + Random.Range(-3,3),transform.position.y + Random.Range(-3, 3)),
                    Quaternion.identity);
                comboAddTxt.Set();
            }
            else
            {
                var scoreAddTxt = Instantiate(_scoreAddTxtPrefab, this.transform.position, Quaternion.identity);
                scoreAddTxt.Set(0);
            }
            
            playerLance.ResetLance();
            
            GameManager.Instance.ResetRecipe(isRecipeSame);

        eat = false;
    }
    IEnumerator hpLoss()
    {
        while (GameManager.Instance.inGame)
        {
            Debug.Log("체력감소");
            hp -= 1;
            if (hp <= 0)
            {
                DOTween.KillAll();
                _GameOver.gameObject.SetActive(true);
                _GameOver.GetComponent<Gameover>().Set(GameManager.Instance.score, PlayerPrefs.GetInt("HIGHSCORE", 0));
                GameManager.Instance.inGame = false;
                Time.timeScale = 0;
                hp = 0;
                GameManager.DestroyGameManager();
            }
            yield return new WaitForSeconds(GameManager.Instance.hpLossRate);
        }
    }
    IEnumerator comboPlus()
    {
        while (GameManager.Instance.inGame)
        {
            if (comboTime <= 0)
            {
                GameManager.Instance.combo = 0;
                yield break;
            }
            comboTime--;
            yield return new WaitForSeconds(1f);
        }
    }

}
