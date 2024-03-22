using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _type;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _login;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _showButton;

    private int _passIndex;
    public Action<int> Delete;
    public Action<int> Show;

    private void OnEnable()
    {
        _deleteButton.onClick.AddListener(OnDeleteButtonClick);
        _showButton.onClick.AddListener(OnShowButtonClick);
    }

    private void OnDisable()
    {
        _deleteButton.onClick.RemoveAllListeners();
        _showButton.onClick.RemoveAllListeners();
    }

    public void Init(PasswordData passwordData, int index)
    {
        _type.text = passwordData.Type;
        _name.text = passwordData.Name;
        _login.text = passwordData.Login;
        _passIndex = index;
    }

    public void OnDeleteButtonClick()
    {
        Delete?.Invoke(_passIndex);
    }

    public void OnShowButtonClick()
    {
        Show?.Invoke(_passIndex);
    }
}
