using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIToast : MonoBehaviour, IUIModule
{
    #region Type

    public enum Mode
    {
        Single,
        Multiple
    }

    #endregion
    
    [SerializeField] private TMP_Text mText;
    [Space]
    [SerializeField] private Button mYesButton;
    [SerializeField] private Button mNoButton;

    private Mode _mMode;
    
    private Action _mOnYes, _mOnNo;

    public IUIModule Enter(IUIScene scene)
    {
       gameObject.SetActive(true);

       SetInteractable(true);
       
       return this;
    }

    public IUIModule Exit(IUIScene scene)
    {
        gameObject.SetActive(false);
        
        return this;
    }
    
    public IUIModule Init(IUIScene scene)
    {
        OnInitYes();
        OnInitNo();
        
        return this;
    }

    private void OnInitYes()
    {
        mYesButton.interactable = true;
        mYesButton.onClick.AddListener(() =>
        {
            SetInteractable(false);
            
            _mOnYes?.Invoke();
            _mOnYes = null;
            
            gameObject.SetActive(false);
        });
    }

    private void OnInitNo()
    {
        mNoButton.interactable = true;
        mNoButton.onClick.AddListener(() =>
        {
            SetInteractable(false);
            
            _mOnNo?.Invoke();
            _mOnNo = null;
            
            gameObject.SetActive(false);
        });
    }

    private void SetInteractable(bool value)
    {
        mNoButton.interactable = value;
        mYesButton.interactable = value;
    }

    public UIToast SetSingle(string text, Action onYes)
    {
        return SetText(text)
            .SetMode(Mode.Single)
            .SetOnYes(onYes);
    }

    public UIToast SetMultiple(string text, Action onYes, Action onNo)
    {
        return SetText(text)
            .SetMode(Mode.Multiple)
            .SetOnYes(onYes)
            .SetOnNo(onNo);
    }

    public UIToast SetText(string text)
    {
        mText.text = text;
        return this;
    }

    public UIToast SetMode(Mode mode)
    {
        _mMode = mode;
        
        mYesButton.gameObject.SetActive(true);
        mNoButton.gameObject.SetActive(mode == Mode.Multiple);
        
        return this;
    }

    public UIToast SetOnYes(Action onYes)
    {
        _mOnYes = onYes;
        return this;
    }

    public UIToast SetOnNo(Action onNo)
    {
        _mOnNo = onNo;
        return this;
    }
}
