using UnityEngine;
using UnityEngine.Pool;

public class Card : MonoBehaviour, ICard
{
    [SerializeField] private MeshRenderer mRender;
    [SerializeField] private Shader mShader;

    #region Key

    private static readonly int FrontTexture = Shader.PropertyToID("_Front_Texture");

    #endregion
    
    public Transform Tr => transform;
    
    public IObjectPool<ICard> Pool { get; set; }
    
    public ICard Init(CardData data)
    {
        mRender.sharedMaterial = new Material(mShader);
        mRender.sharedMaterial.SetTexture(FrontTexture, data?.FrontTexture);
        
        return this;
    }
}
