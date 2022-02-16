using System.Collections;
using UnityEngine;

public static class Direction
{ 
    public enum NESW {None=-1, North, East, South, West};

    public static NESW RandomDirection()
    {
        int num = UnityEngine.Random.Range(0, 4);

        return (NESW)num;
    }

    public static NESW RandomDirection(NESW oppositeDirection)
    {
        NESW dir = oppositeDirection;

        while (dir == oppositeDirection)
        {
            dir = (NESW)Random.Range(0, 4);
        } 

        return dir;
    }

    public static NESW RandomDirection(BitArray bannedDirections)
    {
        if (AllDirectionsBanned(bannedDirections)) 
        {
            return NESW.None;
        }

        NESW dir = (NESW)Random.Range(0, 4);

        while (bannedDirections[(int)dir] == true)
        {
            dir = (NESW)Random.Range(0, 4);
        } 

        return dir;
    }

    public static bool AllDirectionsBanned(BitArray banDirs)
    { 
        foreach (bool dirBan in banDirs) 
        {
            if(dirBan == false) return false;
        }
        return true;
    }

    public static NESW OppositeDirection(NESW dir) 
    {
        NESW opposireDirection;

        switch (dir) 
        {
            case NESW.North:
                opposireDirection = NESW.South;
                break;
            case NESW.South:
                opposireDirection = NESW.North;
                break;
            case NESW.East:
                opposireDirection = NESW.West;
                break;
            case NESW.West:
                opposireDirection = NESW.East;
                break;
            default:
                opposireDirection = NESW.None;
                break;
        }

        return opposireDirection;
    }

    public static bool EmptySpaceTowardsDir(Vector3 plotPos, Direction.NESW dir)
    {
        return !Physics.CheckBox(Direction.PosTowardsDir(plotPos, dir, Plot.Instance.positionOffset), Vector3.one);
    }

    public static Vector3 PosTowardsDir(Vector3 pos, Direction.NESW dirTowardsSide,Vector3 positionOffset)
    {
        Vector3 newPos = Vector3.zero;

        switch (dirTowardsSide)
        {
            case Direction.NESW.North:
                newPos = pos + new Vector3(0, 0, positionOffset.z);
                break;
            case Direction.NESW.South:
                newPos = pos + new Vector3(0, 0, -positionOffset.z);
                break;
            case Direction.NESW.East:
                newPos = pos + new Vector3(positionOffset.x, 0, 0);
                break;
            case Direction.NESW.West:
                newPos = pos + new Vector3(-positionOffset.x, 0, 0);
                break;
        }

        return newPos;
    }


    public static Vector2Int Vec2PosOnSide(Direction.NESW dirTowardsSide, int sidePos)
    {
        Vector2Int vec2Pos = Vector2Int.zero;

        switch (dirTowardsSide)
        {
            case Direction.NESW.North:
                vec2Pos = new Vector2Int(sidePos, 4);
                break;
            case Direction.NESW.South:
                vec2Pos = new Vector2Int(sidePos, -4);
                break;
            case Direction.NESW.East:
                vec2Pos = new Vector2Int(4, sidePos);
                break;
            case Direction.NESW.West:
                vec2Pos = new Vector2Int(-4, sidePos);
                break;
        }

        return vec2Pos;
    }

    public static Vector2Int Vec2PosTowardsDir(Vector2Int vec2Pos, Direction.NESW dirTowardsSide)
    {
        Vector2Int newVec2Pos = Vector2Int.zero;

        switch (dirTowardsSide)
        {
            case Direction.NESW.North:
                newVec2Pos = vec2Pos + new Vector2Int(0, 1);
                break;
            case Direction.NESW.South:
                newVec2Pos = vec2Pos + new Vector2Int(0, -1);
                break;
            case Direction.NESW.East:
                newVec2Pos = vec2Pos + new Vector2Int(1, 0);
                break;
            case Direction.NESW.West:
                newVec2Pos = vec2Pos + new Vector2Int(-1, 0);
                break;
        }

        return newVec2Pos;
    }

    public static Direction.NESW Next_Clockwise_Dir(Direction.NESW dir)
    {
        return ((int)dir + 1 > 3) ? 0 : dir + 1;
    }

    public static Direction.NESW Next_Counter_Clockwise_Dir(Direction.NESW dir)
    {
        return ((int)dir - 1 < 0) ? (Direction.NESW)3 : dir - 1;
    }

}