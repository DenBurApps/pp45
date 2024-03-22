using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PasswordGenerator : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _languageDropdown;
    [SerializeField] private TMP_Dropdown _numbersDropdown;
    [SerializeField] private TMP_Dropdown _capitalDropdown;
    [SerializeField] private TMP_Dropdown _specialDropdown;
    [SerializeField] private TMP_InputField _lengthInput;
    [SerializeField] private Sprite _littleDisableSprite;
    [SerializeField] private Sprite _littleEnableSprite;
    [SerializeField] private Sprite _bigDisableSprite;
    [SerializeField] private Sprite _bigEnableSprite;
    [SerializeField] private Sprite _bigErrorSprite;
    [SerializeField] private Button _generateButton;
    [SerializeField] private GameObject _passwordContainer;
    [SerializeField] private TMP_InputField _password;
    private int _lenght;
    private string _englishUppercaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string _englishLowercaseAlphabet = "abcdefghijklmnopqrstuvwxyz";
    private string _russianUppercaseAlphabet = "¿¡¬√ƒ≈®∆«»… ÀÃÕŒœ–—“”‘’÷◊ÿŸ⁄€‹›ﬁﬂ";
    private string _russianLowercaseAlphabet = "‡·‚„‰Â∏ÊÁËÈÍÎÏÌÓÔÒÚÛÙıˆ˜¯˘˙˚¸˝˛ˇ";
    private string _digits = "0123456789";
    private string _specialCharacters = "!@#$%^&*()_+-=[]{}|;:,.<>?";


    private void Start()
    {
        CheckAvaliableGenerate();
    }

    private void OnEnable()
    {
        _languageDropdown.onValueChanged.AddListener(CheckLanguage);
        _numbersDropdown.onValueChanged.AddListener(CheckNumbers);
        _capitalDropdown.onValueChanged.AddListener(CheckCapital);
        _specialDropdown.onValueChanged.AddListener(CheckSpecial);
        _lengthInput.onValueChanged.AddListener(CheckLength);
    }

    private void OnDisable()
    {
        _languageDropdown.onValueChanged.RemoveListener(CheckLanguage);
        _numbersDropdown.onValueChanged.RemoveListener(CheckNumbers);
        _capitalDropdown.onValueChanged.RemoveListener(CheckCapital);
        _specialDropdown.onValueChanged.RemoveListener(CheckSpecial);
        _lengthInput.onValueChanged.RemoveListener(CheckLength);
    }

    public void CheckAvaliableGenerate()
    {
        if (_languageDropdown.value == 0 || _numbersDropdown.value == 0 || _capitalDropdown.value == 0 || _specialDropdown.value == 0 || _lengthInput.text == "" || _lenght > 30 || _lenght < 3)
        {
            _generateButton.interactable = false;
        }
        else
        {
            _generateButton.interactable = true;
        }
    }

    private void CheckLanguage(int index)
    {
        if(index != 0)
        {
            _languageDropdown.GetComponent<Image>().sprite = _littleEnableSprite;
        }
        else
        {
            _languageDropdown.GetComponent<Image>().sprite = _littleDisableSprite;
        }
        CheckAvaliableGenerate();
    }

    private void CheckNumbers(int index)
    {
        if (index != 0)
        {
            _numbersDropdown.GetComponent<Image>().sprite = _littleEnableSprite;
        }
        else
        {
            _numbersDropdown.GetComponent<Image>().sprite = _littleDisableSprite;
        }
        CheckAvaliableGenerate();
    }

    private void CheckCapital(int index)
    {
        if (index != 0)
        {
            _capitalDropdown.GetComponent<Image>().sprite = _bigEnableSprite;
        }
        else
        {
            _capitalDropdown.GetComponent<Image>().sprite = _bigDisableSprite;
        }
        CheckAvaliableGenerate();
    }

    private void CheckSpecial(int index)
    {
        if (index != 0)
        {
            _specialDropdown.GetComponent<Image>().sprite = _bigEnableSprite;
        }
        else
        {
            _specialDropdown.GetComponent<Image>().sprite = _bigDisableSprite;
        }
        CheckAvaliableGenerate();
    }

    private void CheckLength(string text)
    {
        if(text == "")
        {
            _lengthInput.GetComponent<Image>().sprite = _bigDisableSprite;
        }
        else if (int.Parse(text) <= 30 && int.Parse(text) >= 3)
        {
            _lengthInput.GetComponent<Image>().sprite = _bigEnableSprite;
            _lenght = int.Parse(text);
        }
        else
        {
            _lengthInput.GetComponent<Image>().sprite = _bigErrorSprite;
            _lenght = int.Parse(text);
        }
        CheckAvaliableGenerate();
    }

    public void GeneratePassword()
    {
        int passwordLength = int.Parse(_lengthInput.text);
        string allowedChars = "";

        if (_languageDropdown.value == 1)
        {
            allowedChars += _englishLowercaseAlphabet;
            if(_capitalDropdown.value == 1)
            {
                allowedChars += _englishUppercaseAlphabet; 
            }
        }
        else if(_languageDropdown.value == 2)
        {
            allowedChars += _russianLowercaseAlphabet;
            if (_capitalDropdown.value == 1)
            {
                allowedChars += _russianUppercaseAlphabet;
            }
        }
        if(_numbersDropdown.value == 1)
        {
            allowedChars += _digits;
        }
        if(_specialDropdown.value == 1)
        {
            allowedChars += _specialCharacters;
        }
        char[] passwordChars = new char[passwordLength];
        System.Random random = new System.Random();

        if (_languageDropdown.value == 1)
        {
            if (_capitalDropdown.value == 1)
            {
                passwordChars[0] = _englishUppercaseAlphabet[random.Next(0, _englishUppercaseAlphabet.Length)]; 
            }
        }
        else if (_languageDropdown.value == 2)
        {
            
            if (_capitalDropdown.value == 1)
            {
                passwordChars[0] = _russianUppercaseAlphabet[random.Next(0, _russianUppercaseAlphabet.Length)];
            }
        }
        if (_numbersDropdown.value == 1)
        {
            passwordChars[1] = _digits[random.Next(0, 10)];
        }
        if (_specialDropdown.value == 1)
        {
            passwordChars[2] = _specialCharacters[random.Next(0, _specialCharacters.Length)];
        }
        int itemsCount = 0;
        for (int i = 0; i < passwordChars.Length; i++)
        {
            if(passwordChars[i] != '\0')
            {
                itemsCount++;
            }
        }
        Debug.Log(itemsCount);
        for (int i = itemsCount; i < passwordLength; i++)
        {
            passwordChars[i] = allowedChars[random.Next(0, allowedChars.Length)];
        }

        // œÂÂÏÂ¯Ë‚‡ÂÏ ÒËÏ‚ÓÎ˚ Ô‡ÓÎˇ
        for (int i = 0; i < passwordLength; i++)
        {
            int rndIndex = random.Next(i, passwordLength);
            char temp = passwordChars[rndIndex];
            passwordChars[rndIndex] = passwordChars[i];
            passwordChars[i] = temp;
        }

        string generatedPassword = new string(passwordChars);
        _passwordContainer.SetActive(true);
        _password.text = generatedPassword;

    }
}
