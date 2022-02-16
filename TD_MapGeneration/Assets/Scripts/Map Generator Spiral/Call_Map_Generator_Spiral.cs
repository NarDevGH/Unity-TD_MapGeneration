using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Call_Map_Generator_Spiral : MonoBehaviour
{
    public void GeneratePlot() 
    {
        Map_Generator_Spiral.Instance.Generate_Plot();
    }

}
