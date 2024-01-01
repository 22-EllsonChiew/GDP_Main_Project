using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button optionAButton;
    public Button optionBButton;
    public GameObject canva;
    bool player_detection = false;

    public NeightbourSystem newDisplay;

    // Start is called before the first frame update
    void Start()
    {
        optionAButton.onClick.AddListener(OptionASelected);
        optionBButton.onClick.AddListener(OptionBSelected);
        newDisplay = GetComponent<NeightbourSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player_detection && Input.GetKeyDown(KeyCode.F) && !PlayerMovement.dialogue)
        {
            Debug.Log("dialogue started buttons");
            ActivateButtons(); // Call a method to activate the buttons
        }
        if (PlayerMovement.dialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OptionASelected();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OptionBSelected();
            }
        }
    }

    void ActivateButtons()
    {
        // Do any additional setup for the buttons here
        // For example, you might want to set text on the buttons or enable/disable interactability

        // Activate the buttons
        optionAButton.gameObject.SetActive(true);
        optionBButton.gameObject.SetActive(true);
    }


    public void OptionASelected()
    {
        Debug.Log("Choice made A");
        newDisplay.NewDialogue("Sick blud");
    }
    public void OptionBSelected()
    {
        // Player chose option B
        canva.SetActive(false);
        Debug.Log("Choice made B");
        PlayerMovement.dialogue = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player_detection = false;
    }
}
