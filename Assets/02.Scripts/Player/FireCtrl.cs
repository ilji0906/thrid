using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CapsuleCollider))]

//1.총알 프리팹 2. 발사위치 3. 총소리 오디오소스 오디오 클립
public class FireCtrl : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem CartridgeEjectEffect;
    [SerializeField] private Image magazineimg;
    [SerializeField] private int bulletCount = 0;
    [SerializeField] private int bulletMax = 10;
    [SerializeField] private Animator _animator;
    [SerializeField] private Text Magazinetext;
    private readonly int hashReload = Animator.StringToHash("ReloadTrigger");
    private float timePrev;
    private float fireRate = 0.1f; //발사 간격 시간 
    private Player_Mecanim player;
    private bool isReload = false;
    void Start()
    {
        _animator = GetComponent<Animator>();
        bulletPrefab = Resources.Load("Weapons/Bullet")as GameObject;
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Transform>();
        player = GetComponent<Player_Mecanim>();
        source = GetComponent<AudioSource>();
        muzzleFlash = firePos.GetChild(0).GetComponent<ParticleSystem>();
        CartridgeEjectEffect = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
        fireClip = Resources.Load<AudioClip>("Sounds/p_ak_1");
        magazineimg = GameObject.Find("Panel_Magazine").transform.GetChild(2).GetComponent<Image>();
        Magazinetext = GameObject.Find("Panel_Magazine").transform.GetChild(0).GetComponent<Text>();
        muzzleFlash.Stop();
        bulletCount = bulletMax;
        bulletCount = Mathf.Clamp(bulletCount, 0, 10);
    }

    void Update()
    {
        if(Input.GetMouseButton(0)&&Time.time - timePrev >fireRate)
        {
            if(!player.isRun && !isReload)
            {
                --bulletCount;
                Fire();
                if(bulletCount ==0)
                {
                    StartCoroutine(Reload());
                }
            }
            
            timePrev = Time.time;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
            CartridgeEjectEffect.Stop();
        }
    }
    void Fire()
    {
        source.PlayOneShot(fireClip,1.0f);
        Instantiate(bulletPrefab, firePos.position,firePos.rotation);
        muzzleFlash.Play() ;
        CartridgeEjectEffect.Play();
        magazineimg.fillAmount = (float)bulletCount / (float)bulletMax;

        MagazineTextShow();
    }

    void MagazineTextShow()
    {
        Magazinetext.text = "<color=#ff0000>" + bulletCount.ToString() + "</color>" + "/" + bulletMax.ToString();
    }

    IEnumerator Reload()
    {
        MagazineTextShow();
        isReload = true;
        _animator.SetTrigger(hashReload);
        muzzleFlash.Stop();
        yield return new WaitForSeconds(1.5f);
        magazineimg.fillAmount = 1.0f;
        isReload = false;
        bulletCount = bulletMax;
        MagazineTextShow();
    }

    

}
