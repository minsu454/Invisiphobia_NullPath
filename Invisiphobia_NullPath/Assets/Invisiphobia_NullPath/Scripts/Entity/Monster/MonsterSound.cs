using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip chaseSound;
    [SerializeField] private AudioClip killingSound;

    [Header("BGM")]
    [SerializeField] private AudioClip attackBGM;
    [SerializeField] private float defaultBGMVolume = 0.5f;

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
        Managers.Sound.BGMStop();
        if (!isPlaying)
        {
            isPlaying = true;
            Managers.Sound.SFX2DPlay(killingSound);
        }
    }

    private void PlayHitSound()
    {
        Managers.Sound.BGMPlay(attackBGM, defaultBGMVolume);
        if (!isPlaying)
        {
            isPlaying = true;
            Managers.Sound.SFX3DPlay(hitSound, this.transform);
        }
    }

    private void PlayWalkSound()
    {
        Managers.Sound.SceneBGMRePlay();
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