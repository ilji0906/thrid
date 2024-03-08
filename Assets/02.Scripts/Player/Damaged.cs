using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaged : MonoBehaviour
{
    [SerializeField] private GameObject bloodEffect;
    private readonly string e_BulletTag = "E_BULLET";

    void Start()    
    {
        bloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(e_BulletTag))
        {
            Destroy(col.gameObject);

            ShowBloodEffect(col);
        }
    }

    private void ShowBloodEffect(Collision col)
    {
        Vector3 hitpos = col.contacts[0].point;   //맞은지점
        Vector3 _normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate<GameObject>(bloodEffect, hitpos, rot);
        Destroy(blood, 0.5f);
    }
}
