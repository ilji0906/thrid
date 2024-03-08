using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Transform camPivotTr;
    public bool isShake = false;
    private Vector3 originPos;
    private Quaternion originRot;
    private float shakeTime = 0f;
    private float duration = 0.25f;
    void Start()
    {
        camPivotTr = GetComponent<Transform>();
        
    }
    void Update()
    {
        if(isShake)
        {
            Vector3 shakePos = Random.insideUnitSphere; //�� �ȿ��� �ұ�Ģ�� ���� ���� �Ѵ�.
            camPivotTr.position = shakePos * 0.2f;
                                                   //�޸����̽� �Լ��� �ܵ� �ĵ��� ĥ�� �ұ�Ģ�� �����ϴ��Լ�
            Vector3 shakeRot = new Vector3(0f, 0f, Mathf.PerlinNoise(Time.time * 0.5f, 0.0f));
           //Vector3 shakeRot = new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0f);
            camPivotTr.rotation = Quaternion.Euler(shakeRot);
           
            if(Time.time - shakeTime > duration)
            {
                isShake = false;
                camPivotTr.position = originPos;
                camPivotTr.rotation = originRot;

            }
        }
        


    }
    public void TurnOn()
    {
        if (isShake == false) isShake = true;
        originPos = camPivotTr.position;
        originRot = camPivotTr.rotation;
        shakeTime = Time.time;
    }
}
