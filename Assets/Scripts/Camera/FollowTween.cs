using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script à mettre sur la caméra pour lui faire suivre une cible
public class FollowTween : Tween
{
    public Transform target;
    
    public Vector3 offset;
    public float maxDist;
    private void Start()
    {
        TweenLoop();
    }
    //Méthode appelée pour vérifier si la cible est loin et si c'est le cas la suivre
    private void TweenLoop()
    {
       
        
        if(Vector2.Distance(transform.position, target.position) > maxDist)
        {
            if (easeType == LeanTweenType.animationCurve)
            {
                LeanTween.value(transform.position.x, target.position.x, duration).setOnUpdate(OnUpdateX).setEase(animCurve);
                LeanTween.value(transform.position.y, target.position.y, duration).setOnUpdate(OnUpdateY).setEase(animCurve).setOnComplete(TweenLoop);
            }
            else
            {
                LeanTween.value(transform.position.x, target.position.x, duration).setOnUpdate(OnUpdateX).setEase(easeType);
                LeanTween.value(transform.position.y, target.position.y, duration).setOnUpdate(OnUpdateY).setEase(easeType).setOnComplete(TweenLoop);
            }
            
        }
        else
        {
            StartCoroutine(WaitForTweenLoop());
        }

    }
    IEnumerator WaitForTweenLoop()
    {
        yield return new WaitForSeconds(delay);
        TweenLoop();
    }
    void OnUpdateX(float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    void OnUpdateY(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public override void OnComplete()
    {
        TweenLoop();
    }
}
