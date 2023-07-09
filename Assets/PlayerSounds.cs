using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip leftStep;
    public AudioClip rightStep;
    public AudioClip openInventory;
    public AudioClip closeInventory;
    public AudioClip buySell;
    
    public void playLeftStep()
    {
        audioSource.PlayOneShot(leftStep);
    }

    public void playRightStep()
    {
        audioSource.PlayOneShot(rightStep);
    }

    public void playOpenInventory()
    {
        audioSource.PlayOneShot(openInventory);
    }

    public void playCloseInventory() { 
        audioSource.PlayOneShot(closeInventory);
    }

    public void playBuySell()
    {
        audioSource.PlayOneShot(buySell);
    }
}
