using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourAngerBar : MonoBehaviour
{

    public float maxHappyBar = 100f;
    public float currentHappiness;

    public AngerBarManager angerBarManager;

    public ChatManager complaintMessage;

    
    
    // Start is called before the first frame update
    void Start()
    {
        currentHappiness = maxHappyBar;
    }

    public void HeardNoise(float amount)
    {
        currentHappiness -= amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappyBar);

        angerBarManager.UpdateHappinessBar();

        /*if(complaintMessage.neighbourHakim.currentHappiness < complaintMessage.noiseThreshold)
        {
            Debug.Log($"Hakim's happiness: {complaintMessage.neighbourHakim.currentHappiness}, threshold: {complaintMessage.noiseThreshold}");
            complaintMessage.ReceiveComplaint("Hakim");
            //complaintMessgae.ReceiveComplaint("Sherryl");
        }
        if(complaintMessage.neighbourSherryl.currentHappiness < complaintMessage.noiseThreshold)
        {
            Debug.Log($"Sherryl's happiness: {complaintMessage.neighbourSherryl.currentHappiness}, threshold: {complaintMessage.noiseThreshold}");
            complaintMessage.ReceiveComplaint("Sherryl");
        }
        else
        {
            Debug.Log("Neighbour Happy");
        }*/

        //complaintMessgae.ReceiveComplaint("Hakim");
        //complaintMessgae.ReceiveComplaint("Sherryl");

        complaintMessage.CheckNeighbourHappinessValue();
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
}
