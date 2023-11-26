using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AngerBar : MonoBehaviour
{
    public Slider angerSlider;

    
    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player") && Input.GetMouseButtonDown(0))
        {

            
            DecreaseAnger();
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited trigger");


        // Disable the entire GameObject
        gameObject.SetActive(false);

    }

    public void DecreaseAnger()
    {
        if(angerSlider != null)
        {
            angerSlider.value -= 0.2f;
            Debug.Log("IM ANGRY" + angerSlider.value);
        }

        if (angerSlider.value <= 0.0f)
        {
            // Disable the player or perform other actions
            Debug.Log("Report");
            
        }
    }


    
}
