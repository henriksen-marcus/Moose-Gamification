using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite dot;

    private RectTransform container;
    private RectTransform settings;

    bool moosePopulation;
    bool mooseMales;
    bool mooseFemales;
    bool mooseChildren;

    private int highest;
    // Start is called before the first frame update
    void Awake()
    {
        container = GetComponent<RectTransform>();
        settings = transform.parent.transform.Find("Background").transform.Find("Settings").GetComponent<RectTransform>();
        highest = 150;

        moosePopulation = false;
        mooseMales = false;
        mooseFemales = false;
        mooseChildren = false;

        
    }

    private void Start()
    {
        transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void ToggleGraphMenu()
    {
        TimeManager.instance.SetGamePaused(!transform.parent.gameObject.activeSelf);
        transform.parent.gameObject.SetActive(!transform.parent.gameObject.activeSelf);
        if (transform.parent.gameObject.activeSelf)
            UpdateGraphMenu();
    }

    public void UpdateGraphMenu()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        if (moosePopulation)
        {
            ShowGraph(ElgManager.instance.elg_population_graph, Color.blue);
        }
        if (mooseMales)
        {
            ShowGraph(ElgManager.instance.elg_males_graph, Color.red);
        }
        if(mooseFemales)
        {
            ShowGraph(ElgManager.instance.elg_females_graph, Color.green);
        }
        if(mooseChildren)
        {
            ShowGraph(ElgManager.instance.elg_children_graph, Color.cyan);
        }

    }


    private void ShowGraph(List<int> valueList, Color color)
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
            CreateCircle(new Vector2(xPos, yPos), color);
        }
    }

    void CreateCircle(Vector2 anchoredPosition, Color color)
    {
        GameObject go = new GameObject("circle", typeof(Image));
        go.transform.SetParent(container, false);
        go.GetComponent<Image>().sprite = dot;
        go.GetComponent<Image>().color = color;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(3, 3);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

    }
    public void updateBooleans() 
    {  
        if (settings == null || container == null)
        {
            container = GetComponent<RectTransform>();
            settings = transform.parent.transform.Find("Background").transform.Find("Settings").GetComponent<RectTransform>();
        }
        moosePopulation = settings.Find("MoosePopulation").transform.Find("MoosePopulationCheckBox").GetComponent<Toggle>().isOn;
        mooseMales = settings.Find("MooseMales").transform.Find("MooseMalesCheckBox").GetComponent<Toggle>().isOn;
        mooseFemales = settings.Find("MooseFemales").transform.Find("MooseFemalesCheckBox").GetComponent<Toggle>().isOn;
        mooseChildren = settings.Find("MooseChildren").transform.Find("MooseChildrenCheckBox").GetComponent<Toggle>().isOn;

        if (transform.parent.gameObject.activeSelf)
            UpdateGraphMenu();
    }

}
