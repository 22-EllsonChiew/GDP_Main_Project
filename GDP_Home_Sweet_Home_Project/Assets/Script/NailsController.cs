using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailsController : MonoBehaviour
{
    public GameObject nails;
    private bool isUp = true;
    private float timer = 2f;
    private List<GameObject> holes;

    public HammerMinigame hammerMiniGame;

    // Start is called before the first frame update
    void Start()
    {
        holes = new List<GameObject>();
        holes.Add(gameObject); 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp)
        {
            // Randomly activate or deactivate nails
            if (Random.Range(0, 2) == 0)
            {
                //Debug.Log("Activating Nails");
                nails.SetActive(true);
            }
            else
            {
                //Debug.Log("Deactivating Nails");
                nails.SetActive(false);
            }

            isUp = false;
            StartCoroutine(WaitForNails(2f));
        }

    }

    IEnumerator WaitForNails(float waitTime)
    {
        //Debug.Log("Waiting for Nails");
        yield return new WaitForSeconds(waitTime);
        isUp = true;
        //Debug.Log("Activating Nails");
        nails.SetActive(true); ;
        timer = 2f;
    }

    public void SmackNails()
    {
            Debug.Log("Smacking Nails");
        if(hammerMiniGame != null)
        {
            hammerMiniGame.HandleClick(Input.mousePosition);
        }
            nails.SetActive(false);
            isUp = false;
            StartCoroutine(WaitForNails(2f));
    }

    
}
