using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBroken : MonoBehaviour
{
    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particle.isStopped) Destroy(gameObject);
    }
}
