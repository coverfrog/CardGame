using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeSelector : MonoBehaviour, IUIModule
{
    [SerializeField] private Button mHostButton;
    [SerializeField] private Button mJoinButton;
    
    public IUIModule Init(IUIScene scene)
    {
        if (scene is not UISceneMainMenu mainMenu) return this;
        
        mHostButton.onClick.RemoveAllListeners();
        mHostButton.onClick.AddListener(() =>
        {
            mainMenu.StartHost();
        });
        
        mJoinButton.onClick.RemoveAllListeners();
        mJoinButton.onClick.AddListener(() =>
        {
            mainMenu.ShowRoomJoin();
        });
        
        return this;
    }

    public IUIModule Enter(IUIScene scene)
    {
        gameObject.SetActive(true);

        return this;
    }

    public IUIModule Exit(IUIScene scene)
    {
        gameObject.SetActive(false);

        return this;
    }
}