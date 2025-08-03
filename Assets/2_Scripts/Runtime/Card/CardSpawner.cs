using UnityEngine;
using UnityEngine.Pool;

public class CardSpawner : ICardSpawner
{
    private readonly ICard _mCardPrefab;
    private readonly IObjectPool<ICard> _mPool;

    private readonly Transform _mParent;
    
    #region > Pool

    public CardSpawner(Transform parent, Card cardPrefab)
    {
        _mParent = parent;
        _mCardPrefab = cardPrefab;
        
        _mPool = new ObjectPool<ICard>(
            OnCreate,
            OnGet, 
            OnRelease, 
            OnDestroy, 
            true, 
            50, 
            100000);
    }

    private ICard OnCreate()
    {
        var card = Object.Instantiate((Object)_mCardPrefab, _mParent) as ICard;

        if (card != null)
        {
            card.Pool = _mPool;
        }
        
        return card;
    }

    private void OnGet(ICard card)
    {
        card.Tr.gameObject.SetActive(false);
    }

    private void OnRelease(ICard card)
    {
        card.Tr.gameObject.SetActive(false);
    }

    private void OnDestroy(ICard card)
    {
        
    }
    
    #endregion

    #region > ICardSpawner

    public ICard Get() => _mPool?.Get();

    #endregion
}