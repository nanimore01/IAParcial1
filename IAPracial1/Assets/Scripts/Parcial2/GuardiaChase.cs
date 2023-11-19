using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuardiaChase : IState
{
    FSM _fsm;

    Transform _transform;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    float _viewRadius;
    float _viewAngle;

    public GuardiaChase(FSM fsm, Transform transform, Vector3 velocity, float maxVelocity, float maxForce, float viewRadius, float viewAngle)
    {
        _fsm = fsm;
        _transform = transform;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        AddForce(Seek(Parcial2Manager.instance.jugador.transform.position));

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;

        if (InFOV(Parcial2Manager.instance.jugador.transform) == false) 
        {
            Debug.Log("Te perdi de vista");
            _fsm.ChangeState("Lost view");
        }
       
    }

    public bool InFOV(Transform obj)
    {
        var dir = obj.position - _transform.position;

        if (dir.magnitude < _viewRadius)
        {
            if (Vector3.Angle(_transform.forward, dir) <= _viewAngle * 0.5f)
            {
                return Parcial2Manager.instance.InLineOfSight(_transform.position, obj.position);
            }
        }

        return false;
    }

    Vector3 Seek(Vector3 dir)
    {
        var desired = dir - _transform.position;
        desired.Normalize();
        desired *= _maxVelocity;

        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }

    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    }
}
