using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CalculateIngredients : MonoBehaviour
{
    public List<StickyBurger> ingreds;
    public List<IngredsConnection> connections;
    List<Connectable> allowed_connections;
    public Dictionary<string, float> stuff;
    public Dictionary<string, float> norms;

    public void Calc(out float rating, out float proportions)
    {
        rating = 100;
        proportions = 0;
        ingreds.Clear();
        connections.Clear();
        GetComponent<StickyBurger>().recursive_calc();
        ingreds = ingreds.Distinct().ToList();
        connections = connections.Distinct().ToList();
        stuff.Clear();
        foreach (StickyBurger sb in ingreds)
        {
            if (stuff.ContainsKey(sb.type))
            {
                stuff[sb.type] += sb.amount;
            }
            else
            {
                stuff.Add(sb.type, sb.amount);
            }
        }
        foreach (string tp in norms.Keys)
        {
            if (stuff.ContainsKey(tp))
            {
                proportions += 9 * Mathf.Min(stuff[tp], norms[tp]) / Mathf.Max(stuff[tp], norms[tp]);
            }
            
        }
        int wrong = 0;
        foreach (IngredsConnection ic in connections)
        {
            if(!allowed_connections.Any(x => x.first == ic.first.type && x.second == ic.second.type))
                wrong += 1;
        }
        rating -= wrong / 3 / connections.Count();
    }
    // Start is called before the first frame update
    void Start()
    {
        ingreds = new List<StickyBurger>();
        connections = new List<IngredsConnection>();

        //setting allowed order in a burger
        allowed_connections = new List<Connectable>();
        allowed_connections.Add(new Connectable("plate", "bottom bun"));
        allowed_connections.Add(new Connectable("bottom bun", "patty"));
        allowed_connections.Add(new Connectable("patty", "lettuce"));
        allowed_connections.Add(new Connectable("lettuce", "cheese"));
        allowed_connections.Add(new Connectable("cheese", "onion"));
        allowed_connections.Add(new Connectable("cheese", "tomato"));
        allowed_connections.Add(new Connectable("onion", "tomato"));
        allowed_connections.Add(new Connectable("onion", "onion"));
        allowed_connections.Add(new Connectable("tomato", "tomato"));
        allowed_connections.Add(new Connectable("tomato", "ketchup"));
        allowed_connections.Add(new Connectable("tomato", "mustard"));
        allowed_connections.Add(new Connectable("ketchup", "mustard"));
        allowed_connections.Add(new Connectable("ketchup", "pickle"));
        allowed_connections.Add(new Connectable("mustard", "pickle"));
        allowed_connections.Add(new Connectable("ketchup", "top bun"));
        allowed_connections.Add(new Connectable("mustard", "top bun"));
        allowed_connections.Add(new Connectable("pickle", "top bun"));

        //setting amounts of stuff in a burger
        stuff = new Dictionary<string, float>();
        norms = new Dictionary<string, float>();
        norms.Add("plate",1);
        norms.Add("bottom bun", 1);
        norms.Add("patty", 1);
        norms.Add("lettuce", 1);
        norms.Add("cheese", 1);
        norms.Add("onion", 35f);
        norms.Add("tomato", 0.012f);
        norms.Add("ketchup", 1);
        norms.Add("mustard", 1);
        norms.Add("pickle", 0.9f);
        norms.Add("top bun", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

class Connectable
{
    public string first;
    public string second;
    public Connectable(string a, string b)
    {
        first = a;
        second = b;
    }
}
