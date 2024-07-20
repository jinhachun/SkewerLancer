using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectCntTxt : MonoBehaviour
{
    TMPro.TMP_Text txt;
    void Start()
    {
        txt = GetComponent<TMPro.TMP_Text>();
        StartCoroutine(nameof(SetObjectCntTxt));
    }

    IEnumerator SetObjectCntTxt()
    {
        while (GameManager.Instance.inGame)
        {
            txt.text = $"Food In Field : {GameManager.Instance.foodCnt}/{GameManager.Instance.maxFoodCnt}";
            yield return new WaitForSeconds(0.03f);
        }
    }
}
