using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Guardia : MonoBehaviour
{
    FSM _fsm;

    [Header("Stats")]
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;
    [SerializeField] Collider _collider;
    [SerializeField] float _viewRadius;
    [SerializeField] float _viewAngle;
    [SerializeField] int _currWaypoint;
    Vector3 _velocity;
    public Vector3 Velocity
    {
        get { return _velocity; }
    }
    [Header("Listas")]
    public Nodo[] patrullaje;
    public List<Nodo> _path;



    void Start()
    {
        _collider = GetComponent<Collider>();
        Parcial2Manager.instance.guardias.Add(this);

        _fsm = new FSM();

        _fsm.CreateState("Patrol", new GuardiaPatrol(_fsm, patrullaje, transform, _velocity, _maxVelocity, _maxForce, this, _viewRadius, _viewAngle, _currWaypoint, _collider));
        _fsm.CreateState("Chase", new GuardiaChase(_fsm, transform, _velocity, _maxVelocity, _maxForce, _viewRadius, _viewAngle));
        _fsm.CreateState("Lost view", new GuardiaLostView(_fsm, _viewRadius, _viewAngle, transform));
        _fsm.CreateState("Alerted", new GuardiaAlerted(_fsm, _viewRadius, _viewAngle, transform, _velocity, _maxVelocity, _maxForce, _path, _collider));
        _fsm.CreateState("Back to patrol", new GuardiaBackToPatrol(_fsm, _viewRadius,_viewAngle, transform, _velocity, _maxVelocity, _maxForce, patrullaje, _currWaypoint, _path));

        _fsm.ChangeState("Patrol");
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Execute();
    }

    public void SetPath(List<Nodo> newPath)
    {
        
        _path.Clear();

        foreach (var item in newPath)
        {
            _path.Add(item);
            print(item.ToString());
        }
            
        
    }

    public void Alerta()
    {
        var Gamemanager = Parcial2Manager.instance;

        SetPath(Gamemanager.CalculateAStar(Gamemanager.GetMinNode(transform.position), Gamemanager.GetMinNode(Gamemanager.jugador.transform.position)));
        
        _fsm.ChangeState("Alerted");
    }
}
