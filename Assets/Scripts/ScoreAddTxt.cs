using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class ScoreAddTxt : ScoreText
{
    int bonusScore = 0;
    public override string score => bonusScore<=0?"...?":$"+{bonusScore.ToString()}!";
    public float moveDistance = 100f;
    public float duration = 2f;

    
    public void Set(int bonusScore)
    {
        this.bonusScore = bonusScore;
        // �ؽ�Ʈ�� �ʱ� ���İ� ����
        Color initialColor = textMeshPro.color;
        initialColor.a = 1f;
        textMeshPro.color = initialColor;

        // �ؽ�Ʈ�� ���� �̵��ϸ鼭 ���� ��������� DOTween ����
        textMeshPro.rectTransform.DOMoveY(textMeshPro.rectTransform.position.y + moveDistance, duration).SetEase(Ease.OutQuad);
        textMeshPro.DOFade(0, duration).SetEase(Ease.OutQuad).OnComplete(() => Destroy(gameObject));
    }
}
