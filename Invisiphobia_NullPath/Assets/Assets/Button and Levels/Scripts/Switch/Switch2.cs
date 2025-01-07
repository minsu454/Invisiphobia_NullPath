using UnityEngine;

public class Switch2 : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    [SerializeField] private AudioClip soundOn, soundOff;
    [SerializeField] private ParticleSystem spark;
    public bool switchState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spark.Stop(); 
    }
    public void Interaction()
    {
        switchState = !switchState;
        animator.SetBool("switchState", switchState);
    }
    public void SwitchOn()
    {
        //add your event
        audioSource.PlayOneShot(soundOn);
        spark.Play();
    }

    public void SwitchOff()
    {
        //add your event
        audioSource.PlayOneShot(soundOff);
        spark.Play();
    }

}
