using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckListInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private List<Toggle> _toggles;
    [SerializeField] private List<TMP_Text> _descriptionTexts;
    [SerializeField] private GameObject _favoriteImage;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _showButton;
    private int _checkListIndex;
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

    public void Init(CheckListData checkListData, int index)
    {
        _checkListIndex = index;
        _nameText.text = checkListData.Name;
        _favoriteImage.SetActive(checkListData.IsFavorite);

        foreach (var item in _toggles)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in _descriptionTexts)
        {
            item.gameObject.SetActive(false);
        }
        int iterationCount = 0;
        if (checkListData.CheckNote.Count < 4)
        {
            iterationCount = checkListData.CheckNote.Count;
        }
        else
        {
            iterationCount = 4;
        }
        for (int i = 0; i < iterationCount; i++)
        {
            if(checkListData.CheckNote[i] != null)
            {
                _toggles[i].isOn = checkListData.CheckMarks[i];
                _descriptionTexts[i].text = checkListData.CheckNote[i];
                _toggles[i].gameObject.SetActive(true);
                _descriptionTexts[i].gameObject.SetActive(true);
            }
        }
    }

    public void OnDeleteButtonClick()
    {
        Delete?.Invoke(_checkListIndex);
    }

    public void OnShowButtonClick()
    {
        Show?.Invoke(_checkListIndex);
    }
}
