using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoutineType
{
    NoNoise,
    NotHome
}

public class NeighbourRoutines
{
    public int day;
    public int routineStartHour;
    public int routineStartMinute;
    public int routineEndHour;
    public int routineEndMinute;
    public RoutineType routineType;

}
