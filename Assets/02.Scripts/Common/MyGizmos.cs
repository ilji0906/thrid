using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    [SerializeField] private Color _color = Color.red;
    [SerializeField] private float _radius = 0.3f;

    void Start()
    {
        
    }

    private void OnDrawGizmos() //���̳� ������ �׷��ִ� ����Ƽ ���� �Լ�, �ݹ��Լ�
    {
        Gizmos.color = _color;  //�÷�
        Gizmos.DrawSphere(transform.position, _radius); //����̳� ����(��ġ,�ݰ�)
    }
}
