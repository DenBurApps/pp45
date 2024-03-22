using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CheckListSaveData : SaveData
{
    public List<CheckListData> CheckListDatas { get; set; }

    public CheckListSaveData(List<CheckListData> checkListDatas)
    {
        CheckListDatas = checkListDatas;
    }
}
