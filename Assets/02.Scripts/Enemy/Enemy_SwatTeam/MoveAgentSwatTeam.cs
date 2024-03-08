using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class MoveAgentSwatTeam : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private List<Transform> wayPointsList = new List<Transform>();
    [SerializeField] private NavMeshAgent _navi;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;

    public int nextIdx = 0;
    public float walkSpeed = 1.5f;
    public float runSpeed = 4.0f;
    private float damping;

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            _navi.speed = runSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    private bool _isPatrolling;
    public bool isPatrolling 
    {
        get { return _isPatrolling; }
        set
        {
            _isPatrolling = value;
            if (_isPatrolling)
            {
                _navi.speed = walkSpeed;
                damping = 1.0f;
                WayPointMove();
            }
        }
    }
    public float speed
    {
        get { return _navi.velocity.magnitude; }
    }

    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").transform;
        tr = GetComponent<Transform>();
        points = GameObject.Find("PatrolPoints").GetComponentsInChildren<Transform>();
        for (int i = 0; i < points.Length; i++)
        {
            wayPointsList.Add(points[i]); 
        }
        wayPointsList.RemoveAt(0);
        _navi = GetComponent<NavMeshAgent>();
        _navi.autoBraking = false;
        WayPointMove();
    }

    void Update()
    {
        if (_navi.remainingDistance <= 0.5f)
        {
            nextIdx = ++nextIdx % wayPointsList.Count;
            WayPointMove();
        }
    }
    void WayPointMove()
    {
        if (_navi.isPathStale) return; 
        _navi.isStopped = false;
        _navi.destination = wayPointsList[nextIdx].position; 
    }
    public void TraceTarget(Vector3 traceTarget)
    {
        if (_navi.isPathStale) return;
        _navi.destination = traceTarget;
        _navi.isStopped = false;
    }
    public void Stop()
    {
        _navi.isStopped = true;
        _navi.velocity = Vector3.zero;
        _isPatrolling = false;
    }

}
