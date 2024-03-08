using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject bloodEffect;
    private readonly string playerBulletTag = "BULLET";
    private float hp = 0f;
    private float hpMax = 100f;

    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");
        hp = hpMax;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(playerBulletTag))
        {
            Destroy(col.gameObject);
            hp -= col.gameObject.GetComponent<BulletCtrl>().dmg;
            if (hp <= 0f)
            {
                // GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                GetComponent<EnemyAI>().Die();
            }
            ShowBloodEffect(col);
        }
    }

    private void ShowBloodEffect(Collision col)
    {
        Vector3 hitpos = col.contacts[0].point; //맞은지점
        Vector3 _normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate<GameObject>(bloodEffect, hitpos, rot);
        Destroy(blood, 0.5f);
    }
}
