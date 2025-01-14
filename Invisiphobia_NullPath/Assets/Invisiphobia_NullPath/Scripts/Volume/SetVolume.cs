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

    public void Init()
    {
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out liftGammaGain);
        volume.profile.TryGet(out motionBlur);

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
