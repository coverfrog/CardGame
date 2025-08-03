using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine;

public class UIRoomId : MonoBehaviour
{
    [SerializeField] private TMP_Text mId;
    
    public UIRoomId Init()
    {
        SteamMatchmaking.OnLobbyEntered -= OnEntered;
        
        SteamMatchmaking.OnLobbyEntered += OnEntered;
        
        return this;
    }
    
    private void OnEntered(Lobby lobby)
    {
        // 초기 : Id 
        mId.text = lobby.Id.ToString();
    }
}
