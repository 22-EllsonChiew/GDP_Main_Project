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
    //[SerializeField]
    //private TextMeshProUGUI playerResponse3Text;


    [Header("Neighbour Reference")]
    [SerializeField]
    private Neighbour neighbourHakim;
    [SerializeField]
    private Neighbour neighbourSherryl;

    private InteractionConversation currentConversation;

    private bool isInteractionUIActive;

    private float neighbourGreeting_MoveDistance = 5f;
    private float neighbourGreeting_MoveDuration = 0.5f;
    private float neighbourGreeting_MoveDelay = 1.25f;
    private bool isNeighbourGreetingPlayer = false;


    // Start is called before the first frame update
    void Start()
    {
        isInteractionUIActive = false;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerResponse2.onClick.AddListener(() => ShowInteractionDialogue(Interaction.currentNeighbour.neighbourName, Interaction.currentNeighbour.currentMood));
        playerResponse3.onClick.AddListener(() => EndInteraction());

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void StartInteraction(string name, DialogueType type)
    {

        PlayerMovement.dialogue = true;
        
        if (!Interaction.currentNeighbour.IsNeighbourInRoutine)
        {
            isNeighbourGreetingPlayer = true;
            HandleInteractionAnimations();
        }
        
        ToggleInteractionUI();

        if (type == DialogueType.Mood_Happy)
        {
            type = DialogueType.Greet_Happy;
        }
        else if (type == DialogueType.Mood_Normal)
        {
            type = DialogueType.Greet_Normal;
        }
        else
        {
            type = DialogueType.Greet_Angry;
        }

        ShowInteractionDialogue(name, type);
        
    }

    public void ShowInteractionDialogue(string name, DialogueType type)
    {

        if (Interaction.currentNeighbour.IsNeighbourInRoutine)
        {
            neighbourUIName.text = Interaction.currentNeighbour.neighbourName;
            neighbourUIDialogue.text = "Neighbour is currently busy";
            playerResponse2.interactable = false;
        }
        else
        {
            var conversation = dialogueLoader.GetConversation(name, type);
            if (conversation != null)
            {
                playerResponse2.interactable = true;
                currentConversation = conversation;
                var line = currentConversation.lines[0];
                neighbourUIName.text = line.speaker;
                neighbourUIDialogue.text = line.content;
            }
        }
        
    }

    public void EndInteraction()
    {
        PlayerMovement.dialogue = false;

        if (!Interaction.currentNeighbour.IsNeighbourInRoutine)
        {
            isNeighbourGreetingPlayer = false;
            HandleInteractionAnimations();
            endInteraction = true;
        }

        ToggleInteractionUI();

        endInteraction = true;
        

    }

    private void HandleInteractionAnimations()
    {
        if (Interaction.currentNeighbour.neighbourName == neighbourHakim.neighbourName)
        {
            DoorController.instance.ToggleHakimDoor();
        }
        else if (Interaction.currentNeighbour.neighbourName == neighbourSherryl.neighbourName)
        {
            DoorController.instance.ToggleSherrylDoor();
        }

        StartCoroutine(MoveNeighbour());
    }

    IEnumerator MoveNeighbour()
    {
        Transform neighbourTransform = Interaction.currentNeighbour.neighbourTransform;
        float moveDirection = neighbourGreeting_MoveDistance;
        playerResponse3.interactable = false;

        if (isNeighbourGreetingPlayer)
        {
            yield return new WaitForSeconds(neighbourGreeting_MoveDelay);
            moveDirection *= -1;
        }
        else
        {
            moveDirection *= 1;
        }

        Vector3 startPos = neighbourTransform.position;
        Vector3 endPos = neighbourTransform.position + transform.forward * moveDirection;

        float elapsedTime = 0;

        while (elapsedTime < neighbourGreeting_MoveDuration)
        {
            
            neighbourTransform.position = Vector3.Lerp(startPos, endPos, elapsedTime / neighbourGreeting_MoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        neighbourTransform.position = endPos;
        playerResponse3.interactable = true;
    }

    private void ToggleInteractionUI()
    {
        isInteractionUIActive = !isInteractionUIActive;
        if (isInteractionUIActive)
        {
            neighbourUIGroup.SetActive(true);
            playerObj.SetActive(false);
            mainUIGroup.SetActive(false);
        }
        else
        {
            neighbourUIGroup.SetActive(false);
            playerObj.SetActive(true);
            mainUIGroup.SetActive(true);
        }
    }

}
