using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySwatTeamAI : MonoBehaviour
{
    public enum State { PATROL=1,TRACE,ATTACK,DIE}
    public State state = State.PATROL;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    [SerializeField] private MoveAgentSwatTeam moveAgent;
    [SerializeField] private EnemySwatTeamFire enemyFire;

    public float atkDst = 20f;
    public float traceDst = 30f;
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
        moveAgent = GetComponent<MoveAgentSwatTeam>();
        enemyFire = GetComponent<EnemySwatTeamFire>();
    }

    void OnEnable()
    {
        EnemyState();
        EnemyAction();
    }

    IEnumerator EnemyAction()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.3f);
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
                    _animator.SetBool(hashSpeed, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    enemyFire.isFire = true;
                    _animator.SetBool(hashMove, false);
                    if (!enemyFire.isFire)
                        enemyFire.isFire = true;
                    break;
                case State.DIE:
                    Die();
                    break;
            }
        }
    }


    IEnumerator EnemyState()
    {
        while (!isDie)
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
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void Die()
    {
        enemyFire.isFire = false;
        isDie = true;
        moveAgent.Stop();
        state = State.DIE;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        _animator.SetTrigger(hashDie);
        _animator.SetInteger(hashDieIdx, Random.Range(0, 2));
    }

    private void Update()
    {
        _animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
