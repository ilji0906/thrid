using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1. ��ƼŬ ���� �Ҹ� ���� 
public class RemoveBullet : MonoBehaviour
{
    [SerializeField] GameObject Spark;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hitclip;
    private string bulletTag = "BULLET";
    private string e_bulletTag = "E_BULLET";
    void Start()
    {
        source = GetComponent<AudioSource>();
        hitclip = Resources.Load<AudioClip>("Sounds/bullet_hit_metal_enemy_4");
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(bulletTag) || col.collider.CompareTag(e_bulletTag))
        {       
            Destroy(col.gameObject);
            //���� ��ġ�� hitPos�� �Ѱ��ش�.
            Vector3 hitPos = col.contacts[0].point;
            Vector3 _normal = col.contacts[0].normal; // �߻��� ����
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            var spk = Instantiate(Spark,hitPos,rot);
            Destroy(spk,2.5f);
            source.PlayOneShot(hitclip, 1.0f);
        }

    }
}
