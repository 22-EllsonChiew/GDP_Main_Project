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

    public int ticketGiver;

    private bool keyPressed;

   public void Update() 
    {
        PromiseTicketButton();

    }



   
    public void DecreaseAnger()
    {
        Debug.Log("Ticket counter" + ticketGiver);
        if (ticketGiver == 1)
        {
            ticketGiver++;
            Debug.Log("promise route");
            angerSlider.value -= 0.5f;
            Debug.Log("IM REALLY ANGRY" + angerSlider.value);
            Debug.Log("Ticket counter" + ticketGiver);
            
        }
        
        else if(angerSlider != null) 
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

    
    /*public void HandlerAnger()
    {
        DecreaseAnger();
    }*/

    public void PromiseTicketButton()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && ticketGiver <= 1)
        {
            ticketGiver++;
            Debug.Log("Ticket counter" + ticketGiver);
            
        }

       
    }

   /* public void PromiseRoute()
    {
        PromiseTicketButton();

        Debug.Log("promise route");
        angerSlider.value -= 0.5f;
        Debug.Log("IM REALLY ANGRY" + angerSlider.value);
        Debug.Log("Ticket counter" + ticketGiver);
    }*/

    
}
