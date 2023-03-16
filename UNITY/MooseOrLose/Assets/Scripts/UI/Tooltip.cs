using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private TextMeshProUGUI Text;
    private RectTransform background;
    public static Tooltip instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Text = transform.Find("Background").Find("Text").GetComponent<TextMeshProUGUI>();
        background = transform.Find("Background").GetComponent<RectTransform>();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowToolTip(string input)
    {
        gameObject.SetActive(true);
        Text.text = input;
    }
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),
                                                                Input.mousePosition,
                                                                null,
                                                                out localPoint);
        transform.localPosition = localPoint + new Vector2(1,1);
    }
}
