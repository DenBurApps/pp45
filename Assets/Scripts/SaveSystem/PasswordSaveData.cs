using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PasswordSaveData : SaveData
{
    public List<PasswordData> Passwords { get; set; }

    public PasswordSaveData(List<PasswordData> passwords)
    {
        Passwords = passwords;
    }
}
