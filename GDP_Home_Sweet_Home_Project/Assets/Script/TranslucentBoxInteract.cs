using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslucentBoxInteract : MonoBehaviour
{
    public GameObject translucentCubeText;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player detected beep boop");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered door");
            translucentCubeText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited door");
        translucentCubeText.SetActive(false);
        Destroy(this.gameObject);
    }
}
