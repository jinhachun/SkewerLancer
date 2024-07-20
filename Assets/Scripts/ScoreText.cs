using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;
    virtual public int score => gameManager.score;

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
        while (GameManager.Instance.inGame)
        {
            textMeshPro.text = score.ToString();
            yield return new WaitForSeconds(0.03f);
        }
    }

}
