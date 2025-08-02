using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardShuffler : ICardShuffler
{
    public List<CardData> Shuffle(List<CardData> dataList)
    {
        int count = dataList.Count;
        
        int[] indexes = Enumerable.Range(0, count).ToArray();

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < count; j++)
            {
                int r = Random.Range(0, count);
                
                (indexes[j], indexes[r]) = (indexes[r], indexes[j]);
            }
        }

        var result = new List<CardData>(count);
        
        for (int i = 0; i < count; i++)
        {
            result.Add(dataList[indexes[i]]);
        }
        
        return result;
    }
}
