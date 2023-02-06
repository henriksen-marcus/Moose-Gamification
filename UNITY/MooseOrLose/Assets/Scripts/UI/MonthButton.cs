using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonthButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private int month;
    
    private bool _huntingEnabled = false;
    // private IPointerDownHandler _pointerDownHandlerImplementation;

    public static event Action OnMonthButtonChanged;

    
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _huntingEnabled = !_huntingEnabled;
        _image.color = !_huntingEnabled ? Color.white : Color.green;
        OnMonthButtonChanged?.Invoke();
    }

    public bool HuntingEnabled()
    {
        return _huntingEnabled;
    }

    public int Month()
    {
        return month;
    }
}
