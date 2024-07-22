using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] ScoreAddTxt _scoreAddTxtPrefab;
    [SerializeField] UISkewer uiSkewer;
    PlayerLance playerLance;
    public int hp = 100;
    public int maxHp = 100;
    public int comboTime = 10;
    public int maxComboTime = 10;

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
            }
            else
            {
                var scoreAddTxt = Instantiate(_scoreAddTxtPrefab, this.transform.position, Quaternion.identity);
                scoreAddTxt.Set(0);
            }
            
            playerLance.ResetLance();
            
            GameManager.Instance.ResetRecipe(isRecipeSame);
        }
    }
    IEnumerator hpLoss()
    {
        while (GameManager.Instance.inGame)
        {
            Debug.Log("체력감소");
            hp -= 1;
            if (hp <= 0) hp = 0;
            yield return new WaitForSeconds(0.3f);
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
