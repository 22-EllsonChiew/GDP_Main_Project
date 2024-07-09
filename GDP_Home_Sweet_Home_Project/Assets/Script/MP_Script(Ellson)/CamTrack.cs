using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrack : MonoBehaviour
{

    public Transform camHolder;

    public float pLerp = .02f;
    public float rLerp = .04f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, camHolder.position, pLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, camHolder.rotation, rLerp);
    }
}
