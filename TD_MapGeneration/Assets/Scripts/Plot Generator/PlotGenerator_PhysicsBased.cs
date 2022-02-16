using System.Collections;
using UnityEngine;


// Generates path towards random direction without overlapping existing Plots, until it reaches a dead end

public class PlotGenerator_PhysicsBased : MonoBehaviour
{

    private LastPlotData _lastPlotData;

    public static PlotGenerator_PhysicsBased Instance;

    private void Awake()
    {
        HandleInstances();
    }

    private void Start()
    {
        Generate_Starting_Plot();
    }


    public void Generate_Plot()
    {
        if (_lastPlotData.nextPlotDir == Direction.NESW.None)
        {
            Debug.Log("Dead End Reached");
            return;
        }

        GameObject plot = Plot.Instance.Generate_Plot_Towards_Dir(_lastPlotData.plotPos, _lastPlotData.nextPlotDir);

        Direction.NESW nextPlotDir = NextPlotDir(plot.transform.position, _lastPlotData.nextPlotDir);

        _lastPlotData.plotPos = plot.transform.position;
        _lastPlotData.nextPlotDir = nextPlotDir;
        _lastPlotData.dirTowardsPlot = Direction.OppositeDirection(nextPlotDir);
    }

    private void Generate_Starting_Plot()
    {
        GameObject plot = Plot.Instance.Generate_Plot();

        Direction.NESW nextPlotDir = Direction.RandomDirection();

        _lastPlotData.plotPos = plot.transform.position;
        _lastPlotData.nextPlotDir = nextPlotDir;
        _lastPlotData.dirTowardsPlot = Direction.OppositeDirection(nextPlotDir);
    }


    private Direction.NESW NextPlotDir(Vector3 plotPos, Direction.NESW last_NextPlotDir)
    {
        Direction.NESW nextPlotDir = Direction.NESW.None; // Initialize new nextPlotDir

        if (last_NextPlotDir == Direction.NESW.None)
        {
            nextPlotDir = Direction.RandomDirection();
        }
        else
        {
            BitArray bannedDirections = new BitArray(4);

            bannedDirections[(int)Direction.OppositeDirection(last_NextPlotDir)] = true; // ban dir. where it came from

            Direction.NESW dir;

            while (Direction.AllDirectionsBanned(bannedDirections) == false)
            {
                dir = Direction.RandomDirection(bannedDirections);

                if (Direction.EmptySpaceTowardsDir(plotPos, dir))
                {
                    nextPlotDir = dir;
                    break;
                }

                bannedDirections[(int)dir] = true;
            }
        }

        return nextPlotDir;
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


    private struct LastPlotData
    {
        public Vector3 plotPos;
        public Direction.NESW nextPlotDir;
        public Direction.NESW dirTowardsPlot;
    }
}

