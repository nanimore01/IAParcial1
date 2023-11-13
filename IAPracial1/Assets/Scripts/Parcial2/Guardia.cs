using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardia : MonoBehaviour
{
    public List<Nodo> Patrullaje = new List<Nodo>();
    void Start()
    {
        Parcial2Manager.instance.guardias.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
