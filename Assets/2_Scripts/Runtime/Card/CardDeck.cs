using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

public class CardDeck : MonoBehaviour, ICardDeck
{
    [Title("Init")]
    [SerializeField] private Card mPrefab;
    [SerializeField] private CardDeckData mDeckData;

    [Title("Debug View")]
    [ShowInInspector, ReadOnly] private List<CardData> _mCardDataList;

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
        
  
    }

    /// <summary>
    /// 서버 로컬에서만 진행
    /// </summary>
    /// <returns></returns>
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
        
        return Shuffler.Shuffle(dataList);
    }

    /// <summary>
    /// 서버 로컬에서 진행된 결과를 기반으로 데이터를 다시 적용
    /// </summary>
    public void OnShuffle(string[] codeNames)
    {
        // [25.08.03][cskim]
        // - 기존 덱 데이터의 값들에서 고유 키만 추출
        // - 해당 정보들을 기반으로 데이터 초기화

        _mCardDataList = new List<CardData>(codeNames.Length);
        
        var dictionary = mDeckData
            .OptionList
            .Select(o => o.CardData)
            .ToDictionary(d => d.CodeName, d => d);

        foreach (string codeName in codeNames)
        {
            _mCardDataList.Add(dictionary[codeName]);
        }
    }

    public void Stack(Action onStack)
    {
        Spawner = new CardSpawner(transform, mPrefab);
        Stacker = new CardStacker();
        
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