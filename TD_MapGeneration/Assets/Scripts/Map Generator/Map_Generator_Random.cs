using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Map_Generator_Random : MonoBehaviour
{
    [SerializeField] private GameObject _castle;

    private LastPlotData _lastPlotData;
    public static Map_Generator_Random Instance;

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

        GameObject plot = Plot.Instance.Generate_Plot_Towards_Dir(_lastPlotData.plotPos,_lastPlotData.nextPlotDir);

        Direction.NESW nextPlotDir = NextPlotDir(plot.transform.position,_lastPlotData.nextPlotDir);

        List<Vector2Int> pathPositions = new List<Vector2Int>();
        Vector2Int pathStart = Direction.Vec2PosOnSide(_lastPlotData.dirTowardsPlot,_lastPlotData.pathEndSidePos);
        int newSideEndPos = UnityEngine.Random.Range(-3, 4);
        Vector2Int pathEnd = Direction.Vec2PosOnSide(nextPlotDir,newSideEndPos);
        pathPositions = Plot.Instance.PathThroughPlot(pathStart, pathEnd);

        Grass.Instance.Generate_Grass(plot.transform,pathPositions);

        
        _lastPlotData.plotPos = plot.transform.position;
        _lastPlotData.nextPlotDir = nextPlotDir;
        _lastPlotData.dirTowardsPlot = Direction.OppositeDirection(nextPlotDir);
        _lastPlotData.pathEndSidePos = newSideEndPos;
    }



    private void Generate_Starting_Plot()
    {
        GameObject startingPlot = Plot.Instance.Generate_Plot();
        Instantiate(_castle);
        Direction.NESW nextPlotDir = Direction.RandomDirection();

        List<Vector2Int> pathPositions = new List<Vector2Int>();

        #region Add PathPosition
        switch (nextPlotDir)
        {
            case Direction.NESW.North:
                pathPositions.Add(new Vector2Int(0, 4));
                pathPositions.Add(new Vector2Int(0, 3));
                pathPositions.Add(new Vector2Int(0, 2));
                pathPositions.Add(new Vector2Int(0, 1));
                break;
            case Direction.NESW.South:
                pathPositions.Add(new Vector2Int(0, -4));
                pathPositions.Add(new Vector2Int(0, -3));
                pathPositions.Add(new Vector2Int(0, -2));
                pathPositions.Add(new Vector2Int(0, -1));
                break;
            case Direction.NESW.East:
                pathPositions.Add(new Vector2Int(1, 0));
                pathPositions.Add(new Vector2Int(2, 0));
                pathPositions.Add(new Vector2Int(3, 0));
                pathPositions.Add(new Vector2Int(4, 0));
                break;
            case Direction.NESW.West:
                pathPositions.Add(new Vector2Int(-1, 0));
                pathPositions.Add(new Vector2Int(-2, 0));
                pathPositions.Add(new Vector2Int(-3, 0));
                pathPositions.Add(new Vector2Int(-4, 0));
                break;
        }
        #endregion

        Grass.Instance.Generate_Grass(startingPlot.transform,pathPositions);

        _lastPlotData.plotPos = startingPlot.transform.position;
        _lastPlotData.nextPlotDir = nextPlotDir;
        _lastPlotData.dirTowardsPlot = Direction.OppositeDirection(nextPlotDir);
        _lastPlotData.pathEndSidePos = 0;
    }

    public Direction.NESW NextPlotDir(Vector3 plotPos, Direction.NESW last_NextPlotDir)
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
        public int pathEndSidePos;
    }
}
