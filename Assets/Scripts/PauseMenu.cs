using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649


public class PauseMenu : MonoBehaviour
{
    private static bool _gameIsPaused;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private TMP_Text cherryCount;
    private PlayerController _player;
    private int _currentSceneNumber;
    public Animator transition;
    public float transitionTime = 1f;
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (_gameIsPaused)
        {
            Resume();        
        }
        else
        {
            Pause();
        }
    }

    private void Start()
    {
        _currentSceneNumber = SceneManager.GetActiveScene().buildIndex - 1;
        _player = FindObjectOfType<PlayerController>();
        Resume();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _player.EnableControl();
        _gameIsPaused = false;
        if (PlayerStats.crossbowCollected) return;
        Cursor.visible = false;
    }

    private void Pause()
    {
        _player.DisableControl();
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _gameIsPaused = true;
        cherryCount.text =
            $"{PlayerStats.CollectedCherriesPerLevel[_currentSceneNumber]} / {PlayerStats.MAXCherryCount[_currentSceneNumber]}";
    }

    public void GoToMenu()
    {
        if (!PlayerStats.gameCompleted)
        {
            PlayerStats.crossbowCollected = false;
        }

        Time.timeScale = 1f;
        StartCoroutine(LoadLevel(0));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        if (!PlayerStats.gameCompleted && SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerStats.crossbowCollected = false;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
