using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    private TileGrid grid;
    private List<Tile> tiles;
    public TileState[] tileStates;
    private bool wait;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }

    private void Start()
    {
        CreateTiles();
        CreateTiles();
    }

    private void CreateTiles()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], 1);
        tile.Generate(grid.RandomEmptySpot());
        tiles.Add(tile);
    }

    private void Update()
    {

        if (!wait)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Vector2Int.left, 1, 1, 0, 1);
            }
        }
    }

    public void Move(Vector2Int position, int startX, int incrementX, int startY, int incrementY)
    {
        bool done = false;
        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                TileSpot spot = grid.GetSpot(x, y);
                if (spot.occupied)
                {
                    done |= MoveTile(spot.tile, position);
                }
            }
        }

        if (done)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int position)
    {
        TileSpot nSpot = null;
        TileSpot stickSpot = grid.GetNearestSpot(tile.spot, position);

        while (stickSpot != null)
        {
            if (stickSpot.occupied)
            {
                if (IsMergingPossible(tile, stickSpot.tile))
                {
                    Merging(tile, stickSpot.tile);
                    return true;
                }

                break;
            }

            nSpot = stickSpot;
            stickSpot = grid.GetNearestSpot(stickSpot, position);
        }

        if (nSpot != null)
        {
            tile.MoveTo(nSpot);
            return true;
        }
        return false;
    }

    private IEnumerator WaitForChanges()
    {
        wait = true;
        yield return new WaitForSeconds(0.2f);
        wait = false;

        if (tiles.Count != grid.size)
        {
            CreateTiles();
        }
    }

    private bool IsMergingPossible(Tile first, Tile second)
    {
        int indexFirst = IndexInFibonacciSequence(first.number);
        int indexSecond = IndexInFibonacciSequence(second.number);
        if ((indexFirst == 2 && indexSecond == 2) || (Math.Abs(indexSecond - indexFirst) == 1))
        {
            return true;
        }
        return false;
    }

    private void Merging(Tile first, Tile second)
    {
        tiles.Remove(first);
        first.Merge(second.spot);
        int index = Mathf.Clamp(IndexOf(second.state) + 1, 0, tileStates.Length - 1);
        int number;
        if (second.number == 1)
        {
            number = 2;
        }
        else
        {
            if (IndexInFibonacciSequence(second.number) > IndexInFibonacciSequence(first.number))
            {
                number = GetFibonacciNumber(IndexInFibonacciSequence(second.number) + 1);
            }
            else
            {
                number = GetFibonacciNumber(IndexInFibonacciSequence(second.number) + 1);
            }
            
        }
        second.SetState(tileStates[index], number);
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i]) 
            {
                return i;
            }
        }

        return -1;
    }

    private int GetFibonacciNumber(int number)
    {
        if (number <= 0)
        {
            return 0;
        }
        else if (number == 1)
        {
            return 1;
        }
        return GetFibonacciNumber(number - 1) + GetFibonacciNumber(number - 2);
    }
    
    public int IndexInFibonacciSequence(int number)
    {
        int a = 1, b = 1, index = 2;

        while (b <= number)
        {
            if (b == number) return index;

            int temp = b;
            b = a + b;
            a = temp;
            index++;
        }

        return -1;
    }
}
