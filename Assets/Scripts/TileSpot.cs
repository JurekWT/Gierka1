using UnityEngine;

public class TileSpot : MonoBehaviour
{
    public Vector2Int position { get; set; }
    public Tile tile { get; set; }
    
    public bool empty => tile == null;
    public bool occupied => tile != null;
}
