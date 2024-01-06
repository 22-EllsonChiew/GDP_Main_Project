using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interaction : MonoBehaviour
{

    public delegate void TaskEventHandler(bool isTaskComplete);
    public event TaskEventHandler OnTaskInteract;

    [SerializeField] private ConfirmationWindow confirmationWindow;
    [SerializeField] private GameObject ChestUI;
    public Animator animator;

    public GameObject BuildingChair;
    

    private Collider currentCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object") && Input.GetKey(KeyCode.E)) //check if tag of the object colliding with player is "object"
        {
            confirmationWindow.gameObject.SetActive(true);
            confirmationWindow.confirmButton.onClick.AddListener(() => ConfirmClicked(other)); ;
            confirmationWindow.exitButton.onClick.AddListener(ExitClicked);
        }

        if (other.CompareTag("Chest") && Input.GetKey(KeyCode.E))
        {
            Debug.Log("Opening chest");
            ChestUI.SetActive(true);
            animator.SetTrigger("chestOpen");
        }

        currentCollider = other;
    }

    private void ConfirmClicked(Collider confirmedCollider)
    {
        confirmationWindow.gameObject.SetActive(false);
        

        if (confirmedCollider != null) 
        {
            BuildingChair.gameObject.SetActive(true);
            Destroy(confirmedCollider.gameObject);
            OnTaskInteract?.Invoke(true);
        }
        //call function for minigame
    }

    private void ExitClicked()
    {
        confirmationWindow.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        currentCollider = null;
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
