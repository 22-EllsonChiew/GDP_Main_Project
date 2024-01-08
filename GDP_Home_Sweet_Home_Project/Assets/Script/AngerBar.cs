using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AngerBar : MonoBehaviour
{
    public Slider angerSlider;

    public int reportCounter;
    public Text reportCounterText;

    private bool hasReported = false;


    //public NailGame noiseLevelReference;
    

    public int ticketHolders;


    //public int ticketPromiseValue { get; private set; }

    public void Update()
    {
        PromiseTicketButton();
    }

    


    public void DecreaseAnger()
    {

        if (ticketHolders == 1)
        {
            Debug.Log("promise route");
            angerSlider.value -= 0.5f;
            Debug.Log("IM REALLY ANGRY" + angerSlider.value);
        }
        else if (ticketHolders == 0)
        {
            angerSlider.value -= 0.2f;
            Debug.Log("IM ANGRY" + angerSlider.value);
        }

        if (angerSlider.value <= 0.0f && !hasReported)
        {
            // Disable the player or perform other actions
            reportCounter++;
            reportCounterText.text = "Neighbour Reports: " + reportCounter.ToString();
            Debug.Log("Report");

            hasReported = true;

        }
        
        if (reportCounter == 1)
        {
            SceneManager.LoadScene("Lose Scene");
        }

        
            
    }

    
    public void HandlerAnger()
    {
        DecreaseAnger();
    }

    public void PromiseTicketButton()
    {
        if (Input.GetKeyDown(KeyCode.G) && ticketHolders <= 1)
        {
            ticketHolders += 1;
            Debug.Log("Ticket given: " + ticketHolders);
        }
        
        else if (ticketHolders > 2)
        {
            // Handle the case where ticketHolder is more than 1, if needed
            Debug.Log("Cannot give more tickets, ticketHolder is already greater than 1.");
        }
    }

    

}
