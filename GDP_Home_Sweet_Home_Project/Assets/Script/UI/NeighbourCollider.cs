using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourCollider : MonoBehaviour
{

    [SerializeField]
    private GameObject neighbourUIGroup;

    private bool playerCanInteract;
    void Start()
    {
        neighbourUIGroup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCanInteract && Input.GetKeyUp(KeyCode.E)) 
        {
            neighbourUIGroup.SetActive(true);

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanInteract = false;
            if (neighbourUIGroup.activeSelf)
            {
                neighbourUIGroup.SetActive(false);
            }
        }
    }
}
