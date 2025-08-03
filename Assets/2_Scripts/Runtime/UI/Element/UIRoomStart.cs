using Steamworks;
using Steamworks.Data;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomStart : MonoBehaviour
{
    private const int MinMember = 2;

    [SerializeField] private Button mButton;
    [SerializeField] private TMP_Text mText;
    
    public UIRoomStart Init()
    {
        return this;
    }

    public UIRoomStart Enter(IUIScene scene)
    {
        bool isServer = NetworkManager.Singleton.IsServer;

        mText.text = isServer ? "Start" : "Ready";

#if false
        mButton.interactable = !isServer;
#else
        mButton.interactable = true;
#endif
        
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

    public void OnReadyCountChanged(int count)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }

        mButton.interactable = count >= MinMember && count >= SteamConnect.MemberCount;
    }
}
