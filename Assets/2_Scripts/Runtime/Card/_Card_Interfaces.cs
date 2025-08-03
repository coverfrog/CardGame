using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface ICard
{
    public Transform Tr { get; }
    
    IObjectPool<ICard> Pool { get; set; }
    
    ICard Init(CardData data);
}

public interface ICardDeck
{
    ICardSpawner Spawner { get; }

    ICardShuffler Shuffler { get; }
    
    ICardStacker Stacker { get; }
    
    void Init();
    
    void Stack(Action onStack);
}

public interface ICardSpawner
{
    ICard Get();
}

public interface ICardShuffler
{
    List<CardData> Shuffle(List<CardData> dataList);
}

public interface ICardStacker
{
    void Stack(ICardSpawner spawner, List<CardData> dataList, Action onStack);
}

public interface ICardSystem
{
    
}