using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Chase : IState
{
    FSM _fsm;
    Cazador _cazador;
    public Chase(FSM fsm, Cazador cazador) 
    {
        _fsm = fsm;
        _cazador = cazador;
    }

    public void OnEnter()
    {
        Debug.Log("Veo veo!");
    }
    public void OnUpdate()
    {

        if (_cazador.target != null)
        {
            AddForce(Pursuit(_cazador.target.transform.position + _cazador.target.velocity));
            _cazador.transform.position = GameManager.instance.ApplyBounds(_cazador.transform.position + _cazador.velocity * Time.deltaTime);
            _cazador.transform.forward += _cazador.velocity / Time.deltaTime;
        }
        else
        {
            _fsm.ChangeState("Patrol");
        }
        
        _cazador.energia -= Time.deltaTime;
        if (_cazador.energia <= 0)
            _fsm.ChangeState("Idle");
    }
    public void OnExit()
    {

    }
    Vector3 Pursuit(Vector3 dir)
    {
        return Seek(dir);
    }
    Vector3 Seek(Vector3 dir)
    {
        var desired = dir - _cazador.transform.position;
        desired.Normalize();
        desired *= _cazador.maxVelocity;
        
        var steering = desired - _cazador.velocity;
        steering = Vector3.ClampMagnitude(steering, _cazador.maxForce);

        return steering;
    }
    void AddForce(Vector3 dir)
    {
        _cazador.velocity += dir;
        //Debug.Log(_cazador.velocity);
        _cazador.velocity = Vector3.ClampMagnitude(_cazador.velocity, _cazador.maxVelocity);
    }
}
