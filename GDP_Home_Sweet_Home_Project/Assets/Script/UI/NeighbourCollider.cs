using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourCollider : MonoBehaviour
{
    public Neighbour neighbour;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interaction.currentNeighbour = neighbour;
        }
    }

}
