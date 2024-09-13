using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DialogueLoader : MonoBehaviour
{
    public TextAsset npcInteractionsJson;
    public DialogueData dialogueData;


    // Start is called before the first frame update
    void Start()
    {
        LoadDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadDialogue()
    {

        dialogueData = JsonConvert.DeserializeObject<DialogueData>(npcInteractionsJson.text);

        if (dialogueData != null)
        {
            int totalConversations = dialogueData.conversations.Length;
            int totalLines = 0;

            foreach (var conversation in dialogueData.conversations)
            {
                totalLines += conversation.lines.Length;
            }

            Debug.Log("Total conversations: " + totalConversations);
            Debug.Log("Total lines: " + totalLines);
        }

    }

    public InteractionConversation GetConversation (string name, DialogueType type)
    {
        foreach (var conversation in dialogueData.conversations)
        {
            if (conversation.name == name && conversation.type == type)
            {
                return conversation;
            }
        }

        Debug.LogWarning("Conversation not found. Returning null.");
        return null;
    }
}
