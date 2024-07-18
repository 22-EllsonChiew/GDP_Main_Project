using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class NeighbourUIController : MonoBehaviour
{
    public static NeighbourUIController instance;

    [SerializeField]
    private DialogueLoader dialogueLoader;

    [SerializeField]
    private GameObject playerObj;

    [SerializeField]
    private GameObject phoneUIGroup;

    public bool endInteraction = false; //New implement

    [Header("UI Elements - NPC")]
    [SerializeField]
    private GameObject neighbourUIGroup;
    [SerializeField]
    private TextMeshProUGUI neighbourUIName;
    [SerializeField]
    private TextMeshProUGUI neighbourUIDialogue;

    [Header("UI Elements - Player")]
    [SerializeField]
    private Button playerResponse1;
    [SerializeField]
    private Button playerResponse2;
    [SerializeField]
    private Button playerResponse3;


    private InteractionConversation currentConversation;
    private string currentNeighbourName;


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

        playerResponse2.onClick.AddListener(() => StartInteraction(currentNeighbourName, "Happy"));
        playerResponse3.onClick.AddListener(() => EndInteraction());

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartInteraction(string name, string type)
    {
        phoneUIGroup.SetActive(false);
        neighbourUIGroup.SetActive(true);

        PlayerMovement.dialogue = true;

        currentNeighbourName = name;

        var conversation = dialogueLoader.GetConversation(name, type);
        if (conversation != null)
        {
            currentConversation = conversation;
            var line = currentConversation.lines[0];
            neighbourUIName.text = line.speaker;
            neighbourUIDialogue.text = line.content;
        }
    }

    public void EndInteraction()
    {
        PlayerMovement.dialogue = false;

        phoneUIGroup.SetActive(true);
        neighbourUIGroup.SetActive(false);

        endInteraction = true;
        

    }

   

}
