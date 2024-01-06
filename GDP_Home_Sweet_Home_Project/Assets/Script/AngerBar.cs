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

    public HammerMinigame noiseLevelReference;

    public void Start()
    {
        //// Attempt to get the HammerMinigame script component
        //HammerMinigame noiseLevelReference = GetComponent<HammerMinigame>();

        //if (noiseLevelReference == null)
        //{
        //    Debug.LogError("HammerMinigame script not found on the same GameObject.");
        //}
    }


    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            
            /*if (noiseLevelReference != null)
            {
                Debug.Log("HammerMinigame script found.");
                // Call the HandleClick method from HammerMinigame
                noiseLevelReference.ClickNoiseLevelRef();
            }
            else
            {
                Debug.LogError("HammerMinigame script not found.");
            }*/
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

        if (angerSlider.value <= 0.0f && !hasReported)
        {
            // Disable the player or perform other actions
            reportCounter++;
            reportCounterText.text = "Neighbour Reports: " + reportCounter.ToString();
            Debug.Log("Report");

            hasReported = true;

        }

        if(reportCounter == 1)
        {
            SceneManager.LoadScene("Lose Scene");
        }
    }

    public void HandlerAnger()
    {
        DecreaseAnger();
    }


    
}
