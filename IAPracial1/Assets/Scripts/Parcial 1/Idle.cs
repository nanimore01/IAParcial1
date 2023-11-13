using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Idle : IState
{
    FSM _fsm;
    Cazador _cazador;
    
    public Idle(FSM fsm, Cazador cazador)
    {
        _fsm = fsm;
        _cazador = cazador;
    }
    public void OnEnter()
    {
        Debug.Log("En reposo");
    }
    public void OnUpdate()
    {
        _cazador.energia += Time.deltaTime;
        if(_cazador.energia >= 100)
            _fsm.ChangeState("Patrol");
    }
    public void OnExit()
    {
        Debug.Log("Fin del descanso");
    }

}
