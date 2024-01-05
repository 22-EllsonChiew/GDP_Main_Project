using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits something with a collider
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider is the Box Collider attached to this GameObject
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // The click is on the Box Collider, do something
                    Debug.Log("Box Collider clicked!");
                }
            }
        }

    }

}
