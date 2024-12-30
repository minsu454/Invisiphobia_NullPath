using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    public AudioClip walkSound;
    public AudioClip hitSound;
    public AudioClip chaseSound;

    public float curTime;
    public float maxWalkingTime;

    public bool isPlaying = false;
    Monster monster;

    public void Init(Monster monster)
    {
        this.monster = monster;
        monster.changeStateEvent += OnResetState;
        monster.MyState.WanderingEvent += PlayWalkSound;
        monster.MyState.AttackingEvent += PlayWalkSound;
        monster.MyState.MonsterFleeingEvent += PlayHitSound;
    }

    private void PlaySound()
    {
    }

    public void PlayHitSound()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Managers.Sound.SFX3DPlay(hitSound, this.transform);
        }
    }

    public void PlayWalkSound()
    {
        curTime += Time.deltaTime;
        if (curTime > maxWalkingTime)
        {
            Managers.Sound.SFX3DPlay(walkSound, this.transform);
            curTime = 0f;
        }
    }

    public void OnResetState()
    {
        isPlaying = false;
        curTime = 0f;
    }
}