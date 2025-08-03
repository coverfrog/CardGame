using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public interface ICard
{
    Transform Tr { get; }
    
    NetworkObject Network { get; }
    
    CardData Data { get; }
    
    IObjectPool<ICard> Pool { get; set; }
    
    ICard Init(CardData data);
}

public interface ICardDeck
{
    ICardSpawner Spawner { get; }

    ICardShuffler Shuffler { get; }
    
    ICardStacker Stacker { get; }
    
    void Init();
    //
    // void Stack(Action onStack);
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
    void Stack(ICard[] cards, Action onEnd);
}

public interface ICardSystem
{
    
}