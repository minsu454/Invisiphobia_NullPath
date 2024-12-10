using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : WorldUI
{
    [SerializeField] private Image progressBar;

    public override void Init(IActiveStatable subject)
    {
        
    }

    public override void Subscribe(IActiveStatable subject)
    {
        SetFillAmount(0);
    }

    public override void Unsubscribe(IActiveStatable subject)
    {
        gameObject.SetActive(false);    
    }

    private IEnumerator CoProgress()
    {
        while (true)
        {

        }
    }

    private void SetFillAmount(float value)
    {
        progressBar.fillAmount = value;
    }
}
