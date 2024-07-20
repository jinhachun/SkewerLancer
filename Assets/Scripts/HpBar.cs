using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Player player;
    Slider slider;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        slider = GetComponent<Slider>();
    }
    private void Start()
    {
        StartCoroutine(nameof(HpBarUpdate));
    }
    IEnumerator HpBarUpdate()
    {
        while (GameManager.Instance.inGame)
        {
            Debug.Log("체력바 업데이트");
            slider.value = player.hp;
            slider.maxValue = player.maxHp;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
