using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class ComboAddTxt : ScoreAddTxt
{
    public override string score => GameManager.Instance.combo<=0?"":$"COMBO {GameManager.Instance.combo.ToString()}";
    virtual public void Set()
    {
        //transform.transform.localScale *= 
        // 텍스트의 초기 알파값 설정
        Color initialColor = textMeshPro.color;
        initialColor.a = 1f;
        textMeshPro.color = initialColor;

        // 텍스트가 위로 이동하면서 점점 사라지도록 DOTween 설정
        textMeshPro.rectTransform.DOMoveY(textMeshPro.rectTransform.position.y + moveDistance, duration).SetEase(Ease.OutQuad);
        textMeshPro.DOFade(0, duration).SetEase(Ease.OutQuad).OnComplete(() => Destroy(gameObject));
    }
}
