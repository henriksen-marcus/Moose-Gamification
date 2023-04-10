using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite dot;
    [SerializeField] private GameObject Line;

    private RectTransform container;
    private RectTransform settings;

    bool moosePopulation;
    bool mooseMales;
    bool mooseFemales;
    bool mooseChildren;
    bool forestDensity;
    bool forestQuantity;
    bool forestAge;

    // Start is called before the first frame update
    void Awake()
    {
        container = GetComponent<RectTransform>();
        settings = transform.parent.transform.Find("Background").transform.Find("Settings").GetComponent<RectTransform>();

        moosePopulation = false;
        mooseMales = false;
        mooseFemales = false;
        forestDensity = false;
        forestQuantity = false;
        forestAge = false;

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
            ShowGraph(ElgManager.instance.elg_females_graph, Color.yellow);
        }
        if(mooseChildren)
        {
            ShowGraph(ElgManager.instance.elg_children_graph, Color.cyan);
        }
        if (forestQuantity)
        {
            ShowGraph(ForestManager.instance.forestQuantityAverage, new Color(170f / 255f, 255f / 255f, 0, 1));
        }
        if (forestDensity)
        {
            ShowGraph(ForestManager.instance.forestDensityAverage, new Color(80f / 255f, 200f / 255f, 120f / 255f, 1));
        }
        if (forestAge)
        {
            ShowGraph(ForestManager.instance.forestTreeAgeAverage, new Color(175f / 255f, 225f / 255f, 175f / 255f, 1));
        }

    }


    private void ShowGraph(List<int> valueList, Color color)
    {
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
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(3, 3);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

    }
    private void CreateLine(Vector2 pos1, Vector2 pos2, Color color)
    {
        GameObject go = Instantiate(Line, container);
        Image image = go.GetComponentInChildren<Image>();
        image.color = color;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pos1 + ((pos2-pos1) / 2f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2((pos2 - pos1).magnitude, 2);
        Vector3 rotation = rectTransform.transform.eulerAngles;
        float angle = Vector2.Angle(new Vector2(1,0), pos2 - pos1);

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

    public void updateBooleans() 
    {  
        if (settings == null || container == null)
        {
            container = GetComponent<RectTransform>();
            settings = transform.parent.transform.Find("Background").transform.Find("Settings").GetComponent<RectTransform>();
        }
        moosePopulation = settings.Find("MoosePopulation").GetComponentInChildren<Toggle>().isOn;
        mooseMales = settings.Find("MooseMales").GetComponentInChildren<Toggle>().isOn;
        mooseFemales = settings.Find("MooseFemales").GetComponentInChildren<Toggle>().isOn;
        mooseChildren = settings.Find("MooseChildren").GetComponentInChildren<Toggle>().isOn;
        forestAge = settings.Find("ForestAge").GetComponentInChildren<Toggle>().isOn;
        forestDensity = settings.Find("ForestDensity").GetComponentInChildren<Toggle>().isOn;
        forestQuantity = settings.Find("ForestQuantity").GetComponentInChildren<Toggle>().isOn;
        if (transform.parent.gameObject.activeSelf)
            UpdateGraphMenu();
    }


    
}
