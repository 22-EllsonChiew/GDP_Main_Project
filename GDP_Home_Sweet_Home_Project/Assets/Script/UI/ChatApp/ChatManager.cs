using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    private Transform contactListParent;
    [SerializeField]
    private ContactPanel contactPrefab;

    [SerializeField]
    private GameObject contactScreen;
    [SerializeField]
    private GameObject messageScreen;

    private List<PhoneContact> allPhoneContacts;
    private List<PhoneContact> unlockedPhoneContacts;


    // Start is called before the first frame update
    void Start()
    {
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
