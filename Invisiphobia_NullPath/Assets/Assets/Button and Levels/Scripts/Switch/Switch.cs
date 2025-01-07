using UnityEngine;

public class Switch : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    [SerializeField] private AudioClip soundOn,soundOff;
    public bool switchState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
    }

    public void SwitchOff()
    {
        //add your event
        audioSource.PlayOneShot(soundOff);
    }

}
