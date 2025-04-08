using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }
    public TileSpot spot { get; private set; }
    public int number { get; private set; }
    
    private Image background;
    private TextMeshProUGUI text;
    Random random = new Random();

    private Dictionary<int, List<string>> equations = new Dictionary<int, List<string>>
    {
        {1, new List<string>{"\u221a1", "3\u2070", "\u00b2\u2044\u2082", "|\u22121|", "ln(e)"}},
        {2, new List<string>{"\u221a4", "2\u00b9", "|\u22122|", "log\u2081\u2080(100)", "4\u00f72"}},
        {3, new List<string>{""}},
        {5, new List<string>{""}},
        {8, new List<string>{""}},
        {13, new List<string>{""}},
        {21, new List<string>{""}},
        {34, new List<string>{""}},
        {10, new List<string>{""}},
        {11, new List<string>{""}},
        {12, new List<string>{""}},
        {13, new List<string>{""}},
        {14, new List<string>{""}},
        {15, new List<string>{""}},
        {16, new List<string>{""}},

    };

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetState(TileState state, int number)
    {
        this.state = state;
        this.number = number;
        background.color = state.backgroundColor;
        text.text = number.ToString();
        //text.text = equations[number][random.Next(0, equations[number].Count)];
    }
    

    public void Generate(TileSpot spot)
    {
        if (this.spot != null)
        {
            this.spot.tile = null;
        }
            
        this.spot = spot;
        this.spot.tile = this;
        transform.position = spot.transform.position;
    }

    public void MoveTo(TileSpot spot)
    {
        if (this.spot != null)
        {
            this.spot.tile = null;
        }
        this.spot = spot;
        this.spot.tile = this;
        StartCoroutine(Animate(spot.transform.position, false));
    }

    private IEnumerator Animate(Vector3 to, bool merge)
    {
        float time = 0f;
        float duration = 0.2f;
        Vector3 from = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
        if (merge)
        {
            Destroy(gameObject);
        }
    }

    public void Merge(TileSpot spot)
    {
        if (this.spot != null)
        {
            this.spot.tile = null;
        }

        this.spot = null;
        
        StartCoroutine(Animate(spot.transform.position, true));
        
    }
    
    
}
