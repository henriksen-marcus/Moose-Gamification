using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct TutorialLayer
{
    [TextArea(3, 10)]
    [SerializeField] public string Title;
    [TextArea(3, 10)]
    [SerializeField] public string Text;
}
[System.Serializable]
public struct TutorialAnchors
{
    [SerializeField] public Vector2 Min;
    [SerializeField] public Vector2 Max;
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject PopUp;
    [SerializeField] private GameObject Panel;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Text;

    [SerializeField] private Button Prev;
    [SerializeField] private Button Next;

    [SerializeField] private List<TutorialLayer> tutorialTexts;
    [SerializeField] private List<Vector3> tutorialPositions;
    [SerializeField] private List<TutorialAnchors> tutorialAnchors;
    [SerializeField] private List<GameObject> tutorialItems;
    public int currentLayer;
    private int lastChildIndex;
    private void Awake()
    {
        Prev.onClick.AddListener(PreviousLayer);
        Next.onClick.AddListener(NextLayer);    
    }
    void Start()
    {
        PopUp.SetActive(false);
        Panel.SetActive(false);
        currentLayer = 0;
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(0.25f);

        TimeManager.instance.SetGamePaused(true);
        PopUp.SetActive(true);
        Panel.SetActive(true);
        PopUp.GetComponent<RectTransform>().anchorMin = tutorialAnchors[currentLayer].Min;
        PopUp.GetComponent<RectTransform>().anchorMax = tutorialAnchors[currentLayer].Max;
        PopUp.GetComponent<RectTransform>().anchoredPosition = tutorialPositions[currentLayer];
        Title.text = tutorialTexts[currentLayer].Title;
        Text.text = tutorialTexts[currentLayer].Text;
        Prev.gameObject.SetActive(false);

        lastChildIndex = tutorialItems[0].transform.GetSiblingIndex();

        tutorialItems[0].transform.SetAsLastSibling();
        if (currentLayer == tutorialTexts.Count - 1)
        {
            Next.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        }
        else
        {
            Next.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
        }
    }

    public void NextLayer()
    {
        currentLayer++;
        tutorialItems[currentLayer - 1].transform.SetSiblingIndex(lastChildIndex);
        Prev.gameObject.SetActive(true);

        // Check if we are done
        if (currentLayer >= tutorialTexts.Count)
        {
            EndTutorial();
            return;
        }

        lastChildIndex = tutorialItems[currentLayer].transform.GetSiblingIndex();
        tutorialItems[currentLayer].transform.SetAsLastSibling();

        PopUp.GetComponent<RectTransform>().anchorMin = tutorialAnchors[currentLayer].Min;
        PopUp.GetComponent<RectTransform>().anchorMax = tutorialAnchors[currentLayer].Max;
        PopUp.GetComponent<RectTransform>().anchoredPosition = tutorialPositions[currentLayer];

        Title.text = tutorialTexts[currentLayer].Title;
        Text.text = tutorialTexts[currentLayer].Text;

        if (currentLayer == tutorialTexts.Count - 1)
        {
            Next.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        }
        else
        {
            Next.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
        }
    }

    public void PreviousLayer()
    {
        currentLayer--;
        tutorialItems[currentLayer + 1].transform.SetSiblingIndex(lastChildIndex);
        
        // Check if we are done
        if (currentLayer == 0)
        {
            Prev.gameObject.SetActive(false);
        }

        lastChildIndex = tutorialItems[currentLayer].transform.GetSiblingIndex();
        tutorialItems[currentLayer].transform.SetAsLastSibling();

        PopUp.GetComponent<RectTransform>().anchorMin = tutorialAnchors[currentLayer].Min;
        PopUp.GetComponent<RectTransform>().anchorMax = tutorialAnchors[currentLayer].Max;
        PopUp.GetComponent<RectTransform>().anchoredPosition = tutorialPositions[currentLayer];

        Title.text = tutorialTexts[currentLayer].Title;
        Text.text = tutorialTexts[currentLayer].Text;

        if (currentLayer == tutorialTexts.Count - 1)
        {
            Next.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        }
        else
        {
            Next.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
        }
    }

    public void EndTutorial()
    {
        TimeManager.instance.SetGamePaused(false);
        Destroy(gameObject);
    }
    

}
