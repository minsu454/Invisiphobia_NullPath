using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip chaseSound;
    [SerializeField] private AudioClip killingSound;

    private float curTime;
    private float maxWalkingTime;

    private bool isPlaying = false;

    public void Init(Monster monster)
    {
        monster.changeStateEvent += OnResetState;
        monster.MyState.WanderingEvent += PlayWalkSound;
        monster.MyState.AttackingEvent += PlayWalkSound;
        monster.MyState.MonsterFleeingEvent += PlayHitSound;
        monster.MyState.MonsterKillingEvent += PlayKillingSound;

        maxWalkingTime = walkSound.length;
    }

    private void PlayKillingSound()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Managers.Sound.SFX2DPlay(killingSound);
        }
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