using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Jugador : MonoBehaviour
{
    [SerializeField] float _velocidad; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0) 
        {
            Move(v, h);
        }
    }

    public void Move(float moveX, float moveY)
    {
        Vector3 dir = transform.up * moveX;
        dir += transform.right * moveY;

        transform.position += dir.normalized * _velocidad * Time.deltaTime;
    }
}
