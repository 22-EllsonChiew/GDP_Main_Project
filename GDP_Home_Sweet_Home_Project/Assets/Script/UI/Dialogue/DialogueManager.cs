using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private int currentLineIndex;
    private Conversation currentConversation;

    private DialogueLoader loader;

    // Start is called before the first frame update
    void Start()
    {
        loader = GetComponent<DialogueLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartConversation(string name, string type)
    {
        var conversation = loader.GetConversation(name, type);
        if (conversation != null)
        {
            currentConversation = conversation;
            currentLineIndex = 0;
            NextLine();
        }
    }

    void NextLine()
    {
        if (currentLineIndex < currentConversation.lines.Length)
        {
            var line = currentConversation.lines[currentLineIndex];

        }
    }
}
