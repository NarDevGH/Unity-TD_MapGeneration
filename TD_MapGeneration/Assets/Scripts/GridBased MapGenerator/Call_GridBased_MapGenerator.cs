using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Call_GridBased_MapGenerator : MonoBehaviour
{
    public void GeneratePlot() 
    {
        Map_Generator_GridBased.Instance.Generate_Plot();
    }

}
