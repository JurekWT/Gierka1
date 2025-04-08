using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileSpot[] spots { get; private set; }

    private void Awake()
    {
        spots = GetComponentsInChildren<TileSpot>();
    }
}
