using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdeasCreator : MonoBehaviour
{
    [SerializeField] private SaveAlert _saveAlert;
    [SerializeField] private NotesCreator _notesCreator;
    [SerializeField] private TMP_InputField _noteName;
    [SerializeField] private TMP_InputField _noteText;
    [SerializeField] private Image _favoriteImage;
    [SerializeField] private Sprite _favoriteSprite;
    [SerializeField] private Sprite _unfavoriteSprite;
    private bool _hasData = false;
    private int _dataIndex = 0;
    private bool _isFavorite = false;

    public void OnFavoriteButtonClick()
    {
        if (_isFavorite)
        {
            _isFavorite = false;
        }
        else
        {
            _isFavorite = true;
        }
        SetFavoriteSprite();
    }

    public void SetFavoriteSprite()
    {
        if (_isFavorite)
        {
            _favoriteImage.sprite = _favoriteSprite;
        }
        else
        {
            _favoriteImage.sprite = _unfavoriteSprite;
        }
    }
    public void ResetData()
    {
        _noteName.text = "";
        _noteText.text = "";
        _hasData = false;
        _isFavorite = false;
    }

    public void SetData(int index)
    {
        IdeasData ideasData = SaveSystem.LoadData<IdeasSaveData>().Ideas[index];
        _isFavorite = ideasData.IsFavorite;
        SetFavoriteSprite();
        _noteName.text = ideasData.Name;
        _noteText.text = ideasData.Note;
        _dataIndex = index;
        _hasData = true;
    }

    public void CloseWindow()
    {
        ResetData();
        _notesCreator.OpenNotes();
    }

    public void SaveNotes()
    {
        var ideas = SaveSystem.LoadData<IdeasSaveData>();
        if (_hasData)
        {
            if (_noteName.text != "")
            {
                ideas.Ideas[_dataIndex].Name = _noteName.text;
            }
            else
            {
                ideas.Ideas[_dataIndex].Name = "Note name";
            }
            if (_noteText.text != "")
            {
                ideas.Ideas[_dataIndex].Note = _noteText.text;
            }
            else
            {
                ideas.Ideas[_dataIndex].Note = "The blank text of the note";
            }
            ideas.Ideas[_dataIndex].IsFavorite = _isFavorite;
            
            
        }
        else
        {
            IdeasData ideasData = new IdeasData();
            ideasData.IsFavorite = _isFavorite;
            if (_noteName.text != "")
            {
                ideasData.Name = _noteName.text;
            }
            else
            {
                ideasData.Name = "Note name";
            }
            if (_noteText.text != "")
            {
                ideasData.Note = _noteText.text;
            }
            else
            {
                ideasData.Note = "The blank text of the note";
            }           
            ideas.Ideas.Add(ideasData);
            _dataIndex = ideas.Ideas.Count - 1;
            _hasData = true;
        }
        SaveSystem.SaveData(ideas);
        _saveAlert.ShowSaveAlert();
    }
}
