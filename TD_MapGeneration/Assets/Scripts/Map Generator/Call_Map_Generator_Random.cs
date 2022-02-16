using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Call_Map_Generator_Random : MonoBehaviour
{
    public void GeneratePlot() 
    {
        Map_Generator_Random.Instance.Generate_Plot();
    }

}
