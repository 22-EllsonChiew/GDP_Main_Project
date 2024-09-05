using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxItem : MonoBehaviour
{
    public GameObject hintPanel;
    public InventoryManager.InventoryManagerItems itemType;

    // Start is called before the first frame update
    void Start()
    {
        hintPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem()
    {
        InventoryManager.Instance.AddItem(itemType);
    }

    public void EnableHintPanel()
    {
        hintPanel.SetActive(true);
    }

    public void DisableHintPanel()
    {
        hintPanel.SetActive(false);
    }


}
