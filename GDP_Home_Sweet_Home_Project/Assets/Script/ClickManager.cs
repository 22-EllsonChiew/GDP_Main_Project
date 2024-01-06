using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
   /* void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int layerMask = LayerMask.GetMask("Nails");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 1.0f);

                NailsController nailWhacker = hit.collider.GetComponentInParent<NailsController>();

                if (nailWhacker != null)
                {
                    Debug.Log("Hit NailsController");
                    nailWhacker.SmackNails();
                    Debug.Log("whack3");
                }
                else
                {
                    Debug.Log("Hit GameObject but no NailsController script found");
                }
            }
            else
            {
                Debug.Log("Ray did not hit anything on the 'Nails' layer.");
            }
        }
    }*/
}
