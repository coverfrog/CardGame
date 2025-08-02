using UnityEngine;
using UnityEngine.UI;

public class UIRoomExit : MonoBehaviour
{
    [SerializeField] private Button mButton;
    
    public UIRoomExit Init(IUIScene scene)
    {
        mButton.onClick.RemoveAllListeners();
        mButton.onClick.AddListener(() =>
        {
            if (scene is not UISceneMainMenu mainMenu) return;
            
            mainMenu.Leave();
        });
        
        return this;
    }
}