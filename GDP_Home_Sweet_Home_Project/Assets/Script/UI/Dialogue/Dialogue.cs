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
public class Conversation
{
    public string name;
    public string type;
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueData
{
    public Conversation[] conversations;
}
