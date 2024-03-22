using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SettingSaveData : SaveData
{
    public bool HasPass { get; set; }
    public string Passcode { get; set; }
    public SettingSaveData (bool hasPass, string passcode)
    {
        HasPass = hasPass;
        Passcode = passcode;
    }
}
