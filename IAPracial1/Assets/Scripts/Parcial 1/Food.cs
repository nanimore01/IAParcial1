using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Food : MonoBehaviour
{
    [Header("Caracteristicas de la comida")]

    public float radiusFood;

    private void Update()
    {
        foreach(var item in GameManager.instance.boids) 
        {
            var dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < radiusFood)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
