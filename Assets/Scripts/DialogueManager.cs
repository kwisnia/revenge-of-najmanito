using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portrait;
    private bool _cameraMoved;
    private PlayerController _player;
    private CinemachineVirtualCamera _virtualCamera;
    private AudioManager _am;
    private bool _textDisplayed;
    private Sentence _currentSentence;
    private bool _dialogueStarted;
    
    public Animator animator;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _am = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (!animator.GetBool(IsOpen)) return;
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        if (!_dialogueStarted) return;
        if (_textDisplayed)
        {
            DisplayNextSentence();
        }
        else
        {
            FillSentence(_currentSentence.sentence);
        }
    }

    private Queue<Sentence> _sentences;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    // Use this for initialization
    private void Start () {
        _sentences = new Queue<Sentence>();
    }
    

    private void WasCameraMoved(bool wasMoved)
    {
        _cameraMoved = wasMoved;
    }

    public void StartDialogue (Dialogue dialogue, bool cameraMoved)
    {
        animator.SetBool(IsOpen, true);
        WasCameraMoved(cameraMoved);
        _sentences.Clear();
        
        foreach (var sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        _dialogueStarted = true;
        _textDisplayed = false;
    }

    public void DisplayNextSentence ()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        _currentSentence = _sentences.Dequeue();
        nameText.text = _currentSentence.name;
        portrait.sprite = _currentSentence.portrait;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(_currentSentence.sentence));
    }

    private IEnumerator TypeSentence (string sentence)
    {
        _textDisplayed = false;
        dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        _textDisplayed = true;
    }

    private void FillSentence(string sentence)
    {
        StopAllCoroutines();
        dialogueText.text = sentence;
        _textDisplayed = true;
    }

    private void EndDialogue()
    {
        _player.EnableControl();
        if (_cameraMoved)
        {
            var transform1 = _player.transform;
            _virtualCamera.Follow = transform1;
            _virtualCamera.LookAt = transform1;
        }
        animator.SetBool(IsOpen, false);
        StartCoroutine(_am.FadeOut(0.5f));
        _am.ResumeBackgroundMusic();
        _textDisplayed = true;
        _dialogueStarted = false;
    }
}
