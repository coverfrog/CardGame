using System;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneMainMenu : MonoBehaviour, IUIScene
{
    [Header("[ References ]")]
    [SerializeField] private SteamConnect mSteamConnect;
    [SerializeField] private SteamRoom mSteamRoom;
    
    [Header("[ References ]")]
    [SerializeField] private UIModeSelector mModeSelector;
    [SerializeField] private UIRoomJoin mRoomJoin;
    [SerializeField] private UIRoom mRoom;
    [SerializeField] private UIToast mToast;
    [SerializeField] private UIFade mFade;

    private void Start()
    {
        Init();
        Enter();
    }

    public IUIScene Init()
    {
        SteamMatchmaking.OnLobbyEntered       -= OnLobbyEntered;
        SteamMatchmaking.OnLobbyEntered       += OnLobbyEntered;

        mSteamRoom.OnDataChanged -= OnReadyChanged;
        mSteamRoom.OnDataChanged += OnReadyChanged;

        mSteamRoom.OnCountChanged -= OnReadyCountChanged;
        mSteamRoom.OnCountChanged += OnReadyCountChanged;
        
        _ = mModeSelector.Init(this);
        _ = mRoomJoin.Init(this);
        _ = mRoom.Init(this);
        _ = mToast.Init(this);
        _ = mFade.Init(this);

        return this;
    }

  

    public IUIScene Enter()
    {
        _ = mModeSelector.Enter(this);
        _ = mRoomJoin.Exit(this);
        _ = mRoom.Exit(this);
        _ = mToast.Exit(this);
        _ = mFade.Exit(this);

        return this;
    }

    public IUIScene Exit()
    {
        return this;
    }
    
    //

    public void StartHost()
    {
        mSteamConnect.StartHost();
    }

    public void Leave()
    {
        mSteamConnect.Leave();

        Enter();
    }

    public void JoinWithId(ulong id)
    {
        mSteamConnect.JoinWithId(id, null, ShowToast);
    }

    public void ShowToast(string message)
    {
        mToast.SetSingle(message, () =>
        {
            Enter();
        }).Enter(this);
    }

    public void ShowRoomJoin()
    {
        mRoomJoin.Enter(this);
    }

    public void Ready()
    {
        mSteamRoom.OnReady();
    }

    private void OnReadyChanged(ulong id, bool ready)
    {
        mRoom.OnReadyChanged(id, ready);
    }
    
    private void OnReadyCountChanged(int count, int target)
    {
        mRoom.OnReadyCountChanged(count, target);
    }
    
    public void GameStart()
    {
        mSteamRoom.GameStart();
    }
    
    //

    private void OnLobbyEntered(Lobby lobby)
    {
        _ = mModeSelector.Exit(this);
        _ = mRoomJoin.Exit(this);
        _ = mRoom.Enter(this);
        _ = mToast.Exit(this);
        _ = mFade.Exit(this);
    }
}
