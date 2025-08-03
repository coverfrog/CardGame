using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomJoin : MonoBehaviour, IUIModule
{
    [SerializeField] private Button mExitButton;
    [SerializeField] private Button mJoinButton;
    [Space] 
    [SerializeField] private TMP_InputField mInputField;
    
    public IUIModule Enter(IUIScene scene)
    {
        gameObject.SetActive(true);

        SetInteractable(true);
        
        return this;
    }

    public IUIModule Exit(IUIScene scene)
    {
        gameObject.SetActive(false);
        
        return this;
    }

    private void SetInteractable(bool value)
    {
        mInputField.interactable = value;
        
        mExitButton.interactable = value;
        mJoinButton.interactable = value;
    }
    
    public IUIModule Init(IUIScene scene)
    {
        if (scene is not UISceneMainMenu mainMenu) return this;
        
        mInputField.onValueChanged.AddListener(OnEdit);
        
        mExitButton.interactable = true;
        mExitButton.onClick.AddListener(() =>
        {
            SetInteractable(false);
        });
    
        mJoinButton.interactable = true;
        mJoinButton.onClick.AddListener(() =>
        {
            SetInteractable(false);

            string text = mInputField.text;
            
            if (string.IsNullOrEmpty(text))
            {
                mainMenu.ShowToast("Input Room Id");
            }
            
            else if (!Regex.IsMatch(text, @"^\d+$"))
            {
                mainMenu.ShowToast("Input Only Numbers");
            }
            
            else if (!ulong.TryParse(text, out ulong id))
            {
                mainMenu.ShowToast("Check Id");
            }

            else
            {
                mainMenu.JoinWithId(id);
            }
        });
        
        return this;
    }
    
    private void OnEdit(string str)
    {
        string trim = str.Replace(" ", "");

        if (Equals(trim, str))
        {
            return;
        }

        mInputField.text = str;
    }
}
