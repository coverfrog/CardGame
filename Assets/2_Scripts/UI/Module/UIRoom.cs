using System;
using System.Collections.Generic;
using Steamworks;
using Steamworks.Data;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIRoom : MonoBehaviour, IUIModule
{
    [Header("[ References ]")]
    [SerializeField] private UIRoomId mId;
    [SerializeField] private UIRoomPlayerView mPlayerView;
    [SerializeField] private UIRoomStart mRoomStart;
    [SerializeField] private UIRoomExit mRoomExit;

    private Lobby _mLobby;
 
    public IUIModule Init(IUIScene scene)
    {
        _ = mId.Init();
        _ = mPlayerView.Init();
        _ = mRoomStart.Init();
        _ = mRoomExit.Init(scene);
        
        return this;
    }

    public IUIModule Enter(IUIScene scene)
    {
        _ = mPlayerView.Enter();
        _ = mRoomStart.Enter(scene);
        
        gameObject.SetActive(true);
        
        return this;
    }

    public IUIModule Exit(IUIScene scene)
    {
        _ = mPlayerView.Exit();
        
        gameObject.SetActive(false);
        
        return this;
    }
    
    //

    public void OnReadyChanged(ulong id, bool ready)
    {
        mPlayerView.OnReadyChanged(id, ready);
    }

    public void OnReadyCountChanged(int count, int target)
    {
        mRoomStart.OnReadyCountChanged(count, target);
    }
}
