using System;
using UnityEngine;

[Serializable]
public class Sentence
{
    public string name;
    [TextArea(3, 10)]
    public string sentence;
    public Sprite portrait;
}
