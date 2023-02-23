using System;
using UnityEngine;

public class Oscillation : MonoBehaviour
{
    private void FixedUpdate()
    {
        var magnitude = 0.005f * Mathf.Sin((float) ((Time.fixedTime / 3) * 2 * Math.PI));
        var transform1 = transform;
        var newPos = transform1.position;
        newPos.y += magnitude;
        transform1.position = newPos;

    }
}
