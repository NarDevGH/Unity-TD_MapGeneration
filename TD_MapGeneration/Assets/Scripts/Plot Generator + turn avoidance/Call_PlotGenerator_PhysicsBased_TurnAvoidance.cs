using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Call_PlotGenerator_PhysicsBased_TurnAvoidance : MonoBehaviour
{
    public void GeneratePlot() 
    {
        PlotGenerator_PhysicsBased_TurnAvoidance.Instance.Generate_Plot();
    }

}
