using UnityEngine;

public class Mystery : MonoBehaviour
{
    [SerializeField] private GameObject mystery;
    [SerializeField] private bool isCorrect;
    [SerializeField] private AudioClip mysteryJingle;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var arrow = other.gameObject.GetComponent<Arrow>();
        if (arrow == null) return;
        if (isCorrect)
        {
            mystery.SetActive(false);
            FindObjectOfType<AudioManager>().PlayAudio(mysteryJingle);
        }
    }
}
