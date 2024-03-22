using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasscodeSetter : MonoBehaviour
{
    [SerializeField] private GameObject _passwordSettings;
    [SerializeField] private GameObject _passcodeSetter;
    [SerializeField] private TMP_InputField _keepPasscodeInput;
    [SerializeField] private TMP_InputField _repeatPasscodeInput;
    [SerializeField] private GameObject _keepPasscode;
    [SerializeField] private GameObject _repeatPasscode;
    [SerializeField] private List<Image> _keepImages;
    [SerializeField] private List<Image> _repeatImages;
    [SerializeField] private Sprite _unactiveSprite;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private TMP_Text _errorText;
    private string _keepResult;
    private string _repeatResult;

    public void StartKeepPasscode()
    {
        _errorText.gameObject.SetActive(false);
        _repeatPasscode.SetActive(false);
        _keepPasscodeInput.text = "";
        _repeatPasscodeInput.text = "";
        _keepPasscode.SetActive(true);
        _keepPasscodeInput.ActivateInputField();
        _keepPasscodeInput.Select();
        _keepPasscodeInput.onValueChanged.AddListener(OnKeepPasscodeChange);
    }

    public void OnKeepPasscodeChange(string text)
    {
        foreach (var item in _keepImages)
        {
            item.sprite = _unactiveSprite;
        }
        for (int i = 0; i < text.Length; i++)
        {
            _keepImages[i].sprite = _activeSprite;
        }
        if(text.Length == 6)
        {
            _keepResult = text;
            _keepPasscode.SetActive(false);
            _keepPasscodeInput.DeactivateInputField();
            StartRepeatPasscode();
        }
    }

    public void StartRepeatPasscode()
    {
        _repeatPasscode.SetActive(true);
        _repeatPasscodeInput.ActivateInputField();
        _repeatPasscodeInput.Select();
        _repeatPasscodeInput.onValueChanged.AddListener(OnRepeatPasscodeChange);
    }

    public void OnRepeatPasscodeChange(string text)
    {
        foreach (var item in _repeatImages)
        {
            item.sprite = _unactiveSprite;
        }
        for (int i = 0; i < text.Length; i++)
        {
            _repeatImages[i].sprite = _activeSprite;
        }
        if (text.Length == 6)
        {
            _repeatResult = text;
            if(_repeatResult == _keepResult)
            {
                _repeatPasscodeInput.DeactivateInputField();
                _passwordSettings.SetActive(true);
                _passcodeSetter.SetActive(false);
                var settings = SaveSystem.LoadData<SettingSaveData>();
                settings.HasPass = true;
                settings.Passcode = text;
                SaveSystem.SaveData(settings);
            }
            else
            {
                StartCoroutine(ShowError());
            }
        }
    }

    private IEnumerator ShowError()
    {
        _errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        _errorText.gameObject.SetActive(false);
    }
}
