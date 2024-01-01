using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesSystems : MonoBehaviour
{

    public ParticleSystem particleSystem;

    //public GameObject particle;
    //private GameObject currentParticle;

    public float particleDuration = 1f; 

    private float currentDuration;

    void Start()
    {
        //currentParticle.SetActive(false); 

        particleSystem.Stop();
        currentDuration = particleDuration;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            StartParticle();
        }

        if (particleSystem.isPlaying)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration <= 0f)
            {
                StopParticle();
            }
        }
    }

    void StartParticle()
    {
        /*if (currentParticle == null)
        {
            currentParticle = Instantiate(particle, transform.position, Quaternion.identity);
            currentDuration = particleDuration; 
        }
        else
        {
            
            currentParticle.SetActive(true);
            currentDuration = particleDuration;
        }*/

        particleSystem.Play();
        currentDuration = particleDuration;
    }

    void StopParticle()
    {
        /*if(currentParticle != null)
        {
            currentParticle.SetActive(false);
            currentDuration = particleDuration;
        }*/

        particleSystem.Stop();
        currentDuration = particleDuration;
    }

}
