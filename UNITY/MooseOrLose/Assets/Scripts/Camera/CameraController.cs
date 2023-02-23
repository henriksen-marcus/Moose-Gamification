using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{


    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    Camera mainCamera;

    GameObject GameObjectInfo;
    public GameObject InfoBar;

    private void Awake()
    {
        mainCamera = transform.Find("Main Camera").GetComponent<Camera>();

    }
    private void Start()
    {
        GameObjectInfo = GameObject.Find("UI_Canvas").transform.Find("GameObjectInfo").gameObject;
    }


    /*// Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-pos.x * dragSpeed, 0, -pos.y * dragSpeed);

            transform.Translate(move, Space.World);

            dragOrigin = Input.mousePosition;
        }

        // Scrolling

        Vector3 scroll = transform.position;
        scroll.y -= (Input.mouseScrollDelta.y * 2);
        if (!(scroll.y < 0) && !(scroll.y > 100))
            scroll.z += (Input.mouseScrollDelta.y * 2);
        scroll.y = Mathf.Clamp(scroll.y, 0, 100);
        transform.position = scroll;


        // Finding Objects
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = 1 << 3;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 500, layerMask))
            {
                Debug.Log(hit.collider.gameObject.name);

                // Elg
                if (hit.collider.GetComponent<Elg>() != null)
                {
                    GameObjectInfo.SetActive(true);

                    Elg script = hit.collider.GetComponent<Elg>();

                    Transform background = GameObjectInfo.transform.Find("Background");

                    SpawnElgInfoBar(script, background);
                }

                //Ulv
                if (hit.collider.GetComponent<Ulv>() != null)
                {
                    GameObjectInfo.SetActive(true);

                    Ulv script = hit.collider.GetComponent<Ulv>();

                    Transform background = GameObjectInfo.transform.Find("Background");

                    SpawnUlvInfoBar(script, background);
                }
            }
            else
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    //Forest
                    if (hit.collider.GetComponent<Forest>() != null)
                    {
                        GameObjectInfo.SetActive(true);

                        Forest script = hit.collider.GetComponent<Forest>();

                        Transform background = GameObjectInfo.transform.Find("Background");

                        SpawnForestInfoBar(script, background);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            GameObjectInfo.SetActive(false);
        }
        


    }


    void SpawnElgInfoBar(Elg script, Transform background)
    {
        foreach (Transform child in background)
        {

            GameObject.Destroy(child.gameObject);

        }

        GameObject name = Instantiate(InfoBar, background.transform);
        name.GetComponent<TextMeshProUGUI>().text = "Elg";
        name.GetComponent<TextMeshProUGUI>().fontSize = 36;

        GameObject ib1 = Instantiate(InfoBar, background.transform);
        ib1.GetComponent<TextMeshProUGUI>().text = "Age : " + script.age_years.ToString() + " y " + script.age_months.ToString() + " m " + script.age_days.ToString() + " d";

        GameObject ib2 = Instantiate(InfoBar, background.transform);
        ib2.GetComponent<TextMeshProUGUI>().text = "Gender : " + script.gender.ToString();

        GameObject ib3 = Instantiate(InfoBar, background.transform);
        ib3.GetComponent<TextMeshProUGUI>().text = "Weight : " + script.weight.ToString();

        if (script.gender == Gender.Male)
        {
            GameObject ib4 = Instantiate(InfoBar, background.transform);
            ib4.GetComponent<TextMeshProUGUI>().text = "Antler tags: " + script.antler_tag_number.ToString();
        }

    }


    void SpawnUlvInfoBar(Ulv script, Transform background)
    {
        foreach (Transform child in background)
        {

            GameObject.Destroy(child.gameObject);

        }

        GameObject name = Instantiate(InfoBar, background.transform);
        name.GetComponent<TextMeshProUGUI>().text = "Ulv";
        name.GetComponent<TextMeshProUGUI>().fontSize = 36;

        GameObject ib1 = Instantiate(InfoBar, background.transform);
        ib1.GetComponent<TextMeshProUGUI>().text = "Age : " + script.age_years.ToString() + " y " + script.age_months.ToString() + " m " + script.age_days.ToString() + " d";

        GameObject ib2 = Instantiate(InfoBar, background.transform);
        ib2.GetComponent<TextMeshProUGUI>().text = "Gender : " + script.gender.ToString();

        GameObject ib3 = Instantiate(InfoBar, background.transform);
        ib3.GetComponent<TextMeshProUGUI>().text = "Weight : " + script.weight.ToString();

    }


    void SpawnForestInfoBar(Forest script, Transform background)
    {
        foreach (Transform child in background)
        {

            GameObject.Destroy(child.gameObject);

        }

        GameObject name = Instantiate(InfoBar, background.transform);
        name.GetComponent<TextMeshProUGUI>().text = "Forest";
        name.GetComponent<TextMeshProUGUI>().fontSize = 36;

        GameObject ib1 = Instantiate(InfoBar, background.transform);
        ib1.GetComponent<TextMeshProUGUI>().text = "Tree Type : " + script.forestType.ToString();

        GameObject ib2 = Instantiate(InfoBar, background.transform);
        ib2.GetComponent<TextMeshProUGUI>().text = "Density : " + script.forest_Density.ToString();

        GameObject ib3 = Instantiate(InfoBar, background.transform);
        ib3.GetComponent<TextMeshProUGUI>().text = "Height : " + script.forest_Height.ToString();

    }*/
}
