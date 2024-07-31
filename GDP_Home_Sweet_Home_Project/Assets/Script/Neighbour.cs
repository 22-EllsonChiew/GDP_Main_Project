using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbour : MonoBehaviour
{
    public string neighbourName;
    public Transform neighbourTransform;

    // Start is called before the first frame update
    void Start()
    {
        neighbourTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
