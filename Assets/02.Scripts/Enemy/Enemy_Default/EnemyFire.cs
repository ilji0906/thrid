using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
//에트리뷰트


public class EnemyFire : MonoBehaviour
{
    [SerializeField] private GameObject e_Bullet;
    [SerializeField] private AudioSource aSource;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform enemyTr;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform firePos;
    [SerializeField] private ParticleSystem muzzleFlash;
    private readonly int hashReload = Animator.StringToHash("ReloadTrigger");
    private int bulletCount =10;
    private int MaxBulletCount =10;

    public bool isFire= false;
    public bool isReload=false;
    private float nextTime = 0f;

    private void Awake()
    {
        bulletCount = MaxBulletCount;
        MaxBulletCount = Mathf.Clamp(bulletCount, 0, 10);
    }


    IEnumerator Start() // 초기 호출이 많아지면 호출이 안되는경우가 있음. 몇초간 텀을 두고 호출
    {
        firePos = transform.GetChild(5).GetChild(0).GetChild(0).transform;
        muzzleFlash = firePos.GetChild(0).GetComponent<ParticleSystem>();
        muzzleFlash.Stop();
        aSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        e_Bullet = (GameObject)Resources.Load("Weapons/E_Bullet");
        fireSFX = Resources.Load<AudioClip>("Sounds/p_m4_1");
        yield return new WaitForSeconds(0.3f);
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (isFire && !isReload)
        {
            if (Time.time >= nextTime)
            {
                nextTime = Time.time + Random.Range(0.1f, 0.3f);
                Fire();
            }
            else
                muzzleFlash.Stop();
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * 10f);
        }
        void Fire()
        {
            muzzleFlash.Play();
            aSource.PlayOneShot(fireSFX, 1.0f);
            Instantiate(e_Bullet, firePos.position, firePos.rotation);
            isReload = --bulletCount % MaxBulletCount == 0;
            if (isReload)
            {
                StartCoroutine(Reload());
            }

        }

        IEnumerator Reload()
        {
            isFire = false;
            muzzleFlash.Stop();
            _animator.SetTrigger(hashReload);
            yield return new WaitForSeconds(2f);
            bulletCount = MaxBulletCount;
            isReload = false;
        }
    }
}
