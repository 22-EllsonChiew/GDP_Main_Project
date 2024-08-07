using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum DialogueType
{
    Greet_Happy,
    Greet_Angry,
    Mood_Happy,
    Mood_Angry,
    Complaint_Normal,
    Complaint_Angry
}


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
    [JsonConverter(typeof(StringEnumConverter))]
    public DialogueType type;
    public DialogueLine[] lines;
}

[System.Serializable]
public class MessageConversation 
{
    public string sender;
    [JsonConverter(typeof(StringEnumConverter))]
    public DialogueType type;
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
