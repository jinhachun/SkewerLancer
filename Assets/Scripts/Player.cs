using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp = 100;
    public int maxHp = 100;

    
    void Start()
    {
        StartCoroutine(nameof(hpLoss));
    }

    IEnumerator hpLoss()
    {
        while (true)
        {
            hp -= 1;
            if (hp <= 0) hp = 0;
            yield return new WaitForSeconds(0.25f);
        }
    }

}
