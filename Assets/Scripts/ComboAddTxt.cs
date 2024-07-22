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
        // �ؽ�Ʈ�� �ʱ� ���İ� ����
        Color initialColor = textMeshPro.color;
        initialColor.a = 1f;
        textMeshPro.color = initialColor;

        // �ؽ�Ʈ�� ���� �̵��ϸ鼭 ���� ��������� DOTween ����
        textMeshPro.rectTransform.DOMoveY(textMeshPro.rectTransform.position.y + moveDistance, duration).SetEase(Ease.OutQuad);
        textMeshPro.DOFade(0, duration).SetEase(Ease.OutQuad).OnComplete(() => Destroy(gameObject));
    }
}
