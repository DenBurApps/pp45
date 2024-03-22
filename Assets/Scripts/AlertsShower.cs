using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertsShower : MonoBehaviour
{
    [SerializeField] private GameObject _alertCanvas;
    [SerializeField] private GameObject _createNoteAlert;
    [SerializeField] private GameObject _createPasswordAlert;
    [SerializeField] private GameObject _deletePasswordAlert;
    [SerializeField] private GameObject _deleteListAlert;
    [SerializeField] private GameObject _deleteIdeasAlert;
    private GameObject _currentAlert;

    public void ShowCreateNoteAlert()
    {
        _alertCanvas.SetActive(true);
        _createNoteAlert.SetActive(true);
        _currentAlert = _createNoteAlert;
    }

    public void ShowCreatePasswordAlert()
    {
        _alertCanvas.SetActive(true);
        _createPasswordAlert.SetActive(true);
        _currentAlert = _createPasswordAlert;
    }

    public void ShowDeletePasswordAlert()
    {
        _alertCanvas.SetActive(true);
        _deletePasswordAlert.SetActive(true);
        _currentAlert = _deletePasswordAlert;
    }

    public void ShowDeleteListAlert()
    {
        _alertCanvas.SetActive(true);
        _deleteListAlert.SetActive(true);
        _currentAlert = _deleteListAlert;
    }

    public void ShowDeleteIdeasAlert()
    {
        _alertCanvas.SetActive(true);
        _deleteIdeasAlert.SetActive(true);
        _currentAlert = _deleteIdeasAlert;
    }

    public void HideAlert()
    {
        _currentAlert.SetActive(false);
        _alertCanvas.SetActive(false);
    }
}
