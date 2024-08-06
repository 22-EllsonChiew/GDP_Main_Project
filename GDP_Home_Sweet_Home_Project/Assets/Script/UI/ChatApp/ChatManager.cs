using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private Button playerReplyBtn;

    [Header("Chat Bubble Prefabs")]
    [SerializeField]
    private MessagePanel npcResponsePrefab;
    [SerializeField]
    private MessagePanel playerResponsePrefab;

    private List<PhoneContact> allPhoneContacts;
    private List<PhoneContact> unlockedPhoneContacts;
    private DialogueLine[] _receivedMessages;

    private PhoneContact currentContact;
    private MessageConversation currentConversation;

    [Header("Neighbour")]

    public NeighbourAngerBar neighbourSherryl;
    public NeighbourAngerBar neighbourHakim;

    public float noiseThreshold = 40f;
    public float noiseThresholdSherryl = 40f;

    public float policeCallThresholdSherryl = 60f;
    public float policeCallThresholdHakim = 60f;
        
    public bool hakimSentedComplaint = false;
    public bool sherrylSentedComplaint = false;

    public bool hakimCallPolice = false;
    public bool sherrylCallPolice = false;

    

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
            new PhoneContact() {name = "Myself", photo = null, isUnlocked = false, isAwaitingReply = false, receivedMessages = new DialogueLine[0]},
            new PhoneContact() {name = "Hakim", photo = null, isUnlocked = false, isAwaitingReply = false, receivedMessages = new DialogueLine[0]},
            new PhoneContact() {name = "Sherryl", photo = null, isUnlocked = false, isAwaitingReply = false, receivedMessages = new DialogueLine[0]},
            new PhoneContact() {name = "Mother", photo= null, isUnlocked = false, isAwaitingReply = false, receivedMessages = new DialogueLine[0]},
        };

        unlockedPhoneContacts = new List<PhoneContact>();

        UnlockContact("Myself");
        UnlockContact("Mother");
        UnlockContact("Hakim");
        UnlockContact("Sherryl");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ReceiveComplaint("Hakim");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ReceiveComplaint("Sherryl");
        }
    }

    public void OpenMessages(string contactName)
    {
        PhoneContact targetContact = unlockedPhoneContacts.Find(contact => contact.name == contactName);

        if (targetContact != null) 
        {
            currentContactName.text = contactName;

            RefreshCurrentMessages(targetContact);
            
            currentContact = targetContact;
        }
    }

    public void RefreshCurrentMessages(PhoneContact currentContact)
    {
        foreach (Transform child in chatListParent)
        {
            Destroy(child.gameObject);
        }

        if (currentContact.receivedMessages != null)
        {
            foreach (var message in currentContact.receivedMessages)
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
            errorMessage.text = "No messages with " + currentContact.name + "yet.";
            Debug.Log("No active chat with " + currentContact.name);
        }
    }

    public void ReceiveComplaint(string name)
    {
        
        //PhoneContact targetContact = unlockedPhoneContacts.Find(contact => contact.name == name);
       
        currentContact = unlockedPhoneContacts.Find(contact => contact.name == name);

        if (currentContact.isUnlocked && currentContact != null)
        {
            MessageConversation conversationToAdd = messageLoader.GetMessageConversation(name, "NormalComplaint");

            if (conversationToAdd != null)
            {
                currentConversation = conversationToAdd;

                DialogueLine lineToAdd = conversationToAdd.messages[0];
                DialogueLine[] updatedMessages = new DialogueLine[currentContact.receivedMessages.Length + 1];
                updatedMessages[currentContact.receivedMessages.Length] = lineToAdd;
                currentContact.receivedMessages.CopyTo(updatedMessages, 0);

                currentContact.receivedMessages = updatedMessages;
                currentContact.isAwaitingReply = true;

                RefreshCurrentMessages(currentContact);
            }
        }
        else
        {
            Debug.Log("Disturbed neighbour angered but unable to contact player");
        }
            
    }

    public void CheckNeighbourHappinessValue()
    {
        if(neighbourHakim.currentHappiness < noiseThreshold && !hakimSentedComplaint)
        {
            Debug.Log($"Hakim's happiness: {neighbourHakim.currentHappiness}, threshold: {noiseThreshold}");
            ReceiveComplaint("Hakim");
            hakimSentedComplaint = true;
        }
        if(neighbourSherryl.currentHappiness < noiseThresholdSherryl && !sherrylSentedComplaint)
        {
            Debug.Log($"Sherryl's happiness: {neighbourSherryl.currentHappiness}, threshold: {noiseThresholdSherryl}");
            ReceiveComplaint("Sherryl");
            sherrylSentedComplaint = true;
        }
        if(neighbourSherryl.currentHappiness < policeCallThresholdSherryl && !sherrylCallPolice)
        {
            ReceiveComplaint("Sherryl");
            sherrylCallPolice = true;
        }
        if(neighbourHakim.currentHappiness < policeCallThresholdHakim && !hakimCallPolice)
        {
            ReceiveComplaint("Hakim");
            hakimCallPolice = true;
        }
    }

    public void ResetComplaint()
    {
        hakimSentedComplaint = false;
        sherrylSentedComplaint = false;
    }

    public void SendReply()
    {
        Debug.Log("Sending reply");

        if (currentContact == null)
        {
            Debug.LogWarning("No current contact. Is this intended?");
            return;
        }

        if (currentContact.isAwaitingReply)
        {
            Debug.Log("Reply by player sent");
            DialogueLine lineToAdd = currentConversation.messages[1];
            DialogueLine[] updatedMessages = new DialogueLine[currentContact.receivedMessages.Length + 1];
            updatedMessages[currentContact.receivedMessages.Length] = lineToAdd;
            currentContact.receivedMessages.CopyTo(updatedMessages, 0);

            currentContact.receivedMessages = updatedMessages;

            RefreshCurrentMessages(currentContact);
            StartCoroutine(ReceiveNeighbourReply());
        }
    }

    private IEnumerator ReceiveNeighbourReply()
    {
        yield return new WaitForSeconds(0.6f);

        DialogueLine lineToAdd = currentConversation.messages[2];
        DialogueLine[] updatedMessages = new DialogueLine[currentContact.receivedMessages.Length + 1];
        updatedMessages[currentContact.receivedMessages.Length] = lineToAdd;
        currentContact.receivedMessages.CopyTo(updatedMessages, 0);

        currentContact.receivedMessages = updatedMessages;

        currentContact.isAwaitingReply = false;
        RefreshCurrentMessages(currentContact);
    }

    public bool PlayerRepliedToNeighbour(string name)
    {
        return currentContact != null && currentContact.name == name && !currentContact.isAwaitingReply;
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
