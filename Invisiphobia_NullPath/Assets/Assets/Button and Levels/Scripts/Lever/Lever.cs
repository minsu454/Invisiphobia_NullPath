using UnityEngine;


public class Lever : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    public AudioClip soundOn, soundOff;
    public ParticleSystem spark;
    public bool leverState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spark.Stop();       
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
        spark.Play();
    }

    public void LeverOff()
    {
        //add your event
        audioSource.PlayOneShot(soundOff);
        spark.Play();
    }
}




