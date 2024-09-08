using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private Queue<string> sentences;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI DialogueText;

    public GameObject nextSceneTrigger;
    public GameObject leftBlocker;
    public GameObject rightBlocker;

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
            nextSceneTrigger.SetActive(true);
            return;
        }
        
        if(sentences.Count == 9)
        {
            leftBlocker.SetActive(false);
            rightBlocker.SetActive(false);
        }

        string sentence = sentences.Dequeue();
        DialogueText.text = sentence;

    }

    void EndDialouge()
    {
        animatorDialogue.SetBool("IsOpen", false);
    }
}
