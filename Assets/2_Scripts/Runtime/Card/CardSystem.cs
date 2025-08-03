using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class CardSystem : NetworkBehaviour, ICardSystem
{
    [Header("[ References ]")]
    [SerializeField] private CardDeck mDeck;

    [Header("[ Debug View ]")]
    [SerializeField] private string[] mCardNames;
    
    public override void OnNetworkSpawn()
    {
        // 호스트인 경우에만 
        if (!IsServer)
        {
            return;
        }
        
        // 로그
#if true
        Debug.Log("카드 시스템 시작");
#endif

        Shuffle();
    }

    private void Shuffle()
    {
        // [25.08.03][cskim]
        // - 덱 정보 초기화는 서버가 혼자 진행 해야함 ( 동일한 데이터여야 하므로 )
        // - 그 데이터를 다른 클라에서도 받을 수 있게끔 전송 
        
        var codeNameBytes = mDeck
            .Shuffle()
            .Select(d => new FixedString128Bytes(d.CodeName))
            .ToArray();
        
        Deck_Shuffle_Rpc(codeNameBytes);
    }
    //
    [Rpc(SendTo.ClientsAndHost)]
    private void Deck_Shuffle_Rpc(FixedString128Bytes[] codeNameBytes)
    {
        // [25.08.03][cskim]
        // - 갱신된 카드 덱의 정보 인덱스를 전파
        // - 각 값에 대한 갱신은 클라이언트에서 진행
        
        string[] codeNames = codeNameBytes
            .Select(n => n.Value)
            .ToArray();

        mDeck.OnShuffle(codeNames);
        
        // [25.08.03][cskim]
        // - 카드에 대한 소환 진행 
        
        if (!IsServer)
        {
            return;
        }
        
        mDeck.Spawn();
    }
}
