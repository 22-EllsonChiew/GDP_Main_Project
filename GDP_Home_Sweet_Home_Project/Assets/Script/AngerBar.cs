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
    

    public int ticketHolder;


    //public int ticketPromiseValue { get; private set; }

    public void Update()
    {
        PromiseTicketButton();
    }

    


    public void DecreaseAnger()
    {

        if (ticketHolder == 1)
        {
            Debug.Log("promise route");
            angerSlider.value -= 0.5f;
            Debug.Log("IM REALLY ANGRY" + angerSlider.value);
        }
        else if (angerSlider != null)
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
        if (Input.GetKeyDown(KeyCode.G) && ticketHolder <= 1)
        {
            ticketHolder += 1;
            Debug.Log("Ticket given: " + ticketHolder);
        }
        
        else if (ticketHolder > 2)
        {
            // Handle the case where ticketHolder is more than 1, if needed
            Debug.Log("Cannot give more tickets, ticketHolder is already greater than 1.");
        }
    }

    

}
