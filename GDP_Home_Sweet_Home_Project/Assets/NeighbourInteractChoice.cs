using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NeighbourInteractChoice : MonoBehaviour
{
    public GameObject canva;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player detected beep boop");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered door");
            canva.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited door");
        canva.SetActive(false);
    }
}
