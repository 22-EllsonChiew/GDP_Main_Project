using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AngerBar : MonoBehaviour
{
    public Slider angerSlider;

    //public KeyCode buildingObject = KeyCode.Mouse0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DecreaseAnger();
        }
    }

    private void OntriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DecreaseAnger();
            
        }
    }

    private void DecreaseAnger()
    {
        if(angerSlider != null)
        {
            angerSlider.value -= 0.1f;
            Debug.Log("IM ANGRY" + angerSlider.value);
        }
    }
}
