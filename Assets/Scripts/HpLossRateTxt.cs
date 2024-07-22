using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HpLossRateTxt : ScoreText
{
    public override string score => $"HpLossRate : {Mathf.Round(GameManager.Instance.hpLossRate * 100f) / 100f}";
}
