using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Netcode.Transports.Facepunch;
using Unity.Netcode;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class SteamConnect : MonoBehaviour, ISteamClient
{
    private static Lobby? CurrentLobby { get; set; }
    
    private bool _mIsNetLoaded;

    private Action _mOnStartHostFail, _mOnStartHostSuccess;

    private IEnumerator Start()
    {
        SteamMatchmaking.OnLobbyCreated       -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered       -= OnLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
        
        SteamMatchmaking.OnLobbyCreated       += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered       += OnLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
        
        yield return new WaitUntil(() => NetworkManager.Singleton);

        _mIsNetLoaded = true;
    }

    // private void OnDestroy()
    // {
    //     if (NetworkManager.Singleton is { IsHost: true } or { IsClient: true})
    //     {
    //         NetworkManager.Singleton.Shutdown();
    //     }
    // }

    private async void OnGameLobbyJoinRequested(Lobby lobby, SteamId steamId)
    {
        try
        {
            // 받는 쪽이 아니라 보내는 쪽에서 콜백
            await lobby.Join();
        }
        catch (Exception)
        {
            // ignore
        }
    }

    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result != Result.OK)
        {
            return;
        }
        
        lobby.SetJoinable(true);
        lobby.SetPublic();

        NetworkManager.Singleton.StartHost();
    }
    
    private void OnLobbyEntered(Lobby lobby)
    {
        CurrentLobby = lobby;
        
        NetworkManager.Singleton.GetComponent<FacepunchTransport>().targetSteamId = lobby.Owner.Id;
        
        if (NetworkManager.Singleton.IsServer) return;
        
        NetworkManager.Singleton.StartClient();
    }
    
    //

    public async void StartHost(
        int maxMembers = 2, 
        Action onSuccess = null, 
        Action<string> onFail = null)
    {
        try
        {
            if (!_mIsNetLoaded)
            {
                onFail?.Invoke("Net Not Loaded");
                
                return;
            }

            var task = await SteamMatchmaking.CreateLobbyAsync(maxMembers);

            if (task.HasValue)
            {
                onFail?.Invoke("Lobby Creation Failed");
                return;
            }

            onSuccess?.Invoke();
        }
        catch (Exception e)
        {
            onFail?.Invoke(e.Message);
        }
    }

    public async void JoinWithId(ulong id, Action onSuccess, Action<string> onFail)
    {
        try
        {
            if (!_mIsNetLoaded)
            {
                onFail?.Invoke("Net Not Loaded");
                
                return;
            }
            
            var result = await SteamMatchmaking.LobbyList.WithSlotsAvailable(1).RequestAsync();

            var lobbyArr = result.Where(l => l.Id == id).ToArray();

            if (lobbyArr.Length == 0)
            {
                onFail?.Invoke("Lobby Not Founded");
                return;
            }
            
            _ = await lobbyArr[0].Join();

            onSuccess?.Invoke();
        }
        catch (Exception e)
        {
            onFail?.Invoke(e.Message);
        }
    }

    public void Leave()
    {
        CurrentLobby?.Leave();
        
        NetworkManager.Singleton.Shutdown();
    }
}
