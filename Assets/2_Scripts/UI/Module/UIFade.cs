using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIFade : MonoBehaviour, IUIModule
{
    private Image _mImage;

    private bool _mIsLoopBack;
    
    private float _mStart;
    private float _mEnd;
    private float _mDuration;
    private float _mDelay;
    
    private Color _mColor = Color.black;

    private int _mCount;

    private Action _mLoopAction;
    private Action _mEndAction;

    public IUIModule Init(IUIScene scene)
    {
        _mImage ??= GetComponent<Image>();
        
        return this;
    }

    public IUIModule Enter(IUIScene scene)
    {
        gameObject.SetActive(true);
        
        _mColor.a = _mStart;
        _mCount = 0;

        _mImage.color = _mColor;
        _mImage
            .DOFade(_mEnd, _mDuration)
            .SetLoops(_mIsLoopBack ? 2 : 1, LoopType.Yoyo)
            .SetDelay(_mDelay)
            .OnStepComplete(() =>
            {
                if (!_mIsLoopBack) return;

                ++_mCount;

                if (_mCount >= 2) return;
                
                _mLoopAction?.Invoke();
                _mLoopAction = null;
            }).
            OnComplete(() =>
            {
                _mEndAction?.Invoke();
                _mEndAction = null;
            });
        
        return this;
    }

    public IUIModule Exit(IUIScene scene)
    {
        gameObject.SetActive(false);
        
        return this;
    }
    
    public UIFade SetFadeIn(float duration)
    {
        _mStart = 1.0f;
        _mEnd = 0.0f;
        _mIsLoopBack = false;
        _mDuration = duration;
        return this;
    }

    public UIFade SetFadeOut(float duration)
    {
        _mStart = 0.0f;
        _mEnd = 1.0f;
        _mIsLoopBack = false;
        _mDuration = duration;
        return this;
    }

    public UIFade SetFadeOutIn(float duration)
    {
        _mStart = 0.0f;
        _mEnd = 1.0f;
        _mIsLoopBack = true;
        _mDuration = duration;
        return this;
    }

    public UIFade SetStart(float start)
    {
        _mStart = start;
        return this;
    }

    public UIFade SetEnd(float end)
    {
        _mEnd = end;
        return this;
    }

    public UIFade SetDuration(float duration)
    {
        _mDuration = duration;
        return this;
    }

    public UIFade SetIsLoopBack(bool isLoopBack)
    {
        _mIsLoopBack = isLoopBack;
        return this;
    }

    public UIFade SetDelay(float delay)
    {
        _mDelay = delay;
        return this;
    }

    public UIFade SetColor(Color color)
    {
        _mImage.color = color;
        return this;
    }

    public UIFade SetLoopAction(Action action)
    {
        _mLoopAction = action;
        return this;
    }

    public UIFade SetEndAction(Action action)
    {
        _mEndAction = action;
        return this;
    }
}
