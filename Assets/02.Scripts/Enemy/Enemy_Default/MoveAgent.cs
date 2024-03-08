using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))] //��� ��Ʈ����Ʈ. �� ������Ʈ������ NaviMeshAgent�� ������ �ȵȴ�.
//1.��Ʈ�� ����Ʈ�� �迭�� �޴´�.
//2.���� ������Ʈ


public class MoveAgent : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private List<Transform> wayPointsList = new List<Transform>();
    [SerializeField] private NavMeshAgent _navi;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    public int nextIdx=0;
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
    public bool isPatrolling // ������Ƽ. ���� ������ ��ȣ�ϱ�����,
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
            wayPointsList.Add(points[i]); //��������Ʈ ����Ʈ�� Ʈ������ �迭�� �� ��´�. 
        }
        wayPointsList.RemoveAt(0); //�迭�� ù��° �ε����� ����
        _navi = GetComponent<NavMeshAgent>();
        _navi.autoBraking = false;
        WayPointMove();
    }

    void Update()
    {
        if (_navi.remainingDistance <= 0.5f) //�������� �Ÿ��� 0.5���� �۰ų� ���ٸ�
        {
            nextIdx = ++nextIdx % wayPointsList.Count;
            WayPointMove();
        }
    }

    void WayPointMove()
    {
        if (_navi.isPathStale) return; //isPathStale = �ִ� ��ΰ� Ž������ ������ ����������.
        //������ Ȱ��ȭ�ϰ� ��������� ã�´�.
        _navi.isStopped = false;
        _navi.destination = wayPointsList[nextIdx].position; //��Ʈ�� ����Ʈ ù��°�� ã�´�.
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
