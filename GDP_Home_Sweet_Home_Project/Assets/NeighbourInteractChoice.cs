using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NeighbourInteractChoice : MonoBehaviour
{
    public GameObject neighbourText;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player detected beep boop");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered door");
            neighbourText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited door");
        neighbourText.SetActive(false);
    }
}
