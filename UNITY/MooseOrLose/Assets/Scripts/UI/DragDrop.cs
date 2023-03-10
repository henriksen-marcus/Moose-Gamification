using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas screenCanvas;
    public GameObject prefab;
    
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private DragDrop _copy;
    private void Awake()
    {
        // _rectTransform = GetComponent<RectTransform>();
        // _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        InfoUI.Instance.gridOn = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("On Pointer down");
        _copy = Instantiate(this, transform);
        _rectTransform = _copy.GetComponent<RectTransform>();
        _canvasGroup = _copy.GetComponent<CanvasGroup>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        InfoUI.Instance.gridOn = true;
        // Debug.Log("On Start dragging");
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / screenCanvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        InfoUI.Instance.gridOn = false;
        // Debug.Log("On End dragging");
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        Destroy(_copy.gameObject);
    }
}
