using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordCreator : MonoBehaviour
{
    [SerializeField] private SaveAlert _saveAlert;
    [SerializeField] private PasswordController _passwordController;
    [SerializeField] private TMP_Text _typeText;
    [SerializeField] private TMP_InputField _noteName;
    [SerializeField] private TMP_InputField _loginText;
    [SerializeField] private TMP_InputField _passwordText;
    [SerializeField] private TMP_InputField _urlText;
    [SerializeField] private TMP_InputField _noteText;
    [SerializeField] private ScrollRect _scrollRect;
    private bool _hasData = false;
    private int _dataIndex = 0;

    public void ResetData()
    {
        _noteName.text = "";
        _noteText.text = "";
        _typeText.text = "";
        _loginText.text = "";
        _passwordText.text = "";
        _urlText.text = "";
        _hasData = false;
        _scrollRect.verticalNormalizedPosition = 1;
    }

    public void SetType(string type)
    {
        _typeText.text = type;
    }

    public void SetData(int index)
    {
        PasswordData passData = SaveSystem.LoadData<PasswordSaveData>().Passwords[index];
        _typeText.text = passData.Type;
        _noteName.text = passData.Name;
        _loginText.text = passData.Login;
        _urlText.text = passData.Site;
        _passwordText.text = passData.Password;
        _noteText.text = passData.Note;
        _dataIndex = index;
        _hasData = true;
    }

    public void CloseWindow()
    {
        ResetData();
        _passwordController.OpenPasswords();
    }

    public void SaveNotes()
    {
        var passdata = SaveSystem.LoadData<PasswordSaveData>();
        if (_hasData)
        {
            if (_noteName.text != "")
            {
                passdata.Passwords[_dataIndex].Name = _noteName.text;
            }
            else
            {
                passdata.Passwords[_dataIndex].Name = "Note name";
            }
            if (_loginText.text != "")
            {
                passdata.Passwords[_dataIndex].Login = _loginText.text;
            }
            else
            {
                passdata.Passwords[_dataIndex].Login = "Blank login";
            }
            passdata.Passwords[_dataIndex].Type = _typeText.text;
            passdata.Passwords[_dataIndex].Password = _passwordText.text;
            passdata.Passwords[_dataIndex].Site = _urlText.text;
            passdata.Passwords[_dataIndex].Note = _noteText.text;
        }
        else
        {
            PasswordData passwordData = new PasswordData();
            if (_noteName.text != "")
            {
                passwordData.Name = _noteName.text;
            }
            else
            {
                passwordData.Name = "Note name";
            }
            if (_loginText.text != "")
            {
                passwordData.Login = _loginText.text;
            }
            else
            {
                passwordData.Login = "Blank login";
            }
            passwordData.Type = _typeText.text;
            passwordData.Password = _passwordText.text;
            passwordData.Site = _urlText.text;
            passwordData.Note = _noteText.text;

            passdata.Passwords.Add(passwordData);
            _dataIndex = passdata.Passwords.Count - 1;
            _hasData = true;
        }
        SaveSystem.SaveData(passdata);
        _saveAlert.ShowSaveAlert();
    }
}
