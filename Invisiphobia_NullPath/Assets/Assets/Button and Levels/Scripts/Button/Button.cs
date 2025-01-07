using UnityEngine;

public class Button : MonoBehaviour

{
    Animator animator;
    AudioSource audioSource;
    [SerializeField] private Color color = Color.red;
    [SerializeField] private Material buttMaterial;
    [SerializeField] private AudioClip soundOn,soundOff;
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
    public void ButtonOn()
    {
        //add your event
        audioSource.PlayOneShot(soundOn);
        buttMaterial.SetColor("_EmissionColor", color);
    }
    public void ButtonOff()
    {
        //add your event
        audioSource.PlayOneShot(soundOn);
        buttMaterial.SetColor("_EmissionColor", Color.black);
    }


    private void OnDestroy()
    {
        buttMaterial.SetColor("_EmissionColor", Color.black);
    }
}
