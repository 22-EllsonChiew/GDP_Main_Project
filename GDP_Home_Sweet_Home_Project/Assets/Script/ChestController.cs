using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    public Button exitBtn;
    public Button rubberHammerCover;
    public Button clothLegSock;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void AddRubberHammerCover()
    {
        InventoryManager.Instance.AddItem(InventoryManager.InventoryManagerItems.RubberHammerCover);
        rubberHammerCover.interactable = false;
        Debug.Log("Hammer Added!");
    }

    public void AddClothLegSock()
    {
        InventoryManager.Instance.AddItem(InventoryManager.InventoryManagerItems.ClothLegSocks);
        clothLegSock.interactable = false;
        Debug.Log("Leg socks added!");
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
