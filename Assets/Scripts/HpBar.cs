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
            Debug.Log("ü�¹� ������Ʈ");
            slider.value = player.hp;
            slider.maxValue = player.maxHp;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
