using Common.Yield;
using System.Collections;
using UnityEngine;

public class EnvironmentSoundPlayer : MonoBehaviour
{
    //소리 한번만 재생시킬건지 여부
    public bool soundPlayOneShot = true;

    public float soundMaxDistance;

    public float repeatWaitTime = 0.5f;

    public AudioClip[] environmentSoundClips;
    private Coroutine myCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (soundPlayOneShot)
            {
                SoundOneShotMethod();
            }
            else
            {
                myCoroutine = StartCoroutine(CoSoundRepeatMethod());
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(myCoroutine != null)
            StopCoroutine(myCoroutine);
        }
    }

    private void SoundOneShotMethod()
    {
        int num = Random.Range(0, environmentSoundClips.Length);
        Managers.Sound.SFX3DPlay(environmentSoundClips[num], gameObject.transform, true, soundMaxDistance);
    }

    private IEnumerator CoSoundRepeatMethod()
    {
        while(true)
        {
            SoundOneShotMethod();
            yield return YieldCache.WaitForSeconds(repeatWaitTime);
        }
    }

}
