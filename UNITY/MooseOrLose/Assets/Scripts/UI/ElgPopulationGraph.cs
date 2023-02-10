using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElgPopulationGraph : MonoBehaviour
{
    [SerializeField] private Sprite dot;

    private RectTransform container;


    // Start is called before the first frame update
    void Awake()
    {
        container = GetComponent<RectTransform>();
        
    }

    private void Start()
    {
        TimeManager.instance.OnNewDay += UpdateGraph;
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
        float yMax = 300;
        float xMin = 10;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = xMin + (i * 5);
            float yPos = (valueList[i] / yMax) * height;
            CreateCircle(new Vector2(xPos, yPos));
        }
    }
}
