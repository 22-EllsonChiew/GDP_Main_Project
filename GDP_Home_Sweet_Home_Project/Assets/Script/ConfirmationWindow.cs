using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationWindow : MonoBehaviour
{
    public Button confirmButton;
    public Button exitButton;


    public TextMeshProUGUI furnitureNameField;
    public TextMeshProUGUI furnitureTypeField;
    public TextMeshProUGUI assemblyRequiredField;
    public Image furniturePhotoArea;
    public Image assemblyToolPhotoArea;
    public Image comicStripArea;


    private void Update()
    {
        if (TimeController.instance.isPaused)
        {
            confirmButton.enabled = false;
        }
        else
        {
            confirmButton.enabled = true;
        }
    }

    public void SetFurnitureDetails(Package packageData)
    {
        // set ui text fields using package data
        furnitureNameField.text = packageData.furnitureName;
        furnitureTypeField.text = packageData.GetFurnitureTypeAsString();
        furniturePhotoArea.sprite = packageData.furniturePhoto;
        SetAssemblyBool(packageData.isAssemblyRequired);
        assemblyToolPhotoArea.sprite = packageData.toolRequired;
        comicStripArea.sprite = packageData.comicStrip;

    }

    public void SetAssemblyBool (bool isAssemblyRequired)
    {
        if (isAssemblyRequired)
        {
            assemblyRequiredField.text = "Yes";
        }
        else
        {
            assemblyRequiredField.text = "No";
        }
    }

}

