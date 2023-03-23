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
    Camera_v2 camera;
    private void Awake()
    {
        // _rectTransform = GetComponent<RectTransform>();
        // _canvasGroup = GetComponent<CanvasGroup>();
        camera = GameObject.Find("Camera-v2").GetComponent<Camera_v2>();
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
        camera.SetMovementEnabled(false);
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
        camera.SetMovementEnabled(true);
        InfoUI.Instance.gridOn = false;
        // Debug.Log("On End dragging");
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        Destroy(_copy.gameObject);
    }
}
