using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    public Button exitBtn;
    public Button rubberHammer;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void AddHammer()
    {
        InventoryManager.Instance.AddItem(InventoryManager.AllItems.RubberCover);
        rubberHammer.interactable = false;
        Debug.Log("Hammer Added!");
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
