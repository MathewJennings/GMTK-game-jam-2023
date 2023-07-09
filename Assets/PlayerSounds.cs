using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip leftStep;
    public AudioClip rightStep;
    
    public void playLeftStep()
    {
        audioSource.PlayOneShot(leftStep);
    }

    public void playRightStep()
    {
        audioSource.PlayOneShot(rightStep);
    }
}
