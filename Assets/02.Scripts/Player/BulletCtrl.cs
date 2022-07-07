using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    void Start()
    {
        Destroy(this, 3f);
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("PLAYER")||col.gameObject.CompareTag("ENEMY"))
        {
            Destroy(this);
        }
    }
}
