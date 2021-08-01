using UnityEngine;

public class Cell: MonoBehaviour
{
    public Cell(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2Int ToVec()
    {
        return new Vector2Int(x, y);
    }
    
    public int x = 0;
    public int y = 0;
}