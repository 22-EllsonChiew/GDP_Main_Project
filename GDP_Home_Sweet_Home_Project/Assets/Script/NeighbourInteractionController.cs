using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourInteractionController : MonoBehaviour
{
    [Header("Neighbours")]
    [SerializeField]
    private Transform hakimTransform;
    [SerializeField] 
    private Transform sherrylTransform;

    public static NeighbourInteractionController instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
