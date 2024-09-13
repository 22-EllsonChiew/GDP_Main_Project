using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerHandler : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;


    private void OnTriggerEnter(Collider other)
    {   //adds a unityevent and put in the gameobject with the 
        onEnter.Invoke();
        gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke();

    }
}
