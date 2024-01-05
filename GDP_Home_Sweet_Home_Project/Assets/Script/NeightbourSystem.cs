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

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        /*if(player_detection && Input.GetKeyDown(KeyCode.F) && !PlayerMovement.dialogue)
        {
            canva.SetActive(true);
            PlayerMovement.dialogue = true;
            NewDialogue("Pls dont make too much noise");
            NewDialogue("If not i will report you");
            canva.transform.GetChild(1).gameObject.SetActive(true);

            
        }*/

        if (player_detection && (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.B)) && !PlayerMovement.dialogue)
        {
            canva.SetActive(true);
            PlayerMovement.dialogue = true;
            //NewDialogue("Alright ill take your word for it");
            //NewDialogue("But make sure you keep that promise");
            canva.transform.GetChild(1).gameObject.SetActive(true);
        }
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

    
}
