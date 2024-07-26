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
    private GameObject mainUIGroup;

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

    [Header("Neighbour Models")]
    [SerializeField]
    private Transform hakimObj;
    [SerializeField]
    private Transform sherrylObj;


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

        playerResponse2.onClick.AddListener(() => ShowInteractionDialogue(currentNeighbourName, "Happy"));
        playerResponse3.onClick.AddListener(() => EndInteraction());

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartInteraction(string name, string type)
    {
        playerObj.SetActive(false);
        mainUIGroup.SetActive(false);
        neighbourUIGroup.SetActive(true);

        PlayerMovement.dialogue = true;

        currentNeighbourName = name;

        if (currentNeighbourName == "Sherryl")
        {
            DoorController.instance.ToggleSherrylDoor();
            sherrylObj.Translate(Vector3.forward * 5);
        }
        else if (currentNeighbourName == "Hakim")
        {
            DoorController.instance.ToggleHakimDoor();
            hakimObj.Translate(Vector3.forward * 5);
        }



        ShowInteractionDialogue(name, type);
        
    }

    public void ShowInteractionDialogue(string name, string type)
    {
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

        if (currentNeighbourName == "Sherryl")
        {
            DoorController.instance.ToggleSherrylDoor();
            sherrylObj.Translate(Vector3.forward * -5);
        }
        else if (currentNeighbourName == "Hakim")
        {
            DoorController.instance.ToggleHakimDoor();
            hakimObj.Translate(Vector3.forward * -5);
        }

        playerObj.SetActive(true);
        mainUIGroup.SetActive(true);
        neighbourUIGroup.SetActive(false);

        endInteraction = true;
        

    }

    public void ToggleHakimPosition()
    {

    }
   

}
