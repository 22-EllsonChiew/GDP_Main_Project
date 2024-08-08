using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourAngerBar : MonoBehaviour
{
    // work on this
    //public float maxHappyBar = 100f;
    //public float currentHappiness;

    public AngerBarManager angerBarManager;


    private Neighbour neighbour;
    public BoxCollider soundCollider;

    public Transform player;

    private bool hakimComplained = false;
    private bool sherrylComplained = false;

    // Start is called before the first frame update
    void Start()
    {
        neighbour = GetComponent<Neighbour>();
    }

    public void HeardNoise(float amount)
    {
        if (CheckPlayerInCollider())
        {
            UpdateNeighbourHappinessBar(amount);

            if (neighbour.currentHappiness <= neighbour.complaintThreshold)
            {
                ChatManager.instance.ReceiveComplaint(neighbour.neighbourName, DetermineComplaintType());
                neighbour.EscalateNeighbourComplaint();
            }
            else
            {
                Debug.Log("Neighbour disturbed but not angry.");
            }
        }
    }

    private void UpdateNeighbourHappinessBar(float amount)
    {
        neighbour.ReduceHappiness(amount);
        angerBarManager.UpdateHappinessBar();

    }

    DialogueType DetermineComplaintType()
    {
        if (neighbour.currentHappiness <= neighbour.happinessThreshold_Angry)
        {
            return DialogueType.Complaint_Angry;
        }
        else
        {
            return DialogueType.Complaint_Normal;
        }
    }

    public bool CheckPlayerInCollider()
    {
        return soundCollider.bounds.Contains(player.transform.position);
    }

}
