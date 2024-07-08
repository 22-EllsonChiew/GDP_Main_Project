using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BedSystem : MonoBehaviour
{

    public GameObject packagePrefab;
    public Transform spawnPoint;
    private bool playerInBed = false;


    void Update()
    {
        if (playerInBed && Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene("Main GameSecondDay");
            SpawnPackage();
        }
    }

    private void SpawnPackage()
    {
        if(spawnPoint != null)
        {
            Instantiate(packagePrefab, spawnPoint.position, spawnPoint.rotation);
            Instantiate(packagePrefab, spawnPoint.position, spawnPoint.rotation);
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInBed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInBed = false;
        }
    }

}
