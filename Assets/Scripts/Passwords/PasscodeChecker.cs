using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasscodeChecker : MonoBehaviour
{
    [SerializeField] private TMP_InputField _passcodeInput;
    [SerializeField] private GameObject _passcodeChecker;
    [SerializeField] private List<Image> _enterImages;
    [SerializeField] private Sprite _unactiveSprite;
    [SerializeField] private Sprite _activeSprite;
    private string _result;

    private void Awake()
    {
        var setting = SaveSystem.LoadData<SettingSaveData>();
        if (setting.HasPass)
        {
            _passcodeChecker.SetActive(true);
        }
        else
        {
            _passcodeChecker.SetActive(false);
        }
    }

    public void OnNumberButtonClicked(string value)
    {
        _passcodeInput.text += value;
        foreach (var item in _enterImages)
        {
            item.sprite = _unactiveSprite;
        }
        for (int i = 0; i < _passcodeInput.text.Length; i++)
        {
            _enterImages[i].sprite = _activeSprite;
        }
        if (_passcodeInput.text.Length == 6)
        {
            _result = _passcodeInput.text;
            var settings = SaveSystem.LoadData<SettingSaveData>();
            if (_result == settings.Passcode)
            {
                _passcodeChecker.SetActive(false);
            }
            else
            {
                _passcodeInput.text = "";
                foreach (var item in _enterImages)
                {
                    item.sprite = _unactiveSprite;
                }
                for (int i = 0; i < _passcodeInput.text.Length; i++)
                {
                    _enterImages[i].sprite = _activeSprite;
                }
            }
        }
    }
}
