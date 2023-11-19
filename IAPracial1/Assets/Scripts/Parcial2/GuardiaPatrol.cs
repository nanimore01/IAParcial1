using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardiaPatrol : IState
{
    FSM _fsm;

    Guardia _guardia;
    Nodo[] _patrol;
    int _currWaypoint = 0;
    Transform _transform;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    float _viewRadius;
    float _viewAngle;
    Collider _collider;

    public GuardiaPatrol(FSM fsm, Nodo[] patrol, Transform transform, Vector3 velocity, float maxVelocity, float maxForce, Guardia guardia, float viewRadius, float viewAngle, int currWaypoint, Collider collider)
    {
        _fsm = fsm;
        _patrol = patrol;
        _transform = transform;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _guardia = guardia;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _currWaypoint = currWaypoint;
        _collider = collider;   
    }

    public void OnEnter()
    {
        _collider.isTrigger = true;
    }

    public void OnExit()
    {
        _collider.isTrigger = false;
    }

    public void OnUpdate()
    {
        AddForce(Seek(_patrol[_currWaypoint].transform.position));

        if (Vector3.Distance(_patrol[_currWaypoint].transform.position, _transform.position) <= 0.5f)
        {
            _currWaypoint++;

            if (_currWaypoint >= _patrol.Length)
                _currWaypoint = 0;
        }

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;


        if(InFOV(Parcial2Manager.instance.jugador.transform))
        {
            
            _fsm.ChangeState("Chase");
            Parcial2Manager.instance.Call(_guardia);
            
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

