using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuardiaLostView : IState
{
    FSM _fsm;

    float _viewRadius;
    float _viewAngle;
    Transform _transform;

    float _currCooldown;

    public GuardiaLostView(FSM fsm, float viewRadius, float viewAngle, Transform transform)
    {
        _fsm = fsm;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _transform = transform;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        _currCooldown += Time.deltaTime;
        if(_currCooldown >= 4)
        {
            _fsm.ChangeState("Back to patrol");
        }

        if(InFOV(Parcial2Manager.instance.jugador.transform))
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
}
