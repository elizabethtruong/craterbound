using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource audioSource1;
    //public AudioSource audioSource2;

    void Start() {
        audioSource1.Play();
        //audioSource2.Play();
    }
}