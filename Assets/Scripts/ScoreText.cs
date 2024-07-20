using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;
    int score => gameManager.score;

    TMPro.TMP_Text textMeshPro;


    private void Awake()
    {
        textMeshPro = GetComponent<TMPro.TMP_Text>();
    }
    void Start()
    {
        StartCoroutine(nameof(SetScoretext));
    }
    IEnumerator SetScoretext()
    {
        while (true)
        {
            Debug.Log("점수 텍스트 업데이트");
            textMeshPro.text = score.ToString();
            yield return new WaitForSeconds(0.03f);
        }
    }

}
