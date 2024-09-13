using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<InventoryManagerItems> invItems = new List<InventoryManagerItems>();

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(InventoryManagerItems item)
    {
        if (!invItems.Contains(item) && item != InventoryManagerItems.Tool)
        {
            Debug.Log("Added " + item.ToString());
            invItems.Add(item);
        }
    }

    public void RemoveItem(InventoryManagerItems item)
    {
        if (!invItems.Contains(item))
        {
            invItems.Remove(item);
        }
    }

    public enum InventoryManagerItems
    {
        RubberHammerCover,
        ClothLegSocks,
        Tool,
    }
}
