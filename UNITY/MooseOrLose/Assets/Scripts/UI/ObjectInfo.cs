using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectInfo : MonoBehaviour
{
    TextMeshProUGUI name;
    public GameObject info_bar;
    RectTransform background;

    public void Awake()
    {
        name = transform.Find("Background").transform.Find("Name").GetComponent<TextMeshProUGUI>();
        background = transform.Find("Background").GetComponent<RectTransform>();
        if (name == null || background == null)
        {
            Debug.Log("ObjectInfo Error - Setup failed");
        }

    }
    public void SpawnInfobar(ClickableObjectInfo info)
    {
        if (background != null)
        {
            foreach(RectTransform child in background)
            {
                if (child.gameObject.name == "Exit" || child.gameObject.name == "Name")
                {

                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        switch(info.type)
        {
            case ClickableObjectInfo.ObjectType.Moose:
                name.text = info.type.ToString();

                GameObject scacing1 = Instantiate(info_bar, background.transform);
                scacing1.GetComponent<TextMeshProUGUI>().text = "";

                GameObject age = Instantiate(info_bar, background.transform);
                age.GetComponent<TextMeshProUGUI>().text = "Age";

                GameObject years = Instantiate(info_bar, background.transform);
                years.GetComponent<TextMeshProUGUI>().text = "Years: " + info.age_years.ToString();
                years.GetComponent<TextMeshProUGUI>().fontSize = 15;

                GameObject months = Instantiate(info_bar, background.transform);
                months.GetComponent<TextMeshProUGUI>().text = "Months: " + info.age_months.ToString();
                months.GetComponent<TextMeshProUGUI>().fontSize = 15;

                GameObject days = Instantiate(info_bar, background.transform);
                days.GetComponent<TextMeshProUGUI>().text = "Days: " + info.age_days.ToString();
                days.GetComponent<TextMeshProUGUI>().fontSize = 15;

                GameObject scacing2 = Instantiate(info_bar, background.transform);
                scacing2.GetComponent<TextMeshProUGUI>().text = "";

                GameObject gender = Instantiate(info_bar, background.transform);
                gender.GetComponent<TextMeshProUGUI>().text = "Gender: " + info.gender.ToString();

                switch(info.gender)
                {
                    case Gender.Male:
                        GameObject antler_tags = Instantiate(info_bar, background.transform);
                        antler_tags.GetComponent<TextMeshProUGUI>().text = "Antler Tags: " + info.antler_tags.ToString();
                        antler_tags.GetComponent<TextMeshProUGUI>().fontSize = 15;
                        break;
                    case Gender.Female:
                        GameObject pregnant = Instantiate(info_bar, background.transform);
                        pregnant.GetComponent<TextMeshProUGUI>().text = "Pregnant: " + info.pregnant.ToString();
                        pregnant.GetComponent<TextMeshProUGUI>().fontSize = 15;

                        if (info.pregnant)
                        {
                            GameObject days_pregnant = Instantiate(info_bar, background.transform);
                            days_pregnant.GetComponent<TextMeshProUGUI>().text = "Days pregnant: " + info.days_pregnant.ToString();
                            days_pregnant.GetComponent<TextMeshProUGUI>().fontSize = 12;

                            GameObject children_in_belly = Instantiate(info_bar, background.transform);
                            children_in_belly.GetComponent<TextMeshProUGUI>().text = "Children in belly: " + info.children_in_belly.ToString();
                            children_in_belly.GetComponent<TextMeshProUGUI>().fontSize = 12;
                        }
                        break;
                }
                break;
            case ClickableObjectInfo.ObjectType.Wolf:
                name.text = info.type.ToString();
                break;
            case ClickableObjectInfo.ObjectType.Forest:
                name.text = info.type.ToString();
                break;
        }
    }
}
