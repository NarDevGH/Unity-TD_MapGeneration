using System.Collections;
using UnityEngine;

public class PlotGenerator_PhysicsBased_TurnAvoidance : MonoBehaviour
{

    private int _consecutivesTurns = 0;             
    // amount of times the next plot dir it was a consecutive clockwise turn, or
    // it was a consecutive counter clockwise turn

    private LastPlotData _lastPlotData;

    public static PlotGenerator_PhysicsBased_TurnAvoidance Instance;

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

            BanConsecutiveTurnDir(bannedDirections);

            while (Direction.AllDirectionsBanned(bannedDirections) == false)
            {
                dir = Direction.RandomDirection(bannedDirections);

                if (Direction.EmptySpaceTowardsDir(plotPos,dir))
                {
                    nextPlotDir = dir;
                    break;
                }

                bannedDirections[(int)dir] = true;
            }
        }
        CheckTurns(nextPlotDir);

        return nextPlotDir;
    }


    private void CheckTurns(Direction.NESW nextPlotDir)
    {
        if (nextPlotDir != _lastPlotData.nextPlotDir && _lastPlotData.nextPlotDir != Direction.NESW.None)
        {
            _consecutivesTurns += TurnValue(nextPlotDir, _lastPlotData.nextPlotDir);
        }
        else
        {
            _consecutivesTurns = 0;
        }
    }

    private void BanConsecutiveTurnDir(BitArray banDirs)
    {
        if (_consecutivesTurns == 1)
        {
            banDirs[(int)Direction.Next_Clockwise_Dir(Direction.OppositeDirection(_lastPlotData.dirTowardsPlot))] = true;
            _consecutivesTurns = 0;
        }
        else if (_consecutivesTurns == -1) 
        {
            banDirs[(int)Direction.Next_Counter_Clockwise_Dir(Direction.OppositeDirection(_lastPlotData.dirTowardsPlot))] = true;
            _consecutivesTurns = 0;
        }
    }

    private int TurnValue(Direction.NESW nextPlotDir, Direction.NESW lastPlotDir)
    {
        int value = 0;

        if (lastPlotDir == Direction.NESW.North)
        {
            if (nextPlotDir == Direction.NESW.West) value = -1;
            else if (nextPlotDir == Direction.NESW.East) value = 1;
        }
        else if (lastPlotDir == Direction.NESW.South)
        {
            if (nextPlotDir == Direction.NESW.West) value = 1;
            else if (nextPlotDir == Direction.NESW.East) value = -1;
        }
        else if (lastPlotDir == Direction.NESW.East)
        {
            if (nextPlotDir == Direction.NESW.North) value = -1;
            else if (nextPlotDir == Direction.NESW.South) value = 1;
        }
        else 
        {
            if (nextPlotDir == Direction.NESW.North) value = 1;
            else if (nextPlotDir == Direction.NESW.South) value = -1;
        }
        return value;
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

