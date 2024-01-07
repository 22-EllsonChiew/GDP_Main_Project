using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NailObjectController : MonoBehaviour
{

    public UnityEvent<GameObject> gameStart;

    public Camera gameCamera;

    public int currentClicks;

    // Start is called before the first frame update
    void Start()
    {
        gameStart.AddListener(obj => GameObject.FindGameObjectWithTag("NailGameController").GetComponent<NailGame>().StartMinigame(obj));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to a ray
            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits something with a collider
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider is the Box Collider attached to this GameObject
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    gameStart.Invoke(gameObject);
                    Debug.Log("Starting Hammering");

                }
            }
        }

    }

}
