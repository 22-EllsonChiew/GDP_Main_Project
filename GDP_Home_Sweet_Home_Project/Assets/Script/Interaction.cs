using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public delegate void TaskEventHandler(bool isTaskStarted);
    public event TaskEventHandler OnTaskInteract;

    //[SerializeField] private 


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object") && Input.GetKey(KeyCode.E)) //check if tag of the object colliding with player is "object"
        {
            Debug.Log("Task Started"); //prints "Task Complete"
            OnTaskInteract?.Invoke(true);
            Destroy(other.gameObject); //Destroys the gameobject that is collidng with player
        }
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
