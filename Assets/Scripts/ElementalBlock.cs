using UnityEngine;

public class ElementalBlock : MonoBehaviour
{

    [SerializeField] private NajmanBoss najmanBoss;
    [SerializeField] private AudioClip najmanDeathClip;
    [SerializeField] private AudioClip victoryTheme;
    [SerializeField] private Transform endCheckpoint;
    [SerializeField] private AudioClip triggerJingle;
    private static bool _najmanDead;
    private static int _disabledBlocks;
    private bool _enabled = true;
    // Start is called before the first frame update
    void Start()
    {
        _najmanDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBlocks()
    {
        _disabledBlocks = 0;
        _enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_najmanDead) return;
        var arrow = other.gameObject.GetComponent<Arrow>();
        if (arrow == null) return;
        if (!_enabled) return;
        _enabled = false;
        _disabledBlocks++;
        var audioManager = FindObjectOfType<AudioManager>();
        AudioSource.PlayClipAtPoint(triggerJingle, transform.position);
        Debug.Log(_disabledBlocks);
        if (_disabledBlocks < 6) return;
        if (FindObjectOfType<NajmanBoss>() != null)
        {
            AudioSource.PlayClipAtPoint(najmanDeathClip, najmanBoss.transform.position);
            Destroy(najmanBoss.gameObject);
            _najmanDead = true;
        }
        audioManager.PauseBackgroundMusic();
        audioManager.PlayAudio(victoryTheme);
        Destroy(GameObject.FindGameObjectWithTag("Mystery"));
        FindObjectOfType<GameManager>().SetCheckpoint(endCheckpoint.position);
    }
}
