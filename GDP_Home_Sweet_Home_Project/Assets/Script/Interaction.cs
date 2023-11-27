using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interaction : MonoBehaviour
{

    public delegate void TaskEventHandler(bool isTaskComplete);
    public event TaskEventHandler OnTaskInteract;

    [SerializeField] private ConfirmationWindow confirmationWindow;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object") && Input.GetKey(KeyCode.E)) //check if tag of the object colliding with player is "object"
        {
            confirmationWindow.gameObject.SetActive(true);
            confirmationWindow.confirmButton.onClick.AddListener(ConfirmClicked);
            confirmationWindow.exitButton.onClick.AddListener(ExitClicked);
        }
    }

    private void ConfirmClicked()
    {
        confirmationWindow.gameObject.SetActive(false);
        OnTaskInteract?.Invoke(true);
        //call function for minigame
    }

    private void ExitClicked()
    {
        confirmationWindow.gameObject.SetActive(false);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Object"))
    //    {
    //        StartCoroutine(DestroyObjectWithDelay(other.gameObject, 2f));
    //    }
    //}
    //private IEnumerator DestroyObjectWithDelay(GameObject objectToDestroy, float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    Destroy(objectToDestroy);
    //    Debug.Log("Task Complete");
    //}
}
