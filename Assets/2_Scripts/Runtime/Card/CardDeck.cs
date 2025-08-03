using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardDeck : MonoBehaviour, ICardDeck
{
    [Header("[ Init ]")]
    [SerializeField] private Card mPrefab;
    [SerializeField] private CardDeckData mDeckData;
    
    private List<CardData> _mCardDataList = new List<CardData>();

    #region > ICardDeck

    public ICardSpawner Spawner { get; private set; }
    
    public ICardShuffler Shuffler { get; private set; }

    public ICardStacker Stacker { get; private set; }

    public void Init()
    {
        if (!mPrefab)
        {
            throw new NullReferenceException("Card prefab is null");    
        }
        
        Spawner = new CardSpawner(transform, mPrefab);
        Stacker = new CardStacker();

        if (!mDeckData)
        {
            return;
        }
    }

    public List<CardData> Shuffle()
    {
        Shuffler = new CardShuffler();
        
        var dataList = new List<CardData>();
        foreach (CardDeckData.Option option in mDeckData.OptionList)
        {
            for (int i = 0; i < option.Count; i++)
            {
                dataList.Add(option.CardData);
            }   
        }
        
        return _mCardDataList = Shuffler.Shuffle(dataList);
    }

    public void Stack(Action onStack)
    {
        Stacker.Stack(Spawner, _mCardDataList, onStack);
    }
    
    #endregion
    
    #region > Unity

    private void Start()
    {
#if false
        Init();
#endif
    }

    private void Update()
    {
#if false
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Stack(null);
        }
#endif
    }

    #endregion
}