using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Parcial2Manager : MonoBehaviour
{
    public static Parcial2Manager instance;

    public Jugador jugador;
    public Nodo[] nodos;
    public List<Guardia> guardias = new List<Guardia>();
    [SerializeField] LayerMask _maskWall;

    public List<Nodo> prueba = new List<Nodo>();

    private void Awake()
    {
        if (instance == null)
        instance = this; 
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _maskWall);
    }
    public void Call(Guardia e)
    {
        foreach (Guardia guardia in guardias)
        {
            if(guardia != e)
            {
                guardia.Alerta();
            }
        }
    }

    public Nodo GetMinNode(Vector3 position)
    {
        print("Funciono");
        Nodo minNode = null;
        float minDist = Mathf.Infinity;

        for (int i = 0; i < nodos.Length; i++)
        {
            if (InLineOfSight(nodos[i].transform.position, position))
            {
                if (Vector3.Distance(nodos[i].transform.position, position) < minDist)
                {
                    
                    minNode = nodos[i];
                    minDist = Vector3.Distance(nodos[i].transform.position, position);
                    
                }
            }
        }

        return minNode;
    }

    public List<Nodo> CalculateAStar(Nodo startingNode, Nodo goalNode)
    {
        print("Calculo A estrella");
        PriorityQueue<Nodo> frontier = new PriorityQueue<Nodo>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Nodo, Nodo> cameFrom = new Dictionary<Nodo, Nodo>();
        cameFrom.Add(startingNode, null);

        Dictionary<Nodo, int> costSoFar = new Dictionary<Nodo, int>();
        costSoFar.Add(startingNode, 0);
        
        while (frontier.Count > 0)
        {
            Nodo current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Nodo> path = new List<Nodo>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                    
                }

                path.Reverse();
                return path;
            }

            foreach (var item in current.nodosVecinos)
            {
                print(current);
                int newCost = costSoFar[current] + item.cost;
                float priority = newCost + Vector3.Distance(item.transform.position, goalNode.transform.position);

                if (!costSoFar.ContainsKey(item))
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom.Add(item, current);
                    costSoFar.Add(item, newCost);
                }
                else if (costSoFar[item] > newCost)
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }

                
            }
        }
        
        
        
        return new List<Nodo>();
    }

    public void SetPath(List<Nodo> newPath)
    {
        
        prueba.Clear();

        foreach (var item in newPath)
        {
            print("Agrego item a la lista");
            prueba.Add(item);
        }


    }
}
    


