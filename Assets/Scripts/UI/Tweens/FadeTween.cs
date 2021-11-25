using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTween : Tween
{
    [SerializeField]
    float startAlpha, targetAlpha;
    public void OnEnable()
    {
        
        if (easeType == LeanTweenType.animationCurve)
        {
            LeanTween.value(startAlpha, targetAlpha, duration).setDelay(delay).setOnComplete(OnComplete).setOnUpdate(UpdateAlpha).setEase(animCurve);
        }
        else
        {
            LeanTween.value(startAlpha, targetAlpha, duration).setDelay(delay).setOnComplete(OnComplete).setOnUpdate(UpdateAlpha).setEase(easeType);
        }
    }
    public void UpdateAlpha(float val)
    {
        GetComponent<TMPro.TMP_Text>().alpha = val;
    }
    public override void OnComplete()
    {
        if(startAlpha > targetAlpha)
        {
            Destroy(gameObject);
        }
    }

    
}
