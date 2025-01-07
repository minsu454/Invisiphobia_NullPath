using Unity.VisualScripting;
using UnityEngine;

public class Lever2 : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    public AudioClip soundOn,soundOff;
    public ParticleSystem particle1,particle2;
    public bool leverState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        particle1.Stop();
        particle2.Stop();
    }
    public void Interaction()
    {
        leverState = !leverState;
        animator.SetBool("leverState", leverState);
    }
    public void LeverOn()
    {
        //add your event
        audioSource.PlayOneShot(soundOn);
        particle1.Play();
        particle2.Play();
    }

    public void LeverOff()
    {
        //add your event
        audioSource.PlayOneShot(soundOff);
        particle1.Play();
        particle2.Play();
    }
}




