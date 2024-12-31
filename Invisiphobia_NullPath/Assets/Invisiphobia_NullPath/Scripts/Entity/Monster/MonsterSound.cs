using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    public AudioClip walkSound;
    public AudioClip hitSound;
    public AudioClip chaseSound;

    private float curTime;
    private float maxWalkingTime;

    private bool isPlaying = false;

    public void Init(Monster monster)
    {
        monster.changeStateEvent += OnResetState;
        monster.MyState.WanderingEvent += PlayWalkSound;
        monster.MyState.AttackingEvent += PlayWalkSound;
        monster.MyState.MonsterFleeingEvent += PlayHitSound;

        maxWalkingTime = walkSound.length;
    }

    private void PlaySound()
    {
    }

    private void PlayHitSound()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Managers.Sound.SFX3DPlay(hitSound, this.transform);
        }
    }

    private void PlayWalkSound()
    {
        curTime += Time.deltaTime;
        if (curTime > maxWalkingTime)
        {
            Managers.Sound.SFX3DPlay(walkSound, this.transform);
            curTime = 0f;
        }
    }

    private void OnResetState()
    {
        isPlaying = false;
        curTime = 0f;
    }
}