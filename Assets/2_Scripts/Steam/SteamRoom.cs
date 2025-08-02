using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamRoom : NetworkBehaviour
{
    public delegate void OnChangedDelegate(ulong id, bool isReady);
    
    public delegate void OnCountChangedDelegate(int count, int target);
    
    public event OnChangedDelegate OnDataChanged;
    public event OnCountChangedDelegate OnCountChanged;
    
    private readonly Dictionary<ulong, bool> _readyDict = new Dictionary<ulong, bool>();

    private bool _mIsLocalReady;

    private Lobby _mLobby;

    private IEnumerator Start()
    {
        while (!NetworkManager.Singleton)
        {
            yield return null;
        }
        
        SteamMatchmaking.OnLobbyCreated            -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered            -= OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined       -= OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberDisconnected -= OnLobbyMemberDisconnected;
        
        SteamMatchmaking.OnLobbyCreated            += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered            += OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined       += OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberDisconnected;
        
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    //
    
    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"[Netcode] Client disconnected: {clientId}");

        if (!NetworkManager.Singleton.IsServer)
        {
            var disconnectReason = NetworkManager.Singleton.DisconnectReason;
            Debug.Log($"[Netcode] Disconnect reason: {disconnectReason}");
        }
    }
    
    //
    
    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        Add_Member_Rpc(
            Array.Empty<ulong>(), 
            Array.Empty<bool>(),
            0, 
            SteamClient.SteamId,
            true);
    }
    
    private void OnLobbyEntered(Lobby lobby)
    {
        _mLobby = lobby;
    }
    
    private void OnLobbyMemberJoined(Lobby lobby, Friend friend)
    {
        Debug.Log("조인");
        
        if (!IsServer)
        {
            return;
        }
        
        Add_Member_Rpc(
            _readyDict.Keys.ToArray(), 
            _readyDict.Values.ToArray(),
            _readyDict.Count, 
            friend.Id,
            false);
    }
    
    private void OnLobbyMemberDisconnected(Lobby lobby, Friend friend)
    {
        if (!IsServer)
        {
            return;
        }
        
        Remove_Member_Rpc(friend.Id);
    }

    //

    [ContextMenu("> Context : Report")]
    public void Report()
    {
        foreach (var pair in _readyDict)
        {
            Debug.Log($"{pair.Key}: {pair.Value}");
        }
    }
    
    //

    public void OnReady()
    {
        _mIsLocalReady = !_mIsLocalReady;
        
        Ready_Rpc(SteamClient.SteamId, _mIsLocalReady);
    }

    public void GameStart()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("2_Game", LoadSceneMode.Single);
    }
    
    //
    
    [Rpc(SendTo.ClientsAndHost)]
    private void Add_Member_Rpc(ulong[] ids, bool[] values, int count, ulong id, bool isServer)
    {
        _readyDict.Clear();

        for (int i = 0; i < count; i++)
        {
            _readyDict.Add(ids[i], values[i]);
            
            Debug.Log($"{i} : {values[i]}");
        }
        
        _readyDict.Add(id, isServer);
        
        Debug.Log($"{id}");
        
        OnDataChanged?.Invoke(id, isServer);
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void Remove_Member_Rpc(ulong id)
    {
        _readyDict.Remove(id);
        
        OnDataChanged?.Invoke(id, false);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void Ready_Rpc(ulong id, bool isReady)
    {
        _readyDict[id] = isReady;
        
        OnDataChanged?.Invoke(id, isReady);
        OnCountChanged?.Invoke(_readyDict.Values.Count(b => b), _mLobby.MemberCount - 1);
    }
}
