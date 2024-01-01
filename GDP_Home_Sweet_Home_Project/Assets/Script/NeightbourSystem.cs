using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NeightbourSystem : MonoBehaviour
{
    public GameObject d_template;
    public GameObject canva;
    bool player_detection = false;


    public Button optionAButton;
    public Button optionBButton;
    bool inDialogue = false;

    // Start is called before the first frame update
    void Start()
    {
        optionAButton.onClick.AddListener(OptionASelected);
        optionBButton.onClick.AddListener(OptionBSelected);
    }

    // Update is called once per frame
    void Update()
    {
        if(player_detection && Input.GetKeyDown(KeyCode.F) && !PlayerMovement.dialogue)
        {
            canva.SetActive(true);
            /*PlayerMovement.dialogue = true;
            NewDialogue("Pls dont make too much noise");
            NewDialogue("If not i will report you");
            canva.transform.GetChild(1).gameObject.SetActive(true);*/

            inDialogue = true;
            //canva.SetActive(true);
            PlayerMovement.dialogue = true;

            Debug.Log("Starting dialogue");

            // Display initial dialogue with options
            NewDialogue("Hey there! Just make want sure you're not too noisy brethren.");

            // Wait for the player to make a choice through the UI buttons
            //yield return StartCoroutine(WaitForButtonPress());

            // Process the choice
            if (optionAFlag)
            {
                // Player chose option A
                NewDialogue("Sick blud");
                Debug.Log("Choice made A");
            }
            else if (optionBFlag)
            {
                // Player chose option B
                canva.SetActive(false);
                Debug.Log("Choice made B");
                PlayerMovement.dialogue = false;
            }

            // Reset the choice flags
            optionAFlag = false;
            optionBFlag = false;

            inDialogue = false;
            canva.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void NewDialogue(string text)
    {
        GameObject template_clone = Instantiate(d_template, d_template.transform);
        template_clone.transform.parent = canva.transform;
        template_clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player_detection = false;
    }

    /*IEnumerator StartDialogue()
    {
        inDialogue = true;
        canva.SetActive(true);
        PlayerMovement.dialogue = true;

        Debug.Log("Starting dialogue");

        // Display initial dialogue with options
        NewDialogue("Hey there! Just make want sure you're not too noisy brethren.");

        // Wait for the player to make a choice through the UI buttons
        yield return StartCoroutine(WaitForButtonPress());

        // Process the choice
        if (optionAFlag)
        {
            // Player chose option A
            NewDialogue("Sick blud");
            Debug.Log("Choice made A");
        }
        else if (optionBFlag)
        {
            // Player chose option B
            canva.SetActive(false);
            Debug.Log("Choice made B");
            PlayerMovement.dialogue = false;
        }

        // Reset the choice flags
        optionAFlag = false;
        optionBFlag = false;

        inDialogue = false;
        canva.transform.GetChild(1).gameObject.SetActive(true);
    }*/
    IEnumerator WaitForButtonPress()
    {
        while (!optionAFlag && !optionBFlag)
        {
            // Yielding null allows the event system to process the button click
            yield return null;
        }
    }

    // Button click handlers
    private bool optionAFlag = false;
    private bool optionBFlag = false;

    void OptionASelected()
    {
        // Change the text value when option A is selected
        optionAFlag = true;
    }

    void OptionBSelected()
    {
        // Change the text value when option B is selected
        optionBFlag = true;
    }
}
