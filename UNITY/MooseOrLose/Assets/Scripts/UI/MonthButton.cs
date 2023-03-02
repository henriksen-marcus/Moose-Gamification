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
    
    [SerializeField] private bool huntingEnabled = false;
    // private IPointerDownHandler _pointerDownHandlerImplementation;

    public static event Action OnMonthButtonChanged;

    
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        if (huntingEnabled)
            _image.color = Color.green;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        huntingEnabled = !huntingEnabled;
        _image.color = !huntingEnabled ? Color.white : Color.green;
        OnMonthButtonChanged?.Invoke();
    }

    public bool HuntingEnabled()
    {
        return huntingEnabled;
    }

    public int Month()
    {
        return month;
    }
}
