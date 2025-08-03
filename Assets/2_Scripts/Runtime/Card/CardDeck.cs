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

    private Dictionary<string, CardData> _mCardDataDictionary;
    
    #region > ICardDeck

    public ICardSpawner Spawner { get; private set; }
    
    public ICardShuffler Shuffler { get; private set; }

    public ICardStacker Stacker { get; private set; }

    /// <summary>
    /// 초기화
    /// </summary>
    public void Init()
    {
        _mCardDataDictionary = mDeckData
            .OptionList
            .Select(o => o.CardData)
            .ToDictionary(d => d.CodeName, d => d);
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
        
        foreach (string codeName in codeNames)
        {
            _mCardDataList.Add(_mCardDataDictionary[codeName]);
        }
    }

    /// <summary>
    /// 소환 ( Server Only )
    /// </summary>
    public void Spawn(Action<ulong[], int> onSpawn)
    {
        // [25.08.03][cskim]
        // - 소환하는 구간
        // - 소환'만' 진행 할 것 

        Spawner = new CardSpawner(transform, mPrefab);
        
        int count = _mCardDataList.Count;
        
        ulong[] result = new ulong[count];
        
        for (int i = 0; i < count; i++)
        {
            ICard card = Spawner.Get();
            card.Network.Spawn();
            
            result[i] = card.Network.NetworkObjectId;
        }
        
        onSpawn?.Invoke(result, count);
    }

    public void LoadData(ICard[] cards, string[] codeNames, int count)
    {
        // [25.08.04][cskim]
        // - 시작할 때 만들어놓은 Data 사전에서 정보 얻어서 로딩
        // - 객체 초기화는 데이터만 던져주고 객체 안에서 초기화

        for (int i = 0; i < count; i++)
        {
            if (!_mCardDataDictionary.TryGetValue(codeNames[i], out CardData data))
            {
                throw new Exception("Data not loaded");
            }
            
            _ = cards[i].Init(data);
        }
    }

    /// <summary>
    /// 카드 쌓기 시작
    /// </summary>
    public void Stack(ICard[] cards, Action onEnd)
    {
        Stacker = new CardStacker();
        Stacker.Stack(cards, onEnd);
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