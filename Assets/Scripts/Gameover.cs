using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gameover : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highscoreText;

    public void Set(int score,int highscore)
    {
        scoreText.text = score.ToString();
        highscoreText.text = highscore.ToString();
    }
}
