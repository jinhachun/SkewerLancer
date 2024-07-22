using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboText : ScoreText
{
    public override string score => GameManager.Instance.combo.ToString();
}
