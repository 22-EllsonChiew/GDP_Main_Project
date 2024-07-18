using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    private Transform contactListParent;
    [SerializeField]
    private ContactPanel contactPrefab;

    [SerializeField]
    private MessageLoader messageLoader;

    [Header("Message Screen")]
    [SerializeField] 
    private TextMeshProUGUI currentContactName;
    [SerializeField]
    private Image currentContactPhoto;

    private List<PhoneContact> allPhoneContacts;
    private List<PhoneContact> unlockedPhoneContacts;

    private List<Message> currentConversation;

    public static ChatManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        allPhoneContacts = new List<PhoneContact>()
        {
            new PhoneContact() {name = "Myself", photo = null, isUnlocked = false},
            new PhoneContact() {name = "Hakim", photo = null, isUnlocked = false},
            new PhoneContact() {name = "Sherryl", photo = null, isUnlocked = false},
            new PhoneContact() {name = "Mother", photo= null, isUnlocked = false},
        };

        unlockedPhoneContacts = new List<PhoneContact>();

        UnlockContact("Myself");
        UnlockContact("Mother");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenMessages(string contactName)
    {
        PhoneContact targetContact = unlockedPhoneContacts.Find(contact => contact.name == contactName);

        if (targetContact != null) 
        {
            currentContactName.text = contactName;
            
            // set message data 
            // instantiate message box prefabs
        }
    }


    public void UnlockContact(string contactName)
    {
        PhoneContact contactToUnlock = allPhoneContacts.Find(contact => contact.name == contactName);

        if (contactToUnlock != null && !contactToUnlock.isUnlocked) 
        {
            contactToUnlock.isUnlocked = true;
            unlockedPhoneContacts.Add(contactToUnlock);
            AddContact(contactToUnlock);
        }
    }

    public void AddContact(PhoneContact contact)
    {
        ContactPanel newContactPanel = Instantiate(contactPrefab, contactListParent);
        newContactPanel.SetContactName(contact.name);
        newContactPanel.SetContactImage(contact.photo);
    }

}
