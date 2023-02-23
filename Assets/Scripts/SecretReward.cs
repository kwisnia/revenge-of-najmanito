using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SecretReward : MonoBehaviour
{
    private VideoPlayer _player;
    // Start is called before the first frame update
    private void Start()
    {
        _player = FindObjectOfType<VideoPlayer>();
        _player.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        StartCoroutine(ReturnToMenu());
    }

    private static IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(35.0f);
        SceneManager.LoadScene(0);
    }
}
