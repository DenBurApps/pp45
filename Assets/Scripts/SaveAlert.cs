using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SaveAlert : MonoBehaviour
{
    [SerializeField] private GameObject _saveAlert;

    public void ShowSaveAlert()
    {
        Sequence anim = DOTween.Sequence();
        anim.Append(_saveAlert.transform.DOScale(1, 0.7f).SetEase(Ease.OutBounce))
            .Append(_saveAlert.transform.DOScale(0, 0.7f).SetEase(Ease.InBounce).SetDelay(2f));
        anim.SetLink(gameObject);
        anim.Play();
    }
}
