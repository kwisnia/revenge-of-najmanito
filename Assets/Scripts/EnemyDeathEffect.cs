using System.Collections;
using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayDeathEffect());
    }

    private IEnumerator PlayDeathEffect()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
