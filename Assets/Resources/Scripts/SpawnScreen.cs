using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SpawnScreen : MonoBehaviour
{

    float AnimationHalfTime = 0.2f;

    void Start()
    {
        transform.localScale = Vector3.one/100;
        StartCoroutine(SpawnAnimation());
    }

    IEnumerator SpawnAnimation()
    {
        for(float i = 0; i < AnimationHalfTime; i += Time.deltaTime)
        {
            transform.localScale = new Vector3(0.01f + (i / AnimationHalfTime * 0.99f), 0.01f, 0.01f + (i / AnimationHalfTime * 0.99f));
            yield return 0;
        }
        transform.localScale = new Vector3(1, 0.01f, 1);
        for (float i = 0; i < AnimationHalfTime; i += Time.deltaTime)
        {
            transform.localScale = new Vector3(1, 0.01f + (i / AnimationHalfTime * 0.99f), 1);
            yield return 0;
        }
        transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(WaitForVidToPlay(GetComponent<VideoPlayer>().clip.length));
        yield return 0;
    }

    IEnumerator WaitForVidToPlay(double time)
    {
        GetComponent<VideoPlayer>().Play();
        GetComponentInChildren<ParticleSystem>().Play();
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            yield return 0;
        }
        while (GetComponent<VideoPlayer>().isPlaying)
        {
            yield return 0;
        }
        StartCoroutine(DespawnAnimation());
        yield return 0;
    }

    IEnumerator DespawnAnimation()
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        for (float i = AnimationHalfTime; i > 0; i -= Time.deltaTime)
        {
            transform.localScale = new Vector3(0.01f + (i / AnimationHalfTime * 0.99f), 1, 0.01f + (i / AnimationHalfTime * 0.99f));
            yield return 0;
        }
        for (float i = AnimationHalfTime; i > 0; i -= Time.deltaTime)
        {
            transform.localScale = new Vector3(0.01f, 0.01f + (i / AnimationHalfTime * 0.99f), 0.01f);
            yield return 0;
        }
        Destroy(gameObject);
        yield return 0;
    }


    void Update()
    {
        
    }
}
