using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object") && Input.GetKey(KeyCode.E)) //check if tag of the object colliding with player is "object"
        {
            Debug.Log("Task Complete"); //prints "Task Complete"
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
