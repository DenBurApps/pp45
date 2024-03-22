using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class Settings : MonoBehaviour
{
    [SerializeField] private Navigator _navigator;
    [SerializeField] private GameObject _privacyCanvas;
    [SerializeField] private GameObject _termsCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _passwordSettings;
    [SerializeField] private GameObject _passwordSetCanvas;
    [SerializeField] private GameObject _setPasscodeCanvas;
    [SerializeField] private PasscodeSetter _passcodeSetter;
    [SerializeField] private string _email;

    public void ShowPrivacy()
    {
        _settingsCanvas.SetActive(false);
        _privacyCanvas.SetActive(true);
    }

    public void ShowTerms()
    {
        _settingsCanvas.SetActive(false);
        _termsCanvas.SetActive(true);
    }

    public void RateUs()
    {
        Device.RequestStoreReview();
    }

    public void ContactUs()
    {
        Application.OpenURL("mailto:" + _email + "?subject=Mail to developer");
    }

    public void ShowPassword()
    {
        var settings = SaveSystem.LoadData<SettingSaveData>();
        if (settings.HasPass)
        {
            _passwordSettings.SetActive(true);
            _settingsCanvas.SetActive(false);
        }
        else
        {
            _settingsCanvas.SetActive(false);
            _passwordSetCanvas.SetActive(true);
        }

    }

    public void EnablePassword()
    {
        _passwordSettings.SetActive(false);
        _passwordSetCanvas.SetActive(false);
        _setPasscodeCanvas.SetActive(true);
        _passcodeSetter.StartKeepPasscode();
    }

    public void DisablePasscode()
    {
        var settings = SaveSystem.LoadData<SettingSaveData>();
        settings.HasPass = false;
        settings.Passcode = "";
        SaveSystem.SaveData(settings);
        _navigator.ShowSettings();
    }
}
