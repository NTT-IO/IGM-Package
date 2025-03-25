using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpDestory : MonoBehaviour
{
    Animator Ar;
    bool isDie;
    private void Start()
    {
        Ar = GetComponent<Animator>();
    }
    public void destory()
    {
        Destroy(gameObject);
    }
    public void setIdeo()
    {
        Ar.SetBool("Flash", false);
    }
    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
