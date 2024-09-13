using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum RoutineType
{
    NoNoise,
    Asleep,
    NotHome
}

[System.Serializable]
public class NeighbourRoutines
{
    public int day;
    public int routineStartHour;
    public int routineStartMinute;
    public int routineEndHour;
    public int routineEndMinute;
    [JsonConverter(typeof(StringEnumConverter))]
    public RoutineType routineType;

}

[System.Serializable]
public class RoutineData
{
    public NeighbourRoutines[] routines;
}
