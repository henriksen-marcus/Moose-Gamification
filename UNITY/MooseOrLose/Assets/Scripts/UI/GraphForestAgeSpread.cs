using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GraphForestAgeSpread : MonoBehaviour
{
    [SerializeField] private Sprite dot;
    [SerializeField] private GameObject Line;

    [SerializeField] private RectTransform container;


    public void ShowGraph(List<int> valueList, Color color)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        float height = container.sizeDelta.y - 20;
        float width = container.sizeDelta.x - 20;
        float highest = 150;
        for (int i = 0; i < valueList.Count; i++)
        {
            if (valueList[i] > highest)
            {
                highest = valueList[i];
            }
        }

        float yMax = highest + 50;
        float xMin = 10;
        float increment;
        if (valueList.Count < 10)
        {
            increment = width / 10f;
        }
        else
        {
            increment = width / valueList.Count;
        }

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = xMin + (i * increment);
            float yPos = (valueList[i] / yMax) * height;
            CreateCircle(new Vector2(xPos, yPos), color);
        }

        for (int i = 0; i < valueList.Count - 1; i++)
        {
            float xPos1 = xMin + (i * increment);
            float yPos1 = (valueList[i] / yMax) * height;

            float xPos2 = xMin + ((i + 1) * increment);
            float yPos2 = (valueList[i + 1] / yMax) * height;
            CreateLine(new Vector2(xPos1, yPos1), new Vector2(xPos2, yPos2), color);
        }


    }

    void CreateCircle(Vector2 anchoredPosition, Color color)
    {
        GameObject go = new GameObject("circle", typeof(Image));
        go.transform.SetParent(container, false);
        go.GetComponent<Image>().sprite = dot;
        go.GetComponent<Image>().color = color;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(3, 3);

    }
    private void CreateLine(Vector2 pos1, Vector2 pos2, Color color)
    {
        GameObject go = Instantiate(Line, container);
        Image image = go.GetComponentInChildren<Image>();
        image.color = color;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchoredPosition = pos1 + ((pos2 - pos1) / 2f);
        rectTransform.sizeDelta = new Vector2((pos2 - pos1).magnitude, 2);
        Vector3 rotation = rectTransform.transform.eulerAngles;
        float angle = Vector2.Angle(new Vector2(1, 0), pos2 - pos1);

        if (pos1.y > pos2.y)
        {
            rotation.z = 360 - angle;
        }
        else
        {
            rotation.z = angle;
        }
        rectTransform.transform.eulerAngles = rotation;

    }

}
