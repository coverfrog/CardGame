using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardStacker : ICardStacker
{
    public void Stack(ICardSpawner spawner, List<CardData> dataList, Action onStack)
    {
        const float unitHeight = 0.02f;
        const float topHeight = unitHeight * 100.0f;
        
        const float duration = 0.05f;
        const float delay = duration * 0.5f;
        
        int count = 0;
        
        for (int i = 0; i < dataList.Count; i++)
        {
            CardData data = dataList[i];
            
            // [25.08.03][cskim]
            // - 카드 소환은 Spawner 에서 객체 관리
            // - 초기화 시키는 과정과 실제로 움직이는 부분 부터는 클라이언트도 같이 진행 되어야 한다.
            // - 다음 작업을 Rpc 호출이 필요
            //      - 카드 초기화 
            //      - 카드 이동 관련 ( Netcode 위치 동기화로 사용 하여도 됨 )
            
            ICard card = spawner
                .Get(null)
                .Init(data);
            
            float height = unitHeight * i;

            Transform tr = card.Tr; 
            tr.gameObject.name = data.DisplayName;
            tr.localPosition = Vector3.up * topHeight;
            tr.eulerAngles = new Vector3(0, 0, 180);
            tr.DOLocalMoveY(height, duration)
                .SetEase(Ease.OutCubic)
                .SetDelay(delay * i)
                .OnStart(() =>
                {
                    tr.gameObject.SetActive(true);
                })
                .OnComplete(() =>
                {
                    ++count;

                    if (count < dataList.Count)
                    {
                        return;
                    }
                    
                    onStack?.Invoke();
                });
        }
    }
}
