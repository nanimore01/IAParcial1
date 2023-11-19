using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GuardiaAlerted : IState
{
    FSM _fsm;

    float _viewRadius;
    float _viewAngle;
    Transform _transform;
    Vector3 _velocity;
    float _maxVelocity;
    float _maxForce;
    Collider _collider;

    List<Nodo> _path;
    

    public GuardiaAlerted(FSM fsm, float viewRadius, float viewAngle, Transform transform, Vector3 velocity, float maxVelocity, float maxForce, List<Nodo> path, Collider collider)
    {
        _fsm = fsm;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _transform = transform;
        _velocity = velocity;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _path = path;
        _collider = collider;
    }

    public void OnEnter()
    {
        _collider.enabled = false;
        var Gamemanager = Parcial2Manager.instance;
        SetPath(Gamemanager.CalculateAStar(Gamemanager.GetMinNode(_transform.position), Gamemanager.GetMinNode(Gamemanager.jugador.transform.position)));
    }

    public void OnExit()
    {
        _path.Clear();
    }

    public void OnUpdate()
    {
        
        if (_path.Count > 0)
        {
            var dir = _path[0].transform.position - _transform.position;

            AddForce(Seek(_path[0].transform.position));

            if (dir.magnitude <= 0.5f)
            {
                _path.RemoveAt(0);
            }
        }

        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;
        

        if(_path.Count == 0)
        {
            _fsm.ChangeState("Lost view");
        }

        if (InFOV(Parcial2Manager.instance.jugador.transform))
        {
            _fsm.ChangeState("Chase");
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

    public void SetPath(List<Nodo> newPath)
    {

        _path.Clear();

        foreach (var item in newPath)
        {
            
            _path.Add(item);
        }


    }

}
