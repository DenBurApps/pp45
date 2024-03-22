using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordController : MonoBehaviour
{
    [SerializeField] private PasswordCreator _passwordCreator;
    [SerializeField] private AlertsShower _alertsShower;
    [SerializeField] private GameObject _passwordCanvas;
    [SerializeField] private GameObject _passwordCreatorCanvas;
    [SerializeField] private ContentSizeFitter _listFitter;
    [SerializeField] private ContentSizeFitter _contentSizeFitter;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private List<PasswordInfo> _passwordInfos;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private List<SpriteSwitcher> _spriteSwitchers;
    [SerializeField] private GameObject _noItemsAlert;
    private int _currentFilterIndex = 0;
    private int _currentIndex = 0;

    private void OnEnable()
    {
        UpdatePassword();
        foreach (var item in _passwordInfos)
        {
            item.Delete += OnDeletePassword;
            item.Show += OnShowPassword;
        }
    }

    private void OnDisable()
    {
        foreach (var item in _passwordInfos)
        {
            item.Delete -= OnDeletePassword;
            item.Show -= OnShowPassword;
        }
    }

    private void Start()
    {
        SetAllFilter();
    }

    public void UpdateLayouts()
    {
        Canvas.ForceUpdateCanvases();
        _gridLayoutGroup.CalculateLayoutInputVertical();
        _listFitter.SetLayoutVertical();
        _verticalLayoutGroup.CalculateLayoutInputVertical();
        _contentSizeFitter.SetLayoutVertical();
        _scrollRect.verticalNormalizedPosition = 1;
    }

    public void SetAllFilter()
    {
        _currentFilterIndex = 0;
        foreach (var item in _spriteSwitchers)
        {
            item.SwitchSprite(false);
        }
        _spriteSwitchers[0].SwitchSprite(true);
        UpdatePassword();
        CheckHasElements();
        UpdateLayouts();
    }

    public void SetPersonalFilter()
    {
        _currentFilterIndex = 1;
        foreach (var item in _spriteSwitchers)
        {
            item.SwitchSprite(false);
        }
        _spriteSwitchers[1].SwitchSprite(true);
        UpdatePersonalPassword();
        CheckHasPersonalElements();
        UpdateLayouts();
    }

    public void SetWorkFilter()
    {
        _currentFilterIndex = 2;
        foreach (var item in _spriteSwitchers)
        {
            item.SwitchSprite(false);
        }
        _spriteSwitchers[2].SwitchSprite(true);
        UpdateWorkPassword();
        CheckHasWorkElements();
        UpdateLayouts();
    }

    public void SetCurrentFilter()
    {
        switch (_currentFilterIndex)
        {
            case 0:
                {
                    SetAllFilter();
                    break;
                }
            case 1:
                {
                    SetPersonalFilter();
                    break;
                }
            case 2:
                {
                    SetWorkFilter();
                    break;
                }
        }
    }

    public void CheckHasElements()
    {
        if (SaveSystem.LoadData<PasswordSaveData>().Passwords.Count <= 0)
        {
            _noItemsAlert.SetActive(true);
        }
        else
        {
            _noItemsAlert.SetActive(false);
        }
    }

    public void CheckHasPersonalElements()
    {
        var pass = SaveSystem.LoadData<PasswordSaveData>();
        int personalCount = 0;
        for (int i = 0; i < pass.Passwords.Count; i++)
        {
            if (pass.Passwords[i].Type == "Personal")
            {
                personalCount++;
            }
        }
        if (personalCount <= 0)
        {
            _noItemsAlert.SetActive(true);
        }
        else
        {
            _noItemsAlert.SetActive(false);
        }
    }

    public void CheckHasWorkElements()
    {
        var pass = SaveSystem.LoadData<PasswordSaveData>();
        int personalCount = 0;
        for (int i = 0; i < pass.Passwords.Count; i++)
        {
            if (pass.Passwords[i].Type == "Work")
            {
                personalCount++;
            }
        }
        if (personalCount <= 0)
        {
            _noItemsAlert.SetActive(true);
        }
        else
        {
            _noItemsAlert.SetActive(false);
        }
    }

    public void OpenPasswords()
    {
        _passwordCreatorCanvas.SetActive(false);
        _passwordCanvas.SetActive(true);

        SetCurrentFilter();
    }

    public void OnCreatePasswordClick()
    {
        _alertsShower.ShowCreatePasswordAlert();
    }

    public void OpenCreatePersonalPassword()
    {
        _passwordCreatorCanvas.SetActive(true);
        _passwordCanvas.SetActive(false);
        _passwordCreator.SetType("Personal");
    }

    public void OpenCreateWorkPassword()
    {
        _passwordCreatorCanvas.SetActive(true);
        _passwordCanvas.SetActive(false);
        _passwordCreator.SetType("Work");
    }

    public void OpenCreatePassword()
    {
        _passwordCreatorCanvas.SetActive(true);
        _passwordCanvas.SetActive(false);
    }


    public void UpdatePassword()
    {
        foreach (var item in _passwordInfos)
        {
            item.gameObject.SetActive(false);
        }
        var password = SaveSystem.LoadData<PasswordSaveData>();
        if (password.Passwords.Count <= 0)
        {
            _listFitter.gameObject.SetActive(false);
            return;
        }
        _listFitter.gameObject.SetActive(true);
        for (int i = 0; i < password.Passwords.Count; i++)
        {
            _passwordInfos[i].Init(password.Passwords[i], i);
            _passwordInfos[i].gameObject.SetActive(true);
        }
        UpdateLayouts();
    }

    public void UpdatePersonalPassword()
    {
        foreach (var item in _passwordInfos)
        {
            item.gameObject.SetActive(false);
        }
        var password = SaveSystem.LoadData<PasswordSaveData>();
        if (password.Passwords.Count <= 0)
        {
            _listFitter.gameObject.SetActive(false);
            return;
        }
        _listFitter.gameObject.SetActive(true);
        for (int i = 0; i < password.Passwords.Count; i++)
        {
            if(password.Passwords[i].Type == "Personal")
            {
                _passwordInfos[i].Init(password.Passwords[i], i);
                _passwordInfos[i].gameObject.SetActive(true);
            }
        }
        UpdateLayouts();
    }

    public void UpdateWorkPassword()
    {
        foreach (var item in _passwordInfos)
        {
            item.gameObject.SetActive(false);
        }
        var password = SaveSystem.LoadData<PasswordSaveData>();
        if (password.Passwords.Count <= 0)
        {
            _listFitter.gameObject.SetActive(false);
            return;
        }
        _listFitter.gameObject.SetActive(true);
        for (int i = 0; i < password.Passwords.Count; i++)
        {
            if (password.Passwords[i].Type == "Work")
            {
                _passwordInfos[i].Init(password.Passwords[i], i);
                _passwordInfos[i].gameObject.SetActive(true);
            }
        }
        UpdateLayouts();
    }

    public void OnDeletePassword(int ideasIndex)
    {
        _alertsShower.ShowDeletePasswordAlert();
        _currentIndex = ideasIndex;
    }

    public void DeletePassword()
    {
        var pass = SaveSystem.LoadData<PasswordSaveData>();
        pass.Passwords.RemoveAt(_currentIndex);
        SaveSystem.SaveData(pass);
        SetCurrentFilter();
    }

    public void OnShowPassword(int ideasIndex)
    {
        _passwordCreator.SetData(ideasIndex);
        OpenCreatePassword();
    }
}
