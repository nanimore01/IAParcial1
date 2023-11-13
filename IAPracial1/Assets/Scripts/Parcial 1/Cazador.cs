using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cazador : MonoBehaviour
{
    FSM _fsm;
    public float energia = 100;
    public float viewDistance = 2;
    public float maxVelocity;
    public float maxForce;
    public Vector3 velocity;
    public Boid target;

    void Start()
    {
        _fsm = new FSM();

        _fsm.CreateState("Idle", new Idle(_fsm, this));
        _fsm.CreateState("Chase",new Chase(_fsm, this));
        _fsm.CreateState("Patrol", new Patrol(_fsm, this));

        _fsm.ChangeState("Patrol");
    }
    private void Update()
    {
        _fsm.Execute();
        
    }
}
