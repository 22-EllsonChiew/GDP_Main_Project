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

    private float neighbourGreeting_MoveDuration = 0.5f;
    private float neighbourGreeting_MoveDelay = 1.25f;
    private bool isNeighbourGreetingPlayer = false;


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

        PlayerMovement.dialogue = true;

        currentNeighbourName = name;

        isNeighbourGreetingPlayer = true;

        if (currentNeighbourName == "Sherryl")
        {
            DoorController.instance.ToggleSherrylDoor();
            StartCoroutine(MoveNeighbour(sherrylObj, -5));
        }
        else if (currentNeighbourName == "Hakim")
        {
            DoorController.instance.ToggleHakimDoor();
            StartCoroutine(MoveNeighbour(hakimObj, -5));
        }

        neighbourUIGroup.SetActive(true);

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
        isNeighbourGreetingPlayer = false;

        if (currentNeighbourName == "Sherryl")
        {
            DoorController.instance.ToggleSherrylDoor();
            StartCoroutine(MoveNeighbour(sherrylObj, 5));
        }
        else if (currentNeighbourName == "Hakim")
        {
            DoorController.instance.ToggleHakimDoor();
            StartCoroutine(MoveNeighbour(hakimObj, 5));
        }

        playerObj.SetActive(true);
        mainUIGroup.SetActive(true);
        neighbourUIGroup.SetActive(false);

        endInteraction = true;
        

    }

    IEnumerator MoveNeighbour(Transform neighbourTransform, float dir)
    {
        if (isNeighbourGreetingPlayer)
        {
            yield return new WaitForSeconds(neighbourGreeting_MoveDelay);
        }

        Vector3 startPos = neighbourTransform.position;
        Vector3 endPos = neighbourTransform.position + transform.forward * dir;

        float elapsedTime = 0;

        while (elapsedTime < neighbourGreeting_MoveDuration)
        {
            neighbourTransform.position = Vector3.Lerp(startPos, endPos, elapsedTime / neighbourGreeting_MoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        neighbourTransform.position = endPos;
    }

}
