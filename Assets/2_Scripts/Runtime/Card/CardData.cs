using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(menuName = "Cf/Card/Data", fileName = "Card")]
public class CardData : ScriptableObject, INetworkSerializable
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

    private byte[] _mFrontTextureBytes;
    
    public Texture2D FrontTexture => mFrontTexture;
    
    //
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref mCodeName);
        serializer.SerializeValue(ref mDisplayName);
        serializer.SerializeValue(ref mDescription);
        
        serializer.SerializeValue(ref _mFrontTextureBytes);

        if (serializer.IsReader)
        {
            if (_mFrontTextureBytes != null)
            {
                mFrontTexture = new Texture2D(2, 2);
                mFrontTexture.LoadImage(_mFrontTextureBytes);
            }
        }

        if (serializer.IsWriter)
        {
            _mFrontTextureBytes = mFrontTexture.EncodeToPNG();
        }
    }
}
