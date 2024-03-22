using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesCreator : MonoBehaviour
{
    [SerializeField] private CheckListCreator _checkListCreator;
    [SerializeField] private AlertsShower _alertsShower;
    [SerializeField] private IdeasCreator _ideasCreator;
    [SerializeField] private GameObject _notesCanvas;
    [SerializeField] private GameObject _ideasCreatorCanvas;
    [SerializeField] private GameObject _checkListCanvas;
    [SerializeField] private ContentSizeFitter _listFitter;
    [SerializeField] private ContentSizeFitter _contentSizeFitter;
    [SerializeField] private ContentSizeFitter _ideasFitter;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
    [SerializeField] private VerticalLayoutGroup _ideasLayout;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private List<CheckListInfo> _checkListInfos;
    [SerializeField] private List<IdeasInfo> _ideasInfos;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private List<SpriteSwitcher> _spriteSwitchers;
    [SerializeField] private GameObject _noItemsAlert;
    private int _currentFilterIndex = 0;
    private int _currentIndex = 0;

    private void OnEnable()
    {
        foreach (var item in _checkListInfos)
        {
            item.Delete += OnDeleteCheckList;
            item.Show += OnShowCheckList;
        }
        foreach (var item in _ideasInfos)
        {
            item.Delete += OnDeleteIdeas;
            item.Show += OnShowIdeas;
        }
    }

    private void Start()
    {
        SetAllFilter();
    }

    private void OnDisable()
    {
        foreach (var item in _checkListInfos)
        {
            item.Delete -= OnDeleteCheckList;
            item.Show -= OnShowCheckList;
        }
        foreach (var item in _ideasInfos)
        {
            item.Delete -= OnDeleteIdeas;
            item.Show -= OnShowIdeas;
        }
    }

    public void SetAllFilter()
    {
        _currentFilterIndex = 0;
        foreach (var item in _spriteSwitchers)
        {
            item.SwitchSprite(false);
        }
        _spriteSwitchers[0].SwitchSprite(true);
        _listFitter.gameObject.SetActive(true);
        _ideasFitter.gameObject.SetActive(true);
        UpdateIdeas();
        UpdateCheckLists();
        CheckHasElements();
        UpdateLayouts();
    }

    public void SetIdeasFilter()
    {
        _currentFilterIndex = 2;
        foreach (var item in _spriteSwitchers)
        {
            item.SwitchSprite(false);
        }
        _spriteSwitchers[2].SwitchSprite(true);
        _listFitter.gameObject.SetActive(false);
        _ideasFitter.gameObject.SetActive(true);
        UpdateIdeas();
        if(SaveSystem.LoadData<IdeasSaveData>().Ideas.Count <= 0)
        {
            _noItemsAlert.SetActive(true);
        }
        else
        {
            _noItemsAlert.SetActive(false);
        }
        UpdateLayouts();
    }

    public void SetCheckListFilter()
    {
        _currentFilterIndex = 1;
        foreach (var item in _spriteSwitchers)
        {
            item.SwitchSprite(false);
        }
        _spriteSwitchers[1].SwitchSprite(true);
        _listFitter.gameObject.SetActive(true);
        _ideasFitter.gameObject.SetActive(false);
        UpdateCheckLists();
        if (SaveSystem.LoadData<CheckListSaveData>().CheckListDatas.Count <= 0)
        {
            _noItemsAlert.SetActive(true);
        }
        else
        {
            _noItemsAlert.SetActive(false);
        }
        UpdateLayouts();
    }

    public void SetFavoritesFilter()
    {
        _currentFilterIndex = 3;
        foreach (var item in _spriteSwitchers)
        {
            item.SwitchSprite(false);
        }
        _spriteSwitchers[3].SwitchSprite(true);
        _listFitter.gameObject.SetActive(true);
        _ideasFitter.gameObject.SetActive(true);
        UpdateFavoriteIdeas();
        UpdateFavoriteCheckList();
        CheckHasFavoriteElements();
        UpdateLayouts();
    }

    public void UpdateLayouts()
    {
        Canvas.ForceUpdateCanvases();
        _gridLayoutGroup.CalculateLayoutInputVertical();
        _listFitter.SetLayoutVertical();
        _ideasLayout.CalculateLayoutInputVertical();
        _ideasFitter.SetLayoutVertical();
        _verticalLayoutGroup.CalculateLayoutInputVertical();
        _contentSizeFitter.SetLayoutVertical();
        _scrollRect.verticalNormalizedPosition = 1;    
    }

    public void OpenNotes()
    {        
        _checkListCanvas.SetActive(false);
        _ideasCreatorCanvas.SetActive(false);
        _notesCanvas.SetActive(true);

        SetCurrentFilter();

    }

    public void OnCreateNotesClick()
    {
        _alertsShower.ShowCreateNoteAlert();
    }

    public void OpenCreateIdeas()
    {
        _notesCanvas.SetActive(false);
        _ideasCreatorCanvas.SetActive(true);
        _ideasCreator.SetFavoriteSprite();
    }

    public void OpenCreateList()
    {
        _notesCanvas.SetActive(false);
        _checkListCanvas.SetActive(true);
        _checkListCreator.SetFavoriteSprite();
        _checkListCreator.SetStartData();
    }

    public void CheckHasElements()
    {
        if(SaveSystem.LoadData<CheckListSaveData>().CheckListDatas.Count <= 0 && SaveSystem.LoadData<IdeasSaveData>().Ideas.Count <= 0)
        {
            _noItemsAlert.SetActive(true);
        }
        else
        {
            _noItemsAlert.SetActive(false);
        }
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
                    SetCheckListFilter();
                    break;
                }
            case 2:
                {
                    SetIdeasFilter();
                    break;
                }
            case 3:
                {
                    SetFavoritesFilter();
                    break;
                }
        }
    }

    public void CheckHasFavoriteElements()
    {
        var ideas = SaveSystem.LoadData<IdeasSaveData>();
        var checkList = SaveSystem.LoadData<CheckListSaveData>();
        int favoritesCount = 0;
        for (int i = 0; i < ideas.Ideas.Count; i++)
        {
            if (ideas.Ideas[i].IsFavorite)
            {
                favoritesCount++;
            }
        }
        for (int i = 0; i < checkList.CheckListDatas.Count; i++)
        {
            if (checkList.CheckListDatas[i].IsFavorite)
            {
                favoritesCount++;
            }
        }
        if(favoritesCount <= 0)
        {
            _noItemsAlert.SetActive(true);
        }
        else
        {
            _noItemsAlert.SetActive(false);
        }

    }

    public void UpdateFavoriteIdeas()
    {
        foreach (var item in _ideasInfos)
        {
            item.gameObject.SetActive(false);
        }
        var ideas = SaveSystem.LoadData<IdeasSaveData>();
        if (ideas.Ideas.Count <= 0)
        {
            _ideasFitter.gameObject.SetActive(false);
            return;
        }
        _ideasFitter.gameObject.SetActive(true);
        int favoritesCount = 0;
        for (int i = 0; i < ideas.Ideas.Count; i++)
        {
            if (ideas.Ideas[i].IsFavorite)
            {
                _ideasInfos[i].Init(ideas.Ideas[i], i);
                _ideasInfos[i].gameObject.SetActive(true);
                favoritesCount++;
            }
        }
        if (favoritesCount <= 0)
        {
            _ideasFitter.gameObject.SetActive(false);
        }
        UpdateLayouts();
    }

    public void UpdateFavoriteCheckList()
    {
        foreach (var item in _checkListInfos)
        {
            item.gameObject.SetActive(false);
        }
        var checkList = SaveSystem.LoadData<CheckListSaveData>();
        if (checkList.CheckListDatas.Count <= 0)
        {
            _listFitter.gameObject.SetActive(false);
            return;
        }
        _listFitter.gameObject.SetActive(true);
        int favoritesCount = 0;
        for (int i = 0; i < checkList.CheckListDatas.Count; i++)
        {
            if (checkList.CheckListDatas[i].IsFavorite)
            {
                _checkListInfos[i].Init(checkList.CheckListDatas[i], i);
                _checkListInfos[i].gameObject.SetActive(true);
                favoritesCount++;
            }
        }
        if(favoritesCount <= 0)
        {
            _listFitter.gameObject.SetActive(false);
        }
        UpdateLayouts();
    }

    public void UpdateIdeas()
    {
        foreach (var item in _ideasInfos)
        {
            item.gameObject.SetActive(false);
        }
        var ideas = SaveSystem.LoadData<IdeasSaveData>();
        if(ideas.Ideas.Count <= 0)
        {
            _ideasFitter.gameObject.SetActive(false);
            return;
        }
        _ideasFitter.gameObject.SetActive(true);
        for (int i = 0; i < ideas.Ideas.Count; i++)
        {
            _ideasInfos[i].Init(ideas.Ideas[i], i);
            _ideasInfos[i].gameObject.SetActive(true);
        }
        UpdateLayouts();
    }

    public void UpdateCheckLists()
    {
        foreach (var item in _checkListInfos)
        {
            item.gameObject.SetActive(false);
        }
        var checkList = SaveSystem.LoadData<CheckListSaveData>();
        if(checkList.CheckListDatas.Count <= 0)
        {
            _listFitter.gameObject.SetActive(false);
            return;
        }
        _listFitter.gameObject.SetActive(true);
        for (int i = 0; i < checkList.CheckListDatas.Count; i++)
        {
            _checkListInfos[i].Init(checkList.CheckListDatas[i], i);
            _checkListInfos[i].gameObject.SetActive(true);
        }
        UpdateLayouts();
    }

    public void OnDeleteCheckList(int checkListIndex)
    {
        _alertsShower.ShowDeleteListAlert();
        _currentIndex = checkListIndex;
    }

    public void DeleteCheckList()
    {
        var checkList = SaveSystem.LoadData<CheckListSaveData>();
        checkList.CheckListDatas.RemoveAt(_currentIndex);
        SaveSystem.SaveData(checkList);
        SetCurrentFilter();
    }

    public void OnShowCheckList(int checkListIndex)
    {
        _checkListCreator.SetData(checkListIndex);
        OpenCreateList();
    }

    public void OnDeleteIdeas(int ideasIndex)
    {
        _alertsShower.ShowDeleteIdeasAlert();
        _currentIndex = ideasIndex;
    }

    public void DeleteIdeas()
    {
        var ideas = SaveSystem.LoadData<IdeasSaveData>();
        ideas.Ideas.RemoveAt(_currentIndex);
        SaveSystem.SaveData(ideas);
        SetCurrentFilter();
    }

    public void OnShowIdeas(int ideasIndex)
    {
        _ideasCreator.SetData(ideasIndex);
        OpenCreateIdeas();
    }
}
