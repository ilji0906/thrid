using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { PATROL=1,TRACE,ATTACK,DIE}
    public State state = State.PATROL;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    [SerializeField] private MoveAgent moveAgent;
    [SerializeField] private EnemyFire enemyFire;

    public float atkDst = 5f;
    public float traceDst = 10f;
    public bool isDie = false;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("FowardSpeed");
    private readonly int hashDie = Animator.StringToHash("DieTrigger");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        tr = GetComponent<Transform>();
        moveAgent = GetComponent<MoveAgent>();
        enemyFire = GetComponent<EnemyFire>();
    }

    void OnEnable()  //오브젝트가 활성화 될때 자동 호출 // 오프젝트 풀링 : 실시간 메모리 과부화 방지
    {
        StartCoroutine(EnemyState());    //거리를 재서 현재상태만 알려준다
        StartCoroutine(EnemyAction());  
    }
    IEnumerator EnemyState()
    {
        while (!isDie)  //계속 반복시키기 위해
        {
            float dist = Vector3.Distance(tr.position, playerTr.position);
            if (dist <= atkDst)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDst)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            yield return new WaitForSeconds(0.3f);
        }

    }

    IEnumerator EnemyAction()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.3f); //0.3초후 액션 
            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.isPatrolling = true;
                    _animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    _animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    moveAgent.Stop();
                    _animator.SetBool(hashMove, false);
                    if(!enemyFire.isFire)
                        enemyFire.isFire = true;
                    break;

                case State.DIE:
                    Die();

                    break;
            }
        }
    }

    public void Die()
    {
        enemyFire.isFire = false;
        isDie = true;
        moveAgent.Stop();
        state = State.DIE;
        GetComponent<Rigidbody>().isKinematic = true; //물리 없음
        GetComponent<CapsuleCollider>().enabled = false; //콜라이더 비활성화
        _animator.SetTrigger(hashDie);
        _animator.SetInteger(hashDieIdx, Random.Range(0, 2));
    }

    void Start()
    {
         
    }

    void Update()
    {
        _animator.SetFloat(hashSpeed, moveAgent.speed);
        
    }

    void OnDisable()  //오브젝트가 비활성화 될때 자동 호출
    {

    }



}
