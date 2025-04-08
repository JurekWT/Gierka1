using System;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows {get; private set;}
    public TileSpot[] spots {get; private set;}
    
    public int size => spots.Length;
    public int height => rows.Length;
    public int width => size / height;
    
    

    public void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        spots = GetComponentsInChildren<TileSpot>();
    }

    private void Start()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].spots.Length; x++)
            {
                rows[y].spots[x].position = new Vector2Int(x, y);
            }
        }
    }

    public TileSpot GetSpot(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].spots[x];
        }
        else
        {
            return null;
        }
    }

    public TileSpot RandomEmptySpot()
    {
        int index = UnityEngine.Random.Range(0, spots.Length);
        int startingIndex = index;
        while (spots[index].occupied)
        {
            index++;
            if (index >= spots.Length)
            {
                index = 0;
            }

            if (index == startingIndex)
            {
                return null;
            }
        }
        return spots[index];
    }

    public TileSpot GetSpot(Vector2Int position)
    {
        return GetSpot(position.x, position.y);
    }

    public TileSpot GetNearestSpot(TileSpot spot, Vector2Int direction)
    {
        Vector2Int positon = spot.position;
        positon.x += direction.x;
        positon.y -= direction.y;
        
        return GetSpot(positon);
    }
}
