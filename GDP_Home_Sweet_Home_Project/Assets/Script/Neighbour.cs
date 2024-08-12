using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Neighbour : MonoBehaviour
{
    public string neighbourName;
    public Transform neighbourTransform;

    public float maxHappiness;
    public TextAsset neighbourRoutinesJSON;
    public RoutineData routineArray;

    public float happinessThreshold_Normal {  get; private set; }
    public float happinessThreshold_Angry { get; private set; }
    public float complaintThreshold { get; private set; }
    public int complaintCount { get; private set; }
    public float currentHappiness { get; private set; }
    public DialogueType currentMood { get; private set; }
    public bool IsNeighbourInRoutine {  get; private set; }

    private bool hasMadeWarning;

    
    public NeighbourRoutines currentRoutine {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {

        currentHappiness = maxHappiness;
        happinessThreshold_Normal = maxHappiness * 0.65f;
        happinessThreshold_Angry = maxHappiness * 0.35f;

        complaintThreshold = happinessThreshold_Normal;
        complaintCount = 0;

        routineArray = JsonConvert.DeserializeObject<RoutineData>(neighbourRoutinesJSON.text);
        neighbourTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {


        // check if neighbour has a routine at specified time
        CheckNeighbourRoutines();
        CalculateCurrentMood();

    }

    public void EscalateNeighbourComplaint()
    {
        if (complaintCount < 1)
        {
            complaintCount++;
            complaintThreshold = happinessThreshold_Angry;
            Debug.Log("neighbour has made a complaint - less forgiving to player");
        }
        else if (complaintCount < 2)
        {
            complaintCount++;
            complaintThreshold = 0f;
            Debug.Log("neighbour has made final warning - will no longer make informal complaint");
        }
        else
        {
            Debug.Log("neighbour no longer wants to complain");
        }
    }

    public void ReduceHappiness(float amount)
    {
        currentHappiness -= amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappiness);
    }

    void CheckNeighbourRoutines()
    {
        foreach (var routine in routineArray.routines)
        {
            if (routine.day == TimeController.CurrentDay)
            {
                if (routine.routineStartHour == TimeController.Hour && routine.routineStartMinute == TimeController.Minute)
                {
                    IsNeighbourInRoutine = true;
                    currentRoutine = routine;
                }
                else if (routine.routineEndHour == TimeController.Hour && routine.routineEndMinute == TimeController.Minute)
                {
                    IsNeighbourInRoutine = false;
                    currentRoutine = null;
                }
            }

        }
    }

    void CalculateCurrentMood()
    {
        if (currentHappiness <= happinessThreshold_Angry)
        {
            currentMood = DialogueType.Mood_Angry;
        }
        else if (currentHappiness <= happinessThreshold_Normal)
        {
            currentMood = DialogueType.Mood_Normal;
        }
        else 
        {
            currentMood = DialogueType.Mood_Happy;
        }
    }

}
