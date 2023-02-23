using Pathfinding;
using UnityEngine;

public class EagleController : MonoBehaviour
{
    private bool _following;
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
        if (other.gameObject.GetComponent<PlayerController>() == null) return;
        if(_following) return;
        _following = true;
        GetComponent<AIPath>().canMove = true;
    }
}
