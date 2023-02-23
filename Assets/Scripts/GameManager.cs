using System.Collections;
using TMPro;
using UnityEngine;

#pragma warning disable 0649


public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private int _cherryCount;
    public Animator cherryAnimator;
    private Animator _transitionAnimator;
    public TMP_Text cherryCount;

    private static readonly int CherryMenuIsOpen = Animator.StringToHash("CherryMenuIsOpen");


    private Vector2 _playerRespawnPoint;

    private static readonly int Start1 = Animator.StringToHash("Transition");

    // Start is called before the first frame update
    private void Start()
    {
        _transitionAnimator = GetComponent<Animator>();
        PlayerStats.LookForCherries();
        _playerRespawnPoint = player.transform.position;
    }
    
    public void Restart()
    {
        StartCoroutine(nameof(RestartCo));
    }

    public void SetCheckpoint(Vector2 position)
    {
        _playerRespawnPoint = position;
    }

    public IEnumerator RestartCo()
    {
        var boss = FindObjectOfType<NajmanBoss>();
        _transitionAnimator.SetBool(Start1, true);
        player.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().PlayAudio(player.deathClip);
        var arrowsStuck = GameObject.FindGameObjectsWithTag("ArrowHit");
        foreach (var arrow in arrowsStuck)
        {
            Destroy(arrow);
        }
        yield return new WaitForSeconds(1f);
        if (boss != null)
        {
            foreach (var block in FindObjectsOfType<ElementalBlock>())
            {
                block.ResetBlocks();
            }
            boss.ResetPosition();
        } 
        player.transform.position = _playerRespawnPoint;
        player.gameObject.SetActive(true);
        player.dead = false;
        _transitionAnimator.SetBool(Start1, false);
    }

    public void DisplayCherries()
    {
        cherryAnimator.SetBool(CherryMenuIsOpen, true);
        cherryCount.text = PlayerStats.collectedCherries.ToString();
        StartCoroutine(HideCherries());
    }

    private IEnumerator HideCherries()
    {
        yield return new WaitForSeconds(3.0f);
        cherryAnimator.SetBool(CherryMenuIsOpen, false);
    }
}
