using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckListCreator : MonoBehaviour
{
    [SerializeField] private SaveAlert _saveAlert;
    [SerializeField] private NotesCreator _notesCreator;
    [SerializeField] private ListField _listField;
    [SerializeField] private Transform _fieldsContainer;
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private Image _favoriteImage;
    [SerializeField] private Sprite _favoriteSprite;
    [SerializeField] private Sprite _unfavoriteSprite;
    private List<ListField> _listFields = new List<ListField>();
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

    public void SetStartData()
    {
        if (!_hasData)
        {
            AddNewField();
        }
    }

    public void SetFavoriteSprite()
    {
        if(_isFavorite)
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
        foreach (var item in _listFields)
        {
            Destroy(item.gameObject);
        }
        _listFields.Clear();
        _nameField.text = "";
        _hasData = false;
        _isFavorite = false;
    }

    public void AddNewField()
    {
        if(_listFields.Count >= 30)
        {
            return;
        }
        ListField newField = Instantiate(_listField, _fieldsContainer);
        _listFields.Add(newField);
    }

    public void SetData(int index)
    {
        CheckListData checkListData = SaveSystem.LoadData<CheckListSaveData>().CheckListDatas[index];
        _nameField.text = checkListData.Name;
        _isFavorite = checkListData.IsFavorite;
        SetFavoriteSprite();
        for (int i = 0; i < checkListData.CheckMarks.Count; i++)
        {
            AddNewField();
        }
        for (int i = 0; i < _listFields.Count; i++)
        {
            Debug.Log(checkListData.CheckNote[i]);
            _listFields[i].SetToggle(checkListData.CheckMarks[i]);
            _listFields[i].SetDescription(checkListData.CheckNote[i]);
        }
        _dataIndex = index;
        _hasData = true;
    }

    public void CloseWindow()
    {
        ResetData();
        _notesCreator.OpenNotes();
    }

    public void SaveCheckList()
    {
        var checkList = SaveSystem.LoadData<CheckListSaveData>();       
        if (_hasData)
        {
            if (_nameField.text != "")
            {
                checkList.CheckListDatas[_dataIndex].Name = _nameField.text;
            }
            else
            {
                checkList.CheckListDatas[_dataIndex].Name = "Note name";
            }
            checkList.CheckListDatas[_dataIndex].IsFavorite = _isFavorite;         
            checkList.CheckListDatas[_dataIndex].CheckMarks.Clear();
            checkList.CheckListDatas[_dataIndex].CheckNote.Clear();
            foreach (var item in _listFields)
            {
                checkList.CheckListDatas[_dataIndex].CheckMarks.Add(item.GetToggle());
                if(item.GetDescription() != "")
                {
                    checkList.CheckListDatas[_dataIndex].CheckNote.Add(item.GetDescription());
                }
                else
                {
                    checkList.CheckListDatas[_dataIndex].CheckNote.Add("Blank field");
                }
            }
        }
        else
        {
            CheckListData checkListData = new CheckListData();
            checkListData.IsFavorite = _isFavorite;
            if(_nameField.text != "")
            {
                checkListData.Name = _nameField.text;
            }
            else
            {
                checkListData.Name = "Note name";
            }
            foreach (var item in _listFields)
            {
                checkListData.CheckMarks.Add(item.GetToggle());
                if(item.GetDescription() != "")
                {
                    checkListData.CheckNote.Add(item.GetDescription());
                }
                else
                {
                    checkListData.CheckNote.Add("Blank field");
                }
            }
            checkList.CheckListDatas.Add(checkListData);
            _dataIndex = checkList.CheckListDatas.Count - 1;
            _hasData = true;
        }
        SaveSystem.SaveData(checkList);
        _saveAlert.ShowSaveAlert();
    }

}
