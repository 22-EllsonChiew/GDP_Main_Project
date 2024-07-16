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

    private float increasePerSec = 10f;
    private bool increaseHP = true;
    private float targetValue = 0.8f;
    private float increaseAmountofHp = 0.01f;

    private bool hasIncreasedHealth = false;

    public void Update() 
    {
        PromiseTicketButton();

        IncreaseHpOneTime();
    }





   
    public void DecreaseAnger()
    {
        Debug.Log("Ticket counter" + ticketGiver);
        if (ticketGiver == 1 || ticketGiver == 2 || ticketGiver == 3)
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




    public void IncreaseHpOneTime()
    {
        if (ticketGiver == 2 && !hasIncreasedHealth)
        {
            Debug.Log("Increase Hp");

            StartCoroutine(IncreaseHpContinuouslyCoroutine());
            // Increment the slider value by 0.15f
           // angerSlider.value += 0.15f;

            // Set the flag to true to indicate that health has been increased
            hasIncreasedHealth = true;
        }

        if(ticketGiver == 3 && !hasIncreasedHealth)

        {
            Debug.Log("Increase Hp");

            StartCoroutine(IncreaseHpForSecondNeighbour());
            
            hasIncreasedHealth = true;
        }
    }

    private IEnumerator IncreaseHpContinuouslyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            if (ticketGiver == 2 || ticketGiver == 3)
            {
                Debug.Log("Increase Hp");

                // Increment the slider value by 0.15f
                angerSlider.value += 0.05f;
            }
            if (angerSlider.value >= 0.75f)
            {
                Debug.Log("Reached threshold, stopping increase.");
                yield break; // Stop the coroutine
            }
        }
    }

    private IEnumerator IncreaseHpForSecondNeighbour()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            if (ticketGiver == 2 || ticketGiver == 3)
            {
                Debug.Log("Increase Hp");

                // Increment the slider value by 0.15f
                angerSlider.value += 0.05f;
            }
            if (angerSlider.value >= 0.75f)
            {
                Debug.Log("Reached threshold, stopping increase.");
                yield break; // Stop the coroutine
            }
        }
    }


    /*public void HandlerAnger()
    {
        DecreaseAnger();
    }*/

    public void PromiseTicketButton()
    {
        if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.F) && ticketGiver <= 1)
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
