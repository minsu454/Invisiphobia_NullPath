using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip chaseSound;
    [SerializeField] private AudioClip killingSound;
    [SerializeField] private float maxWalkingTime;

    [Header("BGM")]
    [SerializeField] private AudioClip attackBGM;
    [SerializeField] private float defaultBGMVolume = 0.5f;

    private float curTime;

    private bool isPlaying = false;

    public void Init(Monster monster)
    {
        monster.changeStateEvent += OnResetState;
        monster.MyState.WanderingEvent += PlayWalkSound;
        monster.MyState.WanderingEvent += CheckIsPlaying;
        monster.MyState.AttackingEvent += PlayChaseSound;
        monster.MyState.MonsterFleeingEvent += PlayHitSound;
        monster.MyState.MonsterKillingEvent += PlayKillingSound;

        maxWalkingTime = walkSound.length;
    }

    private void PlayKillingSound()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Managers.Sound.BGMStop();
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

    private void PlayChaseSound()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Managers.Sound.BGMPlay(attackBGM, defaultBGMVolume);
        }
    }

    private void CheckIsPlaying()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Managers.Sound.SceneBGMRePlay();
        }
    }

    private void OnResetState()
    {
        isPlaying = false;
        curTime = 0f;
    }
}