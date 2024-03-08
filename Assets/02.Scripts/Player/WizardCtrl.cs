using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WizardCtrl : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform tr;
    [SerializeField] private Animator animator;
    private Ray ray; //����
    private RaycastHit hit; // � ������Ʈ�� ������ �¾Ҵ��� ����
    private Vector3 target= Vector3.zero;
    public float m_DoubleClickSecond = 0.25f;
    private bool m_IsOneClick = false;
    private double m_Timer = 0f;
    void Start()
    {
        tr = transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        MouseMove();
        UpdateAnimator();
        if(Input.GetKey(KeyCode.LeftShift)&&Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("SkillTrigger");
        }
        else if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("AttackTrigger");
        }

    }

    private void MouseMove()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 30f, Color.green);
        if (m_IsOneClick == true && ((Time.time - m_Timer) > m_DoubleClickSecond))
        {

            Debug.Log("One Click");

            m_IsOneClick = false;

        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!m_IsOneClick)

            {

                m_Timer = Time.time;
                m_IsOneClick = true;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 7))
                {                  //������ �¾Ҵٸ�  �÷ξ 
                    target = hit.point; //������ ���� ��ġ�� target�� ����
                    agent.destination = target;
                    agent.speed = 1.5f;
                    agent.isStopped = false;

                }

            }
            else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
            {

                Debug.Log("Double Click");
                m_IsOneClick = false;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 7))
                {                  //������ �¾Ҵٸ�  �÷ξ 
                    target = hit.point; //������ ���� ��ġ�� target�� ����
                    agent.destination = target;
                    agent.speed = 3.5f;
                    agent.isStopped = false;

                }

            }



        }
    }

    void UpdateAnimator()
    {
        Vector3 _velocity = agent.velocity;
        Vector3 localVelocity = tr.InverseTransformDirection(_velocity);
        //������ǥ�� ������ǥ�� ��ȯ
        float speed = localVelocity.z; // z�� ���ǵ带 ���� 
        animator.SetFloat("ForwardSpeed", speed);
    }

}
