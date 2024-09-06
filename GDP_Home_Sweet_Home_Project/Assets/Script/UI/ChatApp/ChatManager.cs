using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("Neighbour References")]
    [SerializeField]
    private Neighbour neighbourHakim;
    [SerializeField] 
    private Neighbour neighbourSherryl;


    [Header("Contact List UI")]
    [SerializeField]
    private Transform contactListParent;
    [SerializeField]
    private ContactPanel contactPrefab;

    [Header("Neighbour Text Data Loader")]
    [SerializeField]
    private MessageLoader messageLoader;

    [Header("Message Screen")]
    [SerializeField] 
    private TextMeshProUGUI currentContactName;
    [SerializeField]
    private Image currentContactPhoto;
    [SerializeField]
    private TextMeshProUGUI chatErrorMessage;
    [SerializeField]
    private Transform chatListParent;
    [SerializeField]
    private Button playerReplyBtn;

    [Header("Chat Bubble Prefabs")]
    [SerializeField]
    private MessagePanel npcResponsePrefab;
    [SerializeField]
    private MessagePanel playerResponsePrefab;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip sfx_ReceiveMessage;

    private List<PhoneContact> allPhoneContacts;
    private List<PhoneContact> unlockedPhoneContacts;

    public PhoneContact CurrentContact { get; private set; }
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

        playerReplyBtn.onClick.AddListener(() => SendReply());

        allPhoneContacts = new List<PhoneContact>()
        {
            new PhoneContact() {name = neighbourHakim.neighbourName, photo = neighbourHakim.neighbourImageSprite, isUnlocked = false, isAwaitingReply = false, receivedMessages = new DialogueLine[0]},
            new PhoneContact() {name = neighbourSherryl.neighbourName, photo = neighbourSherryl.neighbourImageSprite, isUnlocked = false, isAwaitingReply = false, receivedMessages = new DialogueLine[0]},
        };

        unlockedPhoneContacts = new List<PhoneContact>();

    }

    void Update()
    {

    }

    public void OpenMessages(string contactName)
    {
        PhoneContact targetContact = unlockedPhoneContacts.Find(contact => contact.name == contactName);

        if (targetContact != null) 
        {
            currentContactName.text = contactName;
            currentContactPhoto.sprite = targetContact.photo;
            CurrentContact = targetContact;

            RefreshCurrentMessages();
        }
    }

    public void RefreshCurrentMessages()
    {
        foreach (Transform child in chatListParent)
        {
            Destroy(child.gameObject);
        }

        if (CurrentContact.receivedMessages != null)
        {
            foreach (var message in CurrentContact.receivedMessages)
            {

                if (message.speaker == "Myself")
                {
                    MessagePanel newChatResponse = Instantiate(playerResponsePrefab, chatListParent);
                    newChatResponse.SetMessage(message.content);
                }
                else
                {
                    MessagePanel newChatResponse = Instantiate(npcResponsePrefab, chatListParent);
                    newChatResponse.SetMessage(message.content);
                }

            }
        }
        else
        {
            chatErrorMessage.text = "No messages with " + CurrentContact.name + "yet.";
            Debug.Log("No active chat with " + CurrentContact.name);
        }
    }

    public void ReceiveComplaint(string name, DialogueType type)
    {
        
        PhoneContact targetContact = unlockedPhoneContacts.Find(contact => contact.name == name);

        if (targetContact == null)
        {
            Debug.LogWarning("Unable to find targetContact");
            return;
        }

        if (targetContact.isUnlocked)
        {
            MessageConversation conversationToAdd = messageLoader.GetMessageConversation(name, type);

            if (conversationToAdd != null)
            {
                currentConversation = conversationToAdd;

                DialogueLine lineToAdd = conversationToAdd.messages[0];
                DialogueLine[] updatedMessages = new DialogueLine[targetContact.receivedMessages.Length + 1];
                updatedMessages[targetContact.receivedMessages.Length] = lineToAdd;
                targetContact.receivedMessages.CopyTo(updatedMessages, 0);

                targetContact.receivedMessages = updatedMessages;
                targetContact.isAwaitingReply = true;

                if(CurrentContact == targetContact)
                {
                    AudioManager.Instance.PlaySFX(sfx_ReceiveMessage);
                    RefreshCurrentMessages();
                }
                else
                {
                    PhoneUIController.instance.ReceiveChatNotification();
                }

                MoveContactToTop(targetContact);
                UpdateContactList();

                
            }
        }
        else
        {
            Debug.Log("Neighbour not unlocked");
        }
            
    }

    public void SendReply()
    {
        Debug.Log("Sending reply");

        if (CurrentContact == null)
        {
            Debug.LogWarning("No current contact. Is this intended?");
            return;
        }

        if (CurrentContact.isAwaitingReply)
        {
            AudioManager.Instance.PlaySFX(sfx_ReceiveMessage);
            Debug.Log("Reply by player sent");
            DialogueLine lineToAdd = currentConversation.messages[1];
            DialogueLine[] updatedMessages = new DialogueLine[CurrentContact.receivedMessages.Length + 1];
            updatedMessages[CurrentContact.receivedMessages.Length] = lineToAdd;
            CurrentContact.receivedMessages.CopyTo(updatedMessages, 0);

            CurrentContact.receivedMessages = updatedMessages;

            MakePromiseViaText();

            RefreshCurrentMessages();
            StartCoroutine(ReceiveNeighbourReply());
        }
    }

    void MakePromiseViaText()
    {
        if (CurrentContact.name == neighbourHakim.neighbourName) 
        {
            neighbourHakim.MakePromise();
        }
        else if (CurrentContact.name == neighbourSherryl.neighbourName)
        {
            neighbourSherryl.MakePromise();
        }
    }

    private IEnumerator ReceiveNeighbourReply()
    {
        yield return new WaitForSeconds(0.85f);

        DialogueLine lineToAdd = currentConversation.messages[2];
        DialogueLine[] updatedMessages = new DialogueLine[CurrentContact.receivedMessages.Length + 1];
        updatedMessages[CurrentContact.receivedMessages.Length] = lineToAdd;
        CurrentContact.receivedMessages.CopyTo(updatedMessages, 0);

        CurrentContact.receivedMessages = updatedMessages;

        CurrentContact.isAwaitingReply = false;

        UpdateContactList();
        AudioManager.Instance.PlaySFX(sfx_ReceiveMessage);
        RefreshCurrentMessages();
    }

    public bool PlayerRepliedToNeighbour(string name)
    {
        return CurrentContact != null && CurrentContact.name == name && !CurrentContact.isAwaitingReply;
    }

    public void UnlockContact(string contactName)
    {
        PhoneContact contactToUnlock = allPhoneContacts.Find(contact => contact.name == contactName);

        if (contactToUnlock != null && !contactToUnlock.isUnlocked) 
        {
            Debug.Log("ChatManager - Unlocked Neighbour Contact");
            contactToUnlock.isUnlocked = true;
            unlockedPhoneContacts.Add(contactToUnlock);
            AddContact(contactToUnlock);
        }
    }

    void MoveContactToTop(PhoneContact contact)
    {
        if (contact != null)
        {
            unlockedPhoneContacts.Remove(contact);
            unlockedPhoneContacts.Insert(0, contact);
        }
    }
    void UpdateContactList()
    {
        foreach (Transform child in contactListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (PhoneContact contact in unlockedPhoneContacts)
        {
            AddContact(contact);
        }
    }

    public void AddContact(PhoneContact contact)
    {
        ContactPanel newContactPanel = Instantiate(contactPrefab, contactListParent);
        if (contact.isAwaitingReply)
        {
            newContactPanel.DisplayUnreadNotification();
        }
        else
        {
            newContactPanel.ClearNotification();
            Debug.Log("clearing notification");
        }
        newContactPanel.SetContactName(contact.name);
        newContactPanel.SetContactImage(contact.photo);
        
    }

}
