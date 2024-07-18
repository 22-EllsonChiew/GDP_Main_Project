using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageLoader : MonoBehaviour
{
    public TextAsset messageJson;
    public MessageData messageData;

    // Start is called before the first frame update
    void Start()
    {
        LoadMessages();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadMessages()
    {
        messageData = JsonUtility.FromJson<MessageData>(messageJson.text);

        if (messageData != null )
        {
            int totalConversations = messageData.conversations.Length;
            
            Debug.Log("Total message conversations: " + totalConversations);
        }
    }

    public MessageConversation GetMessageConversation(string name, string type)
    {
        foreach (var conversation in messageData.conversations)
        {
            if (conversation.sender == name && conversation.type == type)
            {
                return conversation;
            }
        }

        Debug.LogWarning("Message conversation not found. Returning null.");
        return null;
    }
}
