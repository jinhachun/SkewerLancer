using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreText : ScoreText
{
    int highscore => PlayerPrefs.GetInt("HIGHSCORE");
    public override string score => highscore.ToString();
    private void Update()
    {
        if (highscore <= GameManager.Instance.score)
        {
            textMeshPro.color = Color.red;
        }
    }
}
