using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<AllItems> invItems = new List<AllItems>();

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
    public void AddItem(AllItems item)
    {
        if (!invItems.Contains(item))
        {
            invItems.Add(item);
        }
    }

    public void RemoveItem(AllItems item)
    {
        if (!invItems.Contains(item))
        {
            invItems.Remove(item);
        }
    }

    public enum AllItems
    {
        RubberCover,
        WoodenPlank,
        MetalPlate
    }
}
