using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageRaycast : MonoBehaviour
{
    [Header("RayCast")]

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private string tagName;
    [SerializeField] private float rayCastRange = 10f;

    private bool onCarpet = false;

    [Header("Script")]
    public NoiseController noiseController;

    // Start is called before the first frame update
    void Start()
    {
        noiseController = GameObject.FindAnyObjectByType<NoiseController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, rayCastRange, layerMask))
        {
            if(hit.transform.CompareTag(tagName))
            {
                Debug.Log("On Carpet");
                onCarpet = true;
            }
            else
            {
                onCarpet = false;
            }
        }
        
    }

    public bool OnCarpet()
    {
        return onCarpet;
    }
}
