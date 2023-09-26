using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : IState
{
    FSM _fsm;
    Cazador _cazador;
    

    public Patrol(FSM fsm,Cazador cazador)
    {
        _fsm = fsm;
        _cazador = cazador;
    }

    public void OnEnter()
    {
        Debug.Log("Se despierta");
        var randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * _cazador.maxForce;
        AddForce(randomDir);
    }
    public void OnUpdate()
    {
        
        _cazador.transform.position = GameManager.instance.ApplyBounds(_cazador.transform.position + _cazador.velocity * Time.deltaTime);
        _cazador.transform.forward += _cazador.velocity / Time.deltaTime;

        
        _cazador.energia -= Time.deltaTime;
        if (_cazador.energia <= 0)
            _fsm.ChangeState("Idle");

        foreach (var items in GameManager.instance.boids)
        {
            var dist = Vector3.Distance(items.transform.position, _cazador.transform.position);
            if (dist <= _cazador.viewDistance)
            {
                _cazador.target = items;
                _fsm.ChangeState("Chase");
            }
            
        }
    }
    public void OnExit()
    {

    }

    
    public void AddForce(Vector3 dir)
    {
        _cazador.velocity += dir;

        _cazador.velocity = Vector3.ClampMagnitude(_cazador.velocity, _cazador.maxVelocity);
    }
}
