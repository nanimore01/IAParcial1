using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcial2Manager : MonoBehaviour
{
    public static Parcial2Manager instance;

    public Jugador jugador;
    public List<Nodo> nodos = new List<Nodo>();
    public List<Guardia> guardias = new List<Guardia>();



    private void Awake()
    {
        if (instance == null)
        instance = this; 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
