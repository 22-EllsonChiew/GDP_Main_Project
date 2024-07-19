using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("Contact List UI")]
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
    [SerializeField]
    private TextMeshProUGUI errorMessage;
    [SerializeField]
    private Transform chatListParent;
    [SerializeField]
    private MessagePanel chatResponsePrefab;

    private List<PhoneContact> allPhoneContacts;
    private List<PhoneContact> unlockedPhoneContacts;
    private DialogueLine[] _receivedMessages;

    private MessageConversation currentConversation;

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
            new PhoneContact() {name = "Myself", photo = null, isUnlocked = false, receivedMessages = new DialogueLine[0]},
            new PhoneContact() {name = "Hakim", photo = null, isUnlocked = false, receivedMessages = new DialogueLine[0]},
            new PhoneContact() {name = "Sherryl", photo = null, isUnlocked = false, receivedMessages = new DialogueLine[0]},
            new PhoneContact() {name = "Mother", photo= null, isUnlocked = false, receivedMessages = new DialogueLine[0]},
        };

        unlockedPhoneContacts = new List<PhoneContact>();

        UnlockContact("Myself");
        UnlockContact("Mother");
        UnlockContact("Hakim");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ReceiveMessage("Hakim", "NormalComplaint");
        }
    }

    public void OpenMessages(string contactName)
    {
        PhoneContact targetContact = unlockedPhoneContacts.Find(contact => contact.name == contactName);

        if (targetContact != null) 
        {
            currentContactName.text = contactName;

            DisplayCurrentMessages(targetContact);
            
        }
    }

    public void DisplayCurrentMessages(PhoneContact currentContact)
    {
        foreach (Transform child in chatListParent)
        {
            Destroy(child.gameObject);
        }

        if (currentContact.receivedMessages != null)
        {
            foreach (var message in currentContact.receivedMessages)
            {
                MessagePanel newChatResponse = Instantiate(chatResponsePrefab, chatListParent);
                newChatResponse.SetMessage(message.content);
            }
        }
        else
        {
            errorMessage.text = "No messages with " + currentContact.name + "yet.";
            Debug.Log("No active chat with " + currentContact.name);
        }
    }

    public void ReceiveMessage(string name, string type)
    {
        PhoneContact targetContact = unlockedPhoneContacts.Find(contact => contact.name == name);
        MessageConversation conversationToAdd = messageLoader.GetMessageConversation(name, type);

        if (conversationToAdd != null)
        {
            DialogueLine[] linesToAdd = conversationToAdd.messages;
            DialogueLine[] updatedMessages = new DialogueLine[targetContact.receivedMessages.Length + linesToAdd.Length];
            targetContact.receivedMessages.CopyTo(updatedMessages, 0);
            linesToAdd.CopyTo(updatedMessages, targetContact.receivedMessages.Length);

            targetContact.receivedMessages = updatedMessages;
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
