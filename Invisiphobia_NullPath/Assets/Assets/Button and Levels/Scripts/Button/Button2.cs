
using UnityEngine;

public class Button2 : MonoBehaviour

{
    Animator animator;
    AudioSource audioSource;
    [SerializeField] private Material buttIndicator1;
    [SerializeField] private Material buttIndicator2;
    [SerializeField] private Color emissionColor = Color.green;
    [SerializeField] private AudioClip sound_1, sound_2;
    public bool buttState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
   
    public void Interaction()
    {
        buttState = !buttState;
        animator.SetBool("buttState", buttState);
    }

    public void Button_1()
    {
        //add your event
        buttIndicator1.SetColor("_EmissionColor", emissionColor);
        buttIndicator2.SetColor("_EmissionColor", Color.black);
        audioSource.PlayOneShot(sound_1);
    }

    public void Button_2()
    {
        //add your event
        buttIndicator1.SetColor("_EmissionColor", Color.black);
        buttIndicator2.SetColor("_EmissionColor", emissionColor);
        audioSource.PlayOneShot(sound_2);
    }
  
    private void OnDestroy()
    {
        buttIndicator1.SetColor("_EmissionColor", Color.black);
        buttIndicator2.SetColor("_EmissionColor", emissionColor);
    }
}
