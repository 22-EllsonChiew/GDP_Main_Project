using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourTranslucentBoxInteract : MonoBehaviour
{
    public GameObject translucentCubeText;
    public GameObject oppositeBox;
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
        StartCoroutine(DisableWithDelay());

        Debug.Log("Player exited door");

        
    }

    IEnumerator DisableWithDelay()
    {
        Debug.Log("Starting coroutine");

        yield return new WaitForSeconds(2f);

        Debug.Log("Removing text");
        translucentCubeText.SetActive(false);

        Destroy(this.gameObject);
        Destroy(oppositeBox);
    }
}
