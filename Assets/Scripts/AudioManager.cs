using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource primarySource;
    private bool _musicStopped;
    private AudioClip _originalClip;
    private int _sceneIndex;
    public bool musicPlayed { get; private set; } = false;
    [SerializeField] private AudioSource secondarySource;

    private void Awake()
    {
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (_sceneIndex != 4) return;
        if (primarySource.isPlaying) return;
        musicPlayed = true;
    }
    

    public void PauseBackgroundMusic()
    {
        if (_musicStopped) return;
        _musicStopped = true;
        primarySource.volume = 0.0f;
    }

    public void ChangeBackgroundMusic(AudioClip newBGM)
    {
        primarySource.Stop();
        primarySource.clip = newBGM;
        primarySource.Play();
    }

    public void ResumeBackgroundMusic()
    {
        if (!_musicStopped) return;
        _musicStopped = false;
        primarySource.volume = 1.0f;
    }
    
    public void PlayAudio(AudioClip music)
    {
        secondarySource.PlayOneShot(music);
    }
    
    public IEnumerator FadeOut (float fadeTime) {
        var startVolume = secondarySource.volume;
 
        while (secondarySource.volume > 0) {
            secondarySource.volume -= startVolume * Time.deltaTime / fadeTime;
 
            yield return null;
        }
 
        secondarySource.Stop();
        secondarySource.volume = startVolume;
    }
}
