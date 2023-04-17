using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForestInfoUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI TreeAmount;
    [SerializeField] private TextMeshProUGUI Age;
    [SerializeField] private TextMeshProUGUI Type;
    [SerializeField] private TextMeshProUGUI Height;
    [SerializeField] private TextMeshProUGUI Density;
    [SerializeField] private GameObject Parent;
    [SerializeField] private GraphForestAgeSpread graph;
    // Update is called once per frame
    void FixedUpdate()
    {
        
        Parent.SetActive(false);
        graph.gameObject.SetActive(false);
        Forest forest = Camera_v2.Instance.GetSelectedForest();
        if (forest != null)
        {
            Parent.SetActive(true);
            graph.gameObject.SetActive(true);
            TreeAmount.text = forest.GetForestTreeAmount().ToString();
            Age.text = forest.averageAge.ToString();
            Type.text = forest.forestType.ToString();
            Height.text = forest.forestHeight.ToString();
            Density.text = forest.forestDensity.ToString();
            graph.ShowGraph(forest.forestAgeSpread, Color.green);
        }
    }

    public void ThinForest(float amount)
    {
        Forest forest = Camera_v2.Instance.GetSelectedForest();
        forest.ThinForest(amount);
    }

    public void RemoveForest()
    {
        Forest forest = Camera_v2.Instance.GetSelectedForest();
        forest.ClearForest();
    }
}
