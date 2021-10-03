using UnityEngine;
using System;


abstract public class CustomLeanTween : MonoBehaviour
{
   
    
    [Serializable]
    public enum FadeStyle { FadeIn, FadeOut }
    
    
    [SerializeField]
    public LeanTweenType easingStyle;
    
    [Tooltip("length of animation in seconds")]
    [SerializeField]
    public float duration;
    
    [Tooltip("Delaying of start of animation in seconds")]
    public float delay;
    
    [Tooltip("Whether to start animation on start")]
    public bool startOnEnable;
    
    [Tooltip("Whether to start animating only on first start")]
    public bool firstTimeOnly;
    
    [Tooltip("loop the animation?")]
    public bool loop;
    private bool first;
    internal void Awake()
    {
        first = true;
    }

    
    internal void OnEnable()
    {
        
        if (startOnEnable)
        {
            if ((firstTimeOnly && first) || !firstTimeOnly)
            {
                first = false;
                Animate();
            }
        }
    }

    
    internal void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }

   
    public abstract void Animate();
}
