using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NeighbourUIReRework : MonoBehaviour
{
    public GameObject canva;
    public GameObject text1;
    public GameObject text2;
    public GameObject promiseButtonObj;
    public GameObject exitButtonObj;
    public GameObject interactText;
    public GameObject neighbourCam;
    public GameObject mainCam;
    public GameObject neighbourBar;
    public GameObject mainCanvas;
    public GameObject playerObj;

    bool player_detection = false;


    public int ticketPromise = 0;

    //public AngerBar angerBarCS;

    // Start is called before the first frame update
    void Start()
    {
        //angerBarCS = GetComponent<AngerBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player_detection)
        {
            interactText.SetActive(true);
        }

        if (player_detection && (Input.GetKeyDown(KeyCode.Tab)) && !PlayerMovement.dialogue)
        {
            mainCam.SetActive(false);
            playerObj.SetActive(false);
            mainCanvas.SetActive(false);
            neighbourBar.SetActive(true);
            neighbourCam.SetActive(true);
            Debug.Log("Started interaction with neighbour");
            canva.SetActive(true);
            PlayerMovement.dialogue = true;
            text1.SetActive(true);
            text2.SetActive(false);
            promiseButtonObj.SetActive(true);
            exitButtonObj.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.G) && PlayerMovement.dialogue)
        {
            //PromiseTicketButton(true);
            mainCam.SetActive(true);
            playerObj.SetActive(true);
            mainCanvas.SetActive(true);
            neighbourBar.SetActive(false);
            neighbourCam.SetActive(false);
            Debug.Log("Ended interaction through promise");
            canva.SetActive(false);
            text2.SetActive(false);
            PlayerMovement.dialogue = false;
        }
    }

    public void promiseButton()
    {
        text1.SetActive(false);
        text2.SetActive(true);
        promiseButtonObj.SetActive(false);
        exitButtonObj.SetActive(false);
    }

    /*public void PromiseTicketButton(bool keyPressed)
    {
        if (keyPressed)
        {
            ticketPromise += 1;
            Debug.Log("Ticket given: " + ticketPromise);
        }
    }*/

    public int ticketPromiseGive()
    {
        return ticketPromise;
    }


    public void exitButton()
    {
        Debug.Log("Ended interaction through exit");
        neighbourCam.SetActive(false);
        Debug.Log("IM Zooming out");
        canva.SetActive(false);
        PlayerMovement.dialogue = false;
        Debug.Log("Back to player");
        mainCam.SetActive(true);
        neighbourBar.SetActive(false);
        playerObj.SetActive(true);

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
