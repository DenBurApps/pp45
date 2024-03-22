using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CheckListData 
{
    public bool IsFavorite { get; set; }
    public string Name { get; set; }
    public List<bool> CheckMarks { get; set; } = new List<bool>();
    public List<string> CheckNote { get; set; } = new List<string>();
}
