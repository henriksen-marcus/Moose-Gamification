using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using SFB;


public class SaveResultsPanel : MonoBehaviour
{
    public SaveType saveType = SaveType.PDF;
    private TextMeshProUGUI errorText;
    private string PDFError = "Could not save PDF file. A backup JSON file has been saved to the chosen directory.";
    private string JSONError = "Could not save JSON file.";
    
    [SerializeField] private GameObject SuccessPopup;
    
    
    void Awake()
    {
        errorText = GameObject.Find("ErrorText").GetComponent<TextMeshProUGUI>();
        errorText.enabled = false;
    }
    
    private void Start()
    {
        // Destroy this if we unpause
        TimeManager.Instance.Pause += OnGamePaused;
    }

    public void ChangeSaveType(int type) => saveType = (SaveType)type;

    public void Save()
    {
        errorText.enabled = false;
        var path = StandaloneFileBrowser.SaveFilePanel(
            "Save as..",
            Application.dataPath,
            "Simulator_Report",
            saveType switch
            {
                SaveType.PDF => "pdf",
                SaveType.JSON => "json",
                _ => throw new ArgumentOutOfRangeException()
            });
        
        // If user selected a path (they didn't cancel the save dialog)
        if (path.Length != 0)
        {
            if (PDFPrompter.SaveSimulationResults(path, saveType)) 
                OnSuccededSave();
            else if (errorText)
            {
                errorText.enabled = true;
                errorText.text = saveType switch
                {
                    SaveType.PDF => PDFError,
                    SaveType.JSON => JSONError,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
    
    private void OnSuccededSave()
    {
        // This object handles destroying itself
        Instantiate(SuccessPopup, GameObject.Find("Screen Canvas").transform);
        Back();
    }


    public void Back()
    {
        Destroy(gameObject);
    }

    private void OnGamePaused(bool paused)
    {
        if (!paused && this != null) Destroy(gameObject);
    }
}
