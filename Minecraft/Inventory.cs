using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string blockType)
    {
        if (items.ContainsKey(blockType))
        {
            items[blockType]++;
        }
        else
        {
            items[blockType] = 1;
        }

        Debug.Log($"Bloco {blockType} adicionado. Total: {items[blockType]}");
    }

    public int GetItemCount(string blockType)
    {
        return items.ContainsKey(blockType) ? items[blockType] : 0;
    }
}
