using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip KnockClip, EatClip;
    [SerializeField]
    private AudioSource Source;

    public void PlayKnock()
    {
        Source.PlayOneShot(KnockClip);
    }

    void Update()
    {
        
    }
}
