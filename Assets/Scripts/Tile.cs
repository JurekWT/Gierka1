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
        {1, new List<string>{"\u221a1", "3\u2070", "\u00b2\u2044\u2082", "|\u22121|", "ln(e)", "1\u00d71", "2\u00d70.5"}},
        {2, new List<string>{"\u221a4", "2\u00b9", "|\u22122|", "log\u2081\u2080(100)", "4\u00f72"}},
        {3, new List<string>{"\u221a9", "3\u00b9", "|\u22123|", "2\u00b2\u22121", "2\u00b3 \u2212 5", "3! \u00f7 2"}},
        {5, new List<string>{"\u221a25", "2\u00b3\u2212 3", "3\u00b2\u2212 4", "|\u22125|", "5!\u00f724", "log\u2082(32)", "ln(e\u2075)"}},
        {8, new List<string>{"2\u00b3", "\u221a64", "4\u00b2 \u00f7 2", "|\u22128|", "3!+2", "log\u2082(256)", "ln(e\u2078)"}},
        {13, new List<string>{"\u221a169", "4\u00b2-3", "|-13|", "16-\u221a9", "ln(e\u00b9\u00b3)", "log\u2082(8192)", "3!+7"}},
        {21, new List<string>{"3\u00b3-6", "\u221a441", "7^(log\u208721)", "84\u00f74"}},
        {34, new List<string>{"5\u00b2+9", "2\u2075+2", "\u221a1156", "5\u00b2+3\u00b2", "6\u00b2-2"}},
        {55, new List<string>{"7\u00b2+6", "110\u00f72", "2\u2076-9", "4\u00b3+9", "8\u00b2-9"}},
        {89, new List<string>{"9\u00b2+8", "44.5\u00d72", "34+55", "11\u00b2-32", "4\u00b3+5\u00b2", "5!-31"}},
        {144, new List<string>{"288\u00f72", "13\u00b2\u221225", "11\u00b2+23", "12\u00b2", "2\u2074\u00d73\u00b2"}},
        {233, new List<string>{"466\u00f72", "15\u00b2+8", "3\u2075-10"}},
        {377, new List<string>{"13\u00d729", "754\u00f72", "20\u00b2-23", "3\u2075+50"}},
        {610, new List<string>{"61\u00d710", "1220\u00f72", "5\u2074-15"}},
        {987, new List<string>{"47\u00d721", "1974\u00f72", "3\u2076+3\u2075\u22123\u00b3", "5\u2074+5\u00b3\u22123"}},
        {1597, new List<string>{"3194\u00f72", "3\u2077-590", "34\u00b2+21\u00b2"}},
        {2584, new List<string>{"38\u00d768", "5168\u00f72", "51\u00b2-17", "3\u2077+397"}}
    };

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetState(TileState state, int number, string difficulty)
    {
        this.state = state;
        this.number = number;
        if (difficulty == "Easy")
        {
            background.color = state.backgroundColor;
            text.text = number.ToString();
        }
        else if (difficulty == "Medium")
        {
            background.color = state.backgroundColor;
            text.text = equations[number][random.Next(0, equations[number].Count)];
        }
        else if (difficulty == "Hard")
        {
            text.text = equations[number][random.Next(0, equations[number].Count)];
        }
        //background.color = state.backgroundColor;
        //text.text = number.ToString();
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
