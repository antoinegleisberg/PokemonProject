using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField][TextArea] List<string> _lines;
    
    public List<string> Lines { get { return _lines; } }
}
