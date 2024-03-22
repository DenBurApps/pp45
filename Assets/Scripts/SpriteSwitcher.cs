using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;
    [SerializeField] private Image _image;


    public void SwitchSprite(bool enabled)
    {
        if (enabled)
        {
            _image.sprite = _enabledSprite;
        }
        else
        {
            _image.sprite = _disabledSprite;
        }
    }
}
