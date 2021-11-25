using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tween : MonoBehaviour
{
    [SerializeField]
    internal float delay, duration;
    [SerializeField]
    internal LeanTweenType easeType;
    [SerializeField]
    internal AnimationCurve animCurve;
    public abstract void OnComplete();

}
