using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBar : HpBar
{
    override public int value => player.comboTime;
    override public int maxValue => player.maxComboTime;
}
