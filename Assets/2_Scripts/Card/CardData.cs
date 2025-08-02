using UnityEngine;

[CreateAssetMenu(menuName = "Cf/Card/Data", fileName = "Card")]
public class CardData : ScriptableObject
{
    [Header("[ Text ]")]
    [SerializeField] private string mCodeName;
    [SerializeField] private string mDisplayName;
    [SerializeField][TextArea] private string mDescription;
    
    public string CodeName => mCodeName;
    public string DisplayName => mDisplayName;
    public string Description => mDescription;
    
    [Header("[ Texture ]")]
    [SerializeField] private Texture2D mFrontTexture;
    
    public Texture2D FrontTexture => mFrontTexture;
}
