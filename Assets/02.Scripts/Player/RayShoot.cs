﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShoot : MonoBehaviour
{
    private RaycastHit hit; // 광선에 맞은 판정
    private const float _pivot = 0.02f;
    [SerializeField] private Transform FirePos_R;
    [SerializeField] private Transform FirePos_L;
    [SerializeField] private Transform FirePos_Cur;
    void Start()
    {
        FirePos_Cur = FirePos_R;
    }

    void Update()
    {
        Debug.DrawRay(FirePos_Cur.position, FirePos_Cur.forward * 25f, Color.green);
        if (Input.GetMouseButtonDown(0)&&AimBehaviourBasic.aim)
        {
            Fire();
        }
    }
    void Fire()
    {
        if (Physics.Raycast(FirePos_Cur.position, FirePos_Cur.forward, out hit, 50f))
        {
            if (hit.collider.tag == "ENEMY")
            {
                object[] _params = new object[2];
                _params[0] = hit.point;
                _params[1] = 25f;
                hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
            }

        }
    }
    public void MoveAim()
    {
        
        if (FirePos_Cur == FirePos_L)
        {
            FirePos_Cur = FirePos_R;
        }
        else if (FirePos_Cur == FirePos_R)
        {
            FirePos_Cur = FirePos_L; 
        }
    }


}
