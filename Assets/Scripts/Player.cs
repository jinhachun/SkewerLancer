using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] UISkewer uiSkewer;
    PlayerLance playerLance;
    public int hp = 100;
    public int maxHp = 100;
    public int comboTime = 0;
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
                GameManager.Instance.EarnScore();
                uiSkewer.ResetSprite();
                hp = System.Math.Min(hp+20+GameManager.Instance.skewerLength,maxHp);
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
            yield return new WaitForSeconds(0.4f);
        }
    }

}
