using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo : MonoBehaviour
{
    public List<Nodo> nodosVecinos = new List<Nodo>();

    void Start()
    {
        Parcial2Manager.instance.nodos.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
