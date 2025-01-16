using Common.Data;
using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundItem : BaseItem
{
    [Header("Sound")]
    [SerializeField] private bool isLoop = false;
    [SerializeField] private AudioSource audioSource;

    public override void Init(int id, PropStateType stateType)
    {
        base.Init(id, stateType);

        audioSource.playOnAwake = false;
        audioSource.loop = isLoop;
    }

    public override void Interact(Player player)
    {
        
    }

    public override void Revealed()
    {
        base.Revealed();

        audioSource.Play();
        Debug.Log("재생");
    }

    //private IEnumerator AudioSourcePlay()
    //{
    //    // 10초 재생
    //    isPlayed = true;
    //    audioSource.Play();
    //    Debug.Log("재생");
    //    yield return YieldCache.WaitForSeconds(10f);

    //    isPlayed = false;
    //    //audioSource.Stop();
    //    Debug.Log("정지");
    //    yield break;
    //}
}
