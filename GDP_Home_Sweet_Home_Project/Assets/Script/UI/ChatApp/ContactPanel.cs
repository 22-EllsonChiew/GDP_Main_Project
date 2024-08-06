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
    public GameObject notificationIcon;

    private void Start()
    {
        notificationIcon.SetActive(false);
    }

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
        if (notificationIcon.gameObject.activeSelf)
        {
            notificationIcon.gameObject.SetActive(false);
        }

        PhoneUIController.instance.OpenMessages();
        ChatManager.instance.OpenMessages(GetContactName());
    }

    public void ReceiveNotification()
    {
        notificationIcon.SetActive(true);
    }
}
