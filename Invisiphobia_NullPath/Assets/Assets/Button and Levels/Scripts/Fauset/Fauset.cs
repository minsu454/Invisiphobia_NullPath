using UnityEngine;
using UnityEngine.LowLevel;

public class Fauset : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    [SerializeField] private AudioClip fauset;
    public bool fausetState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
      
    }
    public void Interaction()
    {
        fausetState = !fausetState;
        animator.SetBool("fausetState", fausetState);
        
    }
    public void FausetOpen()
    {
        //add your event
        audioSource.PlayOneShot(fauset);
    }

    public void FausetClose()
    {
        //add your event
        audioSource.PlayOneShot(fauset);
    }
}
