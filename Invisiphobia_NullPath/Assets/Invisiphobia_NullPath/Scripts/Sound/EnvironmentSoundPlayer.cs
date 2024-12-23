using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSoundPlayer : MonoBehaviour
{
    //소리 한번만 재생시킬건지 여부
    public bool soundPlayOneShot = true;

    public float soundMaxDistance;

    public float repeatWaitTime = 0.5f;

    public AudioClip environmentSoundClip;
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
        Managers.Sound.SFX3DPlay(environmentSoundClip, gameObject.transform, true, soundMaxDistance);
    }

    private IEnumerator CoSoundRepeatMethod()
    {
        while(true)
        {
            CoSoundRepeatMethod();
            yield return YieldCache.WaitForSeconds(repeatWaitTime);
        }
    }

}
