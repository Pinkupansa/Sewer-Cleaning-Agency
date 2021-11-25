using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : Tween
{
    [SerializeField]
    Vector3 startScale,targetScale;
    
    
    // Start is called before the first frame update
    void OnEnable()
    {
        transform.localScale = startScale;
        if (easeType == LeanTweenType.animationCurve)
        {
            LeanTween.scale(gameObject, targetScale, duration).setDelay(delay).setOnComplete(OnComplete).setEase(animCurve);
        }
        else
        {
            LeanTween.scale(gameObject, targetScale, duration).setDelay(delay).setOnComplete(OnComplete).setEase(easeType);
        }
        
    }

    public override void OnComplete()
    {
        if(startScale.magnitude > targetScale.magnitude)
        {
            Destroy(gameObject);
        }
    }
    
}
