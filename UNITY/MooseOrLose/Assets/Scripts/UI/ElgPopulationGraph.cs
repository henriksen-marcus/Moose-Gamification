using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElgPopulationGraph : MonoBehaviour
{
    [SerializeField] private Sprite dot;

    private RectTransform container;

    private int highest;


    // Start is called before the first frame update
    void Awake()
    {
        container = GetComponent<RectTransform>();
        highest = 150;
    }

    private void Start()
    {
        TimeManager.Instance.OnNewDay += UpdateGraph;
    }

    void UpdateGraph()
    {
        foreach(Transform child in container)
        {
            Destroy(child.gameObject);
        }
        ShowGraph(ElgManager.instance.elg_population_graph);
    }

    // Update is called once per frame
    void CreateCircle(Vector2 anchoredPosition)
    {
        GameObject go = new GameObject("circle", typeof(Image));
        go.transform.SetParent(container, false);
        go.GetComponent<Image>().sprite = dot;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(2, 2);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
 
    }

    private void ShowGraph(List<int> valueList)
    {
        float height = container.sizeDelta.y - 20;
        float width = container.sizeDelta.x - 20;
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
            if (valueList[i] > highest)
            {
                highest = valueList[i];
            }
            float xPos = xMin + (i * increment);
            float yPos = (valueList[i] / yMax) * height;
            CreateCircle(new Vector2(xPos, yPos));
        }
    }
}
