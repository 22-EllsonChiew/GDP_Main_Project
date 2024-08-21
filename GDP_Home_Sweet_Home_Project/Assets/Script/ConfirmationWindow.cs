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

    public void SetFurnitureName(string furnitureName)
    {
        furnitureNameField.text = furnitureName;
    }

    public void SetFurnitureType(string furnitureType)
    {
        furnitureTypeField.text = furnitureType;
    }

    public void SetFurniturePhoto(Sprite imageSprite)
    {
        furniturePhotoArea.sprite = imageSprite;
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
    
    public void SetToolRequired(Sprite toolRequired)
    {
        assemblyToolPhotoArea.sprite = toolRequired;
    }

    public void SetManualTips(Sprite comicStrip)
    {
        comicStripArea.sprite = comicStrip;
    }

}

