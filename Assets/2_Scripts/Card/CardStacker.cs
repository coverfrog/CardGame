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
            
            ICard card = spawner
                .Get()
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
