using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Han_button_bullet : MonoBehaviour {

    public GameObject player;

    AudioSource AudioPlay;

    public AudioClip SE_UISound;

    void OnTriggerEnter(Collider other)
    {
        AudioPlay = GetComponent<AudioSource>();

        AudioPlay.PlayOneShot(SE_UISound);

        Han_PlayerFire script = player.GetComponent<Han_PlayerFire>();

        script.m_state = Han_PlayerFire.GameState.bullet;
    }
}
