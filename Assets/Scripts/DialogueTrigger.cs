using Cinemachine;
using UnityEngine;

#pragma warning disable 0649

public class DialogueTrigger : MonoBehaviour
{   
    [SerializeField]
    private Transform cameraTarget;
    public Dialogue dialogue;
    [SerializeField] private bool moveCamera;
    public bool specialMusic;
    public AudioClip backgroundMusic;

    

    public void TriggerDialogue()
    {
        FindObjectOfType<PlayerController>().DisableControl();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, moveCamera);
        if (!specialMusic) return;
        var audioManager = FindObjectOfType<AudioManager>();
        audioManager.PauseBackgroundMusic();
        audioManager.PlayAudio(backgroundMusic);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerObject = other.gameObject.GetComponent<PlayerController>();
        if (playerObject == null)
        {
            return;
        } 
        GetComponent<Collider2D>().enabled = false;
        var position = cameraTarget;
        TriggerDialogue();
        if (!moveCamera) return;
        var cam = FindObjectOfType<CinemachineVirtualCamera>();
        cam.Follow = position;
        cam.LookAt = position;
    }
}