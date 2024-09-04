using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    public TextMeshProUGUI interactionKeyText;
    public TextMeshProUGUI interactionContextText;


    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetInteractionText(string interactionKey, string interactionContext)
    {
        interactionKeyText.text = interactionKey;
        interactionContextText.text = interactionContext;   
    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
    }
    
    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }
}
