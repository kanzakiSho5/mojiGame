using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ParticleWalking());
    }

    IEnumerator ParticleWalking()
    {
        var particle = GetComponent<ParticleSystem>();
        yield return new WaitWhile(() => particle.IsAlive(true));

        Destroy(gameObject);
    }
}
