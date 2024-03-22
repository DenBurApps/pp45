using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IdeasSaveData : SaveData
{
    public List<IdeasData> Ideas { get; set; }

    public IdeasSaveData(List<IdeasData> ideas)
    {
        Ideas = ideas;
    }
}
