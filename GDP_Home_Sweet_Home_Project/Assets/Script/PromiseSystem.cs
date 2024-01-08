using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromiseSystem : MonoBehaviour
{

    public int ticketPromise = 0;
    

    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("Lets Talk");
        
    }

    private void Update()
    {
        PromiseTicketButton(true);
    }

    public void PromiseTicketButton(bool keyPressed)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ticketPromise += 1;
            Debug.Log("Ticket given: " + ticketPromise);
        }
    }

    public int ticketPromiseGive()
    {
        return ticketPromise;
    }
}
