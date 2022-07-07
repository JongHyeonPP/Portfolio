﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ItemSpace;
public class GunEnemy : LivingEntity
{
    public LayerMask PLAYER; //추적대상 레이어

    private LivingEntity targetEntity;//추적대상
    private NavMeshAgent nav; //경로 계산 AI 에이전트
    private float dist; //적과 추적대상과의 거리
    private Rigidbody rigid;
    private float speed;
    private Animator anim;
    private float speedDampTime = 0.1f;
    private float exp = 10f;
    /*public ParticleSystem hitEffect; //피격 이펙트
    public AudioClip deathSound;//사망 사운드
    public AudioClip hitSound; //피격 사운드
    */

    //스태프
    public GameObject firePoint;
    private int spawnMax = 2;
    private Animator enemyAnimator;
    //private AudioSource enemyAudioPlayer; //오디오 소스 컴포넌트

    public float damage = 30f; //공격력
    public float attackDelay = 2.5f; //공격 딜레이
    private float lastAttackTime; //마지막 공격 시점

    private Transform tr;
    private float attackRange = 10f;

    //추적 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            //추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            //그렇지 않다면 false
            return false;
        }
    }

    private bool canMove;
    private bool canAttack;
    Collider[] enemyColliders;
    private Item[] DropItem;
    private void Awake()
    {
        //게임 오브젝트에서 사용할 컴포넌트 가져오기
        nav = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        Setup(100, 25, 3);
        //게임 오브젝트 활성화와 동시에 AI의 탐지 루틴 시작
        StartCoroutine(UpdatePath());
        tr = GetComponent<Transform>();

        //추적 대상과의 멈춤 거리 랜덤하게 설정하기(7~10사이), 적이 뭉쳐있는 것보다 산개된 모습을 주기 위해서
        nav.stoppingDistance = Random.Range(7, 11);
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //enemyAudioPlayer = GetComponent<AudioSource>();
        
        tr = GetComponent<Transform>();
        enemyColliders = GetComponentsInChildren<Collider>();
    }

    //적 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(float newHealth, float newDamage, float newSpeed)
    {
        //체력 설정
        startHp = newHealth;
        health = newHealth;
        //공격력 설정
        damage = newDamage;
        //네비메쉬 에이전트의 이동 속도 설정
        nav.speed = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        speed = rigid.velocity.magnitude;
        anim.SetFloat("Speed",nav.velocity.magnitude);
        //추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        enemyAnimator.SetBool("CanMove", canMove);
        enemyAnimator.SetBool("CanAttack", canAttack);

        if (hasTarget)
        {
            //추적 대상이 존재할 경우 거리 계산은 실시간으로 해야하니 Update()
            dist = Vector3.Distance(tr.position, targetEntity.transform.position);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = true;  
        }
        rigid.isKinematic = false;
        nav.enabled = true;
    }
    public void FixedUpdate()
    {
        if(targetEntity!=null)
        transform.LookAt(targetEntity.transform);
    }

    //추적할 대상의 위치를 주기적으로 찾아 경로 갱신, 대상이 있으면 공격한다.
    private IEnumerator UpdatePath()
    {
        //살아 있는 동안 무한 루프
        while (!dead)
        {
            if (hasTarget)
            {
                Attack();
            }
            else
            {
                //추적 대상이 없을 경우, AI 이동 정지
                nav.isStopped = true;
                canAttack = false;
                canMove = false;

                //반지름 20f의 콜라이더로 PLAYER 레이어를 가진 콜라이더 검출하기
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, PLAYER);

                //모든 콜라이더를 순회하면서 살아 있는 LivingEntity 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    //콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    //LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아 있다면
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        //추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;
                        //for문 루프 즉시 정지
                        break;
                    }
                }
            }

            //0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    //적과 플레이어 사이의 거리 측정, 거리에 따라 공격 메서드 실행
    public virtual void Attack()
    {
        //자신이 사망X, 최근 공격 시점에서 attackDelay 이상 시간이 지났고, 플레이어와의 거리가 공격 사거리안에 있다면 공격 가능
        if (!dead && dist <= attackRange)
        {
            //공격 반경 안에 있으면 움직임을 멈춘다.
            canMove = false;
            rigid.velocity = Vector3.zero;
            //transform.LookAt(targetEntity.transform);

            //공격 딜레이가 지났다면 공격 애니 실행
            if (lastAttackTime + attackDelay <= Time.time)
            {
                canAttack = true;
                lastAttackTime = Time.time; //최근 공격시간 초기화
            }

            //공격 반경 안에 있지만, 딜레이가 남아있을 경우
            else
            {
                //canAttack = false;
            }
        }

        //공격 반경 밖에 있을 경우 추적하기
        else
        {
            //추적 대상이 존재 && 추적 대상이 공격 반경 밖에 있을 경우, 경로를 갱신하고 AI 이동을 계속 진행
            canMove = true;
            canAttack = false;
            nav.isStopped = false; //계속 이동
            nav.SetDestination(targetEntity.transform.position);
        }
    }

    //유니티 애니메이션 이벤트로 공격 메소드
    public void Fire()
    {
        Debug.Log("Fire");
    }



    //데미지를 입었을 때 실행할 처리
    public override void OnDamage(object[] _params)
    {
        /*사망하지 않을 상태에서만 피격 효과 재생
        if (!dead)
        {
            //공격 받은 지점과 방향으로 피격 효과 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            //피격 효과음 재생
            enemyAudioPlayer.PlayOnShot(hitSound);
        }
        */

        //피격 애니메이션 재생
        enemyAnimator.SetTrigger("Hit");


        //LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(_params);
    }

    //사망 처리
    public override void Die()
    {
        //LivingEntity의 DIe()를 실행하여 기본 사망 처리 실행
        base.Die();
        StartCoroutine(Die_IE());
        //다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
            rigid.isKinematic = true;
        }

        //AI추적을 중지하고 네비메쉬 컴포넌트를 비활성화
        nav.isStopped = true;
        nav.enabled = false;
        //사망 애니메이션 재생
        enemyAnimator.SetTrigger("Die");
        
        /*//사망 효과음 재생
        enemyAudioPlayer.PlayOnShot(deathSound);
        */

    }
    IEnumerator Die_IE()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        tr.position = Vector3.zero;            // 포지션 초기화
        tr.rotation = Quaternion.identity;     // 회전 초기화
        rigid.Sleep();                         // 잠시 리지디 바디의 충돌 시뮬레이션을 멈춤.
        GameManager.instance.gameDataObject.Exp += exp;
    }
}