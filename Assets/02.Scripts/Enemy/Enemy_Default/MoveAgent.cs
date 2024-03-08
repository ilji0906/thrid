using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))] //경고 에트리뷰트. 이 오브젝트에서는 NaviMeshAgent가 없으면 안된다.
//1.패트롤 포인트를 배열로 받는다.
//2.각종 컴포넌트


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
    public bool isPatrolling // 프로퍼티. 원본 변수를 보호하기위해,
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
            wayPointsList.Add(points[i]); //웨이포인트 리스트가 트랜스폼 배열을 다 담는다. 
        }
        wayPointsList.RemoveAt(0); //배열의 첫번째 인덱스를 삭제
        _navi = GetComponent<NavMeshAgent>();
        _navi.autoBraking = false;
        WayPointMove();
    }

    void Update()
    {
        if (_navi.remainingDistance <= 0.5f) //목적지의 거리가 0.5보다 작거나 같다면
        {
            nextIdx = ++nextIdx % wayPointsList.Count;
            WayPointMove();
        }
    }

    void WayPointMove()
    {
        if (_navi.isPathStale) return; //isPathStale = 최단 경로가 탐색되지 않으면 빠져나간다.
        //추적을 활성화하고 추적대상을 찾는다.
        _navi.isStopped = false;
        _navi.destination = wayPointsList[nextIdx].position; //패트롤 포인트 첫번째를 찾는다.
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
