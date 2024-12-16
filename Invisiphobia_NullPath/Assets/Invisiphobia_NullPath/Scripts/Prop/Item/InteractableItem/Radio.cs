using Common.Data;
using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : BaseItem
{
    private bool isPlayed = false;
    private AudioSource audioSource;

    public override void Init()
    {
        base.Init();
        audioSource = GetComponent<AudioSource>();
        interactText = DataService.GetInteractText(ItemTable.interactText[0]);
    }

    public override void Interact(Player player)
    {
        if (!isPlayed)
        {
            StartCoroutine(CoPlayRadio());
        }
    }

    private IEnumerator CoPlayRadio()
    {
        // 10초 재생
        isPlayed = true;
        //audioSource.Play();
        Debug.Log("재생");
        yield return YieldCache.WaitForSeconds(10f);

        isPlayed = false;
        //audioSource.Stop();
        Debug.Log("정지");
        yield break;
    }
}
