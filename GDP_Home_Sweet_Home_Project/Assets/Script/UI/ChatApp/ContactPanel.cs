using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ContactPanel : MonoBehaviour
{

    public TextMeshProUGUI contactName;
    public Image contactPhoto;


    public void SetContactName(string _contactName)
    {
        contactName.text = _contactName;
    }

    public void SetContactImage(Sprite _contactPhoto)
    {
        contactPhoto.sprite = _contactPhoto;
    }

    public string GetContactName()
    {
        return contactName.text;
    }

    public Image GetContactPhoto() 
    { 
        return contactPhoto;
    }

    public void OnClick()
    {
        PhoneUIController.instance.OpenMessages();
        ChatManager.instance.OpenMessages(GetContactName());
    }
}
