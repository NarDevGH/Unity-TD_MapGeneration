using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private GameObject _grass;

    public static Grass Instance;

    private void Awake()
    {
        HandleInstances();
    }

    public void Generate_Grass(Transform plot, List<Vector2Int> noGrassPositions = null)
    {
        for (int x = -4; x <= 4; x++)
        {
            for (int z = -4; z <= 4; z++)
            {
                if (noGrassPositions != null)
                    if (noGrassPositions.Contains(new Vector2Int(x, z)))
                        continue;

                var grass_cell = Instantiate(_grass, plot.position + _grass.transform.position + new Vector3(x, 0, z), Quaternion.identity);
                grass_cell.transform.parent = plot;
            }
        }

    }

    private void HandleInstances()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
