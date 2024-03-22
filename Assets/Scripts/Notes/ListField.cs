using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListField : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TMP_InputField _description;

    public string GetDescription()
    {
        return _description.text;
    }

    public bool GetToggle()
    {
        return _toggle.isOn;
    }

    public void SetToggle(bool isOn)
    {
        _toggle.isOn = isOn;
    }

    public void SetDescription(string description)
    {
        string text = description;
        Debug.Log(text);
        _description.text = text;
    }
}
