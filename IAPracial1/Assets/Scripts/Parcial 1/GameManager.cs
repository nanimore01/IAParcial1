using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Boid> boids = new List<Boid>();
    public Food food;
    public Cazador cazador;
    public int width, height;
    

    private void Awake()
    {
        instance = this;
    }

    public Vector3 ApplyBounds(Vector3 pos)
    {
        if (pos.x > width)
            pos.x = -width;

        if (pos.x < -width)
            pos.x = width;

        if (pos.z > height)
            pos.z = -height;

        if (pos.z < -height)
            pos.z = height;

        return pos;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 topLeft = new Vector3(-width, 0, height);
        Vector3 topRight = new Vector3(width, 0, height);
        Vector3 downRight = new Vector3(width, 0, -height);
        Vector3 downLeft = new Vector3(-width, 0, -height);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, downRight);
        Gizmos.DrawLine(downRight, downLeft);
        Gizmos.DrawLine(downLeft, topLeft);
    }
}
