using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string content;
}


[System.Serializable]
public class InteractionConversation
{
    public string name;
    public string type;
    public DialogueLine[] lines;
}

[System.Serializable]
public class MessageConversation 
{
    public string sender;
    public string type;
    public DialogueLine[] messages;
}


[System.Serializable]
public class DialogueData
{
    public InteractionConversation[] conversations;
}

[System.Serializable]
public class MessageData
{
    public MessageConversation[] conversations;
}
