using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator;
    private bool isOpen = false;
    private GameObject triggerZone;


    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();

        //// Find the TriggerZone GameObject by name (you can adjust this based on your hierarchy)
        //triggerZone = GameObject.Find("TriggerZone");

        //if (triggerZone == null)
        //{
        //    Debug.LogError("TriggerZone not found! Make sure it has the correct name or adjust the code.");
        //}
        //else
        //{
        //    Debug.Log("TriggerZone found: " + triggerZone.name);
        //}

    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (InTriggerZone())
    //    {
    //        if (Input.GetKeyDown(KeyCode.F))
    //        {

    //            isOpen = !isOpen;

    //            if (isOpen)
    //                doorAnimator.SetTrigger("Open");
    //            else
    //                doorAnimator.SetTrigger("Close");

    //        }
    //    }
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the "Player" tag and is TriggerZone
        if (other.CompareTag("Player") && other.gameObject == triggerZone)
        {
            // Player has entered the trigger zone, allow door control
            isOpen = true;
            Debug.Log("Player entered the trigger zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider has the "Player" tag and is TriggerZone
        if (other.CompareTag("Player") && other.gameObject == triggerZone)
        {
            // Player has exited the trigger zone, disable door control
            isOpen = false;
            Debug.Log("Player exited the trigger zone");
        }
    }

    //// Check if the player is in the trigger zone
    //private bool InTriggerZone()
    //{
    //    // Check if the player GameObject is within the trigger zone
    //    Collider[] colliders = Physics.OverlapBox(triggerZone.transform.position, triggerZone.transform.localScale / 2, triggerZone.transform.rotation);

    //    foreach (var collider in colliders)
    //    {
    //        if (collider.CompareTag("Player"))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

}

