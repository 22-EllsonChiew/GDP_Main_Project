using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private Queue<string> sentences;

    public Text nameText;
    public Text DialogueText;

    public Animator animatorDialogue;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue (TutorialDialogue dialouge)
    {
        animatorDialogue.SetBool("IsOpen", true);

        nameText.text = dialouge.name;

        sentences.Clear();

        foreach (string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialouge();
            return;
        }

        string sentence = sentences.Dequeue();
        DialogueText.text = sentence;

    }

    void EndDialouge()
    {
        animatorDialogue.SetBool("IsOpen", false);
    }
}