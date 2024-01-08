using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BedSystem : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                SceneManager.LoadScene("Sleep Scene");
            }
        }
    }

}
