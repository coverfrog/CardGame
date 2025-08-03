using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class UIRoomPlayerView : MonoBehaviour
{
    [SerializeField] private UIRoomPlayer mPlayerOrigin;
    [SerializeField] private RectTransform mContent;

    private static readonly Dictionary<ulong, UIRoomPlayer> PlayerDict = new Dictionary<ulong, UIRoomPlayer>();
    
    public UIRoomPlayerView Init()
    {
        SteamMatchmaking.OnLobbyEntered            -= OnEntered;
        SteamMatchmaking.OnLobbyMemberJoined       -= OnMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave        -= OnMemberLeaved;
        SteamMatchmaking.OnLobbyMemberBanned       -= OnMemberBanned;
        SteamMatchmaking.OnLobbyMemberKicked       -= OnMemberKicked;
        SteamMatchmaking.OnLobbyMemberDataChanged  -= OnMemberDataChanged;
        SteamMatchmaking.OnLobbyMemberDisconnected -= OnMemberDisconnected;

        SteamMatchmaking.OnLobbyEntered            += OnEntered;
        SteamMatchmaking.OnLobbyMemberJoined       += OnMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave        += OnMemberLeaved;
        SteamMatchmaking.OnLobbyMemberBanned       += OnMemberBanned;
        SteamMatchmaking.OnLobbyMemberKicked       += OnMemberKicked;
        SteamMatchmaking.OnLobbyMemberDataChanged  += OnMemberDataChanged;
        SteamMatchmaking.OnLobbyMemberDisconnected += OnMemberDisconnected;
        
        return this;
    }

    public UIRoomPlayerView Enter()
    {
        gameObject.SetActive(true);

        return this;
    }
    
    public UIRoomPlayerView Exit()
    {
        gameObject.SetActive(false);
        
        // 종료 : 모든 플레이어들 전부 비 활성화
        foreach (UIRoomPlayer player in PlayerDict.Values)
        {
            _ = player.Exit();
        }
        
        return this;
    }
    
    private void OnEntered(Lobby lobby)
    {
        // 초기 : 플레이어 전원 
        foreach (Friend friend in GetFriends(lobby))
        {
            AddPlayer(lobby, friend);
        }
    }
    
    private void OnMemberJoined(Lobby lobby, Friend friend)
    {
        // 해당 플레이어만 추가
        AddPlayer(lobby, friend);
    }
    
    private void OnMemberLeaved(Lobby lobby, Friend friend)
    {
        // 해당 플레이어만 제거
        RemovePlayer(lobby, friend);
    }
    
    private void OnMemberDisconnected(Lobby lobby, Friend friend)
    {

    }
    
    private void OnMemberKicked(Lobby lobby, Friend friend, Friend friend2)
    {
       
    }

    private void OnMemberBanned(Lobby lobby, Friend friend, Friend friend2)
    {
        
    }

    private void OnMemberDataChanged(Lobby lobby, Friend friend)
    {
        
    }

    private static Friend[] GetFriends(Lobby lobby)
    {
        return lobby
            .Members.OrderBy(f => f.Id == lobby.Owner.Id)
            .ThenBy(f => f.Name)
            .ToArray();
    }

    private void AddPlayer(Lobby lobby, Friend friend)
    {
        if (PlayerDict.TryGetValue(friend.Id, out UIRoomPlayer player))
        {
            
        }

        else
        {
            player = Instantiate(mPlayerOrigin, mContent);
            PlayerDict.Add(friend.Id, player);
        }
        
        player.Enter(friend, lobby.Owner.Id == friend.Id);
    }

    private void RemovePlayer(Lobby lobby, Friend friend)
    {
        UIRoomPlayer player = PlayerDict[friend.Id];
        Destroy(player.gameObject);
        
        PlayerDict.Remove(friend.Id);
    }

    public bool OnReadyChanged(ulong steamId, bool ready)
    {
        if (!PlayerDict.TryGetValue(steamId, out UIRoomPlayer player))
        {
            return false;
        }

        return player.OnReadyChanged(ready);
    }
}
