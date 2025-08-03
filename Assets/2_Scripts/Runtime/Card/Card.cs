using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;
using ReadOnly = Sirenix.OdinInspector.ReadOnlyAttribute;

public class Card : NetworkBehaviour, ICard
{
    [Header("[ References ]")]
    [SerializeField] private NetworkObject mNetworkObject;
    [SerializeField] private MeshRenderer mRender;
    [SerializeField] private Shader mShader;

    [Header("[ Debug View ]")]
    [SerializeField] private string mCodeNameString;
    
    private readonly NetworkVariable<int> _mIndex =  new NetworkVariable<int>();
    private readonly NetworkVariable<FixedString128Bytes> _mCodeName = new NetworkVariable<FixedString128Bytes>();
    
    
    #region Key

    private static readonly int FrontTexture = Shader.PropertyToID("_Front_Texture");

    #endregion

    [ContextMenu("> Context : Report")]
    public void Report()
    {
        Debug.Log(Data?.CodeName);
    }
    
    public Transform Tr => transform;

    public NetworkObject Network => mNetworkObject;
    
    public CardData Data { get; private set; }

    public IObjectPool<ICard> Pool { get; set; }
    
    
    /// <summary>
    /// 로컬에서 초기화 시키는 단계
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public ICard Init(CardData data)
    {
        Data = data;

        gameObject.name = Data.DisplayName;
        
        _mCodeName.OnValueChanged = OnCodeNameChanged;
        _mCodeName.Value = data.CodeName;
        
        mRender.sharedMaterial = new Material(mShader);
        mRender.sharedMaterial.SetTexture(FrontTexture, data?.FrontTexture);
        
        return this;
    }

    private void OnCodeNameChanged(FixedString128Bytes prev, FixedString128Bytes now)
    {
        mCodeNameString = now.Value;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        
        Debug.Log(_mCodeName?.Value.Value);
        
        mCodeNameString = _mCodeName?.Value.Value;
    }
}
