using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [SerializeField] private GameObject _plot;

    public Vector3 positionOffset { get; set; }

    public static Plot Instance;

    private void Awake()
    {
        HandleInstances();

        positionOffset = new Vector3(_plot.transform.localScale.x, 0f, _plot.transform.localScale.z);
    }

    public GameObject Generate_Plot() 
    {
        return Instantiate(_plot);
    }


    public GameObject Generate_Plot_Towards_Dir(Vector3 lastPlotPos, Direction.NESW dir)
    {
        Vector3 plotPos = Direction.PosTowardsDir(lastPlotPos, dir, positionOffset);

        return Instantiate(_plot, plotPos, _plot.transform.rotation);

    }

    public List<Vector2Int> PathThroughPlot(Vector2Int startPos, Vector2Int endPos) 
    {
        List<Vector2Int> path = new List<Vector2Int>();

        path.Add(startPos);

        Vector2Int currentPos = startPos;

        while (currentPos != endPos)
        {
            List<Vector2Int> neightboursPositions = NeightboursPositions(currentPos);

            int closestPosIndex = 0;
            float minDist = Vector2Int.Distance(neightboursPositions[0], endPos);

            for (int i = 1; i < neightboursPositions.Count; i++)
            {
                float dist = Vector2Int.Distance(neightboursPositions[i], endPos);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestPosIndex = i;
                }
            }

            currentPos = neightboursPositions[closestPosIndex];
            path.Add(currentPos);

            if (minDist <= 1) // next to endpoint
            {
                currentPos = endPos;
                path.Add(endPos);
            }
        }

        return path;
    }


    private List<Vector2Int> NeightboursPositions(Vector2Int currentNode)
    {
        List<Vector2Int> neightbours = new List<Vector2Int>();

        if (ValidDir(currentNode, Direction.NESW.North))
        {
            neightbours.Add(currentNode + Vector2Int.up);
        }
        if (ValidDir(currentNode, Direction.NESW.South))
        {
            neightbours.Add(currentNode + Vector2Int.down);
        }
        if (ValidDir(currentNode, Direction.NESW.East))
        {
            neightbours.Add(currentNode + Vector2Int.right);
        }
        if (ValidDir(currentNode, Direction.NESW.West))
        {
            neightbours.Add(currentNode + Vector2Int.left);
        }

        return neightbours;
    }

    private bool ValidDir(Vector2Int vec2Pos, Direction.NESW newDir)
    {
        Vector2Int pos = Direction.Vec2PosTowardsDir(vec2Pos,newDir);
        return ( PosIsWithinThePlot(pos) ); 
    }

    private bool PosIsWithinThePlot(Vector2Int pos)
    {
        return pos.x < 4 &&
               pos.x > -4 &&
               pos.y < 4 &&
               pos.x > -4;
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
