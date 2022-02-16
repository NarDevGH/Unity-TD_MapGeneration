using System;
using System.Collections;
using UnityEngine;



public class Map_Generator_GridBased : MonoBehaviour
{
    private const int MAX_MAP_SIZE = 51;

    [SerializeField] private GameObject _plot;


    private bool[,] _occupiedPosMatrix = new bool[MAX_MAP_SIZE, MAX_MAP_SIZE];
    private LastPlotData _lastPlotData;

    private Vector3 _positionOffset;
    public static Map_Generator_GridBased Instance;

    private void Awake()
    {
        HandleInstances();

        _positionOffset = new Vector3(_plot.transform.localScale.x, 0f, _plot.transform.localScale.z);
    }

    private void Start()
    {
        Generate_Starting_Plot();
    }

    public void Generate_Plot()
    {
        Direction.NESW nextPlotDir = NextPlotDir();

        if (nextPlotDir == Direction.NESW.None)
            return;

        Vector3 plotPos = PlotPosTowardsDir(nextPlotDir);
        GameObject plot = Instantiate(_plot, plotPos, _plot.transform.rotation);

        _lastPlotData.matrixPos = MatrixPosTowardsDir(_lastPlotData.matrixPos, nextPlotDir);

        _lastPlotData.plotPos = plot.transform.position;
        _lastPlotData.dirToLastPlot = Direction.OppositeDirection(nextPlotDir);

        _occupiedPosMatrix[_lastPlotData.matrixPos.x, _lastPlotData.matrixPos.y] = true;
    }

    private void Generate_Starting_Plot()
    {
        GameObject startingPlot = Instantiate(_plot);

        _lastPlotData.plotPos = startingPlot.transform.position;
        _lastPlotData.dirToLastPlot = Direction.NESW.None;

        int pos = (int)Math.Ceiling(MAX_MAP_SIZE / 2f);
        _lastPlotData.matrixPos = new Vector2Int(pos, pos);
        _occupiedPosMatrix[pos, pos] = true;
    }

    private Direction.NESW NextPlotDir()
    {
        Direction.NESW nextPlotDir = Direction.NESW.None;

        if (_lastPlotData.dirToLastPlot == Direction.NESW.None)
        {
            nextPlotDir = Direction.RandomDirection();
        }
        else
        {
            BitArray banDirs = new BitArray(4);
            banDirs[(int)_lastPlotData.dirToLastPlot] = true;

            for (int i = 0; i <= 2; i++)
            {
                var dir = Direction.RandomDirection(banDirs);
                Vector2Int pos = MatrixPosTowardsDir(_lastPlotData.matrixPos, dir);

                if (_occupiedPosMatrix[pos.x, pos.y] == false)
                {
                    nextPlotDir = dir;
                    break;
                }

                banDirs[(int)dir] = true;

                if (i == 2) 
                {
                    Debug.Log("Reached a Dead End");
                }
            }
                
        }
        return nextPlotDir;
    }

    private Vector3 PlotPosTowardsDir(Direction.NESW dir)
    {
        Vector3 plotPos = Vector3.zero;

        switch (dir)
        {
            case Direction.NESW.North:
                plotPos = _lastPlotData.plotPos + Vector3.forward * _positionOffset.z;
                break;
            case Direction.NESW.South:
                plotPos = _lastPlotData.plotPos + Vector3.back * _positionOffset.z;
                break;
            case Direction.NESW.East:
                plotPos = _lastPlotData.plotPos + Vector3.left * _positionOffset.x;
                break;
            case Direction.NESW.West:
                plotPos = _lastPlotData.plotPos + Vector3.right * _positionOffset.x;
                break;
        }

        return plotPos;
    }

    private Vector2Int MatrixPosTowardsDir(Vector2Int matrixPos, Direction.NESW dir)
    {
        Vector2Int nextPlotMatrixPos;
        switch (dir)
        {
            case Direction.NESW.North:
                nextPlotMatrixPos = matrixPos;
                nextPlotMatrixPos.y += 1;
                break;

            case Direction.NESW.South:
                nextPlotMatrixPos = matrixPos;
                nextPlotMatrixPos.y -= 1;
                break;

            case Direction.NESW.East:
                nextPlotMatrixPos = matrixPos;
                nextPlotMatrixPos.x += 1;
                break;

            case Direction.NESW.West:
                nextPlotMatrixPos = matrixPos;
                nextPlotMatrixPos.x -= 1;
                break;
            default:
                nextPlotMatrixPos = Vector2Int.zero;
                break;
        }

        if (nextPlotMatrixPos.x > MAX_MAP_SIZE) nextPlotMatrixPos.x = MAX_MAP_SIZE;
        if (nextPlotMatrixPos.x < 0) nextPlotMatrixPos.x = 0;
        if (nextPlotMatrixPos.y > MAX_MAP_SIZE) nextPlotMatrixPos.y = MAX_MAP_SIZE;
        if (nextPlotMatrixPos.y < 0) nextPlotMatrixPos.y = 0;

        return nextPlotMatrixPos;
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
        public Direction.NESW dirToLastPlot;
        public Vector2Int matrixPos;
    }
}

