using Unity.Netcode;
using UnityEngine;

public class CardSystem : NetworkBehaviour, ICardSystem
{
    [Header("[ References ]")]
    [SerializeField] private CardDeck mDeck;

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
        
        // 덱 초기화
        // [25.08.03][cskim]
        // - 클라에서도 '동일하게' 덱 순서 초기화 되는지 확인
        // - 테스트 종료 후 삭제 요망
        mDeck.Init();
        
        // 댁 스폰
        // [25.08.03][cskim]
        // - Network Behaviour 안 써도 무방한지 검증 용도
        // - 테스트 종료 후 삭제 요망
        mDeck.Stack(null);
    }
}
