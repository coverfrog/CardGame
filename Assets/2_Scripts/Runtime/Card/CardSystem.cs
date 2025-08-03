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
        // 초기 값 
        mDeck.Init();
        
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
        // - 카드에 대한 소환'만' 진행
        //      - 그 안에 정보에 대한 초기화는 클라 전부가 알아야 한다.
        // - 소환은 공용 객체로 동작
        // - 다만, 카드 안에 어떠한 데이터에 대한 동작은 클라이언트 전부가 알아야함
        
        if (!IsServer)
        {
            return;
        }

        mDeck.Spawn((ids, count) =>
        {
            // [25.08.03][cskim]
            // - 생성 되었던 codeNames 기반의 순서대로 Instance가 생성
            // - Instance 생성시 발생되는 NetworkObjectId 를 배열에 넣는다.

            Deck_Info_Set_Rpc(ids, codeNameBytes, count);
        });
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void Deck_Info_Set_Rpc(ulong[] ids, FixedString128Bytes[] codeNameBytes, int count)
    {
        // [25.08.04][cskim]
        // - 넙겨 받은 정보를 기반으로 모든 유저에게 상태를 최신화 시킨다.
        // - 초기화 자체는 각자 클라이언트에서 진행
        //      - 초기화가 클라이언트에서 되다보니 우려되는 사항들이 존재
        //      - 임의의 커스텀 카드 생성 여부
        
        string[] codeNames = codeNameBytes
            .Select(n => n.Value)
            .ToArray();
        
        for (int i = 0; i < count; i++)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(ids[i], out NetworkObject obj))
            {
                continue;
            }
            
            if (obj.TryGetComponent(out ICard card))
            {
                mDeck.LoadData(card, codeNames[i]);
            }
        }
    }
}
