using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Texture2D cursor;
    public Animator transition;
    public float transitionTime = 1f;
    private static readonly int Start1 = Animator.StringToHash("Start");

    public void Start()
    {
        PlayerStats.ResetStats();
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void PlayGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        if (!PlayerStats.gameCompleted)
        {
            Cursor.visible = false;
        }
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger(Start1);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
}
