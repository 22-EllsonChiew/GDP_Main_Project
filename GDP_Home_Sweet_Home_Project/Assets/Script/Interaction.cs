using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Interaction : MonoBehaviour
{

    public delegate void TaskEventHandler(bool isTaskComplete);
    public event TaskEventHandler OnTaskInteract;

    [SerializeField] private ConfirmationWindow confirmationWindow;
    [SerializeField] private GameObject ChestUI;
    public Animator animator;

    public GameObject mainCam;
    public GameObject minigameCam;

    public GameObject builtChair;

    private Collider currentCollider;

    public UnityEvent<bool> isGameStarting;


    private void Start()
    {
        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            isGameStarting.AddListener(isTaskComplete => playerObject.GetComponent<PlayerMovement>().CheckMinigame(isTaskComplete));
        }
        else
        {
            Debug.LogError("Player GameObject not found in the scene!");
        }
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
        //isGameStarting.Invoke(true);

        confirmationWindow.gameObject.SetActive(false);

        if (confirmedCollider != null) 
        {
            

            minigameCam.SetActive(true);
            mainCam.SetActive(false);

            Destroy(confirmedCollider.gameObject);
            Instantiate(builtChair, confirmedCollider.gameObject.transform.position, Quaternion.Euler(0f, 180f, 0f));

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
