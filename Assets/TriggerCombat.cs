using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCombat : MonoBehaviour
{
    [SerializeField] private AudioClip activationRoar;
    [SerializeField] private AudioClip chaseMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        var audioManager = FindObjectOfType<AudioManager>();
        if (player == null) return;
        Debug.Log("Szukam najmana");
        var najman = Resources.FindObjectsOfTypeAll<NajmanBoss>();
        if (najman.Length == 0) return;
        GetComponent<Collider2D>().enabled = false;
        Debug.Log("Najman goni!");
        najman[0].gameObject.SetActive(true);
        audioManager.ChangeBackgroundMusic(chaseMusic);
        AudioSource.PlayClipAtPoint(activationRoar, najman[0].transform.position);
        FindObjectOfType<GameManager>().SetCheckpoint(transform.position);
    }
}
