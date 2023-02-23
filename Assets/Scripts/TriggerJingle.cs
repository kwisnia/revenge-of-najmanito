using UnityEngine;

public class TriggerJingle : MonoBehaviour
{
    [SerializeField] private AudioClip jingle;
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
        FindObjectOfType<AudioManager>().PlayAudio(jingle);
        Destroy(gameObject);
    }
}
