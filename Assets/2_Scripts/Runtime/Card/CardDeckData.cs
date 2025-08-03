using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cf/Card/Deck/Data", fileName = "Deck")]
public class CardDeckData : ScriptableObject
{
    [Serializable]
    public class Option
    {
        [SerializeField] private CardData mCardData;
        [SerializeField] private int mCount = 1;
        
        public CardData CardData => mCardData;
        public int Count => mCount;
    }
    
    [SerializeField] private List<Option> mOptionList =  new List<Option>();
    
    public IReadOnlyList<Option> OptionList => mOptionList;
}
