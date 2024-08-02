using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourAngerBar : MonoBehaviour
{

    public float maxHappyBar = 100f;
    public float currentHappiness;

    public AngerBarManager angerBarManager;

    public ChatManager complaintMessage;

    public BoxCollider sherrylSideCollider;
    public BoxCollider hakimSideCollider;
    public Transform player;

    private bool hakimComplained = false;
    private bool sherrylComplained = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHappiness = maxHappyBar;
    }

    public void HeardNoise(float amount)
    {
        UpdateNeighbourHappinessBar(amount);
        CheckForSherrylComplainMessage();
        CheckForHakimComplainMessage();

        // If no complaints were received, log that neighbors are happy
        if (!complaintMessage.hakimSentedComplaint && !complaintMessage.sherrylSentedComplaint)
        {
            Debug.Log("Neighbours are happy.");
        }

        //Check neighbour happiness value
        //complaintMessage.CheckNeighbourHappinessValue();
    }

    private void UpdateNeighbourHappinessBar(float amount)
    {
        currentHappiness -= amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappyBar);
        angerBarManager.UpdateHappinessBar();

    }

    private void CheckForSherrylComplainMessage()
    {
        if (complaintMessage.neighbourSherryl.CheckPlayerInColliderSherryl() && complaintMessage.neighbourSherryl.currentHappiness < complaintMessage.noiseThresholdSherryl && !complaintMessage.sherrylSentedComplaint)
        {
            // Send complaint from Sherryl
            complaintMessage.ReceiveComplaint("Sherryl");
            complaintMessage.sherrylSentedComplaint = true;
        }
    }

    private void CheckForHakimComplainMessage()
    {
        if (complaintMessage.neighbourHakim.CheckPlayerInColliderHakim() && complaintMessage.neighbourHakim.currentHappiness < complaintMessage.noiseThreshold && !complaintMessage.hakimSentedComplaint)
        {
            // Send complaint from Hakim
            complaintMessage.ReceiveComplaint("Hakim");
            complaintMessage.hakimSentedComplaint = true;
        }
    }





    /*public void RestoreComplaint(float amount)
    {
        currentHappiness += amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappyBar);

        angerBarManager.UpdateHappinessBar();

        if(currentHappiness >= complaintMessage.noiseThreshold || currentHappiness >= complaintMessage.noiseThresholdSherryl)
        {
            complaintMessage.ResetComplaint();
        }
    }*/

    public bool CheckPlayerInColliderSherryl()
    {
        return sherrylSideCollider.bounds.Contains(player.transform.position);
    }

    public bool CheckPlayerInColliderHakim()
    {
        return hakimSideCollider.bounds.Contains(player.transform.position);
    }
}
