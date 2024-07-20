using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboText : ScoreText
{
    public override int score => GameManager.Instance.combo;
}
