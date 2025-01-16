using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private Volume volume;
    public Volume Volume { get { return volume; } }

    private ColorAdjustments colorAdjustments;
    private LiftGammaGain liftGammaGain;
    private MotionBlur motionBlur;
    private LimitlessGlitch8 glitch8; // 타블렛 화면전환 효과
    private LimitlessGlitch6 glitch6; // 공포감 표현 효과

    public LimitlessGlitch8 Glitch8 { get { return glitch8; } }
    public LimitlessGlitch6 Glitch6 { get { return glitch6; } }


    public void Init()
    {
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out liftGammaGain);
        volume.profile.TryGet(out motionBlur);
        volume.profile.TryGet(out glitch8);
        volume.profile.TryGet(out glitch6);

        InitPlayerPrefs();
    }

    private void InitPlayerPrefs()
    {
        colorAdjustments.active = true;

        SetBrightness(PlayerPrefs.GetFloat("Brightness", 0.5f));
        SetGamma(PlayerPrefs.GetFloat("Gamma", 0.5f));
        SetMotionBlur(PlayerPrefs.GetString("MotionBlur", "true"));

    }

    public void SetBrightness(float value)
    {
        colorAdjustments.saturation.value = (value - .5f) * 200;
    }

    public void SetGamma(float value)
    {
        liftGammaGain.gamma.value = new Vector4(1f, 1f, 1f, (value - .5f) * 2);
    }

    public void SetMotionBlur(string str)
    {
        motionBlur.active = str == "true" ? true : false;
    }
}
