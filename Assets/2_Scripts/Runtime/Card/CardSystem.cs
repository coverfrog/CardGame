using System.Collections.Generic;
using System.Linq;
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
        
        string[] cardCodeNames = mDeck.Shuffle().Select(d => d.CodeName).ToArray();
        
        mCardNames = cardCodeNames;
        
        Deck_Shuffle_Rpc(cardCodeNames);
    }
    
    [Rpc(SendTo.NotServer)]
    private void Deck_Shuffle_Rpc(string[] cardCodeNames)
    {
        mCardNames = cardCodeNames;
        
        Debug.Log("Call");
    }
}
