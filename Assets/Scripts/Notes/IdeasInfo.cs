using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdeasInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionTexts;
    [SerializeField] private GameObject _favoriteImage;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _showButton;
    private int _ideasIndex;
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

    public void Init(IdeasData ideasData, int index)
    {
        _nameText.text = ideasData.Name;
        _favoriteImage.SetActive(ideasData.IsFavorite);
        _descriptionTexts.text = ideasData.Note;
        _ideasIndex = index;
    }

    public void OnDeleteButtonClick()
    {
        Delete?.Invoke(_ideasIndex);
    }

    public void OnShowButtonClick()
    {
        Show?.Invoke(_ideasIndex);
    }
}
