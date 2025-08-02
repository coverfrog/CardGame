using Steamworks;
using Steamworks.Data;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomStart : MonoBehaviour
{
    [SerializeField] private Button mButton;
    [SerializeField] private TMP_Text mText;

    private const int MinMember = 2;
    
    public UIRoomStart Init()
    {
        return this;
    }

    public UIRoomStart Enter(IUIScene scene)
    {
        bool isHost = NetworkManager.Singleton.IsHost;

        mText.text = isHost ? "Start" : "Ready";

        mButton.interactable = !NetworkManager.Singleton.IsServer;
        mButton.onClick.RemoveAllListeners();
        mButton.onClick.AddListener(() =>
        {
            if (scene is not UISceneMainMenu mainMenu)
            {
                return;
            }
            
            if (NetworkManager.Singleton.IsServer)
            {
                mainMenu.GameStart();
            }
            else
            {
                mainMenu.Ready();
            }
        });
        
        return this;
    }
    
    //

    public void OnReadyCountChanged(int count, int memberCount)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        mButton.interactable = count >= MinMember && count >= memberCount;
    }
}
