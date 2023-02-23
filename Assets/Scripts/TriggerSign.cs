using UnityEngine;

public class TriggerSign : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;

    [SerializeField] private GameObject popupLetter;

    private bool _triggerDialogue;
    // Start is called before the first frame update
    private void Start()
    {
     popupLetter.SetActive(false);   
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _triggerDialogue = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        popupLetter.SetActive(true);
        if (!_triggerDialogue) return;
        dialogueTrigger.TriggerDialogue();
        _triggerDialogue = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        popupLetter.SetActive(false);
    }
}
