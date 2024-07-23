using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPackages : MonoBehaviour
{
    [SerializeField] private GameObject packageBig;
    [SerializeField] private GameObject packageSmall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            packageBig.SetActive(true);
            packageSmall.SetActive(true);
        }
    }
}
