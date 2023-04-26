using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tipToShow;
    private float _timeToWait = 0.5f;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hovered
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Exited
        StopAllCoroutines();
        HoverTipManager.Instance.OnMouseLoseFocus();
    }

    private void ShowMessage()
    {
        HoverTipManager.Instance.OnMouseHover(tipToShow, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_timeToWait);
        
        ShowMessage();
    }
    
}
